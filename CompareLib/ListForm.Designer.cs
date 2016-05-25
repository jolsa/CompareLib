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
            this.SuspendLayout();
            // 
            // theList
            // 
            this.theList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.theList.FullRowSelect = true;
            this.theList.HideSelection = false;
            this.theList.Location = new System.Drawing.Point(0, 0);
            this.theList.MultiSelect = false;
            this.theList.Name = "theList";
            this.theList.Size = new System.Drawing.Size(903, 275);
            this.theList.TabIndex = 0;
            this.theList.UseCompatibleStateImageBehavior = false;
            this.theList.View = System.Windows.Forms.View.Details;
            this.theList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.theList_MouseDown);
            this.theList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.theList_MouseMove);
            // 
            // ListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(903, 275);
            this.Controls.Add(this.theList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "ListForm";
            this.Text = "CompareForm";
            this.Load += new System.EventHandler(this.CompareForm_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ListForm_KeyPress);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView theList;
	}
}