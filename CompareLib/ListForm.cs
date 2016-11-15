using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ComparerLib
{
	internal partial class ListForm : Form
	{
		private const string ComparePathKey = "comparer";

		private Color HyperLinkColor = Color.Blue;
		private CompareData _parent;
		private AppSettings _settings;
		private List<ListViewItem> _allItems; // Saved ListViewItems to remove and re-add when filters are applied
		private List<DiffItem> _items;

		private string _comparePath;
		private int _startHyperlinks;

		public ListForm(CompareData parent)
		{
			InitializeComponent();

			_parent = parent;
			_parent.ItemUpdated = OnItemUpdate;
			_settings = new AppSettings();
			_items = parent.Items.ToList();

			//	Set Caption
			string caption = parent.DescriptionA == null || parent.DescriptionB == null
				? "Compare Items"
				: $"Compare {parent.DescriptionA} to {parent.DescriptionB}";
			string plural = _items.Count == 1 ? "" : "s";
			Text = $"{caption} - {_items.Count:#,0} item{plural}";

			//	Make sure the comparer exists
			_comparePath = ConfigurationManager.AppSettings[ComparePathKey];
			if (string.IsNullOrWhiteSpace(_comparePath) || !File.Exists(_comparePath))
				throw new FileNotFoundException($@"Cannot file compare app at ""{_comparePath}""");

			//	Set menu description
			if (parent.DescriptionA != null && parent.DescriptionB != null)
			{
				menuAOnly.Text = $"&A: {parent.DescriptionA}";
				menuBOnly.Text = $"&B: {parent.DescriptionB}";
			}

		}
		private void OnItemUpdate(int index, DiffItem item, UpdateTypes type)
		{
			if (type == UpdateTypes.Delete)
			{
				_allItems.RemoveAt(index);
				_items.RemoveAt(index);
				theList.Items.RemoveAt(index);
			}
			else
			{
				_items[index] = item;
				UpdateRow(index);
			}
		}

		private void CompareForm_Load(object sender, EventArgs e)
		{
			//	In case the list of names isn't the same for all items, get the max
			int maxNames = _items.Any() ? _items.Max(item => item.Names.Count) : 0;
			//	Get the lesser of the label count or maxNames
			int labelCount = Math.Min(_parent.NameLabels.Count, maxNames);
			//	Create the name labels and any empty ones needed
			var nameLabels = _parent.NameLabels.Take(maxNames).Concat(new string[maxNames - labelCount]).Select(s => s ?? "").ToList();
			//	First label will be Name if not supplied
			if (string.IsNullOrWhiteSpace(nameLabels[0]))
				nameLabels[0] = "Name";

			//	Add Headers
			theList.Columns.Add(new ColumnHeader() { Text = "Issue", TextAlign = HorizontalAlignment.Left });
			theList.Columns.AddRange(nameLabels.Select(n => new ColumnHeader() { Text = n, TextAlign = HorizontalAlignment.Left }).ToArray());
			theList.Columns.Add(new ColumnHeader() { Text = "Action", TextAlign = HorizontalAlignment.Center });
			_startHyperlinks = theList.Columns.Count;
			var actions = (_parent.CustomActionLabels ?? new ReadOnlyCollection<string>(new List<string>())).ToList();
			if (actions.Any())
				actions.ForEach(l => theList.Columns.Add(new ColumnHeader() { Text = "", TextAlign = HorizontalAlignment.Center }));

			theList.Columns.Add(new ColumnHeader() { Text = "" });  //	A blank column keeps the ListView from looking odd after resizing

			//	Create a list of list items
			_allItems =
				_items.Select(item =>
					//	Build List View Item with an array of values
					new ListViewItem(
						new[] { item.DiffDescription }.Concat(
						item.Names).Concat(
						new string[maxNames - item.Names.Count]).Concat( // If not all items have the same number of names, add empty items
						new[] { item.ActionName }.Concat(actions)).ToArray()
						)).ToList();

			theList.Items.AddRange(_allItems.ToArray());

			//	Make the action subitems look like hyperlinks
			for (int i = 0; i < theList.Items.Count; i++)
				UpdateRow(i);

			//	Auto-size the listview
			theList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			theList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
		}

		private void UpdateRow(int index)
		{
			var item = theList.Items[index];
			var diffItem = _items[index];
			item.SubItems[0].Text = diffItem.DiffDescription;
			item.SubItems[_startHyperlinks - 1].Text = diffItem.ActionName;

			//	Make the action subitems look like hyperlinks
			item.Tag = diffItem.Condition.ToString();
			item.UseItemStyleForSubItems = false;
			for (int i = _startHyperlinks; i < theList.Columns.Count; i++)
			{
				var subItem = item.SubItems[i - 1];
				subItem.Tag = (i == _startHyperlinks);
				bool enabled = i == _startHyperlinks || (_parent.CustomAction != null && (_parent.CheckEnabled?.Invoke(subItem.Text, index, diffItem) ?? true));
				if (enabled)
				{
					subItem.ForeColor = HyperLinkColor;
					subItem.Font = new Font(subItem.Font, FontStyle.Underline);
				}
				else
					subItem.ForeColor = Color.Gray;
			}
		}

		private void theList_MouseMove(object sender, MouseEventArgs e)
		{
			//	Are we over a "hyperlink"? 
			var hit = theList.HitTest(e.X, e.Y);
			bool hasAction = hit.Item != null &&
					hit.SubItem != null &&
					!string.IsNullOrEmpty(hit.SubItem.Text) &&
					hit.SubItem.ForeColor == HyperLinkColor;
			//	If so, change the cursor and store the item in the tag
			theList.Cursor = hasAction ? Cursors.Hand : Cursors.Default;
			theList.Tag = hasAction ? hit.Item : null;
			if (hasAction)
				hit.Item.Tag = new Tuple<string, bool>(hit.SubItem.Text, (bool)hit.SubItem.Tag);
		}

		private void theList_MouseDown(object sender, MouseEventArgs e)
		{
			var item = theList.Tag as ListViewItem;
			if (item != null)
			{
				//	Select the item
				theList.SelectedItems.Clear();
				item.Selected = true;
				item.Focused = true;
				var data = (Tuple<string, bool>)item.Tag;
				var defaultAction = data.Item2;
				var actionText = data.Item1;
				//	Get the action
				if (defaultAction)
				{
					var action = Utility.GetActionFromName(actionText);
					if (action == Actions.Compare)
						CompareItems(_items[item.Index]);
					else if (action == Actions.View)
						ViewItem(_items[item.Index]);
					else
						throw new NotImplementedException("This should not happen");
				}
				//	Invoke custom action (if there is one)
				else _parent.CustomAction?.Invoke(actionText, item.Index, _items[item.Index]);
			}
		}

		private void ViewItem(DiffItem item)
		{
			string name = item.Condition == DiffConditions.AOnly ? (_parent.DescriptionA ?? "A") : (_parent.DescriptionB ?? "B");
			using (var frm = new ViewForm(_settings, name, item.ContentsA ?? item.ContentsB))
				frm.ShowDialog();
		}

		private void CompareItems(DiffItem item)
		{
			string path = Path.GetTempPath();
			if (!path.EndsWith("\\"))
				path += '\\';

			Func<string, string> fixFileName = name => Regex.Replace(name, $"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}]", " ").Trim();

			//	Create valid file names
			string file1 = path + fixFileName($"{_parent.DescriptionA ?? "A"} {item.Names.FirstOrDefault()}");
			string file2 = path + fixFileName($"{_parent.DescriptionB ?? "B"} {item.Names.FirstOrDefault()}");

			var files = new[]
			{
				new { file = file1, content = item.ContentsA },
				new { file = file2, content = item.ContentsB }
			}.ToList();

			try
			{
				//	Create the files
				files.ForEach(f =>
				{
					using (TextWriter tw = new StreamWriter(f.file, false, Encoding.UTF8))
						tw.Write(f.content);
				});
				//	Start the comparer
				using (var proc = new Process()
				{
					StartInfo = new ProcessStartInfo(_comparePath, $@"""{file1}"" ""{file2}""")
					{ WindowStyle = ProcessWindowStyle.Maximized }
				})
				{
					proc.Start();
					proc.WaitForExit();
				}
			}
			finally
			{
				//	Delete the files
				files.ForEach(f =>
				{
					if (File.Exists(f.file))
						File.Delete(f.file);
				});
			}

		}

		//	Close on Escape
		private void ListForm_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Escape)
			{
				e.Handled = true;
				Close();
			}
		}

		//	Copy items to clipboard
		private void menuCopy_Click(object sender, EventArgs e)
		{
			int take = theList.Columns.Count - 2;
			string headers = string.Join("\t", theList.Columns.Cast<ColumnHeader>().Take(take).Select(c => c.Text));
			string data = string.Join("\r\n", theList.Items.Cast<ListViewItem>()
				.Select(item => string.Join("\t", item.SubItems.Cast<ListViewItem.ListViewSubItem>().Take(take).Select(sb => sb.Text))));
			Clipboard.SetText($"{headers}\r\n{data}");
		}

		private void menuViewItem_Click(object sender, EventArgs e)
		{
			string tag;
			var menu = sender as ToolStripMenuItem;
			if (menu == null || (tag = menu.Tag as string) == null)
				return;

			menu.Checked = !menu.Checked;
			var view = new[] { menuDifferent, menuSame, menuAOnly, menuBOnly }
				.ToDictionary(k => Utility.GetConditionFromName((string)k.Tag), v => v.Checked);

			theList.BeginUpdate();
			theList.Items.Clear();
			theList.Items.AddRange(_allItems.Where(item => view[Utility.GetConditionFromName((string)item.Tag)]).ToArray());
			theList.EndUpdate();

		}
	}
}
