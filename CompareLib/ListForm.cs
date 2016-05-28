using System;
using System.Collections.Generic;
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

		private CompareData _parent;
		private AppSettings _settings;
		private List<ListViewItem> _allItems; // Saved ListViewItems to remove and re-add when filters are applied
		private List<DiffItem> _items;

		private string _comparePath;

		public ListForm(CompareData parent)
		{
			InitializeComponent();

			_parent = parent;
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
			theList.Columns.Add(new ColumnHeader() { Text = "" });	//	A blank column keeps the ListView from looking odd after resizing

			//	Create a list of list items
			_allItems =
				_items.Select(item =>
					//	Build List View Item with an array of values
					new ListViewItem(
						new[] { item.DiffDescription }.Concat(
						item.Names).Concat(
						new string[maxNames - item.Names.Count]).Concat( // If not all items have the same number of names, add empty items
						new[] { item.ActionName }).ToArray()
						)).ToList();

			theList.Items.AddRange(_allItems.ToArray());

			//	Make the last subitem look like a hyperlink
			int index = 0;
			theList.Items.Cast<ListViewItem>().ToList().ForEach(item =>
			{
				item.Tag = _items[index++].Condition.ToString();
				item.UseItemStyleForSubItems = false;
				var subItem = item.SubItems.Cast<ListViewItem.ListViewSubItem>().Last();
				subItem.ForeColor = Color.Blue;
				subItem.Font = new Font(subItem.Font, FontStyle.Underline);
			});

			//	Auto-size the listview
			theList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
			theList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
		}

		private void theList_MouseMove(object sender, MouseEventArgs e)
		{
			//	Are we over a "hyperlink"? 
			var hit = theList.HitTest(e.X, e.Y);
			bool hasAction = hit.Item != null &&
					hit.SubItem != null &&
					!string.IsNullOrEmpty(hit.SubItem.Text) &&
					hit.SubItem == hit.Item.SubItems.Cast<ListViewItem.ListViewSubItem>().Last();
			//	If so, change the cursor and store the item in the tag
			theList.Cursor = hasAction ? Cursors.Hand : Cursors.Default;
			theList.Tag = hasAction ? hit.Item : null;
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
				//	Get the action
				var action = Utility.GetActionFromName(item.SubItems.Cast<ListViewItem.ListViewSubItem>().Last().Text);
				if (action == Actions.Compare)
					CompareItems(_items[item.Index]);
				else if (action == Actions.View)
					ViewItem(_items[item.Index]);
				else
					throw new NotImplementedException("This should not happen");
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
