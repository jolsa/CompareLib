namespace ComparerLib
{
	partial class ListForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListForm));
			this.theList = new System.Windows.Forms.ListView();
			this.menuStripMain = new System.Windows.Forms.MenuStrip();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuDifferent = new System.Windows.Forms.ToolStripMenuItem();
			this.menuSame = new System.Windows.Forms.ToolStripMenuItem();
			this.menuAOnly = new System.Windows.Forms.ToolStripMenuItem();
			this.menuBOnly = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStripMain.SuspendLayout();
			this.SuspendLayout();
			// 
			// theList
			// 
			this.theList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.theList.FullRowSelect = true;
			this.theList.HideSelection = false;
			this.theList.Location = new System.Drawing.Point(0, 24);
			this.theList.MultiSelect = false;
			this.theList.Name = "theList";
			this.theList.Size = new System.Drawing.Size(903, 251);
			this.theList.TabIndex = 0;
			this.theList.UseCompatibleStateImageBehavior = false;
			this.theList.View = System.Windows.Forms.View.Details;
			this.theList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.theList_MouseDown);
			this.theList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.theList_MouseMove);
			// 
			// menuStripMain
			// 
			this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
			this.menuStripMain.Location = new System.Drawing.Point(0, 0);
			this.menuStripMain.Name = "menuStripMain";
			this.menuStripMain.Size = new System.Drawing.Size(903, 24);
			this.menuStripMain.TabIndex = 1;
			this.menuStripMain.Text = "menuStrip1";
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCopy,
            this.viewToolStripMenuItem});
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.optionsToolStripMenuItem.Text = "&Options";
			// 
			// menuCopy
			// 
			this.menuCopy.Name = "menuCopy";
			this.menuCopy.Size = new System.Drawing.Size(169, 22);
			this.menuCopy.Text = "&Copy to clipboard";
			this.menuCopy.Click += new System.EventHandler(this.menuCopy_Click);
			// 
			// viewToolStripMenuItem
			// 
			this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuDifferent,
            this.menuSame,
            this.menuAOnly,
            this.menuBOnly});
			this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
			this.viewToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.viewToolStripMenuItem.Text = "&View";
			// 
			// menuDifferent
			// 
			this.menuDifferent.Checked = true;
			this.menuDifferent.CheckState = System.Windows.Forms.CheckState.Checked;
			this.menuDifferent.Name = "menuDifferent";
			this.menuDifferent.Size = new System.Drawing.Size(152, 22);
			this.menuDifferent.Tag = "Different";
			this.menuDifferent.Text = "&Different";
			this.menuDifferent.Click += new System.EventHandler(this.menuViewItem_Click);
			// 
			// menuSame
			// 
			this.menuSame.Checked = true;
			this.menuSame.CheckState = System.Windows.Forms.CheckState.Checked;
			this.menuSame.Name = "menuSame";
			this.menuSame.Size = new System.Drawing.Size(152, 22);
			this.menuSame.Tag = "Same";
			this.menuSame.Text = "&Same";
			this.menuSame.Click += new System.EventHandler(this.menuViewItem_Click);
			// 
			// menuAOnly
			// 
			this.menuAOnly.Checked = true;
			this.menuAOnly.CheckState = System.Windows.Forms.CheckState.Checked;
			this.menuAOnly.Name = "menuAOnly";
			this.menuAOnly.Size = new System.Drawing.Size(152, 22);
			this.menuAOnly.Tag = "AOnly";
			this.menuAOnly.Text = "&A Only";
			this.menuAOnly.Click += new System.EventHandler(this.menuViewItem_Click);
			// 
			// menuBOnly
			// 
			this.menuBOnly.Checked = true;
			this.menuBOnly.CheckState = System.Windows.Forms.CheckState.Checked;
			this.menuBOnly.Name = "menuBOnly";
			this.menuBOnly.Size = new System.Drawing.Size(152, 22);
			this.menuBOnly.Tag = "BOnly";
			this.menuBOnly.Text = "&B Only";
			this.menuBOnly.Click += new System.EventHandler(this.menuViewItem_Click);
			// 
			// ListForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(903, 275);
			this.Controls.Add(this.theList);
			this.Controls.Add(this.menuStripMain);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MainMenuStrip = this.menuStripMain;
			this.Name = "ListForm";
			this.Text = "CompareForm";
			this.Load += new System.EventHandler(this.CompareForm_Load);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ListForm_KeyPress);
			this.menuStripMain.ResumeLayout(false);
			this.menuStripMain.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView theList;
		private System.Windows.Forms.MenuStrip menuStripMain;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem menuCopy;
		private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem menuDifferent;
		private System.Windows.Forms.ToolStripMenuItem menuSame;
		private System.Windows.Forms.ToolStripMenuItem menuAOnly;
		private System.Windows.Forms.ToolStripMenuItem menuBOnly;
	}
}