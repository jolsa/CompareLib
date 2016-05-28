using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ComparerLib
{
	internal partial class ViewForm : Form
	{
		private const int DefaultTabSize = 4;
		private int _tabSize;
		private AppSettings _settings;
		private List<int?> _tabMenuChoices;

		public ViewForm(AppSettings settings, string name, string content)
		{
			InitializeComponent();

			Text = $"View {name}";
			_settings = settings;

			//	Get the value from the config file (if any)
			string tab = _settings["tabSize", $"{DefaultTabSize}"];

			//	Convert to int
			int tabSize;
			_tabSize = int.TryParse(tab, out tabSize) ? tabSize : DefaultTabSize;

			//	Get tab choices
			_tabMenuChoices = TabMenus.Select(t => int.TryParse(t.Tag as string, out tabSize) ? (int?)tabSize : null)
				.Where(t => t.HasValue).OrderBy(t => t).ToList();

			SetTabSize(_tabSize);
			textView.Text = content;

		}

		private List<ToolStripMenuItem> TabMenus
		{
			get { return menuTabs.DropDownItems.OfType<ToolStripMenuItem>().ToList(); }
		}

		private void SetTabSize(int tabSize)
		{
			_tabSize = tabSize;
			TabMenus.ForEach(menu => menu.Checked = (string)menu.Tag == $"{_tabSize}");
			textView.SetTabStopWidth(_tabSize);
			_settings["tabSize"] = $"{tabSize}";
			_settings.SaveSettings();
		}

		private void TabChange_Click(object sender, EventArgs e)
		{
			var item = sender as ToolStripMenuItem;
			int tabSize;
			if (item != null && int.TryParse(item.Tag as string, out tabSize) && tabSize != _tabSize)
				SetTabSize(tabSize);
		}

		private void ViewForm_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Escape)
			{
				e.Handled = true;
				Close();
			}
		}

		private void ViewForm_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Control)
			{
				char k = (char)e.KeyValue;
				if (k >= '1' && k <= '9')
				{
					int tabSize = k - '0';
					//	If it's a menu item, let the menu handle it
					if (!_tabMenuChoices.Contains(tabSize))
					{
						SetTabSize(tabSize);
						e.Handled = true;
					}
				}

			}
		}
	}
}
