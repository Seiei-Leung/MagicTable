namespace TemplateCustom
{
    partial class FrmSingle
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
            this.grid = new unvell.ReoGrid.ReoGridControl();
            this.SuspendLayout();
            // 
            // grid
            // 
            this.grid.BackColor = System.Drawing.Color.White;
            this.grid.ColumnHeaderContextMenuStrip = null;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.LeadHeaderContextMenuStrip = null;
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.Name = "grid";
            this.grid.RowHeaderContextMenuStrip = null;
            this.grid.Script = null;
            this.grid.SheetTabContextMenuStrip = null;
            this.grid.SheetTabNewButtonVisible = true;
            this.grid.SheetTabVisible = false;
            this.grid.SheetTabWidth = 146;
            this.grid.ShowScrollEndSpacing = true;
            this.grid.Size = new System.Drawing.Size(576, 416);
            this.grid.TabIndex = 3;
            this.grid.Text = "逻辑";
            // 
            // FrmSingle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 416);
            this.Controls.Add(this.grid);
            this.Name = "FrmSingle";
            this.Text = "FrmSingle";
            this.ResumeLayout(false);

        }

        #endregion

        private unvell.ReoGrid.ReoGridControl grid;


    }
}