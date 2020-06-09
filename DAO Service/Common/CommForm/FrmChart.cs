using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using Common.Tools;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraVerticalGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.Utils;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTab;
using System.Collections;
using DevExpress.XtraEditors.Controls;
using TWControls;
using DevExpress.XtraEditors.Repository;

namespace Common.CommForm
{
    public partial class FrmChart : FrmBase
    {
        private int _Width = 0, _Height = 0;

        private GridControl _LeftTopGridControl = null, _RightTopGridControl = null;
        private GridControl _LeftButtomGridControl = null, _RightButtomGridControl = null;

        private ChartControl _LeftTopChart = null, _RightTopChart = null;
        private ChartControl _LeftButtomChart = null, _RightButtomChart = null;
        private string Guid_1 = "", Guid_2 = "", Guid_3 = "", Guid_4 = "";

        private PanelControl _panel = new PanelControl();
        private string _FieldNameList = "", _Guid = "";
        SplitContainerControl _scc = null;
        private DataTable _dtScreeen = new DataTable();
        //private List<string,string> _listQueryCondiction = new List<string,string>();
        private Hashtable _htQueryCondiction = new Hashtable();
        private DataTable _TreeChart = new DataTable();
        private Hashtable _htCurSQL = new Hashtable();//当前图表的SQL
        private DataTable _dtQueryWhere = new DataTable();//保存查询条件
        private DataTable _dtCategorySubQuery = new DataTable(); //保存二级查询条件
        private DataTable _dtQueryCtr = new DataTable();//查询控件配置
        private bool _IsCustomerEmpty = true;//是否输入客户
        private GridView _GridView = new GridView();//数据表格
        private MyGridControl _gcZuan = new MyGridControl();//数据钻取的网格
        private DataTable _dtZuanOptions = new DataTable();//数据钻取网格
        private XtraTabControl _XtraTab = new XtraTabControl();//选项卡

        public FrmChart()
        {
            InitializeComponent();
            RegeditEvent();
            BindTreeData();
            DataBind("");
            RecalSize();
            //创建查询条件的table
            CreateQueryTable();
        }

        #region 左边的缩略图

        #region 树形数据绑定
        /// <summary>
        /// 树形数据绑定
        /// </summary>
        private void BindTreeData()
        {
            try
            {
                //DataTable dtCategory = new DataTable();
                string strSQL = "";
                //int Index = 2, IndexSub = 0;

                strSQL = " exec sp_Chart_GetByRole '" + Common.Comm._user.UserCode + "'";
                //strSQL = " exec sp_Chart_GetByRoleking  '" + Common.Comm._user.UserCode + "'";

                _TreeChart = DataService.Data.OpenDataSingle(strSQL, "t_syChartOptions");

                _TreeChart.Columns.Add("pic", typeof(Image));
                _TreeChart.Columns.Add("IsSelect", typeof(bool));

                foreach (DataRow drChart in _TreeChart.Rows)
                {
                    if (!string.IsNullOrEmpty(drChart["ChartType"].ToString()))
                        drChart["pic"] = imgTools.Images[drChart["ChartType"].ToString()];
                }

                BindingSource bs = new BindingSource();
                bs.DataSource = _TreeChart;
                treeCategory.DataSource = bs;
            }
            catch (Exception ex)
            {
                Common.Message.MsgError(ex.Message.ToString());
            }
        }

        #endregion

        #endregion

        #region 注册事件
        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegeditEvent()
        {
            treeCategory.GetStateImage -= new DevExpress.XtraTreeList.GetStateImageEventHandler(treeCategory_GetStateImage);
            treeCategory.GetStateImage += new DevExpress.XtraTreeList.GetStateImageEventHandler(treeCategory_GetStateImage);

            treeCategory.DoubleClick -= new EventHandler(treeCategory_DoubleClick);
            treeCategory.DoubleClick += new EventHandler(treeCategory_DoubleClick);

            splitContainerControl1.SplitGroupPanelCollapsed -= new SplitGroupPanelCollapsedEventHandler(splitContainerControl1_SplitGroupPanelCollapsed);
            splitContainerControl1.SplitGroupPanelCollapsed += new SplitGroupPanelCollapsedEventHandler(splitContainerControl1_SplitGroupPanelCollapsed);

            splitContainerControl1.SplitterPositionChanged -= new EventHandler(splitContainerControl1_SplitterPositionChanged);
            splitContainerControl1.SplitterPositionChanged += new EventHandler(splitContainerControl1_SplitterPositionChanged);

        }

        void splitContainerControl1_SplitterPositionChanged(object sender, EventArgs e)
        {
            //改变尺寸
            RecalSize();
        }

        void splitContainerControl1_SplitGroupPanelCollapsing(object sender, SplitGroupPanelCollapsingEventArgs e)
        {
            //位置发生变化
            try
            {
                TreeListNode node = treeCategory.FocusedNode as TreeListNode;

                DataRow dr = (node.TreeList.GetDataRecordByNode(node) as DataRowView).Row;

                showAllScreen(dr["Guid"].ToString(), "");

            }
            catch (Exception ex)
            {
                Message.MsgAlert(ex.Message);
            }
        }

        void splitContainerControl1_SplitGroupPanelCollapsed(object sender, SplitGroupPanelCollapsedEventArgs e)
        {
            //位置发生变化
            try
            {
                //改变尺寸
                if (pnlTotal.Visible)
                {
                    RecalSize();
                }
                else
                {
                    TreeListNode node = treeCategory.FocusedNode as TreeListNode;
                    DataRow dr = (node.TreeList.GetDataRecordByNode(node) as DataRowView).Row;
                    showAllScreen(dr["Guid"].ToString(), "");
                }
            }
            catch (Exception ex)
            {
                Message.MsgAlert(ex.Message);
            }
        }

        void treeCategory_GetStateImage(object sender, DevExpress.XtraTreeList.GetStateImageEventArgs e)
        {
            //图片
            DataRow dr = (e.Node.TreeList.GetDataRecordByNode(e.Node) as DataRowView).Row;
            if (dr == null) return;
            int ImgIndex = 10;

            if (dr["ChartType"].ToString() != "")
            {
                switch (dr["ChartType"].ToString())
                {
                    case "柱形图":
                        ImgIndex = 5;
                        break;
                    case "曲线":
                        ImgIndex = 6;
                        break;
                    case "饼图":
                        ImgIndex = 7;
                        break;
                    case "3D饼图":
                        ImgIndex = 8;
                        break;
                    case "3D柱形图":
                        ImgIndex = 9;
                        break;
                    default:
                        ImgIndex = 10;
                        break;
                }
            }
            else
            {
                ImgIndex = e.Node.Expanded ? 10 : 11;
            }
            e.NodeImageIndex = ImgIndex;
        }

        #endregion

        #region 创建查询条件的table
        /// <summary>
        /// 创建查询条件的table
        /// </summary>
        private void CreateQueryTable()
        {
            _dtQueryWhere = new DataTable();
            _dtQueryWhere.Columns.Add("Guid", typeof(string));
            _dtQueryWhere.Columns.Add("ParentId", typeof(Int32));
            _dtQueryWhere.Columns.Add("ControlName", typeof(string));
            _dtQueryWhere.Columns.Add("ControlValue", typeof(string));

            //按二级分类保存查询条件
            _dtCategorySubQuery = new DataTable();
            _dtCategorySubQuery.Columns.Add("ParentId", typeof(Int32));
            _dtCategorySubQuery.Columns.Add("CondictionSQL", typeof(string));


        }

        #endregion

        #region 数据绑定
        private void DataBind(string ListGuid)
        {
            try
            {
                string FieldX = "", FieldY = "";
                string FieldNameY = "", FieldNameX = "";
                string strSQL = "";

                //订阅的数据魔方

                if (string.IsNullOrEmpty(ListGuid))
                {
                    strSQL = " select t2.*,t1.TopCount,t1.OrderType,t1.ChartType ChartType2  from t_syChartLayout t1 join  t_syChartOptions t2 on ";
                    strSQL += " t1.module_serialno=t2.guid  where t1.buser='" + Common.Comm._user.UserCode + "' and t1.IsSelect=1 ";

                }
                else
                {
                    strSQL = " select t2.*,t1.TopCount,t1.OrderType,t1.ChartType ChartType2  from t_syChartLayout t1 right join  t_syChartOptions t2 on ";
                    strSQL += " t1.module_serialno=t2.guid  where  t2.guid in (" + ListGuid + ")  and t1.buser='" + Common.Comm._user.UserCode + "'";

                }

                DataTable dtData = DataService.Data.OpenDataSingle(strSQL, "t_syChartLayout");

                dtData.TableName = "t_syChartLayout";
                ChartControl chart = null;

                Series sr = null;
                ViewType ChartType = ViewType.Bar;
                PointView pv = PointView.Argument;
                ChartTitleDockStyle titleDockStyle = ChartTitleDockStyle.Left;

                int i = 1;
                string viewType = "";
                string OrderFieldName = "", OrderType = "";

                foreach (DataRow dr in dtData.Rows)
                {
                    chart = new ChartControl();

                    FieldNameX = dr["ChartFieldName"].ToString();
                    FieldNameY = dr["ChartFieldValue"].ToString();

                    #region 组装SQL

                    strSQL = dr["ExecSQL"].ToString().Replace("''", "'");

                    if (dr["IsOrderBy"].ToString() == "True")
                    {
                        //显示前几名客户

                        strSQL += " and 客户 in (select top " + dr["TopCount"].ToString() + " 客户 from  " + dr["TableName"].ToString();
                        strSQL += " where 年份=year(getdate()) ";

                        strSQL += " group by  客户  ";

                        //排名字段
                        if (dr["TopFieldName"].ToString() != "")
                            strSQL += " order by sum(" + dr["TopFieldName"].ToString() + ")  ";
                        else
                            strSQL += " order by sum(" + FieldNameY + ") ";

                        if (dr["OrderType"].ToString() == "0")
                            strSQL += " asc ) ";
                        else
                            strSQL += " desc )  ";
                    }

                    //统计角度            
                    if (dr["SumFieldName"].ToString() != "")
                    {
                        //第几行
                        //第几行  Y轴计算公式
                        if (dr["FormulaY"].ToString() != "")
                            strSQL = " select " + dr["GroupName"].ToString() + " ," + dr["FormulaY"].ToString() + " from ( " + strSQL;
                        else
                            strSQL = " select " + dr["GroupName"].ToString() + " ," + dr["SumFieldName"].ToString() + " from ( " + strSQL;

                        strSQL += " ) aa group by  " + dr["GroupName"].ToString();
                    }

                    //最外层的排序                
                    if (dr["OrderFieldName"].ToString() != "")
                    {
                        strSQL = " select  * from  ( " + strSQL + " ) bb  order by " + dr["OrderFieldName"].ToString();

                        if (dr["OrderType"].ToString() == "0")
                        {
                            strSQL += " asc ";
                            OrderType = " asc ";
                        }
                        else
                        {
                            strSQL += " desc  ";
                            OrderType = " desc ";
                        }

                        OrderFieldName = dr["OrderFieldName"].ToString();
                    }


                    #endregion

                    //判断是不是全屏缩小

                    if (_htCurSQL.Count > 0 && _htCurSQL[dr["guid"].ToString()] != null)
                        strSQL = _htCurSQL[dr["guid"].ToString()].ToString();

                    DataTable dtTemp = DataService.Data.OpenDataSingle(strSQL, "t_SYModSearch_B");

                    chart.Dock = DockStyle.Fill;

                    if (dr["ChartType2"].ToString() != "")
                        viewType = dr["ChartType2"].ToString();
                    else
                        viewType = dr["ChartType"].ToString();

                    if (viewType == "柱形图")
                        ChartType = ViewType.Bar;
                    else if (viewType == "饼图")
                        ChartType = ViewType.Pie;
                    else if (viewType == "曲线")
                        ChartType = ViewType.Line;
                    else if (viewType == "3D柱形图")
                        ChartType = ViewType.Bar3D;
                    else if (viewType == "3D饼图")
                        ChartType = ViewType.Pie3D;
                    else if (viewType == "3D曲线")
                        ChartType = ViewType.Line3D;

                    //图表Y轴值绑定
                    string[] ArrValue;

                    if (string.IsNullOrEmpty(FieldY))
                        ArrValue = dr["ChartFieldValue"].ToString().Split(',');
                    else
                        ArrValue = FieldY.Split(',');

                    if (dr["SeriesFieldName"].ToString() != "")
                    {
                        #region 使用串联字段
                        try
                        {
                            CreateCutomerSeries(dtTemp, chart, dr["SeriesFieldName"].ToString(),
                               FieldNameX, FieldNameY, ChartType, OrderFieldName, OrderType);
                        }
                        catch (Exception ex)
                        {
                            this.MsgAlert(ex.Message.ToString());
                        }

                        #endregion
                    }
                    else
                    {
                        #region 不使用串联字段
                        try
                        {
                            if (string.IsNullOrEmpty(FieldNameY))
                            {
                                #region 默认配置

                                foreach (string v in ArrValue)
                                {
                                    if (!string.IsNullOrEmpty(v))
                                    {
                                        sr = new Series(v.Trim(), ChartType);

                                        if (dr["ShowNameOrValue"].ToString() == "显示名称")
                                            pv = PointView.Argument;
                                        else if (dr["ShowNameOrValue"].ToString() == "显示值")
                                            pv = PointView.Values;
                                        else if (dr["ShowNameOrValue"].ToString() == "显示名称和值")
                                            pv = PointView.ArgumentAndValues;

                                        sr.Label.PointOptions.PointView = pv;
                                        sr.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.Number;//用百分比表示
                                        sr.CrosshairLabelVisibility = DevExpress.Utils.DefaultBoolean.True;
                                        sr.ArgumentScaleType = ScaleType.Qualitative;
                                        sr.ValueScaleType = ScaleType.Numerical;//数字类型
                                        sr.LabelsVisibility = DefaultBoolean.True;

                                        if (string.IsNullOrEmpty(FieldX))
                                            sr.ArgumentDataMember = dr["ChartFieldName"].ToString();
                                        else
                                            sr.ArgumentDataMember = FieldX;

                                        sr.ValueDataMembers[0] = v.Trim();

                                        sr.DataSource = dtTemp;

                                        //添加到统计图上
                                        chart.Series.Add(sr);
                                    }
                                }

                                #endregion
                            }
                            else
                            {
                                #region 个人配置

                                sr = new Series(FieldNameY, ChartType);

                                if (dr["ShowNameOrValue"].ToString() == "显示名称")
                                    pv = PointView.Argument;
                                else if (dr["ShowNameOrValue"].ToString() == "显示值")
                                    pv = PointView.Values;
                                else if (dr["ShowNameOrValue"].ToString() == "显示名称和值")
                                    pv = PointView.ArgumentAndValues;

                                sr.Label.PointOptions.PointView = pv;
                                sr.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.Number;//用百分比表示
                                sr.CrosshairLabelVisibility = DevExpress.Utils.DefaultBoolean.True;
                                sr.ArgumentScaleType = ScaleType.Qualitative;
                                sr.ValueScaleType = ScaleType.Numerical;//数字类型
                                sr.LabelsVisibility = DefaultBoolean.True;

                                //if (string.IsNullOrEmpty(FieldNameX))
                                //    sr.ArgumentDataMember = dr["ChartFieldName"].ToString();
                                //else
                                sr.ArgumentDataMember = FieldNameX;

                                sr.ValueDataMembers[0] = FieldNameY;

                                sr.DataSource = dtTemp;

                                //添加到统计图上
                                chart.Series.Add(sr);
                                #endregion
                            }

                        }
                        catch (Exception ex)
                        {
                            this.MsgAlert(ex.Message.ToString());
                        }
                        #endregion
                    }

                    if (dr["ChartAlign"].ToString() == "上")
                        titleDockStyle = ChartTitleDockStyle.Top;
                    else if (dr["ChartAlign"].ToString() == "下")
                        titleDockStyle = ChartTitleDockStyle.Bottom;
                    else if (dr["ChartAlign"].ToString() == "左")
                        titleDockStyle = ChartTitleDockStyle.Left;
                    else if (dr["ChartAlign"].ToString() == "右")
                        titleDockStyle = ChartTitleDockStyle.Right;

                    //设置图表标题
                    ChartTitle ct = new ChartTitle();
                    ct.Text = dr["ChartTitle"].ToString();
                    ct.TextColor = Color.Black;//颜色
                    ct.Font = new Font("Tahoma", 12);//字体
                    ct.Dock = titleDockStyle;//停靠在上方
                    ct.Alignment = StringAlignment.Center;//居中显示
                    ct.WordWrap = true;
                    chart.Titles.Add(ct);

                    ChartTitle ctLeft = new ChartTitle();
                    ctLeft.Text = FieldNameY; ;
                    ctLeft.TextColor = Color.Black;//颜色
                    ctLeft.Font = new Font("Tahoma", 12);//字体
                    ctLeft.Dock = ChartTitleDockStyle.Left;
                    ctLeft.Alignment = StringAlignment.Center;//居中显示
                    ctLeft.WordWrap = true;
                    chart.Titles.Add(ctLeft);

                    //数据网格
                    MyGridControl gc = new MyGridControl();
                    MyGridView gv = new MyGridView();
                    gc.Dock = DockStyle.Fill;
                    gv.OptionsView.ShowGroupPanel = false;
                    gv.OptionsView.ShowFooter = false;
                    gv.OptionsBehavior.ReadOnly = true;
                    gv.OptionsView.ShowViewCaption = false;
                    gv.OptionsView.ShowFooter = true;
                    gv.OptionsView.ShowAutoFilterRow = true;

                    foreach (DataColumn dc in dtTemp.Columns)
                    {
                        MyGridColumn column = new MyGridColumn();
                        column.FieldName = dc.ColumnName;
                        column.Caption = dc.Caption;
                        column.Visible = true;
                        column.DisplayFormat.FormatString = "{0:###,###.##}";
                        column.DisplayFormat.FormatType = FormatType.Numeric;

                        //统计
                        if (dc.ColumnName == FieldNameY)
                        {
                            column.SummaryItem.FieldName = FieldNameY;
                            column.SummaryItem.DisplayFormat = "{0:N2}";
                            column.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        }
                        gv.Columns.Add(column);

                    }

                    gc.Height = 70;
                    gc.MainView = gv;
                    gc.DataSource = dtTemp;

                    SplitContainerControl scc = new SplitContainerControl();
                    scc.Collapsed = true;
                    scc.Dock = DockStyle.Fill;
                    scc.Horizontal = false;
                    scc.SplitterPosition = gLeftTop.Height / 6 * 5;

                    if (i == 1)
                    {
                        //左上角    
                        scc.Name = "sccLeftTop";
                        chart.Name = "sccLeftTop";
                        gLeftTop.Controls.RemoveByKey("sccLeftTop");
                        gRightTop.Controls.RemoveByKey("sccRightTop");
                        gLeftButtom.Controls.RemoveByKey("sccLeftButtom");
                        gRightButtom.Controls.RemoveByKey("sccRightButtom");

                        scc.Panel1.Controls.Add(chart);
                        scc.Panel2.Controls.Add(gc);
                        scc.Collapsed = true;
                        scc.CollapsePanel = SplitCollapsePanel.Panel2;
                        gLeftTop.Controls.Add(scc);
                        _LeftTopGridControl = gc;
                        _LeftTopChart = chart;
                        gLeftTop.Tag = dr["Guid"];
                        Guid_1 = dr["Guid"].ToString();
                    }
                    else if (i == 2)
                    {
                        //右上角
                        scc.Name = "sccRightTop";
                        chart.Name = "sccRightTop";

                        scc.Panel1.Controls.Add(chart);
                        scc.Panel2.Controls.Add(gc);
                        scc.Collapsed = true;
                        scc.CollapsePanel = SplitCollapsePanel.Panel2;
                        gRightTop.Tag = dr["Guid"];
                        gRightTop.Controls.Add(scc);

                        _RightTopChart = chart;
                        _RightTopGridControl = gc;
                        gRightTop.Tag = dr["Guid"];
                        Guid_2 = dr["Guid"].ToString();
                    }
                    else if (i == 3)
                    {
                        //左下角
                        scc.Name = "sccLeftButtom";
                        chart.Name = "sccLeftButtom";

                        scc.Panel1.Controls.Add(chart);
                        scc.Panel2.Controls.Add(gc);
                        scc.Collapsed = true;
                        scc.CollapsePanel = SplitCollapsePanel.Panel2;
                        gLeftButtom.Controls.Add(scc);

                        _LeftButtomChart = chart;
                        _LeftButtomGridControl = gc;

                        gLeftButtom.Tag = dr["Guid"];
                        Guid_3 = dr["Guid"].ToString();
                    }
                    else if (i == 4)
                    {
                        //右下角
                        scc.Name = "sccRightButtom";
                        chart.Name = "sccRightButtom";

                        scc.Panel1.Controls.Add(chart);
                        scc.Panel2.Controls.Add(gc);
                        scc.Collapsed = true;
                        scc.CollapsePanel = SplitCollapsePanel.Panel2;
                        gRightButtom.Controls.Add(scc);

                        _RightButtomChart = chart;
                        _RightButtomGridControl = gc;
                        gRightButtom.Tag = dr["Guid"];

                        Guid_4 = dr["Guid"].ToString();
                    }

                    chart.DoubleClick -= new EventHandler(chartShowScreen_DoubleClick);
                    chart.DoubleClick += new EventHandler(chartShowScreen_DoubleClick);

                    chart.BringToFront();
                    gc.Show();
                    i++;
                }
            }
            catch (Exception ex)
            {
                this.MsgAlert(ex.Message.ToString());
            }
        }

        #endregion

        void chartShowScreen_DoubleClick(object sender, EventArgs e)
        {
            //双击小图时，全屏
            ChartControl chart = sender as ChartControl;
            string guid = "";

            ////清空过滤条件
            //_htQueryCondiction.Clear();

            switch (chart.Name)
            {
                case "sccLeftTop":
                    //左上角全屏
                    if (gLeftTop.Tag != null) guid = gLeftTop.Tag.ToString();
                    break;
                case "sccRightTop":
                    //右上角全屏
                    if (gRightTop.Tag != null) guid = gRightTop.Tag.ToString();
                    break;
                case "sccLeftButtom":
                    //左下角全屏
                    if (gLeftButtom.Tag != null) guid = gLeftButtom.Tag.ToString();
                    break;
                case "sccRightButtom":
                    //右下角全屏
                    if (gRightButtom.Tag != null) guid = gRightButtom.Tag.ToString();
                    break;
            }

            if (!string.IsNullOrEmpty(guid))
                showAllScreen(guid, "");
        }

        #region
        /// <summary>
        /// 串联字段
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="chart"></param>
        /// <param name="SeriesFieldName"></param>
        /// <param name="FieldNameX">串联字段名</param>
        /// <param name="FieldValue">串联字段值</param>
        private void CreateCutomerSeries(DataTable dtData, ChartControl chart,
            string SeriesFieldName, string FieldNameX, string FieldValue, ViewType viewType,
            string OrderFieldName, string OrderType)
        {
            //先创建Series
            string SeriesName = "";
            DataTable dtTemp = new DataTable();

            dtTemp.Columns.Add(SeriesFieldName, typeof(string));
            dtTemp.Columns.Add(OrderFieldName, typeof(string));

            foreach (DataRow dr in dtData.Rows)
            {
                DataRow drtemp = dtTemp.NewRow();
                drtemp[SeriesFieldName] = dr[SeriesFieldName];

                if (!string.IsNullOrEmpty(OrderFieldName))
                    drtemp[OrderFieldName] = dr[OrderFieldName];

                dtTemp.Rows.Add(drtemp);
            }

            string[] Arr = null;

            DataView dv = new DataView(dtTemp);

            string str = "", ot = "";

            if (string.IsNullOrEmpty(OrderFieldName))
                Arr = new string[] { SeriesFieldName };
            else
            {
                Arr = new string[] { SeriesFieldName, OrderFieldName };
            }

            DataTable dtOne = dv.ToTable(true, Arr);

            if (string.IsNullOrEmpty(OrderFieldName))
                str = SeriesFieldName;
            else
                str = OrderFieldName;

            //排序方式
            if (!string.IsNullOrEmpty(ot))
                ot = OrderType;
            else
                ot = " asc ";

            dtOne.DefaultView.Sort = str + ot;

            DataTable dtAfter = new DataTable();

            dtAfter.Columns.Add(SeriesFieldName, typeof(string));

            foreach (DataRow d in dtOne.Rows)
            {
                DataRow[] ArrDR = dtAfter.Select(SeriesFieldName + " ='" + d[0].ToString() + "'");

                if (ArrDR == null || ArrDR.Length <= 0)
                {
                    DataRow dd = dtAfter.NewRow();
                    dd[0] = d[0];
                    dtAfter.Rows.Add(dd);
                }
            }

            DataView dvOrder = new DataView(dtOne);

            //DataTable dtOrderType = dvOrder.ToTable(true, OrderFieldName);

            //创建Series
            try
            {
                foreach (DataRow drSeries in dtAfter.Rows)
                {
                    SeriesName = drSeries[0].ToString();
                    Series series = new Series(SeriesName, viewType);

                    if (string.IsNullOrEmpty(SeriesName))
                    {
                        Common.Message.MsgError("字段：" + SeriesFieldName + " 有空值，请检查！");
                        return;
                    }

                    DataRow[] ArrDR = dtData.Select(SeriesFieldName + "='" + SeriesName + "'");

                    if (ArrDR != null)
                    {
                        DataTable dtTemp2 = new DataTable();
                        dtTemp2.Columns.Add(FieldNameX, typeof(string));
                        dtTemp2.Columns.Add(FieldValue, typeof(object));

                        foreach (DataRow drr in ArrDR)
                        {
                            DataRow drtemp = dtTemp2.NewRow();
                            drtemp[0] = drr[FieldNameX].ToString();
                            drtemp[1] = drr[FieldValue].ToString();
                            dtTemp2.Rows.Add(drtemp);
                        }

                        //排序
                        dtTemp2.DefaultView.Sort = OrderFieldName + OrderType;

                        for (int ii = 0; ii < dtTemp2.Rows.Count; ii++)
                        {
                            if (string.IsNullOrEmpty(FieldValue))
                            {
                                this.MsgError(FieldNameX + " 为空值");
                                return;
                            }
                            SeriesPoint sp = new SeriesPoint(dtTemp2.Rows[ii][FieldNameX].ToString(), dtTemp2.Rows[ii][FieldValue]);
                            series.Points.Add(sp);
                        }
                    }

                    series.Label.PointOptions.PointView = PointView.Values;
                    series.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.Number;
                    series.CrosshairLabelVisibility = DevExpress.Utils.DefaultBoolean.True;
                    series.ArgumentScaleType = ScaleType.Qualitative;
                    series.ValueScaleType = ScaleType.Numerical;//数字类型
                    series.LabelsVisibility = DefaultBoolean.True;

                    chart.Series.Add(series);
                }
            }
            catch (Exception ex)
            {
                Common.Message.MsgError(ex.Message.ToString());
            }
        }

        #endregion

        #region 计算宽度
        /// <summary>
        /// 计算宽度
        /// </summary>
        private void RecalSize()
        {

            //Size size = pnlTotal.ClientSize;
            Size size = splitContainerControl1.Panel2.ClientSize;
            int p = 20;

            int width = Convert.ToInt32(Math.Ceiling((size.Width - p * 3) / 2d));
            int height = Convert.ToInt32(Math.Ceiling((size.Height - p * 3) / 2d));

            this._Width = width;
            this._Height = height;

            gLeftTop.Width = width;
            gLeftTop.Height = height;
            gRightTop.Width = width;
            gRightTop.Height = height;
            gLeftButtom.Width = width;
            gLeftButtom.Height = height;
            gRightButtom.Width = width;
            gRightButtom.Height = height;

            gLeftTop.Left = p;
            gLeftTop.Top = 5;

            gLeftButtom.Left = p;
            gLeftButtom.Top = 10 + height;

            gRightTop.Left = p * 2 + width;
            gRightTop.Top = 5;
            gRightButtom.Left = p * 2 + width;
            gRightButtom.Top = 10 + height;

            //左上角 查询配置
            lblLeftTopQuery.Click -= new EventHandler(lblLeftTopQuery_Click);
            lblLeftTopQuery.Click += new EventHandler(lblLeftTopQuery_Click);
            //左上角 全屏
            lblPLeftTopTotal.Click -= new EventHandler(lblPLeftTopTotal_Click);
            lblPLeftTopTotal.Click += new EventHandler(lblPLeftTopTotal_Click);

            //右上角 查询配置
            lblRightTopQuery.Click -= new EventHandler(lblRightTopQuery_Click);
            lblRightTopQuery.Click += new EventHandler(lblRightTopQuery_Click);

            //左下角 查询配置
            lblLeftButtomQuery.Click -= new EventHandler(lblLeftButtomQuery_Click);
            lblLeftButtomQuery.Click += new EventHandler(lblLeftButtomQuery_Click);

            //右上角 全屏
            lblPRightTopTotal.Click -= new EventHandler(lblPRightTopTotal_Click);
            lblPRightTopTotal.Click += new EventHandler(lblPRightTopTotal_Click);

            lblPRightTop.Appearance.Image = imgTools.Images["Left"];
            lblPRightButtom.Appearance.Image = imgTools.Images["Left"];
            lblPRightTop.ToolTip = "缩进";
            lblPRightButtom.ToolTip = "缩进";

            //左上角 缩进
            lblPRightTop.Click -= new EventHandler(lblPRightTop_Click);
            lblPRightTop.Click += new EventHandler(lblPRightTop_Click);

            //左下角 缩进
            lblPRightButtom.Click -= new EventHandler(lblPRightButtom_Click);
            lblPRightButtom.Click += new EventHandler(lblPRightButtom_Click);
            //左下角 全屏
            lblPLeftButtomTotal.Click -= new EventHandler(lblPLeftButtomTotal_Click);
            lblPLeftButtomTotal.Click += new EventHandler(lblPLeftButtomTotal_Click);

            //右下角 查询配置
            lblRightButtomQuery.Click -= new EventHandler(lblRightButtomQuery_Click);
            lblRightButtomQuery.Click += new EventHandler(lblRightButtomQuery_Click);

            //右下角 全屏
            lblPRightButtomTotal.Click -= new EventHandler(lblPRightButtomTotal_Click);
            lblPRightButtomTotal.Click += new EventHandler(lblPRightButtomTotal_Click);

            lblPLeftTopTotal.Appearance.Image = imgTools.Images["Total"];
            lblPLeftButtomTotal.Appearance.Image = imgTools.Images["Total"];
            lblPRightTopTotal.Appearance.Image = imgTools.Images["Total"];
            lblPRightButtomTotal.Appearance.Image = imgTools.Images["Total"];
        }

        void lblPRightButtom_Click(object sender, EventArgs e)
        {
            //左下角            
            int p = 20;
            if (lblPRightButtom.ToolTip != null && lblPRightButtom.ToolTip == "缩进")
            {
                //保存网格的原始宽度
                if (gLeftButtom.Tag == null)
                    gLeftButtom.Tag = gLeftButtom.Width;

                //当前图表，缩小四分之三
                lblPRightButtom.ToolTip = "展开";
                gLeftButtom.Width = _Width / 6;
                gRightButtom.Width = _Width / 6 * 5 + p + _Width;
                gRightButtom.Left = _Width / 6 + p * 2;

                //改变图片
                lblPRightButtom.Appearance.Image = imgTools.Images["Right"];

                gLeftButtom.Tag = lblleftTopLeft.Text;
                lblLeftButtomButtom.Text = "";
            }
            else
            {
                gLeftButtom.Width = _Width;
                gRightButtom.Left = _Width + p * 2;
                gRightButtom.Width = _Width;
                //改变图片
                lblPRightButtom.Appearance.Image = imgTools.Images["Left"];
                lblPRightButtom.ToolTip = "缩进";

                lblLeftButtomButtom.Text = gLeftButtom.Tag.ToString();
            }

        }
        #endregion

        #region  全屏

        /// <summary>
        /// 获取同级的查询条件
        /// </summary>
        private string GetRememberCondiction(int ParentId)
        {

            string strSQL = " select  * from  t_syChartOptionsCtl where serialno='" + _Guid + "'";

            DataTable dtQueryCtrl = DataService.DataServer.Proxy_WCF.OpenDataSingle(strSQL, "t_syChartOptionsCtl");

            //查询控件
            if (dtQueryCtrl == null || dtQueryCtrl.Rows.Count <= 0) return "";
            //同级的查询条件
            if (_dtQueryWhere == null || _dtQueryWhere.Rows.Count <= 0) return "";
            string where = "", fieldNameAfter = "";
            string fieldnameBefore = "";

            DataRow[] ArrWhere = _dtQueryWhere.Select(" ParentId=" + ParentId);

            foreach (DataRow drCtr in dtQueryCtrl.Rows)
            {
                foreach (DataRow drW in ArrWhere)
                {
                    fieldnameBefore = drW["ControlName"].ToString();

                    fieldNameAfter = fieldnameBefore.Replace("beg_", "").Replace("end_", "");

                    if (drCtr["FieldName"].ToString() == fieldNameAfter)
                    {
                        //判断是不是多选，即系有时间段的
                        if (fieldnameBefore.Contains("beg"))
                            where += " and " + drCtr["FieldName"].ToString() + " >= " + drW["ControlValue"].ToString();
                        else if (fieldnameBefore.Contains("end"))
                            where += " and " + drCtr["FieldName"].ToString() + " <= " + drW["ControlValue"].ToString();
                        else
                            where += " and " + drCtr["FieldName"].ToString() + " ='" + drW["ControlValue"].ToString() + "'";

                        //break;
                    }
                }
            }
            return where;
        }
        
        /// <summary>
        ///  全屏
        /// </summary>
        private void showAllScreen(string guid, string condiction)
        {
            if (string.IsNullOrEmpty(guid)) return;

            string FieldNameY = "", FieldNameX = "";
            string OrderFieldName = "", OrderType = "";
            string CustomerOrSupply = "", StandardMax = "", StandardMin = "";

            int ParentId = 0;
            DataRow[] ArrDR = null;

            _Guid = guid;
            Panel panel = new Panel();

            panel.BackColor = Color.Transparent;
            panel.Name = "panelone";
            _panel = splitContainerControl1.Panel1;
            panel.Height = (pnlTotal.ClientSize).Height;
            panel.Width = (pnlTotal.ClientSize).Width;
            panel.Top = 5;
            panel.Left = 5;

            if (string.IsNullOrEmpty(condiction))
                _htCurSQL.Remove(guid);

            try
            {
                if (treeCategory.FocusedNode != null && treeCategory.FocusedNode.ParentNode != null)
                    ParentId = treeCategory.FocusedNode.ParentNode.Id;

                string FieldX = "", FieldY = "";
                string strSQL = "", LastYearValue = "";
                int TopCount = 0;
                strSQL = " select t2.*,t1.SeriesFieldName,t1.ChartType ChartType2,t1.TopCount TopCount2,t1.OrderType  ";
                strSQL += "  from t_syChartLayout t1 right join  t_syChartOptions t2 on  ";
                strSQL += " t1.module_serialno=t2.guid  where  t2.guid='" + guid + "'";
                strSQL += "  and t1.buser='" + Common.Comm._user.UserCode + "' ";

                DataTable dtData = DataService.Data.OpenDataSingle(strSQL, "t_syChartLayout");

                dtData.TableName = "t_syChartLayout";
                ChartControl chart = null;

                Series sr = null;
                ViewType ChartType = ViewType.Bar;
                PointView pv = PointView.Argument;
                ChartTitleDockStyle titleDockStyle = ChartTitleDockStyle.Left;

                int i = 1;
                string viewType = "";
                DataRow dr = null;

                if (dtData.Rows.Count <= 0)
                    Message.MsgError("没有配置信息");

                dr = dtData.Rows[0];

                FieldNameX = dr["ChartFieldName"].ToString();
                FieldNameY = dr["ChartFieldValue"].ToString();
                StandardMax = dr["StandardMax"].ToString();
                StandardMin = dr["StandardMin"].ToString();

                chart = new ChartControl();

                #region 组装SQL

                //二级分类的查询条件，全部一致
                if (string.IsNullOrEmpty(condiction) && _dtCategorySubQuery.Rows.Count > 0)
                {
                    //查询条件
                    condiction = GetRememberCondiction(ParentId);
                }

                //获取默认值的查询条件
                if (string.IsNullOrEmpty(condiction))
                {
                    strSQL = " select  FieldName,DefaultValue from   t_syChartOptionsCtl  where defaultvalue is not null ";
                    strSQL += " and serialno='" + guid + "' and IsQuery=1 ";

                    DataTable dtCondiction = DataService.Data.OpenDataSingle(strSQL, "t_syChartOptionsCtl");

                    if (dtCondiction != null && dtCondiction.Rows.Count > 0)
                    {
                        foreach (DataRow drwhere in dtCondiction.Rows)
                        {
                            //组装SQL查询语句
                            if (drwhere["DefaultValue"].ToString().Trim() == "") continue;

                            condiction += " and " + drwhere["FieldName"].ToString() + "='" + drwhere["DefaultValue"].ToString() + "' ";
                        }
                    }
                }

                strSQL = dr["ExecSQL"].ToString().Replace("''", "'") + condiction;

                if (dr["TopCount2"].ToString() != "")
                    TopCount = int.Parse(dr["TopCount2"].ToString());
                else
                    TopCount = int.Parse(dr["TopCount"].ToString());

                //判断显示前几名客户
                if (dr["IsOrderBy"].ToString() == "True")
                {
                    CustomerOrSupply = "客户";
                }

                //判断显示前几名供应商
                if (dr["IsOrderBySupply"].ToString() == "True")
                {
                    CustomerOrSupply = "供应商";
                }

                if (dr["IsOrderBy"].ToString() == "True" || dr["IsOrderBySupply"].ToString() == "True")
                {
                    //显示前几名客户
                    LastYearValue = GetLastYearValue();

                    //如果在客户输入框有值，则
                    if (!_IsCustomerEmpty && dr["IsOrderBy"].ToString() == "True")
                        TopCount = int.MaxValue;

                    strSQL += " and " + CustomerOrSupply + " in (select top " + TopCount + " " + CustomerOrSupply + " from  " + dr["TableName"].ToString() + "  where ";

                    if (LastYearValue == "")
                        strSQL += "  年份=year(getdate())  ";
                    else
                        strSQL += "  年份=" + LastYearValue;


                    strSQL += " group by  " + CustomerOrSupply;
                    //排名字段
                    if (dr["TopFieldName"].ToString() != "")
                        strSQL += " order by sum(" + dr["TopFieldName"].ToString() + ")  ";
                    else
                        strSQL += " order by sum(" + FieldNameY + ") ";

                    if (dr["OrderType"].ToString() == "0" || dr["OrderType"].ToString() == "")
                        strSQL += " asc )";
                    else
                        strSQL += " desc )";

                }

                //统计角度            
                if (dr["SumFieldName"].ToString() != "")
                {
                    //第几行  Y轴计算公式
                    if (dr["FormulaY"].ToString() != "")
                    {
                        strSQL = " select " + dr["GroupName"].ToString() + " ," + dr["FormulaY"].ToString() + " from ( " + strSQL;
                    }
                    else  //不使用Y轴计算公式
                    {
                        strSQL = " select " + dr["GroupName"].ToString() + " ," + dr["SumFieldName"].ToString() + " from ( " + strSQL;
                    }

                    strSQL += " ) aa group by  " + dr["GroupName"].ToString();
                }

                //最外层的排序                
                if (dr["OrderFieldName"].ToString() != "")
                {
                    strSQL = " select  * from  ( " + strSQL + " ) bb  order by " + dr["OrderFieldName"].ToString();

                    if (dr["OrderType"].ToString() == "0")
                    {
                        strSQL += " asc ";
                        OrderType = " asc ";
                    }
                    else
                    {
                        strSQL += " desc  ";
                        OrderType = " desc ";
                    }

                    OrderFieldName = dr["OrderFieldName"].ToString();
                }

                #endregion

                DataTable dtTemp = null;

                if (_htCurSQL[guid] != null)
                    strSQL = _htCurSQL[guid].ToString();

                //启动调试时，显示SQL
                Common.Comm.ShowMemo(strSQL);

                dtTemp = DataService.Data.OpenDataSingle(strSQL, "t_SYModSearch_B");
                _dtScreeen = dtTemp;

                //保存全屏时的SQL
                if (!_htCurSQL.ContainsKey(guid))
                    _htCurSQL.Add(guid, strSQL);
                else
                    _htCurSQL[guid] = strSQL;

                chart.Dock = DockStyle.Fill;

                if (dr["ChartType2"].ToString() != "")
                    viewType = dr["ChartType2"].ToString();
                else
                    viewType = dr["ChartType"].ToString();

                if (viewType == "柱形图")
                    ChartType = ViewType.Bar;
                else if (viewType == "饼图")
                    ChartType = ViewType.Pie;
                else if (viewType == "曲线")
                    ChartType = ViewType.Line;
                else if (viewType == "3D柱形图")
                    ChartType = ViewType.Bar3D;
                else if (viewType == "3D饼图")
                    ChartType = ViewType.Pie3D;
                else if (viewType == "3D曲线")
                    ChartType = ViewType.Line3D;

                //图表Y轴值绑定
                string[] ArrValue;

                if (string.IsNullOrEmpty(FieldY))
                    ArrValue = dr["ChartFieldValue"].ToString().Split(',');
                else
                    ArrValue = FieldY.Split(',');

                //标准线
                CreateSeriesMax(StandardMax, FieldNameX, FieldNameY, chart, dtTemp);
                CreateSeriesMin(StandardMin, FieldNameX, FieldNameY, chart, dtTemp);

                if (dr["SeriesFieldName"].ToString() != "")
                {
                    #region 使用串联字段
                    try
                    {
                        CreateCutomerSeries(dtTemp, chart, dr["SeriesFieldName"].ToString(),
                            FieldNameX, FieldNameY, ChartType, OrderFieldName, OrderType);
                    }
                    catch (Exception ex)
                    {
                        this.MsgAlert(ex.Message.ToString());
                    }

                    #endregion
                }
                else
                {
                    #region 不使用串联字段
                    try
                    {
                        //使用个人配置
                        sr = new Series(FieldNameY, ChartType);

                        if (ChartType == ViewType.Pie)
                        {
                            PieSeriesView pieSeriesView1 = new PieSeriesView();
                            pieSeriesView1.ExplodeMode = DevExpress.XtraCharts.PieExplodeMode.All;
                            pieSeriesView1.HeightToWidthRatio = 70D;
                            pieSeriesView1.RuntimeExploding = false;
                            pieSeriesView1.SizeAsPercentage = 90D;
                            sr.View = pieSeriesView1;
                        }

                        if (dr["ShowNameOrValue"].ToString() == "显示名称")
                            pv = PointView.Argument;
                        else if (dr["ShowNameOrValue"].ToString() == "显示值")
                            pv = PointView.Values;
                        else if (dr["ShowNameOrValue"].ToString() == "显示名称和值")
                            pv = PointView.ArgumentAndValues;

                        sr.Label.PointOptions.PointView = pv;
                        sr.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.Number;//用百分比表示
                        sr.CrosshairLabelVisibility = DevExpress.Utils.DefaultBoolean.True;
                        sr.ArgumentScaleType = ScaleType.Qualitative;
                        sr.ValueScaleType = ScaleType.Numerical;//数字类型
                        sr.LabelsVisibility = DefaultBoolean.True;

                        //if (string.IsNullOrEmpty(FieldX))
                        //    sr.ArgumentDataMember = dr["ChartFieldName"].ToString();
                        //else
                        //    sr.ArgumentDataMember = FieldX;

                        sr.ArgumentDataMember = FieldNameX;

                        sr.ValueDataMembers[0] = FieldNameY;

                        sr.DataSource = dtTemp;

                        //添加到统计图上
                        chart.Series.Add(sr);

                    }
                    catch (Exception ex)
                    {
                        this.MsgAlert(ex.Message.ToString());
                    }
                    #endregion
                }

                if (dr["ChartAlign"].ToString() == "上")
                    titleDockStyle = ChartTitleDockStyle.Top;
                else if (dr["ChartAlign"].ToString() == "下")
                    titleDockStyle = ChartTitleDockStyle.Bottom;
                else if (dr["ChartAlign"].ToString() == "左")
                    titleDockStyle = ChartTitleDockStyle.Left;
                else if (dr["ChartAlign"].ToString() == "右")
                    titleDockStyle = ChartTitleDockStyle.Right;

                //设置图表标题
                ChartTitle ct = new ChartTitle();
                ct.Text = dr["ChartTitle"].ToString();
                ct.TextColor = Color.Black;//颜色
                ct.Font = new Font("Tahoma", 12);//字体
                ct.Dock = titleDockStyle;//停靠在上方
                ct.Alignment = StringAlignment.Center;//居中显示
                ct.WordWrap = true;
                chart.Titles.Add(ct);

                ChartTitle ctLeft = new ChartTitle();
                ctLeft.Text = FieldNameY; ;
                ctLeft.TextColor = Color.Black;//颜色
                ctLeft.Font = new Font("Tahoma", 12);//字体
                ctLeft.Dock = ChartTitleDockStyle.Left;
                ctLeft.Alignment = StringAlignment.Center;//居中显示
                ctLeft.WordWrap = true;
                chart.Titles.Add(ctLeft);

                //数据网格
                MyGridControl gc = new MyGridControl();
                MyGridView gv = new MyGridView();
                gc.Dock = DockStyle.Fill;
                gv.OptionsView.ShowGroupPanel = false;
                gv.OptionsView.ShowFooter = false;
                gv.OptionsBehavior.ReadOnly = true;
                gv.OptionsView.ShowViewCaption = false;
                gv.OptionsView.ShowFooter = true;
                gv.OptionsView.ShowAutoFilterRow = true;

                //右健菜单
                CreateRightButton(gc);

                //数据钻取字段
                strSQL = " select  * from  t_syChartZuan where serialno='" + guid + "'";
                _dtZuanOptions= DataService.Data.OpenDataSingle(strSQL, "t_syChartZuan");
                DataRow[] ArrZuan = null;
                foreach (DataColumn dc in dtTemp.Columns)
                {
                    MyGridColumn column = new MyGridColumn();
                    column.FieldName = dc.ColumnName;
                    column.Caption = dc.Caption;
                    column.Visible = true;
                    column.DisplayFormat.FormatString = "{0:###,###.##}";
                    column.DisplayFormat.FormatType = FormatType.Numeric;

                    RepositoryItemHyperLinkEdit link = new RepositoryItemHyperLinkEdit();

                    //设置注脚汇总方式
                    SetGridViewFooterFormula(dc, dr, column, FieldNameY);

                    //数据钻取
                    if (_dtZuanOptions != null && _dtZuanOptions.Rows.Count > 0)
                    {
                        ArrZuan = _dtZuanOptions.Select(" fieldname='" + dc.ColumnName + "' and IsUsed=True");

                        if (ArrZuan != null && ArrZuan.Length > 0)
                        {
                            column.ColumnEdit = link;

                            link.DoubleClick -= new EventHandler(link_DoubleClick);
                            link.DoubleClick += new EventHandler(link_DoubleClick); 
                        }
                    }

                    gv.Columns.Add(column);
                }

                gc.MainView = gv;
                gc.DataSource = dtTemp;

                SplitContainerControl scc = new SplitContainerControl();
                scc.Collapsed = false;
                scc.Height = pnlTotal.ClientSize.Height - 10;
                scc.Width = pnlTotal.ClientSize.Width;

                scc.Horizontal = false;
                scc.SplitterPosition = splitContainerControl1.Panel1.ClientSize.Height / 4 * 1;

                SplitContainerControl sccQuery = new SplitContainerControl();
                sccQuery.Collapsed = false;
                sccQuery.Height = pnlTotal.ClientSize.Height - 10;
                sccQuery.Width = pnlTotal.ClientSize.Width;

                sccQuery.Dock = DockStyle.Fill;
                sccQuery.Horizontal = false;
                sccQuery.SplitterPosition = 100;
                sccQuery.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

                _FieldNameList = dr["CustomQuery"].ToString();
                _scc = sccQuery;

                //获取查询控件的配置信息
                CreateQueryBySerialno();

                //创建查询条件
                //if (string.IsNullOrEmpty(condiction))
                CreateControlAfterScreen(_FieldNameList, _scc, dtData);

                //选项卡
                XtraTabControl tab = new XtraTabControl();
                tab.Dock = DockStyle.Fill;
                tab.Name = "tabChart";
                XtraTabPage pageQuery = new XtraTabPage();
                pageQuery.Text = "查询条件";
                tab.TabPages.Add(pageQuery);
                pageQuery.Controls.Add(sccQuery);

                XtraTabPage pageGrid = new XtraTabPage();
                pageGrid.Text = "数据";
            
                pageGrid.Controls.Add(gc);
                tab.TabPages.Add(pageGrid);

                XtraTabPage pageGridDetail = new XtraTabPage();
                pageGridDetail.Text = "钻取后的数据";
                _gcZuan = CreateGridControlDetail();
                pageGridDetail.Controls.Add(_gcZuan);
                tab.TabPages.Add(pageGridDetail);

                scc.Panel1.Controls.Add(tab);
                scc.Panel2.Controls.Add(chart);

                scc.CollapsePanel = SplitCollapsePanel.Panel1;
                scc.Top = 0;
                scc.Left = 0;

                gc.Show();

                chart.DoubleClick -= new EventHandler(chart_DoubleClick);
                chart.DoubleClick += new EventHandler(chart_DoubleClick);

                panel.Controls.Add(scc);
                //用于导出数据
                _GridView = gv;

                _XtraTab = tab;
            }
            catch (Exception ex)
            {
                this.MsgAlert(ex.Message.ToString());
            }

            pnlTotal.Visible = false;
            splitContainerControl1.Panel2.Controls.RemoveByKey("panelone");
            splitContainerControl1.Panel2.Controls.Add(panel);

        } 
         
        /// <summary>
        /// 创建右健菜单
        /// </summary>
        private void CreateRightButton(MyGridControl gc)
        {
            System.Windows.Forms.ContextMenuStrip contextGrid = new ContextMenuStrip();
            System.Windows.Forms.ToolStripMenuItem GridOption = new ToolStripMenuItem();

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmChart));

            GridOption.Image = ((System.Drawing.Image)(resources.GetObject("TreeOption.Image")));
            GridOption.Name = "TreeOption";
            GridOption.Size = new System.Drawing.Size(152, 22);
            GridOption.Text = "数据导出";
            GridOption.Click += new EventHandler(GridOption_Click);

            contextGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            GridOption});
            contextGrid.Name = "contextTree";
            contextGrid.Size = new System.Drawing.Size(119, 26);

            gc.ContextMenuStrip = contextGrid;
        }

        void GridOption_Click(object sender, EventArgs e)
        {
            //右健数据导出            
            FileHandler.ExportOutFromGrv(_GridView);
        }



        /// <summary>
        /// 点击全屏时，动态创建控件
        /// </summary>
        private void CreateControlAfterScreen(string FieldNameList, SplitContainerControl scc, DataTable dtTemp)
        {
            int columnCount = 10;
            int SameFieldName = 0, ParentId = 0;
            string[] ArrEnum = null;

            List<string> listFieldName = new List<string>();

            TableLayoutPanel table = new TableLayoutPanel();
            table.ColumnCount = columnCount;
            table.Dock = DockStyle.Fill;
            table.Padding = new System.Windows.Forms.Padding(3);

            table.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 150));

            string[] ArrFieldName = null;

            if (FieldNameList.Contains(';'))
                ArrFieldName = FieldNameList.Split(';');
            else
                ArrFieldName = FieldNameList.Split(',');

            if (ArrFieldName == null || ArrFieldName.Length <= 0) return;

            int RowIndex = 0, ColumnIndex = 0;

            if (treeCategory.FocusedNode.ParentNode != null)
                ParentId = treeCategory.FocusedNode.ParentNode.Id;

            ArrayList al = new ArrayList();
            al.AddRange(ArrFieldName);
            al.Sort();

            //自定义查询
            DataRow[] ArrCtrl = _dtQueryCtr.Select(" serialno='" + _Guid + "'");

            foreach (string s in al)
            {
                SameFieldName = 0;

                if (string.IsNullOrEmpty(s)) continue;

                //判断是不是开始和结束控件
                foreach (string ss in al)
                {
                    if (s == ss)
                        SameFieldName++;
                }

                //Control c = new Control();

                LabelControl lc = new LabelControl();
                lc.Text = s + "：";

                //TextEdit te1 = new TextEdit();
                Control ct = null;
                //Control cc = null;
                //自定义查询

                //ct.Text = "";
                foreach (DataRow dd in ArrCtrl)
                {
                    if (dd["FieldName"].ToString() == s)
                    {
                        if (dd["ControlType"].ToString() == "日期输入框")
                        {
                            ct = new DateEdit();
                            ((DateEdit)ct).EditValue = dd["DefaultValue"].ToString();
                            ct.Width = 100;
                        }
                        else if (dd["ControlType"].ToString() == "下拉列表框")
                        {
                            if (dd["Enum"].ToString() != "")
                                ArrEnum = dd["Enum"].ToString().Split(';');

                            if (ArrEnum == null)
                            {
                                this.MsgError("自定义查询的“下拉列表框”的枚举值为空");
                                return;
                            }

                            ct = new ComboBoxEdit();
                            ct.Width = 80;
                            ((ComboBoxEdit)ct).EditValue = dd["DefaultValue"].ToString();
                            //枚举值
                            foreach (string ss in ArrEnum)
                            {
                                if (!string.IsNullOrEmpty(ss))
                                    (ct as ComboBoxEdit).Properties.Items.Add(ss);
                            }

                        }
                        else if (dd["ControlType"].ToString() == "按钮输入框")
                        {
                            ct = new ButtonEdit();
                            ct.Tag = dd["UseSQL"];
                            ct.Width = 120;
                            ((ButtonEdit)ct).EditValue = dd["DefaultValue"].ToString();
                            //(ct as ButtonEdit).Properties.ReadOnly = true;

                            (ct as ButtonEdit).ButtonClick -= new ButtonPressedEventHandler(FrmChart_ButtonClick);
                            (ct as ButtonEdit).ButtonClick += new ButtonPressedEventHandler(FrmChart_ButtonClick);
                        }
                        else if (dd["ControlType"].ToString() == "" || dd["ControlType"].ToString() == "文本输入框")
                        {
                            ct = new TextEdit();
                            ((TextEdit)ct).EditValue = dd["DefaultValue"].ToString();
                            ct.Width = 100;
                        }


                        break;
                    }
                }

                if (ArrCtrl.Length == 0)
                    ct = new TextEdit();

                ct.Name = s;
                //ct.Tag = s;

                if (SameFieldName > 1)
                {
                    if (!listFieldName.Contains(s))
                    {
                        lc.Text = "开始" + s + "：";
                        //ct.ToolTip = "开始" + s;
                        //ct.Tag = "beg_" + s;
                        ct.Name = "beg_" + s;
                    }
                    else
                    {
                        lc.Text = "结束" + s + "：";
                        //ct.ToolTip = "结束" + s;
                        //ct.Tag = "end_" + s;
                        ct.Name = "end_" + s;
                    }
                }

                foreach (Control c in table.Controls)
                {
                    if (c is LabelControl)
                    {
                        if ((c as LabelControl).Text.ToString() == s + "：")
                        {
                            lc.Text = " 至 ";
                            ct.Name = s + "_end";
                            //ct.Tag = s + "_end";
                            //ct.ToolTip = "结束的" + s;
                        }
                    }
                }
                if (ColumnIndex > columnCount)
                {
                    RowIndex++;
                    ColumnIndex = 1;
                }

                listFieldName.Add(s);

                //显示上一次查询输入的值             
                DataRow[] ArrDr = _dtQueryWhere.Select(" ParentId='" + ParentId + "' and ControlName='" + ct.Name + "'");

                if (ArrDr != null && ArrDr.Length > 0)
                    ct.Text = ArrDr[0]["ControlValue"].ToString();

                table.Controls.Add(ct, ColumnIndex, RowIndex);
                table.Controls.Add(lc, ColumnIndex, RowIndex);

                ColumnIndex++;
            }

            SimpleButton sb = new SimpleButton();
            sb.Text = "查询";

            sb.Click -= new EventHandler(sb_Click);
            sb.Click += new EventHandler(sb_Click);

            table.Controls.Add(sb, (ColumnIndex + 1), RowIndex);
            table.SetColumnSpan(sb, 2);

            scc.Panel1.Controls.Add(table);

            //DefaultValue

            //创建图表配置
            CreateChartOption(_Guid, scc, dtTemp);
        }

        void FrmChart_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            //动态控件的按钮事件

            ButtonEdit btn = sender as ButtonEdit;
            string strSQL = "", where = "";

            if (btn.Tag != null)
                strSQL = btn.Tag.ToString();

            btn.Text = "";

            DataTable dt = Common.Comm.showSelectTextWithCheckBox("选择资料", strSQL);

            if (dt == null || dt.Rows.Count <= 0) return;

            if (dt != null && dt.Rows.Count > 1)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (string.IsNullOrEmpty(where))
                        where = "'" + dr[1].ToString() + "'";
                    else
                        where += ",'" + dr[1].ToString() + "'";
                }
            }
            else if (dt.Rows.Count == 1)
                where = dt.Rows[0][1].ToString();

            btn.Text = where;

        }


        #region 图表配置
        /// <summary>
        /// 图表配置
        /// </summary>
        private void CreateChartOption(string guid, SplitContainerControl scc, DataTable dtTemp)
        {
            //初始化串联字段

            bool blFlag = false, blFlag2 = false;
            string strSQL = "", ChartType = "", FieldNameX = "", SeriesFieldName = "", FieldNameY = "";
            string TopCount = "", OrderFieldName = "";
            int OrderType = 1;//默认是倒序
            DataTable dtChartOptions = new DataTable();

            int columnCount = 20;

            scc.Panel2.Controls.Clear();

            strSQL = " select  * from t_syChartLayout where buser='" + Common.Comm._user.UserCode + "'";
            strSQL += "  and module_serialno='" + guid + "'";

            dtChartOptions = DataService.Data.OpenDataSingle(strSQL, "t_syChartLayout");

            if (dtChartOptions != null && dtChartOptions.Rows.Count > 0)
            {
                ChartType = dtChartOptions.Rows[0]["ChartType"].ToString();
                FieldNameX = dtChartOptions.Rows[0]["FieldNameX"].ToString();
                SeriesFieldName = dtChartOptions.Rows[0]["SeriesFieldName"].ToString();
                FieldNameY = dtChartOptions.Rows[0]["FieldNameY"].ToString();
                TopCount = dtChartOptions.Rows[0]["TopCount"].ToString();
                OrderFieldName = dtChartOptions.Rows[0]["OrderFieldName"].ToString();

                if (dtChartOptions.Rows[0]["OrderType"].ToString() != "")
                    OrderType = int.Parse(dtChartOptions.Rows[0]["OrderType"].ToString());
            }

            //排序字段
            ComboBoxEdit cbeOrderFieldName = new ComboBoxEdit();

            TableLayoutPanel table = new TableLayoutPanel();
            table.ColumnCount = columnCount;
            table.Dock = DockStyle.Fill;
            table.Padding = new System.Windows.Forms.Padding(3);

            table.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
            table.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize, 150));

            string TableName = "";
            DataTable dtChartOption = new DataTable();

            strSQL = " select  * from t_syChartOptions where Guid='" + guid + "'";

            dtChartOption = DataService.Data.OpenDataSingle(strSQL, "t_syChartOptions");

            if (dtChartOption == null || dtChartOption.Rows.Count <= 0) return;

            TableName = dtChartOption.Rows[0]["TableName"].ToString();

            string[] AllFieldName = dtChartOption.Rows[0]["ShowFieldName"].ToString().Split(',');

            strSQL = " select * from   " + TableName + "  where 1=2 ";

            DataTable dtAllFieldName = DataService.Data.OpenDataSingle(strSQL, "aaa");

            LabelControl lccbeSeriesFieldName = new LabelControl();
            lccbeSeriesFieldName.Text = "串联字段名称：";

            LabelControl lccbeChartX = new LabelControl();
            lccbeChartX.Text = "图表X轴绑定：";
            lccbeChartX.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            ComboBoxEdit cbeSeriesFieldName = new ComboBoxEdit();
            cbeSeriesFieldName.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cbeSeriesFieldName.EditValue = SeriesFieldName;
            cbeSeriesFieldName.Name = "SeriesFieldName";

            ComboBoxEdit cbeChartX = new ComboBoxEdit();
            cbeChartX.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cbeChartX.EditValue = FieldNameX;
            cbeChartX.Name = "FieldNameX";

            cbeSeriesFieldName.Properties.Items.Add("");
            cbeChartX.Properties.Items.Add("");
            foreach (DataColumn dcc in dtAllFieldName.Columns)
            {
                //串联字段
                cbeSeriesFieldName.Properties.Items.Add(dcc.ColumnName.Trim());
                //X 轴字段
                cbeChartX.Properties.Items.Add(dcc.ColumnName.Trim());
            }

            //串联字段
            //table.Controls.Add(lccbeSeriesFieldName, 0, 1);
            //table.Controls.Add(cbeSeriesFieldName, 1, 1);

            //图表X轴绑定：
            //table.Controls.Add(lccbeChartX, 2, 1);
            //table.Controls.Add(cbeChartX, 3, 1);

            ComboBoxEdit cbeChartY = new ComboBoxEdit();
            cbeChartY.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cbeChartY.EditValue = FieldNameY;
            cbeChartY.Name = "FieldNameY";

            LabelControl lccbeChartY = new LabelControl();
            lccbeChartY.Text = "图表Y轴绑定：";
            lccbeChartY.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            foreach (DataColumn dc in dtAllFieldName.Columns)
            {
                blFlag = false;
                blFlag2 = false;

                if (dc.DataType == typeof(Int32) || dc.DataType == typeof(decimal)
                    || dc.DataType == typeof(double))
                {
                    //在显示字段上
                    foreach (string s in AllFieldName)
                    {
                        if (s.Trim() == dc.ColumnName.Trim())
                        {
                            blFlag = true;
                            break;
                        }
                    }
                    //串联值字段
                    if (blFlag)
                        cbeChartY.Properties.Items.Add(dc.ColumnName.Trim());
                }

                cbeOrderFieldName.Properties.Items.Add(dc.ColumnName.Trim());
            }

            //图表Y轴绑定：
            //table.Controls.Add(lccbeChartY, 4, 1);
            //table.Controls.Add(cbeChartY, 5, 1);

            ComboBoxEdit cbeChartType = new ComboBoxEdit();
            cbeChartType.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cbeChartType.EditValue = ChartType;
            cbeChartType.Name = "ChartType";

            LabelControl lccbeChartType = new LabelControl();
            lccbeChartType.Text = "图表类型：";

            string[] ArrChartType = { "柱形图", "饼图", "曲线", "3D柱形图", "3D饼图", "3D曲线" };
            cbeChartType.Properties.Items.AddRange(ArrChartType);

            //图表类型
            table.Controls.Add(lccbeChartType, 0, 1);
            table.Controls.Add(cbeChartType, 1, 1);

            //显示前几条
            TextEdit teTopCount = new TextEdit();
            teTopCount.Properties.Mask.EditMask = "\\d+";
            teTopCount.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;

            if (TopCount == "") TopCount = "10";

            teTopCount.EditValue = TopCount;
            teTopCount.Name = "TopCount";

            LabelControl lcteTopCountBefore = new LabelControl();
            lcteTopCountBefore.Text = "显示前：";
            lcteTopCountBefore.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            LabelControl lcteTopCountAfter = new LabelControl();

            if (dtTemp.Rows[0]["IsOrderBy"].ToString() == "True")
                lcteTopCountAfter.Text = " 名客户";
            else
                lcteTopCountAfter.Text = " 名供应商";

            if (dtTemp != null && (dtTemp.Rows[0]["IsOrderBy"].ToString() == "True"
                || dtTemp.Rows[0]["IsOrderBySupply"].ToString() == "True"))
            {
                table.Controls.Add(lcteTopCountBefore, 6, 1);
                table.Controls.Add(teTopCount, 7, 1);
                table.Controls.Add(lcteTopCountAfter, 8, 1);
            }

            //   
            LabelControl lcOrderFieldName = new LabelControl();
            lcOrderFieldName.Text = "排序字段：";
            lcOrderFieldName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            cbeOrderFieldName.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            cbeOrderFieldName.EditValue = OrderFieldName;
            cbeOrderFieldName.Name = "OrderFieldName";

            //table.Controls.Add(lcOrderFieldName, 2, 1);
            //table.Controls.Add(cbeOrderFieldName, 3, 1);

            RadioGroup rg = new RadioGroup();
            rg.Properties.Items.Add(new RadioGroupItem(0, "顺序"));
            rg.Properties.Items.Add(new RadioGroupItem(1, "倒序"));
            rg.Properties.Columns = 2;
            rg.Properties.BorderStyle = BorderStyles.NoBorder;
            rg.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            rg.Properties.Appearance.BackColor2 = System.Drawing.Color.Transparent;
            rg.Properties.Appearance.Options.UseBackColor = true;
            rg.Height = 25;
            rg.SelectedIndex = OrderType;
            rg.Name = "OrderType";
            table.Controls.Add(rg, 4, 1);

            scc.Panel2.Controls.Add(table);
        }

        #endregion

        void sb_Click(object sender, EventArgs e)
        {
            //全屏时的查询

            if (_scc == null) return;

            //清空输入的记录集
            _htQueryCondiction.Clear();

            //清空保存的SQL语句
            //_htCurSQL.Clear();
            _htCurSQL.Remove(_Guid);

            string strWhere = "", fieldName = "", strSQL = "", ChartType = "", TopCount = "10";
            string ctl = "";
            int OrderType = 0, ParentId = 0;
            TextEdit te = new TextEdit();

            if (treeCategory.FocusedNode != null && treeCategory.FocusedNode.ParentNode != null)
                ParentId = treeCategory.FocusedNode.ParentNode.Id;

            //客户的输入框是否有值
            _IsCustomerEmpty = true;

            try
            {
                //先保存图表配置
                //排序
                OrderType = (_scc.Panel2.Controls[0].Controls["OrderType"] as RadioGroup).SelectedIndex;

                ChartType = (_scc.Panel2.Controls[0].Controls["ChartType"] as ComboBoxEdit).Text.ToString();

                if (_scc.Panel2.Controls[0].Controls.ContainsKey("TopCount"))
                    TopCount = (_scc.Panel2.Controls[0].Controls["TopCount"] as TextEdit).Text.ToString();

                strSQL = " if exists (select null from t_syChartLayout  where module_serialno='" + _Guid + "' and  buser='" + Common.Comm._user.UserCode + "')";
                strSQL += " begin  ";
                strSQL += " update  t_syChartLayout set ";
                strSQL += " ChartType ='" + ChartType + "'";

                if (_scc.Panel2.Controls[0].Controls.ContainsKey("TopCount"))
                    strSQL += " ,TopCount =" + TopCount + "";

                strSQL += " ,OrderType =" + OrderType + "";
                strSQL += " where  module_serialno='" + _Guid + "' and buser='" + Common.Comm._user.UserCode + "'";
                strSQL += " end else ";
                strSQL += "  begin ";
                strSQL += "  insert t_syChartLayout (module_serialno,buser,ChartType,TopCount) ";
                strSQL += " select '" + _Guid + "','" + Common.Comm._user.UserCode + "','" + ChartType + "','" + TopCount + "'";
                strSQL += "  end ";

                DataService.Data.ExecuteNonQuery(strSQL);

                foreach (Control cl in _scc.Panel1.Controls[0].Controls)
                {
                    if (cl is TextEdit)
                    {
                        te = cl as TextEdit;
                        //fieldName = te.Tag.ToString();
                        fieldName = te.Name.ToString();

                        if (fieldName.Contains("beg_"))
                        {
                            ctl = fieldName.Split('_')[1];
                        }

                        //先删除同级的查询条件
                        DataRow[] ArrDr = _dtQueryWhere.Select(" ParentId='" + ParentId + "' and ControlName='" + fieldName + "'");
                        if (ArrDr != null && ArrDr.Length > 0)
                        {
                            foreach (DataRow d in ArrDr)
                                d.Delete();
                        }

                        if (te.Text.ToString().Trim() != "")
                        {
                            if (fieldName.Contains("beg"))
                            {
                                if (string.IsNullOrEmpty(strWhere))
                                    strWhere = " and " + ctl + " >= " + te.Text.Trim();
                                else
                                    strWhere += "  and  " + ctl + " >= " + te.Text.Trim();
                            }
                            else if (fieldName.Contains("end"))
                            {
                                if (string.IsNullOrEmpty(strWhere))
                                {
                                    strWhere = " and " + ctl + " <= " + te.Text.Trim();
                                }
                                else
                                {
                                    strWhere += " and " + ctl + " <= " + te.Text.Trim();
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(strWhere))
                                {
                                    //判断是不是多选
                                    if (te.Text.Split(',').Length > 1)
                                    {
                                        //多选
                                        strWhere = " and " + te.Name + "  in ( " + te.Text.Trim() + " )";
                                    }
                                    else
                                        strWhere = " and " + te.Name + " = '" + te.Text.Trim() + "'";
                                }
                                else
                                {
                                    if (te.Text.Split(',').Length > 1)
                                    {
                                        //多选
                                        strWhere += " and " + te.Name + "  in ( " + te.Text.Trim() + " ) ";
                                    }
                                    else
                                        strWhere += " and " + te.Name + " ='" + te.Text.Trim() + "'";
                                }
                            }

                            //DataRow[] ArrDr = _dtQueryWhere.Select(" Guid='" + _Guid + "' and ControlName='" + fieldName + "'");

                            DataRow drWhere = _dtQueryWhere.NewRow();
                            drWhere["Guid"] = _Guid;
                            drWhere["ParentId"] = ParentId;
                            drWhere["ControlName"] = fieldName;
                            drWhere["ControlValue"] = te.Text.Trim();
                            _dtQueryWhere.Rows.Add(drWhere);

                            if (te.Name == "客户")
                                _IsCustomerEmpty = false;
                        }
                    }
                }

                //保存二级分类下的查询条件
                if (_dtCategorySubQuery.Rows.Count > 0)
                {
                    DataRow[] ArrSub = _dtCategorySubQuery.Select(" ParentId='" + ParentId + "'");

                    if (ArrSub != null && ArrSub.Length > 0)
                        foreach (DataRow d in ArrSub)
                            d.Delete();
                }

                DataRow drSubWhere = _dtCategorySubQuery.NewRow();
                drSubWhere["ParentId"] = ParentId;
                drSubWhere["CondictionSQL"] = strWhere;

                _dtCategorySubQuery.Rows.Add(drSubWhere);
                //重新查询
                showAllScreen(_Guid, strWhere);

            }
            catch (Exception ex)
            {
                Common.Message.MsgAlert(ex.Message.ToString());
            }
        }

        void chart_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                ChartControl chart = sender as ChartControl;
                //chart.Parent.Visible = false;
                chart.Parent.Parent.Parent.Visible = false;
                pnlTotal.Visible = true;

                if (pnlTotal.Visible)
                {
                    //重新加载数据
                    DataBind("");
                    return;
                }
            }
            catch (Exception ex)
            {
                Common.Message.MsgError(ex.Message.ToString());
            }
        }

        #endregion

        #region 树形的数据配置
        /// <summary>
        /// 树形的数据配置
        /// </summary>
        private void TreeDataOption()
        {
            string guid = "";

            TreeListNode node = treeCategory.FocusedNode as TreeListNode;

            DataRow dr = (node.TreeList.GetDataRecordByNode(node) as DataRowView).Row;

            if (dr == null) return;

            if (dr["Guid"].ToString() == "") return;

            guid = dr["Guid"].ToString();

            FrmChartQueryNew frm = new FrmChartQueryNew(guid);
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (!pnlTotal.Visible)
                    showAllScreen(guid, "");
                else
                    DataBind("");
            }

        }

        #endregion

        void lblPRightTop_Click(object sender, EventArgs e)
        {
            // 右上角 收缩和展开

            int p = 20;
            if (lblPRightTop.ToolTip != null && lblPRightTop.ToolTip == "缩进")
            {
                //保存网格的原始宽度
                if (gLeftTop.Tag == null)
                    gLeftTop.Tag = gLeftTop.Width;

                //当前图表，缩小四分之三
                lblPRightTop.ToolTip = "展开";
                gLeftTop.Width = _Width / 6;
                gRightTop.Width = _Width / 6 * 5 + p + _Width;
                gRightTop.Left = _Width / 6 + p * 2;

                //改变图片
                lblPRightTop.Appearance.Image = imgTools.Images["Right"];

                gLeftTop.Tag = lblleftTopLeft.Text;
                lblleftTopLeft.Text = "";
            }
            else
            {
                gLeftTop.Width = _Width;
                gRightTop.Left = _Width + p * 2;
                gRightTop.Width = _Width;

                //改变图片
                lblPRightTop.Appearance.Image = imgTools.Images["Left"];
                lblPRightTop.ToolTip = "缩进";

                lblleftTopLeft.Text = gLeftTop.Tag.ToString();
            }
        }

        #region 查询控件的配置
        /// <summary>
        /// 查询控件的配置
        /// </summary>
        private void CreateQueryBySerialno()
        {
            string strSQL = " select  * from  t_syChartOptionsCtl where serialno='" + _Guid + "'";

            _dtQueryCtr = DataService.DataServer.Proxy_WCF.OpenDataSingle(strSQL, "t_syChartOptionsCtl");
        }
        #endregion

        #region 获取结束年份的值
        /// <summary>
        /// 获取结束年份的值
        /// </summary>
        private string GetLastYearValue()
        {
            string fieldValue = "";
            Control cc = null;

            if (_scc == null) return "";

            if (_scc.Panel1 == null || _scc.Panel1.Controls.Count <= 0) return "";

            foreach (Control cl in _scc.Panel1.Controls[0].Controls)
            {
                if (cl is TextEdit)
                {
                    cc = cl as TextEdit;

                    if (cc.Name.ToString().Contains("end_年份"))
                    {
                        fieldValue = cc.Text.Trim();
                    }
                }
            }

            return fieldValue;
        }

        #endregion

        void treeCategory_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                TreeListNode node = treeCategory.FocusedNode as TreeListNode;

                DataRow dr = (node.TreeList.GetDataRecordByNode(node) as DataRowView).Row;
                ////清空过滤条件
                //_htQueryCondiction.Clear(); 
                if (node.Level == 2)
                {
                    //显示子节点所有的图表
                    this.ShowWaitForm(true);
                    this.SetWaitFormDescription("系统正在加载数据...");
                    string ListGuid = "";
                    GetAllChildNodes(node, out ListGuid);

                    if (ListGuid != "")
                    {
                        DataBind(ListGuid);
                    }

                    this.CloseWaitForm(true);
                    return;
                }

                //显示单个图表
                if (node.Level == 3)
                    showAllScreen(dr["Guid"].ToString(), "");

            }
            catch (Exception ex)
            {
                Message.MsgAlert(ex.Message);
            }
        }

        /// <summary>
        /// 获取所有节点
        /// </summary>
        /// <param name="node"></param>
        /// <param name="list">图表GUID</param>
        private void GetAllChildNodes(TreeListNode node, out string list)
        {
            list = "";
            if (node.Nodes.Count > 0)
            {
                foreach (TreeListNode ChildNode in node.Nodes)
                {
                    DataRow dr = (ChildNode.TreeList.GetDataRecordByNode(ChildNode) as DataRowView).Row;

                    if (string.IsNullOrEmpty(list))
                        list = "'" + dr["Guid"].ToString() + "'";
                    else
                        list += ",'" + dr["Guid"].ToString() + "'";

                    if (ChildNode.Nodes.Count > 0)
                    {
                        GetAllChildNodes(ChildNode, out list);
                    }
                }
            }
        }

        #region  标准线最大值
        /// <summary>
        /// 标准线最大值
        /// </summary>
        private void CreateSeriesMax(string StandardMax, string FieldNameX, string FieldValue,
            ChartControl chart, DataTable dtData)
        {
            try
            {
                if (string.IsNullOrEmpty(StandardMax)) return;

                if (int.Parse(StandardMax) == 0) return;

                Series series = new Series("最高标准线", ViewType.Bar);

                //DataRow[] ArrDR = null;
                //ArrDR = dtData.Select(FieldValue + "='" + StandardMax + "'");

                DataView dv = new DataView(dtData);

                DataTable dtTemp = dv.ToTable(true, new string[] { FieldNameX });

                foreach (DataRow drX in dtTemp.Rows)
                {
                    SeriesPoint sp = new SeriesPoint(drX[0], StandardMax);
                    series.Points.Add(sp);
                }

                LineSeriesView lv = new LineSeriesView();
                lv.LineMarkerOptions.Visible = true;
                series.View = lv;

                series.Label.PointOptions.PointView = PointView.Values;
                series.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.Number;
                series.CrosshairLabelVisibility = DevExpress.Utils.DefaultBoolean.True;
                series.ArgumentScaleType = ScaleType.Qualitative;
                series.ValueScaleType = ScaleType.Numerical;//数字类型
                series.LabelsVisibility = DefaultBoolean.True;

                chart.Series.Add(series);
            }
            catch (Exception ex)
            {
                Common.Message.MsgError("标准线最大值:\n" + ex.Message.ToString());
            }

        }
        #endregion

        #region  标准线最小值
        /// <summary>
        /// 标准线最小值
        /// </summary>
        private void CreateSeriesMin(string StandardMin, string FieldNameX, string FieldValue,
            ChartControl chart, DataTable dtData)
        {
            try
            {
                if (string.IsNullOrEmpty(StandardMin)) return;

                if (int.Parse(StandardMin) == 0) return;

                Series series = new Series("最低标准线", ViewType.Bar);

                //DataRow[] ArrDR = null;
                //ArrDR = dtData.Select(FieldValue + "='" + StandardMax + "'");

                DataView dv = new DataView(dtData);

                DataTable dtTemp = dv.ToTable(true, new string[] { FieldNameX });

                foreach (DataRow drX in dtTemp.Rows)
                {
                    SeriesPoint sp = new SeriesPoint(drX[0], StandardMin);
                    series.Points.Add(sp);
                }

                LineSeriesView lv = new LineSeriesView();
                lv.LineMarkerOptions.Visible = true;
                series.View = lv;

                series.Label.PointOptions.PointView = PointView.Values;
                series.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.Number;
                series.CrosshairLabelVisibility = DevExpress.Utils.DefaultBoolean.True;
                series.ArgumentScaleType = ScaleType.Qualitative;
                series.ValueScaleType = ScaleType.Numerical;//数字类型
                series.LabelsVisibility = DefaultBoolean.True;

                chart.Series.Add(series);
            }
            catch (Exception ex)
            {
                Common.Message.MsgError("标准线最小值:\n" + ex.Message.ToString());
            }

        }

        #endregion

        #region 表格页脚汇总
        /// <summary>
        /// 表格页脚汇总
        /// </summary>
        private void SetGridViewFooterFormula(DataColumn dc, DataRow dr,
            MyGridColumn column, string FieldNameY)
        {
            if (dr["FooterFormula"].ToString().Trim() != "")
            {
                string[] ArrCol = dr["FooterFormula"].ToString().Trim().Split(';');
                string[] ArrField = null;

                if (ArrCol == null || ArrCol.Length <= 0) return;

                foreach (string s in ArrCol)
                {
                    if (string.IsNullOrEmpty(s)) break;

                    ArrField = s.Split(':');

                    if (ArrField == null) break;

                    if (dc.ColumnName.Trim() == ArrField[0].ToString())
                    {
                        //字段名相同                        
                        column.SummaryItem.FieldName = dc.ColumnName.Trim();
                        if (ArrField[1].ToString() == "Sum")
                        {
                            column.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                            column.SummaryItem.DisplayFormat = "合计:{0:N2}";
                        }
                        else if (ArrField[1].ToString() == "Average")
                        {
                            column.SummaryItem.DisplayFormat = "平均值:{0:N2}";
                            column.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Average;
                        }
                        else if (ArrField[1].ToString() == "Count")
                        {
                            column.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
                            column.SummaryItem.DisplayFormat = "总记录数:{0:N2}";
                        }
                        else if (ArrField[1].ToString() == "Max")
                        {
                            column.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Max;
                            column.SummaryItem.DisplayFormat = "最大值:{0:N2}";
                        }
                        else if (ArrField[1].ToString() == "Min")
                        {
                            column.SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Min;
                            column.SummaryItem.DisplayFormat = "最小值:{0:N2}";
                        }

                        break;
                    }
                }
            }

        }

        #endregion

        #region 创建数据钻取Gridconrol
        /// <summary>
        /// 创建数据钻取Gridconrol
        /// </summary>
        /// <returns></returns>
        private MyGridControl CreateGridControlDetail()
        {
            MyGridControl gc = new MyGridControl();
            MyGridView gv = new MyGridView();
            gc.Dock = DockStyle.Fill;
            gv.OptionsView.ShowGroupPanel = false;
            gv.OptionsView.ShowFooter = false;
            gv.OptionsBehavior.ReadOnly = true;
            gv.OptionsView.ShowViewCaption = false;
            gv.OptionsView.ShowFooter = true;
            gv.OptionsView.ShowAutoFilterRow = true;

            gc.MainView = gv;
            gc.Name = "gcDetail";
            return gc;
        }
        #endregion

        #region 点击数据钻取
       
        void link_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                (_gcZuan.MainView as GridView).Columns.Clear();

                RepositoryItemHyperLinkEdit rep = sender as RepositoryItemHyperLinkEdit;

                string FieldName = "", SQL = "",ParamValue="";
                int rowHandle = 0;
                // _gcZuan.DataSource = _dtQueryCtr;
                GridColumn column = _GridView.FocusedColumn;
                FieldName = column.FieldName.Trim();
                rowHandle = _GridView.FocusedRowHandle;
                
                //获取数据钻取每个字段要执行的SQL
                foreach(DataRow drr in _dtZuanOptions.Rows )
                {
                   if (drr["FieldName"].ToString() == FieldName)
                   {
                       SQL = drr["ExecSQL"].ToString();
                      break;
                   }
                }

                if (string.IsNullOrEmpty(SQL))
                {
                    this.MsgWarn("请在数据钻取配置" + FieldName + " 需要执行的SQL脚本");
                    return;
                }

                ParamValue = _GridView.GetFocusedValue().ToString();

                if (string.IsNullOrEmpty(ParamValue))
                    return;

                SQL = SQL.Replace("@Param", "'" + ParamValue + "'");

                _gcZuan.DataSource = DataService.Data.OpenDataSingle(SQL, "t_syChartZuan");

                _XtraTab.SelectedTabPageIndex = 2;

            }
            catch (Exception ex)
            {
                this.MsgAlert(ex.Message.ToString());
            }
        }
        #endregion
    

        private void menuOrder_Click(object sender, EventArgs e)
        {
            FrmChartOrder frm = new FrmChartOrder();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                DataBind("");

        }
         
        /// 打开查询窗体
        /// </summary>
        /// <param name="condiction"></param>
        /// <param name="guid"></param>
        private void OpenQuery(string condiction, string guid, int Index)
        {
            //FrmChartQuery frm = new FrmChartQuery(condiction, guid);

            if (Index == 1)
                guid = Guid_1;
            else if (Index == 2)
                guid = Guid_2;
            else if (Index == 3)
                guid = Guid_3;
            else if (Index == 4)
                guid = Guid_4;

            FrmChartQueryNew frm = new FrmChartQueryNew(guid);

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                DataBind("");
        }


        private void FrmChart_SizeChanged(object sender, EventArgs e)
        {
            RecalSize();
        }

        void lblPLeftTopTotal_Click(object sender, EventArgs e)
        {
            //左上角全屏
            if (gLeftTop.Tag != null)
                showAllScreen(gLeftTop.Tag.ToString(), "");
        }

        private void lblPRightTopTotal_Click(object sender, EventArgs e)
        {
            //右上角全屏
            if (gRightTop.Tag != null)
                showAllScreen(gRightTop.Tag.ToString(), "");
        }

        private void lblPLeftButtomTotal_Click(object sender, EventArgs e)
        {
            //左下角全屏            
            if (gLeftButtom.Tag != null)
                showAllScreen(gLeftButtom.Tag.ToString(), "");
        }

        private void lblPRightButtomTotal_Click(object sender, EventArgs e)
        {
            //右下角全屏
            if (gRightButtom.Tag != null)
                showAllScreen(gRightButtom.Tag.ToString(), "");
        }

        void lblLeftTopQuery_Click(object sender, EventArgs e)
        {
            //设置查询条件
            //左上角
            string condiction = "", guid = "";

            if (lblLeftTopQuery.Tag != null)
                condiction = lblLeftTopQuery.Tag.ToString();

            if (gLeftTop.Tag != null)
                guid = gLeftTop.Tag.ToString();

            if (string.IsNullOrEmpty(guid)) return;

            OpenQuery(condiction, guid, 1);
        }

        private void lblRightTopQuery_Click(object sender, EventArgs e)
        {
            //设置查询条件
            //右上角
            string condiction = "", guid = "";

            if (lblRightTopQuery.Tag != null)
                condiction = lblRightTopQuery.Tag.ToString();

            if (gRightTop.Tag != null)
                guid = gRightTop.Tag.ToString();

            if (string.IsNullOrEmpty(guid)) return;

            OpenQuery(condiction, guid, 2);
        }

        private void lblLeftButtomQuery_Click(object sender, EventArgs e)
        {
            //设置查询条件
            //左下角
            string condiction = "", guid = "";

            if (lblLeftButtomQuery.Tag != null)
                condiction = lblLeftButtomQuery.Tag.ToString();

            if (gLeftButtom.Tag != null)
                guid = gLeftButtom.Tag.ToString();

            if (string.IsNullOrEmpty(guid)) return;

            OpenQuery(condiction, guid, 3);
        }

        private void lblRightButtomQuery_Click(object sender, EventArgs e)
        {
            //设置查询条件
            //右下角
            string condiction = "", guid = "";

            if (lblRightButtomQuery.Tag != null)
                condiction = lblRightButtomQuery.Tag.ToString();

            if (gRightButtom.Tag != null)
                guid = gRightButtom.Tag.ToString();

            if (string.IsNullOrEmpty(guid)) return;

            OpenQuery(condiction, guid, 4);
        }

        private void TreeOption_Click(object sender, EventArgs e)
        {
            //树形数据配置
            TreeDataOption();
        }

    }
}
