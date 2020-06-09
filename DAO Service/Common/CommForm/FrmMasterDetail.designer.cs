namespace Common
{
    partial class FrmMasterDetail
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMasterDetail));
            this.imageCollection2 = new DevExpress.Utils.ImageCollection(this.components);
            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage3 = new DevExpress.XtraTab.XtraTabPage();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.panelFoot = new DevExpress.XtraEditors.PanelControl();
            this.labApproveDate = new System.Windows.Forms.Label();
            this.labApprover = new System.Windows.Forms.Label();
            this.labCheckDate = new System.Windows.Forms.Label();
            this.labChecker = new System.Windows.Forms.Label();
            this.dataSet1 = new System.Data.DataSet();
            this.panelMaster = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.panelFoot.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            this.SuspendLayout();
            // 
            // imageCollection2
            // 
            this.imageCollection2.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection2.ImageStream")));
            this.imageCollection2.Images.SetKeyName(29, "SaveAs_16x16.png");
            // 
            // imageCollection1
            // 
            this.imageCollection1.ImageSize = new System.Drawing.Size(32, 32);
            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
            this.imageCollection1.Images.SetKeyName(10, "Checkmark.png");
            this.imageCollection1.Images.SetKeyName(11, "Chart_Graph_Ascending.png");
            this.imageCollection1.Images.SetKeyName(12, "File_Delete.png");
            this.imageCollection1.Images.SetKeyName(13, "Blocknotes_Blue.png");
            this.imageCollection1.Images.SetKeyName(14, "Error_Symbol.png");
            this.imageCollection1.Images.SetKeyName(15, "File_Checked.png");
            this.imageCollection1.Images.SetKeyName(16, "Error.png");
            this.imageCollection1.Images.SetKeyName(17, "Folder_Remove.png");
            this.imageCollection1.Images.SetKeyName(18, "Folder_Checked.png");
            this.imageCollection1.Images.SetKeyName(19, "Comment_Reply.png");
            this.imageCollection1.Images.SetKeyName(20, "Comment_Add.png");
            this.imageCollection1.Images.SetKeyName(21, "save.png");
            this.imageCollection1.Images.SetKeyName(22, "save_add2.png");
            this.imageCollection1.Images.SetKeyName(23, "save_delete.png");
            this.imageCollection1.Images.SetKeyName(24, "document_inspector.png");
            this.imageCollection1.Images.SetKeyName(25, "printer.png");
            this.imageCollection1.Images.SetKeyName(26, "document-print-preview.png");
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 0);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage3;
            this.xtraTabControl1.Size = new System.Drawing.Size(1082, 409);
            this.xtraTabControl1.TabIndex = 3;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage1,
            this.xtraTabPage2,
            this.xtraTabPage3});
            // 
            // xtraTabPage3
            // 
            this.xtraTabPage3.Name = "xtraTabPage3";
            this.xtraTabPage3.Size = new System.Drawing.Size(1076, 380);
            this.xtraTabPage3.Text = "xtraTabPage3";
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.gridControl1);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(1076, 380);
            this.xtraTabPage1.Text = "xtraTabPage1";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.ShowOnlyPredefinedDetails = true;
            this.gridControl1.Size = new System.Drawing.Size(1076, 380);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ShowFooter = true;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(1076, 380);
            this.xtraTabPage2.Text = "xtraTabPage2";
            // 
            // panelFoot
            // 
            this.panelFoot.BackColor = System.Drawing.SystemColors.Window;
            this.panelFoot.Controls.Add(this.labApproveDate);
            this.panelFoot.Controls.Add(this.labApprover);
            this.panelFoot.Controls.Add(this.labCheckDate);
            this.panelFoot.Controls.Add(this.labChecker);
            this.panelFoot.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFoot.Location = new System.Drawing.Point(0, 364);
            this.panelFoot.Name = "panelFoot";
            this.panelFoot.Size = new System.Drawing.Size(1082, 45);
            this.panelFoot.TabIndex = 4;
            // 
            // labApproveDate
            // 
            this.labApproveDate.AutoSize = true;
            this.labApproveDate.Location = new System.Drawing.Point(669, 15);
            this.labApproveDate.Name = "labApproveDate";
            this.labApproveDate.Size = new System.Drawing.Size(65, 12);
            this.labApproveDate.TabIndex = 3;
            this.labApproveDate.Text = "审批日期：";
            // 
            // labApprover
            // 
            this.labApprover.AutoSize = true;
            this.labApprover.Location = new System.Drawing.Point(429, 15);
            this.labApprover.Name = "labApprover";
            this.labApprover.Size = new System.Drawing.Size(77, 12);
            this.labApprover.TabIndex = 2;
            this.labApprover.Text = "审  批  人：";
            // 
            // labCheckDate
            // 
            this.labCheckDate.AutoSize = true;
            this.labCheckDate.Location = new System.Drawing.Point(213, 15);
            this.labCheckDate.Name = "labCheckDate";
            this.labCheckDate.Size = new System.Drawing.Size(65, 12);
            this.labCheckDate.TabIndex = 1;
            this.labCheckDate.Text = "审核日期：";
            // 
            // labChecker
            // 
            this.labChecker.AutoSize = true;
            this.labChecker.Location = new System.Drawing.Point(12, 15);
            this.labChecker.Name = "labChecker";
            this.labChecker.Size = new System.Drawing.Size(77, 12);
            this.labChecker.TabIndex = 0;
            this.labChecker.Text = "审  核  人：";
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "NewDataSet";
            // 
            // panelMaster
            // 
            this.panelMaster.BackColor = System.Drawing.SystemColors.Window;
            this.panelMaster.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMaster.Location = new System.Drawing.Point(0, 0);
            this.panelMaster.Name = "panelMaster";
            this.panelMaster.Size = new System.Drawing.Size(1082, 125);
            this.panelMaster.TabIndex = 5;
            // 
            // FrmMasterDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1082, 409);
            this.Controls.Add(this.panelMaster);
            this.Controls.Add(this.panelFoot);
            this.Controls.Add(this.xtraTabControl1);
            this.Name = "FrmMasterDetail";
            this.Text = "FormMasterDetail";
            this.Load += new System.EventHandler(this.FormMasterDetail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.panelFoot.ResumeLayout(false);
            this.panelFoot.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.ImageCollection imageCollection2;
        private DevExpress.Utils.ImageCollection imageCollection1;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage3;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.PanelControl panelFoot;
        private System.Windows.Forms.Label labApproveDate;
        private System.Windows.Forms.Label labApprover;
        private System.Windows.Forms.Label labCheckDate;
        private System.Windows.Forms.Label labChecker;
        private System.Data.DataSet dataSet1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        public DevExpress.XtraEditors.PanelControl panelMaster;
    }
}