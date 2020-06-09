namespace Common.CommForm
{
    partial class FrmChartQueryAndSum
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
            this.repChk = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repSumType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOptionOK = new DevExpress.XtraEditors.SimpleButton();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.xtraTabPage1 = new DevExpress.XtraTab.XtraTabPage();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repGroupBy = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repSumType2 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repIsShow = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.xtraTabPage2 = new DevExpress.XtraTab.XtraTabPage();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.gcQuery = new DevExpress.XtraGrid.GridControl();
            this.gvQuery = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repCompare = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControl5 = new DevExpress.XtraEditors.PanelControl();
            this.btnGroupClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnQueryOK = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.toolTipController1 = new DevExpress.Utils.ToolTipController(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.repChk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repSumType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.xtraTabControl1.SuspendLayout();
            this.xtraTabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repGroupBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repSumType2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repIsShow)).BeginInit();
            this.xtraTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            this.panelControl3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gcQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCompare)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).BeginInit();
            this.panelControl5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.SuspendLayout();
            // 
            // repChk
            // 
            this.repChk.AutoHeight = false;
            this.repChk.Name = "repChk";
            this.repChk.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // repSumType
            // 
            this.repSumType.AutoHeight = false;
            this.repSumType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repSumType.Items.AddRange(new object[] {
            "求和",
            "平均值",
            "最小值",
            "最大值",
            "求和"});
            this.repSumType.Name = "repSumType";
            this.repSumType.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnCancel);
            this.panelControl1.Controls.Add(this.btnOptionOK);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(2, 257);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(442, 39);
            this.panelControl1.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(228, 9);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 20);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOptionOK
            // 
            this.btnOptionOK.Location = new System.Drawing.Point(108, 9);
            this.btnOptionOK.Name = "btnOptionOK";
            this.btnOptionOK.Size = new System.Drawing.Size(63, 20);
            this.btnOptionOK.TabIndex = 0;
            this.btnOptionOK.Text = "确认";
            this.btnOptionOK.Click += new System.EventHandler(this.btnOptionOK_Click);
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Location = new System.Drawing.Point(5, 5);
            this.xtraTabControl1.Margin = new System.Windows.Forms.Padding(20);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.SelectedTabPage = this.xtraTabPage1;
            this.xtraTabControl1.Size = new System.Drawing.Size(452, 327);
            this.xtraTabControl1.TabIndex = 2;
            this.xtraTabControl1.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.xtraTabPage2,
            this.xtraTabPage1});
            // 
            // xtraTabPage1
            // 
            this.xtraTabPage1.Controls.Add(this.panelControl2);
            this.xtraTabPage1.Name = "xtraTabPage1";
            this.xtraTabPage1.Size = new System.Drawing.Size(446, 298);
            this.xtraTabPage1.Text = "分组条件";
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.gridControl1);
            this.panelControl2.Controls.Add(this.panelControl1);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 0);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(446, 298);
            this.panelControl2.TabIndex = 0;
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(2, 2);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repSumType2,
            this.repGroupBy,
            this.repIsShow});
            this.gridControl1.Size = new System.Drawing.Size(442, 255);
            this.gridControl1.TabIndex = 1;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsCustomization.AllowColumnMoving = false;
            this.gridView1.OptionsCustomization.AllowFilter = false;
            this.gridView1.OptionsCustomization.AllowGroup = false;
            this.gridView1.OptionsCustomization.AllowQuickHideColumns = false;
            this.gridView1.OptionsCustomization.AllowSort = false;
            this.gridView1.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridView1.OptionsFilter.AllowFilterEditor = false;
            this.gridView1.OptionsFilter.AllowFilterIncrementalSearch = false;
            this.gridView1.OptionsFilter.AllowMRUFilterList = false;
            this.gridView1.OptionsFilter.AllowMultiSelectInCheckedFilterPopup = false;
            this.gridView1.OptionsFind.AllowFindPanel = false;
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsMenu.EnableFooterMenu = false;
            this.gridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView1.OptionsView.ShowDetailButtons = false;
            this.gridView1.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            this.gridView1.OptionsView.ShowIndicator = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn1.Caption = "字段";
            this.gridColumn1.FieldName = "FieldName";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 260;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "是否分组";
            this.gridColumn2.ColumnEdit = this.repGroupBy;
            this.gridColumn2.FieldName = "GroupBy";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 71;
            // 
            // repGroupBy
            // 
            this.repGroupBy.AutoHeight = false;
            this.repGroupBy.Name = "repGroupBy";
            this.repGroupBy.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "统计类别";
            this.gridColumn3.ColumnEdit = this.repSumType2;
            this.gridColumn3.FieldName = "SumType";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 106;
            // 
            // repSumType2
            // 
            this.repSumType2.AutoHeight = false;
            this.repSumType2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repSumType2.Items.AddRange(new object[] {
            "求和",
            "最大值",
            "最小值",
            "平均值",
            "计数",
            ""});
            this.repSumType2.Name = "repSumType2";
            this.repSumType2.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "是否显示";
            this.gridColumn4.ColumnEdit = this.repIsShow;
            this.gridColumn4.FieldName = "IsShow";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Width = 78;
            // 
            // repIsShow
            // 
            this.repIsShow.AutoHeight = false;
            this.repIsShow.Name = "repIsShow";
            this.repIsShow.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // xtraTabPage2
            // 
            this.xtraTabPage2.Controls.Add(this.panelControl3);
            this.xtraTabPage2.Name = "xtraTabPage2";
            this.xtraTabPage2.Size = new System.Drawing.Size(446, 298);
            this.xtraTabPage2.Text = "过滤条件";
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.gcQuery);
            this.panelControl3.Controls.Add(this.panelControl5);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(0, 0);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(446, 298);
            this.panelControl3.TabIndex = 0;
            // 
            // gcQuery
            // 
            this.gcQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gcQuery.Location = new System.Drawing.Point(2, 2);
            this.gcQuery.MainView = this.gvQuery;
            this.gcQuery.Name = "gcQuery";
            this.gcQuery.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repCompare});
            this.gcQuery.Size = new System.Drawing.Size(442, 258);
            this.gcQuery.TabIndex = 7;
            this.gcQuery.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvQuery});
            // 
            // gvQuery
            // 
            this.gvQuery.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11});
            this.gvQuery.GridControl = this.gcQuery;
            this.gvQuery.Name = "gvQuery";
            this.gvQuery.OptionsCustomization.AllowColumnMoving = false;
            this.gvQuery.OptionsCustomization.AllowFilter = false;
            this.gvQuery.OptionsCustomization.AllowGroup = false;
            this.gvQuery.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvQuery.OptionsCustomization.AllowRowSizing = true;
            this.gvQuery.OptionsCustomization.AllowSort = false;
            this.gvQuery.OptionsMenu.EnableColumnMenu = false;
            this.gvQuery.OptionsMenu.EnableFooterMenu = false;
            this.gvQuery.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvQuery.OptionsMenu.ShowAutoFilterRowItem = false;
            this.gvQuery.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvQuery.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvQuery.OptionsMenu.ShowSplitItem = false;
            this.gvQuery.OptionsView.ColumnAutoWidth = false;
            this.gvQuery.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gvQuery.OptionsView.ShowGroupPanel = false;
            this.gvQuery.OptionsView.ShowIndicator = false;
            this.gvQuery.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(this.gvQuery_CustomRowCellEdit);
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "字段";
            this.gridColumn5.FieldName = "FieldName";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 0;
            this.gridColumn5.Width = 139;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = " ";
            this.gridColumn6.ColumnEdit = this.repCompare;
            this.gridColumn6.FieldName = "Compare";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 1;
            this.gridColumn6.Width = 54;
            // 
            // repCompare
            // 
            this.repCompare.AutoHeight = false;
            this.repCompare.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repCompare.Items.AddRange(new object[] {
            "=",
            ">",
            ">=",
            "<",
            "<=",
            "包含",
            ""});
            this.repCompare.Name = "repCompare";
            this.repCompare.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn7.Caption = "值";
            this.gridColumn7.FieldName = "InputValue";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 2;
            this.gridColumn7.Width = 114;
            // 
            // gridColumn8
            // 
            this.gridColumn8.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn8.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn8.Caption = "关系";
            this.gridColumn8.FieldName = "AndOr";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 3;
            this.gridColumn8.Width = 55;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "Guid";
            this.gridColumn9.FieldName = "Guid";
            this.gridColumn9.Name = "gridColumn9";
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "Serialno_Module";
            this.gridColumn10.FieldName = "Serialno_Module";
            this.gridColumn10.Name = "gridColumn10";
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "BUser";
            this.gridColumn11.FieldName = "BUser";
            this.gridColumn11.Name = "gridColumn11";
            // 
            // panelControl5
            // 
            this.panelControl5.Controls.Add(this.btnGroupClose);
            this.panelControl5.Controls.Add(this.btnQueryOK);
            this.panelControl5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl5.Location = new System.Drawing.Point(2, 260);
            this.panelControl5.Name = "panelControl5";
            this.panelControl5.Size = new System.Drawing.Size(442, 36);
            this.panelControl5.TabIndex = 6;
            // 
            // btnGroupClose
            // 
            this.btnGroupClose.Location = new System.Drawing.Point(269, 5);
            this.btnGroupClose.Name = "btnGroupClose";
            this.btnGroupClose.Size = new System.Drawing.Size(75, 23);
            this.btnGroupClose.TabIndex = 5;
            this.btnGroupClose.Text = "关闭";
            this.btnGroupClose.Click += new System.EventHandler(this.btnGroupClose_Click);
            // 
            // btnQueryOK
            // 
            this.btnQueryOK.Location = new System.Drawing.Point(92, 5);
            this.btnQueryOK.Name = "btnQueryOK";
            this.btnQueryOK.Size = new System.Drawing.Size(75, 23);
            this.btnQueryOK.TabIndex = 4;
            this.btnQueryOK.Text = "确认";
            this.btnQueryOK.Click += new System.EventHandler(this.btnQueryOK_Click);
            // 
            // panelControl4
            // 
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl4.Location = new System.Drawing.Point(0, 0);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(456, 333);
            this.panelControl4.TabIndex = 3;
            // 
            // FrmChartQueryAndSum
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 333);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.panelControl4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmChartQueryAndSum";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查询条件";
            ((System.ComponentModel.ISupportInitialize)(this.repChk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repSumType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.xtraTabControl1.ResumeLayout(false);
            this.xtraTabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repGroupBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repSumType2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repIsShow)).EndInit();
            this.xtraTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            this.panelControl3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gcQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCompare)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl5)).EndInit();
            this.panelControl5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton btnOptionOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage1;
        private DevExpress.XtraTab.XtraTabPage xtraTabPage2;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.SimpleButton btnGroupClose;
        private DevExpress.XtraEditors.SimpleButton btnQueryOK;
        private DevExpress.XtraEditors.PanelControl panelControl5;
        private DevExpress.XtraGrid.GridControl gcQuery;
        private DevExpress.XtraGrid.Views.Grid.GridView gvQuery;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repChk;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repSumType;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repCompare;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repSumType2;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repGroupBy;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repIsShow;
        private DevExpress.Utils.ToolTipController toolTipController1;

    }
}