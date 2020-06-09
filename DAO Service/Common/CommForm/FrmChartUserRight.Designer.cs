namespace Common
{
    partial class FrmChartUserRight
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmChartUserRight));
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.splitContainerControl2 = new DevExpress.XtraEditors.SplitContainerControl();
            this.treeListGroup = new DevExpress.XtraTreeList.TreeList();
            this.colGroup = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.gridUser = new TWControls.MyGridControl();
            this.gridView2 = new TWControls.MyGridView();
            this.colCodeLL = new TWControls.MyGridColumn();
            this.colNameLL = new TWControls.MyGridColumn();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repSelect = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.myControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).BeginInit();
            this.splitContainerControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repSelect)).BeginInit();
            this.SuspendLayout();
            // 
            // myControl
            // 
            this.myControl.ExpandCollapseItem.Id = 0;
            this.myControl.ExpandCollapseItem.Name = "";
            this.myControl.Size = new System.Drawing.Size(905, 84);
            // 
            // imgAccessory
            // 
            this.imgAccessory.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgAccessory.ImageStream")));
            this.imgAccessory.Images.SetKeyName(0, "bmp.png");
            this.imgAccessory.Images.SetKeyName(1, "excel.png");
            this.imgAccessory.Images.SetKeyName(2, "ico.png");
            this.imgAccessory.Images.SetKeyName(3, "jpg.png");
            this.imgAccessory.Images.SetKeyName(4, "png.png");
            this.imgAccessory.Images.SetKeyName(5, "rar.png");
            this.imgAccessory.Images.SetKeyName(6, "word.png");
            this.imgAccessory.Images.SetKeyName(7, "txt.png");
            this.imgAccessory.Images.SetKeyName(8, "ai.png");
            this.imgAccessory.Images.SetKeyName(9, "dll.png");
            this.imgAccessory.Images.SetKeyName(10, "ini.png");
            this.imgAccessory.Images.SetKeyName(11, "tif.png");
            this.imgAccessory.Images.SetKeyName(12, "空白.png");
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.CollapsePanel = DevExpress.XtraEditors.SplitCollapsePanel.Panel1;
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 84);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.splitContainerControl2);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.gridControl1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(905, 525);
            this.splitContainerControl1.SplitterPosition = 349;
            this.splitContainerControl1.TabIndex = 1;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // splitContainerControl2
            // 
            this.splitContainerControl2.CollapsePanel = DevExpress.XtraEditors.SplitCollapsePanel.Panel1;
            this.splitContainerControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl2.Horizontal = false;
            this.splitContainerControl2.Location = new System.Drawing.Point(0, 0);
            this.splitContainerControl2.Name = "splitContainerControl2";
            this.splitContainerControl2.Panel1.Controls.Add(this.treeListGroup);
            this.splitContainerControl2.Panel1.Text = "Panel1";
            this.splitContainerControl2.Panel2.Controls.Add(this.gridUser);
            this.splitContainerControl2.Panel2.Text = "Panel2";
            this.splitContainerControl2.Size = new System.Drawing.Size(349, 525);
            this.splitContainerControl2.SplitterPosition = 263;
            this.splitContainerControl2.TabIndex = 0;
            this.splitContainerControl2.Text = "splitContainerControl2";
            // 
            // treeListGroup
            // 
            this.treeListGroup.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.treeListGroup.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colGroup});
            this.treeListGroup.Dock = System.Windows.Forms.DockStyle.Top;
            this.treeListGroup.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeListGroup.KeyFieldName = "TreeCode";
            this.treeListGroup.Location = new System.Drawing.Point(0, 0);
            this.treeListGroup.Name = "treeListGroup";
            this.treeListGroup.OptionsBehavior.Editable = false;
            this.treeListGroup.OptionsPrint.PrintHorzLines = false;
            this.treeListGroup.OptionsPrint.PrintVertLines = false;
            this.treeListGroup.OptionsPrint.UsePrintStyles = true;
            this.treeListGroup.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.treeListGroup.OptionsView.ShowFocusedFrame = false;
            this.treeListGroup.OptionsView.ShowHorzLines = false;
            this.treeListGroup.OptionsView.ShowIndicator = false;
            this.treeListGroup.OptionsView.ShowVertLines = false;
            this.treeListGroup.ParentFieldName = "PCode";
            this.treeListGroup.PreviewFieldName = "Name";
            this.treeListGroup.Size = new System.Drawing.Size(349, 235);
            this.treeListGroup.TabIndex = 18;
            this.treeListGroup.TreeLevelWidth = 14;
            this.treeListGroup.Click += new System.EventHandler(this.treeListGroup_Click);
            // 
            // colGroup
            // 
            this.colGroup.Caption = "用户组别";
            this.colGroup.FieldName = "Name";
            this.colGroup.MinWidth = 33;
            this.colGroup.Name = "colGroup";
            this.colGroup.Visible = true;
            this.colGroup.VisibleIndex = 0;
            // 
            // gridUser
            // 
            this.gridUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridUser.Location = new System.Drawing.Point(0, 0);
            this.gridUser.MainView = this.gridView2;
            this.gridUser.Name = "gridUser";
            this.gridUser.Size = new System.Drawing.Size(349, 257);
            this.gridUser.TabIndex = 20;
            this.gridUser.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView2});
            // 
            // gridView2
            // 
            this.gridView2.Appearance.FocusedRow.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView2.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gridView2.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Blue;
            this.gridView2.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gridView2.Appearance.FocusedRow.Options.UseFont = true;
            this.gridView2.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gridView2.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView2.Appearance.HideSelectionRow.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gridView2.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.Blue;
            this.gridView2.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gridView2.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gridView2.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gridView2.Appearance.SelectedRow.BackColor = System.Drawing.Color.MistyRose;
            this.gridView2.Appearance.SelectedRow.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gridView2.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Blue;
            this.gridView2.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gridView2.Appearance.SelectedRow.Options.UseFont = true;
            this.gridView2.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colCodeLL,
            this.colNameLL});
            this.gridView2.GetDataRowInfo = null;
            this.gridView2.GetDataRowList = null;
            this.gridView2.GetList = null;
            this.gridView2.GridControl = this.gridUser;
            this.gridView2.Name = "gridView2";
            this.gridView2.OptionsBehavior.Editable = false;
            this.gridView2.OptionsView.ShowAutoFilterRow = true;
            this.gridView2.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.ShowAlways;
            this.gridView2.OptionsView.ShowGroupPanel = false;
            this.gridView2.OptionsView.ShowViewCaption = true;
            this.gridView2.ViewCaption = "用户列表";
            this.gridView2.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gridView2_RowCellClick);
            // 
            // colCodeLL
            // 
            this.colCodeLL.Caption = "编码";
            this.colCodeLL.FieldName = "Code";
            this.colCodeLL.Name = "colCodeLL";
            this.colCodeLL.UseAdvancedFiltering = true;
            this.colCodeLL.Visible = true;
            this.colCodeLL.VisibleIndex = 0;
            // 
            // colNameLL
            // 
            this.colNameLL.Caption = "姓名";
            this.colNameLL.FieldName = "Name";
            this.colNameLL.Name = "colNameLL";
            this.colNameLL.UseAdvancedFiltering = true;
            this.colNameLL.Visible = true;
            this.colNameLL.VisibleIndex = 1;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.MenuManager = this.myControl;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repSelect});
            this.gridControl1.Size = new System.Drawing.Size(551, 525);
            this.gridControl1.TabIndex = 4;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn4,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn5,
            this.gridColumn6});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = " ";
            this.gridColumn4.ColumnEdit = this.repSelect;
            this.gridColumn4.FieldName = "Sel";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 0;
            this.gridColumn4.Width = 35;
            // 
            // repSelect
            // 
            this.repSelect.AutoHeight = false;
            this.repSelect.Name = "repSelect";
            this.repSelect.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "标题";
            this.gridColumn1.FieldName = "ChartTitle";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 1;
            this.gridColumn1.Width = 179;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "建档人";
            this.gridColumn2.FieldName = "Buser";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            this.gridColumn2.Width = 88;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "时间";
            this.gridColumn3.DisplayFormat.FormatString = "{0:yyyy-MM-dd}";
            this.gridColumn3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.gridColumn3.FieldName = "Btime";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 3;
            this.gridColumn3.Width = 92;
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "一级分类";
            this.gridColumn5.FieldName = "Category";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "二级分类";
            this.gridColumn6.FieldName = "CategorySub";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 5;
            // 
            // FrmChartUserRight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 609);
            this.Controls.Add(this.splitContainerControl1);
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "FrmChartUserRight";
            this.Text = "数据魔方用户权限设置";
            this.Controls.SetChildIndex(this.myControl, 0);
            this.Controls.SetChildIndex(this.splitContainerControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.myControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl2)).EndInit();
            this.splitContainerControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeListGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repSelect)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl2;
        public DevExpress.XtraTreeList.TreeList treeListGroup;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colGroup;
        private TWControls.MyGridControl gridUser;
        private TWControls.MyGridView gridView2;
        private TWControls.MyGridColumn colCodeLL;
        private TWControls.MyGridColumn colNameLL;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repSelect;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;

    }
}