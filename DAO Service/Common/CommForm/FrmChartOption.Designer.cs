namespace Common.CommForm
{
    partial class FrmChartOption
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
            this.vGrid = new DevExpress.XtraVerticalGrid.VGridControl();
            this.repChartAlign = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repChartType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repShowNameOrValue = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repBtnQuery = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repBtnGroup = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repCategory = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repCategorySub = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repCustomQuery = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.repSQL = new DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit();
            this.repOrderBy = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repOrderType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.TableName = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.ShowFieldName = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.Filter = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.SumFieldName = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.ChartTitle = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.ChartAlign = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.ChartType = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.ChartFieldName = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.ChartFieldValue = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.SerialName = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.ShowNameOrValue = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.Category = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.CategorySub = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.CustomQuery = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.QuerySQL = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.SeriesFieldName = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.OrderFieldName = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.IsOrderBy = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.TopCount = new DevExpress.XtraVerticalGrid.Rows.EditorRow();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.vGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repChartAlign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repChartType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repShowNameOrValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repBtnQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repBtnGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCategory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCategorySub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCustomQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repSQL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repOrderBy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repOrderType)).BeginInit();
            this.SuspendLayout();
            // 
            // vGrid
            // 
            this.vGrid.Location = new System.Drawing.Point(3, 3);
            this.vGrid.Name = "vGrid";
            this.vGrid.RecordWidth = 521;
            this.vGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repChartAlign,
            this.repChartType,
            this.repShowNameOrValue,
            this.repBtnQuery,
            this.repBtnGroup,
            this.repCategory,
            this.repCategorySub,
            this.repCustomQuery,
            this.repSQL,
            this.repOrderBy,
            this.repOrderType});
            this.vGrid.RowHeaderWidth = 131;
            this.vGrid.Rows.AddRange(new DevExpress.XtraVerticalGrid.Rows.BaseRow[] {
            this.TableName,
            this.ShowFieldName,
            this.Filter,
            this.SumFieldName,
            this.ChartTitle,
            this.ChartAlign,
            this.ChartType,
            this.ChartFieldName,
            this.ChartFieldValue,
            this.SerialName,
            this.ShowNameOrValue,
            this.Category,
            this.CategorySub,
            this.CustomQuery,
            this.QuerySQL,
            this.SeriesFieldName,
            this.OrderFieldName,
            this.IsOrderBy,
            this.TopCount});
            this.vGrid.Size = new System.Drawing.Size(663, 417);
            this.vGrid.TabIndex = 0;
            this.vGrid.CustomRecordCellEdit += new DevExpress.XtraVerticalGrid.Events.GetCustomRowCellEditEventHandler(this.vGrid_CustomRecordCellEdit);
            this.vGrid.CellValueChanged += new DevExpress.XtraVerticalGrid.Events.CellValueChangedEventHandler(this.vGrid_CellValueChanged);
            // 
            // repChartAlign
            // 
            this.repChartAlign.AutoHeight = false;
            this.repChartAlign.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repChartAlign.Items.AddRange(new object[] {
            "上",
            "下",
            "左",
            "右",
            ""});
            this.repChartAlign.Name = "repChartAlign";
            this.repChartAlign.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // repChartType
            // 
            this.repChartType.AutoHeight = false;
            this.repChartType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repChartType.Items.AddRange(new object[] {
            "柱形图",
            "饼图",
            "曲线",
            "3D柱形图",
            "3D饼图",
            "3D曲线",
            ""});
            this.repChartType.Name = "repChartType";
            this.repChartType.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // repShowNameOrValue
            // 
            this.repShowNameOrValue.AutoHeight = false;
            this.repShowNameOrValue.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repShowNameOrValue.Items.AddRange(new object[] {
            "显示名称",
            "显示值",
            "显示名称和值"});
            this.repShowNameOrValue.Name = "repShowNameOrValue";
            this.repShowNameOrValue.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // repBtnQuery
            // 
            this.repBtnQuery.AutoHeight = false;
            this.repBtnQuery.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repBtnQuery.Name = "repBtnQuery";
            // 
            // repBtnGroup
            // 
            this.repBtnGroup.AutoHeight = false;
            this.repBtnGroup.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repBtnGroup.Name = "repBtnGroup";
            // 
            // repCategory
            // 
            this.repCategory.AutoHeight = false;
            this.repCategory.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repCategory.Items.AddRange(new object[] {
            "生产",
            "财务",
            "品质",
            "业务",
            "其它"});
            this.repCategory.Name = "repCategory";
            this.repCategory.NullText = " ";
            // 
            // repCategorySub
            // 
            this.repCategorySub.AutoHeight = false;
            this.repCategorySub.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repCategorySub.Items.AddRange(new object[] {
            "全部客户",
            "个别客户"});
            this.repCategorySub.Name = "repCategorySub";
            // 
            // repCustomQuery
            // 
            this.repCustomQuery.AutoHeight = false;
            this.repCustomQuery.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repCustomQuery.Name = "repCustomQuery";
            this.repCustomQuery.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repCustomQuery_ButtonClick);
            // 
            // repSQL
            // 
            this.repSQL.AutoHeight = false;
            this.repSQL.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repSQL.Name = "repSQL";
            // 
            // repOrderBy
            // 
            this.repOrderBy.AutoHeight = false;
            this.repOrderBy.Name = "repOrderBy";
            this.repOrderBy.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // repOrderType
            // 
            this.repOrderType.AutoHeight = false;
            this.repOrderType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repOrderType.Items.AddRange(new object[] {
            "顺序",
            "倒序"});
            this.repOrderType.Name = "repOrderType";
            this.repOrderType.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // TableName
            // 
            this.TableName.Name = "TableName";
            this.TableName.Properties.Caption = "数据表或视图";
            this.TableName.Properties.FieldName = "TableName";
            // 
            // ShowFieldName
            // 
            this.ShowFieldName.Height = 17;
            this.ShowFieldName.Name = "ShowFieldName";
            this.ShowFieldName.Properties.Caption = "显示字段";
            this.ShowFieldName.Properties.FieldName = "ShowFieldName";
            // 
            // Filter
            // 
            this.Filter.Name = "Filter";
            this.Filter.Properties.Caption = "默认过滤条件";
            this.Filter.Properties.FieldName = "Filter";
            this.Filter.Properties.ReadOnly = true;
            this.Filter.Properties.RowEdit = this.repBtnQuery;
            // 
            // SumFieldName
            // 
            this.SumFieldName.Name = "SumFieldName";
            this.SumFieldName.Properties.Caption = "默认统计角度";
            this.SumFieldName.Properties.FieldName = "SumFieldName";
            this.SumFieldName.Properties.ReadOnly = true;
            this.SumFieldName.Properties.RowEdit = this.repBtnGroup;
            // 
            // ChartTitle
            // 
            this.ChartTitle.Name = "ChartTitle";
            this.ChartTitle.Properties.Caption = "图表标题";
            this.ChartTitle.Properties.FieldName = "ChartTitle";
            // 
            // ChartAlign
            // 
            this.ChartAlign.Name = "ChartAlign";
            this.ChartAlign.Properties.Caption = "图表停靠位置";
            this.ChartAlign.Properties.FieldName = "ChartAlign";
            this.ChartAlign.Properties.RowEdit = this.repChartAlign;
            // 
            // ChartType
            // 
            this.ChartType.Name = "ChartType";
            this.ChartType.Properties.Caption = "图标类型";
            this.ChartType.Properties.FieldName = "ChartType";
            this.ChartType.Properties.RowEdit = this.repChartType;
            // 
            // ChartFieldName
            // 
            this.ChartFieldName.Name = "ChartFieldName";
            this.ChartFieldName.Properties.Caption = "图标X轴绑定";
            this.ChartFieldName.Properties.FieldName = "ChartFieldName";
            // 
            // ChartFieldValue
            // 
            this.ChartFieldValue.Name = "ChartFieldValue";
            this.ChartFieldValue.Properties.Caption = "图表Y轴绑定";
            this.ChartFieldValue.Properties.FieldName = "ChartFieldValue";
            this.ChartFieldValue.Properties.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            // 
            // SerialName
            // 
            this.SerialName.Name = "SerialName";
            this.SerialName.Properties.Caption = "系列名称";
            this.SerialName.Properties.FieldName = "SerialName";
            // 
            // ShowNameOrValue
            // 
            this.ShowNameOrValue.Name = "ShowNameOrValue";
            this.ShowNameOrValue.Properties.Caption = "每项显示方式";
            this.ShowNameOrValue.Properties.FieldName = "ShowNameOrValue";
            this.ShowNameOrValue.Properties.RowEdit = this.repShowNameOrValue;
            // 
            // Category
            // 
            this.Category.Name = "Category";
            this.Category.Properties.Caption = "一级分类";
            this.Category.Properties.FieldName = "Category";
            this.Category.Properties.RowEdit = this.repCategory;
            // 
            // CategorySub
            // 
            this.CategorySub.Name = "CategorySub";
            this.CategorySub.Properties.Caption = "二级分类";
            this.CategorySub.Properties.FieldName = "CategorySub";
            this.CategorySub.Properties.RowEdit = this.repCategorySub;
            // 
            // CustomQuery
            // 
            this.CustomQuery.Name = "CustomQuery";
            this.CustomQuery.Properties.Caption = "自定义查询";
            this.CustomQuery.Properties.FieldName = "CustomQuery";
            this.CustomQuery.Properties.ReadOnly = true;
            this.CustomQuery.Properties.RowEdit = this.repCustomQuery;
            // 
            // QuerySQL
            // 
            this.QuerySQL.Name = "QuerySQL";
            this.QuerySQL.Properties.Caption = "查询SQL";
            this.QuerySQL.Properties.FieldName = "ExecSQL";
            this.QuerySQL.Properties.ReadOnly = true;
            this.QuerySQL.Properties.RowEdit = this.repSQL;
            // 
            // SeriesFieldName
            // 
            this.SeriesFieldName.Name = "SeriesFieldName";
            this.SeriesFieldName.Properties.Caption = "串联字段";
            this.SeriesFieldName.Properties.FieldName = "SeriesFieldName";
            // 
            // OrderFieldName
            // 
            this.OrderFieldName.Name = "OrderFieldName";
            this.OrderFieldName.Properties.Caption = "排序字段";
            this.OrderFieldName.Properties.FieldName = "OrderFieldName";
            // 
            // IsOrderBy
            // 
            this.IsOrderBy.Name = "IsOrderBy";
            this.IsOrderBy.Properties.Caption = "是否显示前几名客户";
            this.IsOrderBy.Properties.FieldName = "IsOrderBy";
            this.IsOrderBy.Properties.RowEdit = this.repOrderBy;
            // 
            // TopCount
            // 
            this.TopCount.Name = "TopCount";
            this.TopCount.Properties.Caption = "前几名客户";
            this.TopCount.Properties.FieldName = "TopCount";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(448, 426);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // FrmChartOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 461);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.vGrid);
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmChartOption";
            this.Text = "数据魔方配置";
            ((System.ComponentModel.ISupportInitialize)(this.vGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repChartAlign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repChartType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repShowNameOrValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repBtnQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repBtnGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCategory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCategorySub)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCustomQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repSQL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repOrderBy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repOrderType)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraVerticalGrid.VGridControl vGrid;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow TableName;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow row2;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow SumFieldName;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow ShowFieldName;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow ChartTitle;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow ChartAlign;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow ChartType;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow ChartFieldName;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow ChartFieldValue;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow SerialName;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow ShowNameOrValue;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repChartAlign;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repChartType;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repShowNameOrValue;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow Filter;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repBtnQuery;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repBtnGroup;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow Category;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repCategory;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow CategorySub;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repCategorySub;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow CustomQuery;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repCustomQuery;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow QuerySQL;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit repSQL;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow SeriesFieldName;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow OrderFieldName;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow IsOrderBy;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repOrderBy;
        private DevExpress.XtraVerticalGrid.Rows.EditorRow TopCount;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repOrderType;
    }
}