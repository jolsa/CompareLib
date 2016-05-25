namespace ComparerLib
{
	partial class ViewForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTabs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTab2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTab4 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTab8 = new System.Windows.Forms.ToolStripMenuItem();
            this.textView = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOptions});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1309, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuOptions
            // 
            this.menuOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuTabs});
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.Size = new System.Drawing.Size(61, 20);
            this.menuOptions.Text = "&Options";
            // 
            // menuTabs
            // 
            this.menuTabs.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuTab2,
            this.menuTab4,
            this.menuTab8});
            this.menuTabs.Name = "menuTabs";
            this.menuTabs.Size = new System.Drawing.Size(99, 22);
            this.menuTabs.Text = "&Tabs";
            // 
            // menuTab2
            // 
            this.menuTab2.Name = "menuTab2";
            this.menuTab2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
            this.menuTab2.Size = new System.Drawing.Size(120, 22);
            this.menuTab2.Tag = "2";
            this.menuTab2.Text = "&2";
            this.menuTab2.Click += new System.EventHandler(this.TabChange_Click);
            // 
            // menuTab4
            // 
            this.menuTab4.Checked = true;
            this.menuTab4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuTab4.Name = "menuTab4";
            this.menuTab4.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D4)));
            this.menuTab4.Size = new System.Drawing.Size(120, 22);
            this.menuTab4.Tag = "4";
            this.menuTab4.Text = "&4";
            this.menuTab4.Click += new System.EventHandler(this.TabChange_Click);
            // 
            // menuTab8
            // 
            this.menuTab8.Name = "menuTab8";
            this.menuTab8.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D8)));
            this.menuTab8.Size = new System.Drawing.Size(120, 22);
            this.menuTab8.Tag = "8";
            this.menuTab8.Text = "&8";
            this.menuTab8.Click += new System.EventHandler(this.TabChange_Click);
            // 
            // textView
            // 
            this.textView.BackColor = System.Drawing.SystemColors.Window;
            this.textView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textView.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textView.Location = new System.Drawing.Point(0, 24);
            this.textView.MaxLength = 2147483647;
            this.textView.Multiline = true;
            this.textView.Name = "textView";
            this.textView.ReadOnly = true;
            this.textView.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textView.Size = new System.Drawing.Size(1309, 761);
            this.textView.TabIndex = 1;
            // 
            // ViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1309, 785);
            this.Controls.Add(this.textView);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MinimizeBox = false;
            this.Name = "ViewForm";
            this.ShowInTaskbar = false;
            this.Text = "ViewForm";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ViewForm_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ViewForm_KeyUp);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem menuOptions;
		private System.Windows.Forms.ToolStripMenuItem menuTabs;
		private System.Windows.Forms.ToolStripMenuItem menuTab2;
		private System.Windows.Forms.ToolStripMenuItem menuTab4;
		private System.Windows.Forms.ToolStripMenuItem menuTab8;
		private System.Windows.Forms.TextBox textView;
	}
}