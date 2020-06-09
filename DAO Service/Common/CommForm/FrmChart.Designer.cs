namespace Common.CommForm
{
    partial class FrmChart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmChart));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.imgTools = new DevExpress.Utils.ImageCollection(this.components);
            this.pnl = new DevExpress.XtraEditors.PanelControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.treeCategory = new DevExpress.XtraTreeList.TreeList();
            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.treeListColumn3 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.pnlTotal = new DevExpress.XtraEditors.PanelControl();
            this.gLeftTop = new DevExpress.XtraEditors.GroupControl();
            this.lbLeftUpDown = new DevExpress.XtraEditors.LabelControl();
            this.lblPLeftTopTotal = new DevExpress.XtraEditors.LabelControl();
            this.lblPRightTop = new DevExpress.XtraEditors.LabelControl();
            this.lblleftTopLeft = new DevExpress.XtraEditors.LabelControl();
            this.lblLeftTopQuery = new DevExpress.XtraEditors.LabelControl();
            this.gRightTop = new DevExpress.XtraEditors.GroupControl();
            this.lblPRightTopTotal = new DevExpress.XtraEditors.LabelControl();
            this.lblRighTopRight = new DevExpress.XtraEditors.LabelControl();
            this.lblRightTopQuery = new DevExpress.XtraEditors.LabelControl();
            this.gLeftButtom = new DevExpress.XtraEditors.GroupControl();
            this.lblPRightButtom = new DevExpress.XtraEditors.LabelControl();
            this.lblPLeftButtomTotal = new DevExpress.XtraEditors.LabelControl();
            this.lblLeftButtomButtom = new DevExpress.XtraEditors.LabelControl();
            this.lblLeftButtomQuery = new DevExpress.XtraEditors.LabelControl();
            this.gRightButtom = new DevExpress.XtraEditors.GroupControl();
            this.lblPRightButtomTotal = new DevExpress.XtraEditors.LabelControl();
            this.lblRightButtomRight = new DevExpress.XtraEditors.LabelControl();
            this.lblRightButtomQuery = new DevExpress.XtraEditors.LabelControl();
            this.contextTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TreeOption = new System.Windows.Forms.ToolStripMenuItem();
            this.comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgTools)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnl)).BeginInit();
            this.pnl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTotal)).BeginInit();
            this.pnlTotal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gLeftTop)).BeginInit();
            this.gLeftTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gRightTop)).BeginInit();
            this.gRightTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gLeftButtom)).BeginInit();
            this.gLeftButtom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gRightButtom)).BeginInit();
            this.gRightButtom.SuspendLayout();
            this.contextTree.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOrder});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(119, 26);
            // 
            // menuOrder
            // 
            this.menuOrder.Image = ((System.Drawing.Image)(resources.GetObject("menuOrder.Image")));
            this.menuOrder.Name = "menuOrder";
            this.menuOrder.Size = new System.Drawing.Size(118, 22);
            this.menuOrder.Text = "订阅图表";
            this.menuOrder.Click += new System.EventHandler(this.menuOrder_Click);
            // 
            // imgTools
            // 
            this.imgTools.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imgTools.ImageStream")));
            this.imgTools.Images.SetKeyName(0, "Right");
            this.imgTools.Images.SetKeyName(1, "Left");
            this.imgTools.Images.SetKeyName(2, "Down");
            this.imgTools.Images.SetKeyName(3, "UP");
            this.imgTools.Images.SetKeyName(4, "Total");
            this.imgTools.Images.SetKeyName(5, "柱形图");
            this.imgTools.Images.SetKeyName(6, "曲线");
            this.imgTools.Images.SetKeyName(7, "饼图");
            this.imgTools.Images.SetKeyName(8, "3D饼图");
            this.imgTools.Images.SetKeyName(9, "3D柱形图");
            this.imgTools.Images.SetKeyName(10, "Folder");
            this.imgTools.Images.SetKeyName(11, "CloseFolder");
            // 
            // pnl
            // 
            this.pnl.Controls.Add(this.splitContainerControl1);
            this.pnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl.Location = new System.Drawing.Point(0, 0);
            this.pnl.Name = "pnl";
            this.pnl.Size = new System.Drawing.Size(984, 574);
            this.pnl.TabIndex = 6;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.CollapsePanel = DevExpress.XtraEditors.SplitCollapsePanel.Panel1;
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(2, 2);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.treeCategory);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.pnlTotal);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(980, 570);
            this.splitContainerControl1.SplitterPosition = 202;
            this.splitContainerControl1.TabIndex = 9;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // treeCategory
            // 
            this.treeCategory.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.treeListColumn1,
            this.treeListColumn2,
            this.treeListColumn3});
            this.treeCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeCategory.Location = new System.Drawing.Point(0, 0);
            this.treeCategory.Name = "treeCategory";
            this.treeCategory.OptionsBehavior.Editable = false;
            this.treeCategory.OptionsPrint.UsePrintStyles = true;
            this.treeCategory.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.treeCategory.OptionsView.ShowIndicator = false;
            this.treeCategory.Size = new System.Drawing.Size(202, 570);
            this.treeCategory.StateImageList = this.imgTools;
            this.treeCategory.TabIndex = 0;
            // 
            // treeListColumn1
            // 
            this.treeListColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.treeListColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.treeListColumn1.Caption = "名称";
            this.treeListColumn1.FieldName = "CatName";
            this.treeListColumn1.MinWidth = 33;
            this.treeListColumn1.Name = "treeListColumn1";
            this.treeListColumn1.Visible = true;
            this.treeListColumn1.VisibleIndex = 0;
            this.treeListColumn1.Width = 92;
            // 
            // treeListColumn2
            // 
            this.treeListColumn2.Caption = "Guid";
            this.treeListColumn2.FieldName = "Guid";
            this.treeListColumn2.Name = "treeListColumn2";
            // 
            // treeListColumn3
            // 
            this.treeListColumn3.Caption = "ChartType";
            this.treeListColumn3.FieldName = "ChartType";
            this.treeListColumn3.Name = "treeListColumn3";
            // 
            // pnlTotal
            // 
            this.pnlTotal.ContextMenuStrip = this.contextMenuStrip1;
            this.pnlTotal.Controls.Add(this.comboBoxEdit1);
            this.pnlTotal.Controls.Add(this.gLeftTop);
            this.pnlTotal.Controls.Add(this.gRightTop);
            this.pnlTotal.Controls.Add(this.gLeftButtom);
            this.pnlTotal.Controls.Add(this.gRightButtom);
            this.pnlTotal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTotal.Location = new System.Drawing.Point(0, 0);
            this.pnlTotal.Name = "pnlTotal";
            this.pnlTotal.Size = new System.Drawing.Size(773, 570);
            this.pnlTotal.TabIndex = 9;
            // 
            // gLeftTop
            // 
            this.gLeftTop.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.gLeftTop.Controls.Add(this.lbLeftUpDown);
            this.gLeftTop.Controls.Add(this.lblPLeftTopTotal);
            this.gLeftTop.Controls.Add(this.lblPRightTop);
            this.gLeftTop.Controls.Add(this.lblleftTopLeft);
            this.gLeftTop.Controls.Add(this.lblLeftTopQuery);
            this.gLeftTop.Location = new System.Drawing.Point(28, 31);
            this.gLeftTop.Name = "gLeftTop";
            this.gLeftTop.Size = new System.Drawing.Size(315, 212);
            this.gLeftTop.TabIndex = 2;
            // 
            // lbLeftUpDown
            // 
            this.lbLeftUpDown.Location = new System.Drawing.Point(257, 2);
            this.lbLeftUpDown.Name = "lbLeftUpDown";
            this.lbLeftUpDown.Size = new System.Drawing.Size(12, 14);
            this.lbLeftUpDown.TabIndex = 5;
            this.lbLeftUpDown.Text = "   ";
            // 
            // lblPLeftTopTotal
            // 
            this.lblPLeftTopTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPLeftTopTotal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblPLeftTopTotal.Location = new System.Drawing.Point(236, 3);
            this.lblPLeftTopTotal.Name = "lblPLeftTopTotal";
            this.lblPLeftTopTotal.Size = new System.Drawing.Size(16, 14);
            this.lblPLeftTopTotal.TabIndex = 4;
            this.lblPLeftTopTotal.Text = "    ";
            this.lblPLeftTopTotal.ToolTip = "全屏显示";
            // 
            // lblPRightTop
            // 
            this.lblPRightTop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPRightTop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblPRightTop.Location = new System.Drawing.Point(264, 3);
            this.lblPRightTop.Name = "lblPRightTop";
            this.lblPRightTop.Size = new System.Drawing.Size(16, 14);
            this.lblPRightTop.TabIndex = 3;
            this.lblPRightTop.Text = "    ";
            // 
            // lblleftTopLeft
            // 
            this.lblleftTopLeft.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("lblleftTopLeft.Appearance.Image")));
            this.lblleftTopLeft.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftTop;
            this.lblleftTopLeft.Location = new System.Drawing.Point(3, -1);
            this.lblleftTopLeft.Name = "lblleftTopLeft";
            this.lblleftTopLeft.Size = new System.Drawing.Size(21, 20);
            this.lblleftTopLeft.TabIndex = 2;
            // 
            // lblLeftTopQuery
            // 
            this.lblLeftTopQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLeftTopQuery.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("lblLeftTopQuery.Appearance.Image")));
            this.lblLeftTopQuery.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblLeftTopQuery.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftTop;
            this.lblLeftTopQuery.Location = new System.Drawing.Point(289, 0);
            this.lblLeftTopQuery.Name = "lblLeftTopQuery";
            this.lblLeftTopQuery.Size = new System.Drawing.Size(21, 20);
            this.lblLeftTopQuery.TabIndex = 1;
            this.lblLeftTopQuery.ToolTip = "设置查询条件";
            // 
            // gRightTop
            // 
            this.gRightTop.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.gRightTop.Controls.Add(this.lblPRightTopTotal);
            this.gRightTop.Controls.Add(this.lblRighTopRight);
            this.gRightTop.Controls.Add(this.lblRightTopQuery);
            this.gRightTop.Location = new System.Drawing.Point(369, 20);
            this.gRightTop.Name = "gRightTop";
            this.gRightTop.Size = new System.Drawing.Size(366, 223);
            this.gRightTop.TabIndex = 3;
            // 
            // lblPRightTopTotal
            // 
            this.lblPRightTopTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPRightTopTotal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblPRightTopTotal.Location = new System.Drawing.Point(322, 4);
            this.lblPRightTopTotal.Name = "lblPRightTopTotal";
            this.lblPRightTopTotal.Size = new System.Drawing.Size(12, 14);
            this.lblPRightTopTotal.TabIndex = 4;
            this.lblPRightTopTotal.Text = "   ";
            this.lblPRightTopTotal.ToolTip = "全屏显示";
            // 
            // lblRighTopRight
            // 
            this.lblRighTopRight.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("lblRighTopRight.Appearance.Image")));
            this.lblRighTopRight.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftTop;
            this.lblRighTopRight.Location = new System.Drawing.Point(3, -1);
            this.lblRighTopRight.Name = "lblRighTopRight";
            this.lblRighTopRight.Size = new System.Drawing.Size(21, 20);
            this.lblRighTopRight.TabIndex = 3;
            // 
            // lblRightTopQuery
            // 
            this.lblRightTopQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRightTopQuery.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("lblRightTopQuery.Appearance.Image")));
            this.lblRightTopQuery.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblRightTopQuery.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftTop;
            this.lblRightTopQuery.Location = new System.Drawing.Point(340, 0);
            this.lblRightTopQuery.Name = "lblRightTopQuery";
            this.lblRightTopQuery.Size = new System.Drawing.Size(21, 20);
            this.lblRightTopQuery.TabIndex = 1;
            this.lblRightTopQuery.ToolTip = "设置查询条件";
            // 
            // gLeftButtom
            // 
            this.gLeftButtom.Controls.Add(this.lblPRightButtom);
            this.gLeftButtom.Controls.Add(this.lblPLeftButtomTotal);
            this.gLeftButtom.Controls.Add(this.lblLeftButtomButtom);
            this.gLeftButtom.Controls.Add(this.lblLeftButtomQuery);
            this.gLeftButtom.Location = new System.Drawing.Point(28, 287);
            this.gLeftButtom.Name = "gLeftButtom";
            this.gLeftButtom.Size = new System.Drawing.Size(322, 230);
            this.gLeftButtom.TabIndex = 4;
            // 
            // lblPRightButtom
            // 
            this.lblPRightButtom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPRightButtom.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblPRightButtom.Location = new System.Drawing.Point(271, 3);
            this.lblPRightButtom.Name = "lblPRightButtom";
            this.lblPRightButtom.Size = new System.Drawing.Size(16, 14);
            this.lblPRightButtom.TabIndex = 5;
            this.lblPRightButtom.Text = "    ";
            // 
            // lblPLeftButtomTotal
            // 
            this.lblPLeftButtomTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPLeftButtomTotal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblPLeftButtomTotal.Location = new System.Drawing.Point(243, 3);
            this.lblPLeftButtomTotal.Name = "lblPLeftButtomTotal";
            this.lblPLeftButtomTotal.Size = new System.Drawing.Size(16, 14);
            this.lblPLeftButtomTotal.TabIndex = 4;
            this.lblPLeftButtomTotal.Text = "    ";
            // 
            // lblLeftButtomButtom
            // 
            this.lblLeftButtomButtom.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("lblLeftButtomButtom.Appearance.Image")));
            this.lblLeftButtomButtom.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftTop;
            this.lblLeftButtomButtom.Location = new System.Drawing.Point(3, -1);
            this.lblLeftButtomButtom.Name = "lblLeftButtomButtom";
            this.lblLeftButtomButtom.Size = new System.Drawing.Size(21, 20);
            this.lblLeftButtomButtom.TabIndex = 3;
            // 
            // lblLeftButtomQuery
            // 
            this.lblLeftButtomQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLeftButtomQuery.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("lblLeftButtomQuery.Appearance.Image")));
            this.lblLeftButtomQuery.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblLeftButtomQuery.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftTop;
            this.lblLeftButtomQuery.Location = new System.Drawing.Point(296, 0);
            this.lblLeftButtomQuery.Name = "lblLeftButtomQuery";
            this.lblLeftButtomQuery.Size = new System.Drawing.Size(21, 20);
            this.lblLeftButtomQuery.TabIndex = 0;
            this.lblLeftButtomQuery.ToolTip = "设置查询条件";
            // 
            // gRightButtom
            // 
            this.gRightButtom.Controls.Add(this.lblPRightButtomTotal);
            this.gRightButtom.Controls.Add(this.lblRightButtomRight);
            this.gRightButtom.Controls.Add(this.lblRightButtomQuery);
            this.gRightButtom.Location = new System.Drawing.Point(440, 274);
            this.gRightButtom.Name = "gRightButtom";
            this.gRightButtom.Size = new System.Drawing.Size(316, 232);
            this.gRightButtom.TabIndex = 5;
            // 
            // lblPRightButtomTotal
            // 
            this.lblPRightButtomTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPRightButtomTotal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblPRightButtomTotal.Location = new System.Drawing.Point(268, 3);
            this.lblPRightButtomTotal.Name = "lblPRightButtomTotal";
            this.lblPRightButtomTotal.Size = new System.Drawing.Size(16, 14);
            this.lblPRightButtomTotal.TabIndex = 4;
            this.lblPRightButtomTotal.Text = "    ";
            // 
            // lblRightButtomRight
            // 
            this.lblRightButtomRight.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("lblRightButtomRight.Appearance.Image")));
            this.lblRightButtomRight.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftTop;
            this.lblRightButtomRight.Location = new System.Drawing.Point(3, -1);
            this.lblRightButtomRight.Name = "lblRightButtomRight";
            this.lblRightButtomRight.Size = new System.Drawing.Size(21, 20);
            this.lblRightButtomRight.TabIndex = 3;
            // 
            // lblRightButtomQuery
            // 
            this.lblRightButtomQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRightButtomQuery.Appearance.Image = ((System.Drawing.Image)(resources.GetObject("lblRightButtomQuery.Appearance.Image")));
            this.lblRightButtomQuery.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblRightButtomQuery.ImageAlignToText = DevExpress.XtraEditors.ImageAlignToText.LeftTop;
            this.lblRightButtomQuery.Location = new System.Drawing.Point(290, 0);
            this.lblRightButtomQuery.Name = "lblRightButtomQuery";
            this.lblRightButtomQuery.Size = new System.Drawing.Size(21, 20);
            this.lblRightButtomQuery.TabIndex = 0;
            this.lblRightButtomQuery.ToolTip = "设置查询条件";
            // 
            // contextTree
            // 
            this.contextTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TreeOption});
            this.contextTree.Name = "contextTree";
            this.contextTree.Size = new System.Drawing.Size(119, 26);
            // 
            // TreeOption
            // 
            this.TreeOption.Image = ((System.Drawing.Image)(resources.GetObject("TreeOption.Image")));
            this.TreeOption.Name = "TreeOption";
            this.TreeOption.Size = new System.Drawing.Size(152, 22);
            this.TreeOption.Text = "数据配置";
            this.TreeOption.Click += new System.EventHandler(this.TreeOption_Click);
            // 
            // comboBoxEdit1
            // 
            this.comboBoxEdit1.Location = new System.Drawing.Point(228, 261);
            this.comboBoxEdit1.Name = "comboBoxEdit1";
            this.comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEdit1.Properties.Items.AddRange(new object[] {
            "aaa;bbb;ccc"});
            this.comboBoxEdit1.Size = new System.Drawing.Size(100, 20);
            this.comboBoxEdit1.TabIndex = 6;
            // 
            // FrmChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 574);
            this.Controls.Add(this.pnl);
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmChart";
            this.Text = "数据魔方";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.SizeChanged += new System.EventHandler(this.FrmChart_SizeChanged);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imgTools)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnl)).EndInit();
            this.pnl.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTotal)).EndInit();
            this.pnlTotal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gLeftTop)).EndInit();
            this.gLeftTop.ResumeLayout(false);
            this.gLeftTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gRightTop)).EndInit();
            this.gRightTop.ResumeLayout(false);
            this.gRightTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gLeftButtom)).EndInit();
            this.gLeftButtom.ResumeLayout(false);
            this.gLeftButtom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gRightButtom)).EndInit();
            this.gRightButtom.ResumeLayout(false);
            this.gRightButtom.PerformLayout();
            this.contextTree.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuOrder;
        private DevExpress.XtraEditors.PanelControl pnl;
        private DevExpress.Utils.ImageCollection imgTools;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraTreeList.TreeList treeCategory;
        private DevExpress.XtraEditors.PanelControl pnlTotal;
        private DevExpress.XtraEditors.GroupControl gLeftTop;
        private DevExpress.XtraEditors.LabelControl lbLeftUpDown;
        private DevExpress.XtraEditors.LabelControl lblPLeftTopTotal;
        private DevExpress.XtraEditors.LabelControl lblPRightTop;
        private DevExpress.XtraEditors.LabelControl lblleftTopLeft;
        private DevExpress.XtraEditors.LabelControl lblLeftTopQuery;
        private DevExpress.XtraEditors.GroupControl gRightTop;
        private DevExpress.XtraEditors.LabelControl lblPRightTopTotal;
        private DevExpress.XtraEditors.LabelControl lblRighTopRight;
        private DevExpress.XtraEditors.LabelControl lblRightTopQuery;
        private DevExpress.XtraEditors.GroupControl gLeftButtom;
        private DevExpress.XtraEditors.LabelControl lblPRightButtom;
        private DevExpress.XtraEditors.LabelControl lblPLeftButtomTotal;
        private DevExpress.XtraEditors.LabelControl lblLeftButtomButtom;
        private DevExpress.XtraEditors.LabelControl lblLeftButtomQuery;
        private DevExpress.XtraEditors.GroupControl gRightButtom;
        private DevExpress.XtraEditors.LabelControl lblPRightButtomTotal;
        private DevExpress.XtraEditors.LabelControl lblRightButtomRight;
        private DevExpress.XtraEditors.LabelControl lblRightButtomQuery;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn3;
        private System.Windows.Forms.ContextMenuStrip contextTree;
        private System.Windows.Forms.ToolStripMenuItem TreeOption;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;

    }
}