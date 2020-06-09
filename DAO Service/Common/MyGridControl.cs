using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Registrator;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using System.Drawing;
using DevExpress.XtraGrid.Views.BandedGrid;

namespace Common
{/*
    public class MyGridControl : GridControl
    {
        protected override BaseView CreateDefaultView()
        {
            return CreateView("MyGridView");
        }
        protected override void RegisterAvailableViewsCore(InfoCollection collection)
        {
            base.RegisterAvailableViewsCore(collection);
            collection.Add(new MyGridViewInfoRegistrator());
        }
    }

    public class MyGridViewInfoRegistrator : GridInfoRegistrator
    {
        public override string ViewName { get { return "MyGridView"; } }
        public override BaseView CreateView(GridControl grid)
        {
            return new MyGridView(grid as GridControl);
        }
    }


    public class MyGridView : GridView
    {

        protected override DevExpress.XtraGrid.Columns.GridColumnCollection CreateColumnCollection()
        {
            return new MyGridColumnCollection(this);
        }
        protected override DevExpress.XtraGrid.Columns.ColumnFilterInfo CreateFilterRowInfo(DevExpress.XtraGrid.Columns.GridColumn column, object _value)
        {
            try
            {
                if (true)//是否允许用户模糊查询
                {
                    if ((column) is MyGridColumn && ((MyGridColumn)column).UseAdvancedFiltering == true && (string)_value != "")
                    {
                        string FilterText = (string)_value;
                        //if (FilterText.Contains("%") == false && FilterText.Contains("_") == false)
                        if (FilterText.Contains("%") == false)
                        {
                            FilterText = String.Format("%{0}%", _value);
                        }
                        return new DevExpress.XtraGrid.Columns.ColumnFilterInfo(DevExpress.XtraGrid.Columns.ColumnFilterType.AutoFilter, _value, new DevExpress.Data.Filtering.BinaryOperator(column.FieldName, FilterText, DevExpress.Data.Filtering.BinaryOperatorType.Like), null);
                    }
                    else
                    {
                        return base.CreateFilterRowInfo(column, _value);
                    }
                }
                else
                {
                    return base.CreateFilterRowInfo(column, _value);
                }
            }
            catch (Exception)
            {
                return base.CreateFilterRowInfo(column, _value);
            }
        }

        /// <summary>
        ///函数
        /// </summary>
        /// <param name="ownerGrid"></param>
        public MyGridView(GridControl ownerGrid) : base(ownerGrid) 
        {
            _SetStyle();
        }

        /// <summary>
        /// 函数
        /// </summary>
        public MyGridView()
        {
            _SetStyle();
        }

        private void _SetStyle()
        {
            ///选中行字体颜色
            this.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Blue;
            this.Appearance.SelectedRow.Options.UseForeColor = true;
            this.Appearance.SelectedRow.Font = new Font(this.Appearance.SelectedRow.Font, FontStyle.Bold);
            this.Appearance.SelectedRow.Options.UseFont = true;
            this.Appearance.SelectedRow.BackColor = System.Drawing.Color.MistyRose;
            this.Appearance.SelectedRow.Options.UseBackColor = true;

            this.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Blue;
            this.Appearance.FocusedRow.Options.UseForeColor = true;
            this.Appearance.FocusedRow.Font = new Font(this.Appearance.SelectedRow.Font, FontStyle.Bold);
            this.Appearance.FocusedRow.Options.UseFont = true;
            this.Appearance.FocusedRow.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Appearance.FocusedRow.Options.UseBackColor = true;

            this.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.Blue;
            this.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.Appearance.HideSelectionRow.Font = new Font(this.Appearance.SelectedRow.Font, FontStyle.Bold);
            this.Appearance.HideSelectionRow.Options.UseFont = true;
            this.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Appearance.HideSelectionRow.Options.UseBackColor = true;
        }

        /// <summary>
        /// 初始化一些默认值
        /// </summary>
        public override void BeginInit()
        {
            base.BeginInit();
            this.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.ShowAlways;
            //// 设置为多选
            //this.OptionsSelection.MultiSelect = true;
            //this.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.RowSelect;
            //this.FocusedRowHandle = 0;
            //this.Focus();
            ////显示筛选
            //this.OptionsView.ShowAutoFilterRow = true;
            ////隐藏提示
            //this.OptionsView.ShowGroupPanel = true;
            ////禁止编辑
            //this.OptionsBehavior.Editable = false;
            ////this.OptionsView.EnableAppearanceEvenRow = false; //是否启用偶数行外观
            ////this.OptionsView.EnableAppearanceOddRow = false; //是否启用奇数行外观
            //this.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never; //是否显示过滤面板

            //this.OptionsCustomization.AllowColumnMoving = true; //是否允许移动列
            //this.OptionsCustomization.AllowColumnResizing = true; //是否允许调整列宽
            //this.OptionsCustomization.AllowGroup = true; //是否允许分组
            //this.OptionsCustomization.AllowFilter = true; //是否允许过滤
            //this.OptionsCustomization.AllowSort = true; //是否允许排序  
        }

        public override void EndInit()
        {
            base.EndInit();
            this.SelectionChanged += MyGridView_SelectionChanged;
        }

        void MyGridView_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
            int[] grvarry = this.GetSelectedRows();
            if (grvarry.Length > 1)
            {
                //for (int i = 0; i < grvarry.Length; i++)
                // {
                //     GetDataRowList[i] = this.GetDataRow(grvarry[i]);
                // }
                GetList = grvarry;

            }
            else
            {
                GetDataRowInfo = this.GetFocusedDataRow();
            }
            //GetDataRowInfo = this.GetFocusedDataRow();
        }

        /// <summary>
        /// 获取用户选择行数据
        /// </summary>
        public DataRow GetDataRowInfo { get; set; }
        /// <summary>
        /// 返回选择行数组
        /// </summary>
        public DataRow[] GetDataRowList { get; set; }
        /// <summary>
        /// 返回用户选择行索引
        /// </summary>
        public int[] GetList { get; set; }
    }


    public class MyBandedGridView : BandedGridView
    {
        /// <summary>
        ///函数
        /// </summary>
        /// <param name="ownerGrid"></param>
        public MyBandedGridView(GridControl ownerGrid)
            : base(ownerGrid)
        {
            _SetStyle();
        }

        /// <summary>
        /// 函数
        /// </summary>
        public MyBandedGridView()
        {
            _SetStyle();
        }

        private void _SetStyle()
        {
            ///选中行字体颜色
            this.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Blue;
            this.Appearance.SelectedRow.Options.UseForeColor = true;
            this.Appearance.SelectedRow.Font = new Font(this.Appearance.SelectedRow.Font, FontStyle.Bold);
            this.Appearance.SelectedRow.Options.UseFont = true;
            this.Appearance.SelectedRow.BackColor = System.Drawing.Color.MistyRose;
            this.Appearance.SelectedRow.Options.UseBackColor = true;

            this.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Blue;
            this.Appearance.FocusedRow.Options.UseForeColor = true;
            this.Appearance.FocusedRow.Font = new Font(this.Appearance.SelectedRow.Font, FontStyle.Bold);
            this.Appearance.FocusedRow.Options.UseFont = true;
            this.Appearance.FocusedRow.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Appearance.FocusedRow.Options.UseBackColor = true;

            this.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.Blue;
            this.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.Appearance.HideSelectionRow.Font = new Font(this.Appearance.SelectedRow.Font, FontStyle.Bold);
            this.Appearance.HideSelectionRow.Options.UseFont = true;
            this.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Appearance.HideSelectionRow.Options.UseBackColor = true;
        }

    }


    public class MyGridColumn : DevExpress.XtraGrid.Columns.GridColumn
    {
        public MyGridColumn()
            : base()
        {
        }
        private bool mUseAdvancedFiltering = true;
        public bool UseAdvancedFiltering
        {
            get { return this.mUseAdvancedFiltering; }
            set { this.mUseAdvancedFiltering = value; }
        }
    }
    public class MyGridColumnCollection : GridColumnCollection
    {
        public MyGridColumnCollection(DevExpress.XtraGrid.Views.Base.ColumnView View)
            : base(View)
        {
        }
        protected override DevExpress.XtraGrid.Columns.GridColumn CreateColumn()
        {
            return new MyGridColumn();
        }
    }   
*/
}
