namespace Common
{
    partial class FrmChartRoleRight
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmChartRoleRight));
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.gcRole = new TWControls.MyGridControl();
            this.gvRole = new TWControls.MyGridView();
            this.colCodeLL = new TWControls.MyGridColumn();
            this.colNameLL = new TWControls.MyGridColumn();
            this.myGridColumn1 = new TWControls.MyGridColumn();
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
            ((System.ComponentModel.ISupportInitialize)(this.gcRole)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRole)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repSelect)).BeginInit();
            this.SuspendLayout();
            // 
            // myControl
            // 
            this.myControl.ExpandCollapseItem.Id = 0;
            this.myControl.ExpandCollapseItem.Name = "";
            this.myControl.Size = new System.Drawing.Size(814, 84);
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
            this.splitContainerControl1.Panel1.Controls.Add(this.gcRole);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.gridControl1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(814, 404);
            this.splitContainerControl1.SplitterPosition = 231;
            this.splitContainerControl1.TabIndex = 2;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // gcRole
            // 
            this.gcRole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcRole.Location = new System.Drawing.Point(0, 0);
            this.gcRole.MainView = this.gvRole;
            this.gcRole.Name = "gcRole";
            this.gcRole.Size = new System.Drawing.Size(231, 404);
            this.gcRole.TabIndex = 20;
            this.gcRole.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvRole});
            // 
            // gvRole
            // 
            this.gvRole.Appearance.FocusedRow.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gvRole.Appearance.FocusedRow.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gvRole.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Blue;
            this.gvRole.Appearance.FocusedRow.Options.UseBackColor = true;
            this.gvRole.Appearance.FocusedRow.Options.UseFont = true;
            this.gvRole.Appearance.FocusedRow.Options.UseForeColor = true;
            this.gvRole.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gvRole.Appearance.HideSelectionRow.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gvRole.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.Blue;
            this.gvRole.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.gvRole.Appearance.HideSelectionRow.Options.UseFont = true;
            this.gvRole.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.gvRole.Appearance.SelectedRow.BackColor = System.Drawing.Color.MistyRose;
            this.gvRole.Appearance.SelectedRow.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gvRole.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Blue;
            this.gvRole.Appearance.SelectedRow.Options.UseBackColor = true;
            this.gvRole.Appearance.SelectedRow.Options.UseFont = true;
            this.gvRole.Appearance.SelectedRow.Options.UseForeColor = true;
            this.gvRole.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colCodeLL,
            this.colNameLL,
            this.myGridColumn1});
            this.gvRole.GetDataRowInfo = null;
            this.gvRole.GetDataRowList = null;
            this.gvRole.GetList = null;
            this.gvRole.GridControl = this.gcRole;
            this.gvRole.Name = "gvRole";
            this.gvRole.OptionsBehavior.Editable = false;
            this.gvRole.OptionsView.ShowAutoFilterRow = true;
            this.gvRole.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.ShowAlways;
            this.gvRole.OptionsView.ShowGroupPanel = false;
            this.gvRole.OptionsView.ShowViewCaption = true;
            this.gvRole.ViewCaption = "用户列表";
            this.gvRole.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.gvRole_RowCellClick);
            // 
            // colCodeLL
            // 
            this.colCodeLL.Caption = "编码";
            this.colCodeLL.FieldName = "Code";
            this.colCodeLL.Name = "colCodeLL";
            this.colCodeLL.UseAdvancedFiltering = true;
            this.colCodeLL.Visible = true;
            this.colCodeLL.VisibleIndex = 0;
            this.colCodeLL.Width = 85;
            // 
            // colNameLL
            // 
            this.colNameLL.Caption = "角色";
            this.colNameLL.FieldName = "Name";
            this.colNameLL.Name = "colNameLL";
            this.colNameLL.UseAdvancedFiltering = true;
            this.colNameLL.Visible = true;
            this.colNameLL.VisibleIndex = 1;
            this.colNameLL.Width = 128;
            // 
            // myGridColumn1
            // 
            this.myGridColumn1.Caption = "Guid";
            this.myGridColumn1.FieldName = "Guid";
            this.myGridColumn1.Name = "myGridColumn1";
            this.myGridColumn1.UseAdvancedFiltering = true;
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
            this.gridControl1.Size = new System.Drawing.Size(578, 404);
            this.gridControl1.TabIndex = 3;
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
            // FrmChartRoleRight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.ClientSize = new System.Drawing.Size(814, 488);
            this.Controls.Add(this.splitContainerControl1);
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "FrmChartRoleRight";
            this.Text = "魔方角色权限设置";
            this.Controls.SetChildIndex(this.myControl, 0);
            this.Controls.SetChildIndex(this.splitContainerControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.myControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcRole)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvRole)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repSelect)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private TWControls.MyGridControl gcRole;
        private TWControls.MyGridView gvRole;
        private TWControls.MyGridColumn colCodeLL;
        private TWControls.MyGridColumn colNameLL;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repSelect;
        private TWControls.MyGridColumn myGridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
    }
}
