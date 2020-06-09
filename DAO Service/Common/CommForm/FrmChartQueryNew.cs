using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;

namespace Common.CommForm
{
    public partial class FrmChartQueryNew : FrmBase
    {
        private string _strSQL = "", _Serialno = "";
        string[] ArrFieldName = null;
        public string FieldNameX = "", FieldNameY = "";
        //private string _TableName = "";
        private Dictionary<string, string> _dicSumType = new Dictionary<string, string>();
        private DataTable _dtChartOption = new DataTable();
        private DataTable _dtChartLayout = new DataTable();

        public FrmChartQueryNew()
        {
            InitializeComponent();
        }

        public FrmChartQueryNew(string serialno)
        {
            InitializeComponent();
            this._Serialno = serialno;
            //过滤条件
            GetQueryFieldName();
            //配置界面
            GetFieldName();
            //配置界面
            InitOption();
            //注册事件
            RegesterControl();
            //统计类别
            AddSumType();
            xtraTabControl1.SelectedTabPageIndex = 0;
        }

        /// <summary>
        /// 注册控件
        /// </summary>
        private void RegesterControl()
        {
            //分组显示     
            repGroupBy.CheckStateChanged -= new EventHandler(repGroupBy_CheckStateChanged);
            repGroupBy.CheckStateChanged += new EventHandler(repGroupBy_CheckStateChanged);

            //统计类别
            repSumType2.SelectedValueChanged -= new EventHandler(repSumType2_SelectedValueChanged);
            repSumType2.SelectedValueChanged += new EventHandler(repSumType2_SelectedValueChanged);
        }

        /// <summary>
        /// 统计类别
        /// </summary>
        private void AddSumType()
        {
            if (!_dicSumType.ContainsKey("求和"))
                _dicSumType.Add("求和", "Sum");

            if (!_dicSumType.ContainsKey("平均值"))
                _dicSumType.Add("平均值", "Avg");

            if (!_dicSumType.ContainsKey("最大值"))
                _dicSumType.Add("最大值", "Max");

            if (!_dicSumType.ContainsKey("最小值"))
                _dicSumType.Add("最小值", "Min");

            if (!_dicSumType.ContainsKey("计数"))
                _dicSumType.Add("计数", "Count");

        }


        /// <summary>
        /// 初始化
        /// </summary>
        private void InitOption()
        {
            //ArrFieldName
            //初始化串联字段
            if (_dtChartOption == null) return;

            bool blFlag = false, blFlag2 = false;

            try
            {
                //分组名称
                string[] ArrGroupName = _dtChartOption.Rows[0]["GroupName"].ToString().Split(',');
                //所有字段名称
                string[] AllFieldName = _dtChartOption.Rows[0]["ShowFieldName"].ToString().Split(',');
                string TableName = _dtChartOption.Rows[0]["TableName"].ToString();
                string strSQL = " select * from   " + TableName + "  where 1=2";

                DataTable dtAllFieldName = DataService.Data.OpenDataSingle(strSQL, "aaa");

                cbeSeriesFieldName.Properties.Items.Add("");
                cbeChartX.Properties.Items.Add("");
                cbeOrderFieldName.Properties.Items.Add("");

                //插入串联字段的值字段
                cbeChartY.Properties.Items.Add("");

                foreach (DataColumn dcc in dtAllFieldName.Columns)
                {
                    //串联字段
                    cbeSeriesFieldName.Properties.Items.Add(dcc.ColumnName.Trim());
                    //X 轴字段
                    cbeChartX.Properties.Items.Add(dcc.ColumnName.Trim());
                }


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
                    //else
                    //{
                    //    //字符串
                    //    foreach (string s in AllFieldName)
                    //    {
                    //        if (s.Trim() == dc.ColumnName.Trim())
                    //        {
                    //            blFlag2 = true;
                    //            break;
                    //        }
                    //    }

                    //    if (blFlag2)
                    //    {
                    //        //串联名称字段
                    //        cbeSeriesFieldName.Properties.Items.Add(dc.ColumnName.Trim());
                    //        //X 轴字段
                    //        cbeChartX.Properties.Items.Add(dc.ColumnName.Trim());
                    //    }
                    //}                    
                    cbeOrderFieldName.Properties.Items.Add(dc.ColumnName.Trim());
                }

                strSQL = " select  * from t_syChartLayout where buser='" + Common.Comm._user.UserCode + "'";
                strSQL += "  and module_serialno='" + _Serialno + "'";

                _dtChartLayout = DataService.Data.OpenDataSingle(strSQL, "t_syChartLayout");

                cbeChartType.EditValue = _dtChartLayout.Rows[0]["ChartType"].ToString();
                cbeChartX.EditValue = _dtChartLayout.Rows[0]["FieldNameX"].ToString();
                cbeSeriesFieldName.EditValue = _dtChartLayout.Rows[0]["SeriesFieldName"].ToString();
                cbeChartY.EditValue = _dtChartLayout.Rows[0]["FieldNameY"].ToString();
                teTopCount.EditValue = _dtChartLayout.Rows[0]["TopCount"].ToString();
                cbeOrderFieldName.EditValue = _dtChartLayout.Rows[0]["OrderFieldName"].ToString();

                //排顺方式
                if (_dtChartLayout.Rows[0]["OrderType"].ToString() != "")
                    rdoOrderBy.SelectedIndex = int.Parse(_dtChartLayout.Rows[0]["OrderType"].ToString());
            }
            catch (Exception ex)
            {
                Common.Message.MsgAlert(ex.Message.ToString());
            }

        }

        void repSumType2_SelectedValueChanged(object sender, EventArgs e)
        {
            //统计类别
            try
            {
                ComboBoxEdit cb = sender as ComboBoxEdit;
                int rowHandle = gvGroupBy.FocusedRowHandle;

                DataRow dr = gvGroupBy.GetDataRow(rowHandle);

                if (cb.EditValue != null && cb.EditValue.ToString() != "")
                    dr["GroupBy"] = false;
            }
            catch (Exception ex)
            {
                Common.Message.MsgAlert(ex.Message.ToString());
            }
        }

        void repGroupBy_CheckStateChanged(object sender, EventArgs e)
        {
            //分组显示
            CheckEdit ce = sender as CheckEdit;
            int rowHandle = gvGroupBy.FocusedRowHandle;

            DataRow dr = gvGroupBy.GetDataRow(rowHandle);

            if (ce.CheckState == CheckState.Checked)
            {
                dr["SumType"] = "";
            }

        }

        /// <summary>
        /// 获取所有显示的字段
        /// </summary>
        private void GetFieldName()
        {
            string strSQL = "";
            try
            {
                BindingSource bs = new BindingSource();

                strSQL = " select  * from t_syChartOptions where Guid='" + _Serialno + "'";
          
                _dtChartOption = DataService.Data.OpenDataSingle(strSQL, "t_syChartOptions");

                if (_dtChartOption != null && _dtChartOption.Rows.Count > 0)
                {
                    ArrFieldName = _dtChartOption.Rows[0]["ShowFieldName"].ToString().Split(',');
                }

                strSQL = " select  * from t_syChartOptionsUser where Serialno_Module='" + _Serialno + "'";
                strSQL += " and buser='"+Common.Comm._user.UserCode+"'";

                DataTable dtOptionsUser = DataService.Data.OpenDataSingle(strSQL, "t_syChartOptionsUser");

                if (dtOptionsUser != null && dtOptionsUser.Rows.Count > 0)
                {
                    if (dtOptionsUser.Rows.Count != ArrFieldName.Length)
                    {
                        dtOptionsUser.Rows.Clear();

                        foreach (string s in ArrFieldName)
                        {
                            DataRow dr = dtOptionsUser.NewRow();
                            dr["FieldName"] = s.Trim();
                            dr["Serialno_Module"] = this._Serialno;
                            dr["BUser"] = Common.Comm._user.UserCode;
                            dr["IsShow"] = true;
                            dtOptionsUser.Rows.Add(dr);
                        }
                    }
                }
                else
                {
                    foreach (string s in ArrFieldName)
                    {
                        DataRow dr = dtOptionsUser.NewRow();
                        dr["FieldName"] = s.Trim();
                        dr["Serialno_Module"] = this._Serialno;
                        dr["BUser"] = Common.Comm._user.UserCode;
                        dr["IsShow"] = true;
                        dtOptionsUser.Rows.Add(dr);
                    }
                }

                bs.DataSource = dtOptionsUser;
                gcGroupBy.DataSource = bs;

            }
            catch (Exception ex)
            {
                Common.Message.MsgError(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 过淲条件
        /// </summary>
        private void GetQueryFieldName()
        {
            string strSQL = "";
            int iCount = 0;
            DataTable dt = null;
            strSQL = " select  * from t_syChartQuery where buser='" + Common.Comm._user.UserCode + "'  ";
            strSQL += " and Serialno_Module='" + _Serialno + "'  order by  OrderID ";

            dt = DataService.Data.OpenDataSingle(strSQL, "t_syChartQuery");
            dt.TableName = "t_syChartQuery";

            if (dt == null || dt.Rows.Count <= 0)
            {
                strSQL = " select  * from t_syChartQuery where buser='system' ";
                strSQL += " and Serialno_Module='" + _Serialno + "'  order by  OrderID ";
                dt = DataService.Data.OpenDataSingle(strSQL, "t_syChartQuery");
            }

            iCount = dt.Rows.Count;

            for (int i = 1; i <= (5 - iCount); i++)
            {
                DataRow dr = dt.NewRow();
                dr["OrderID"] = i;
                dr["Guid"] = Guid.NewGuid().ToString();
                dr["BUser"] = Common.Comm._user.UserCode;
                dr["Serialno_Module"] = _Serialno;
                dr["Compare"] = "=";
                dt.Rows.Add(dr);
            }

            BindingSource bs = new BindingSource();
            bs.DataSource = dt;
            gcQuery.DataSource = bs;

        }

        /// <summary>
        /// 保存过滤条件
        /// </summary>
        private void SaveQuery()
        {
            DataTable dtQuery = (gcQuery.DataSource as BindingSource).DataSource as DataTable;

            dtQuery.TableName = "t_syChartQuery";
            DataTable dtChange = dtQuery.GetChanges();

            if (dtChange != null)
            {
                DataService.DataServer.proxy.SaveData(dtChange);
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>
        /// 统计角度
        /// </summary>
        private void SaveGroupBy()
        {
            DataTable dtOption = (gcGroupBy.DataSource as BindingSource).DataSource as DataTable;
            dtOption.TableName = "t_syChartOptionsUser";

            DataTable dtChange = dtOption.GetChanges();

            if (dtChange != null)
            {
                DataService.DataServer.proxy.SaveData(dtChange);
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>
        /// 保存需要执行的SQL
        /// </summary>
        private void SaveExecSQL()
        {
            string strTotalSQL = "", AndOr = "", strQuerySQL = "", TableName = "";
            string strGroupByFieldName = "", strSumFieldName = "", strGroupBySQL = "";
            string topCount = "0", OrderFieldName = "", AscOrDesc = "";
            string strSQL = " select  * from  t_syChartOptions where Guid='" + _Serialno + "'";
            string strOrderSQL = "", FieldNameX = "", FieldNameXList = "",Filter="";
            int i = 1;

            try
            {

                _dtChartOption = DataService.Data.OpenDataSingle(strSQL, "t_syChartOptions");

                DataTable dtQuery = (gcQuery.DataSource as BindingSource).DataSource as DataTable;
                DataTable dtGroupBy = (gcGroupBy.DataSource as BindingSource).DataSource as DataTable;

                //----------------------------start  图表查询 ------------------------------------------
                //查询条件
                //foreach (DataRow dr in dtQuery.Rows)
                //{
                //    if (dr["FieldName"].ToString().Trim() != ""
                //        && dr["Compare"].ToString().Trim() != "" && dr["InputValue"].ToString().Trim() != "")
                //    {
                //        if (string.IsNullOrEmpty(strQuerySQL))
                //        {
                //            strQuerySQL = " where " + dr["FieldName"].ToString().Trim() + dr["Compare"].ToString().Trim();
                //            strQuerySQL += "''" + dr["InputValue"].ToString().Trim() + "''";
                //        }
                //        else
                //        {
                //            if (string.IsNullOrEmpty(AndOr))
                //                AndOr = " and ";

                //            strQuerySQL += "  " + AndOr + "  " + dr["FieldName"].ToString().Trim() + dr["Compare"].ToString().Trim();
                //            strQuerySQL += "''" + dr["InputValue"].ToString().Trim() + "''";
                //        }
                //        AndOr = dr["AndOr"].ToString().Trim();
                //    }
                //}

                TableName = _dtChartOption.Rows[0]["TableName"].ToString();

                Filter = _dtChartOption.Rows[0]["Filter"].ToString();

                strQuerySQL = " select  " + _dtChartOption.Rows[0]["ShowFieldName"].ToString() + " from " + TableName + Filter;

             
                if (_dtChartLayout != null)
                {
                    //排序字段
                    FieldNameX = _dtChartLayout.Rows[0]["FieldNameX"].ToString();
                    topCount = _dtChartLayout.Rows[0]["FieldNameX"].ToString();
                    OrderFieldName = _dtChartLayout.Rows[0]["OrderFieldName"].ToString();
                }

                if (cbeChartX.EditValue != null)
                    FieldNameX = cbeChartX.EditValue.ToString();

                if (teTopCount.EditValue != null)
                    topCount = teTopCount.EditValue.ToString();

                if (cbeOrderFieldName.EditValue != null)
                    OrderFieldName = cbeOrderFieldName.EditValue.ToString();

                //图表查询
                //if (_dtChartOption != null)
                //{
                //    strTotalSQL = " select    " + _dtChartOption.Rows[0]["ShowFieldName"].ToString() + " from ";
                //    strTotalSQL += TableName + strQuerySQL;
                //}

                //排序               
                //----------------------------end   图表查询 ------------------------------------------

                //----------------------------start  分组 ------------------------------------------

                //分组过滤
                foreach (DataRow dr in dtGroupBy.Rows)
                {
                    if (dr["GroupBy"].ToString() == "True")
                    {
                        //是否分组 
                        if (string.IsNullOrEmpty(strGroupByFieldName))
                            strGroupByFieldName = dr["FieldName"].ToString();
                        else
                            strGroupByFieldName += "," + dr["FieldName"].ToString();
                    }

                    if (dr["SumType"].ToString() != "")
                    {
                        //统计类别
                        if (string.IsNullOrEmpty(strSumFieldName))
                            strSumFieldName = _dicSumType[dr["SumType"].ToString()] + "( " + dr["FieldName"].ToString() + " ) " + dr["FieldName"].ToString();
                        else
                            strSumFieldName += " , " + _dicSumType[dr["SumType"].ToString()] + "( " + dr["FieldName"].ToString() + " ) " + dr["FieldName"].ToString();
                    }
                }

                //组装分组的SQL
                if (!string.IsNullOrEmpty(strTotalSQL))
                {
                    TableName = " ( " + strTotalSQL + " ) tbl ";
                }

                if (!string.IsNullOrEmpty(strGroupByFieldName))
                {
                    if (!string.IsNullOrEmpty(strSumFieldName))
                    {
                        strGroupBySQL = " select  " + strGroupByFieldName + "," + strSumFieldName + " from " + TableName;
                        strGroupBySQL += " group by " + strGroupByFieldName;
                    }
                    else
                    {
                        strGroupBySQL = " select  " + strGroupByFieldName + "  from " + TableName;
                    }
                }

                if (string.IsNullOrEmpty(strGroupBySQL))
                    strGroupBySQL = strTotalSQL;

                //----------------------------end  分组 ------------------------------------------

                //排序字段
                AscOrDesc = rdoOrderBy.SelectedIndex == 0 ? " ASC " : " DESC ";

                if (!string.IsNullOrEmpty(strGroupBySQL))
                {

                    strOrderSQL = " select  " + FieldNameX + " from  ( " + strGroupBySQL + " ) aa order by ";
                    strOrderSQL += " " + OrderFieldName + " " + AscOrDesc;

                    DataTable dtTemp = DataService.Data.OpenDataSingle(strOrderSQL.Replace("''", "'"), "dt");

                    if (dtTemp != null && dtTemp.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtTemp.Rows)
                        {
                            if (string.IsNullOrEmpty(FieldNameXList))
                                FieldNameXList = "''" + dr[FieldNameX].ToString() + "''";
                            else
                            {
                                if (!FieldNameXList.Contains("'" + dr[FieldNameX].ToString() + "'"))
                                {
                                    FieldNameXList += ",''" + dr[FieldNameX].ToString() + "''";

                                    i++;

                                    if (i >= int.Parse(topCount))
                                        break;
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(FieldNameXList))
                        strGroupBySQL = " select  * from ( " + strGroupBySQL + " ) bb  where " + FieldNameX + "  in (" + FieldNameXList + ") ";
                    else
                        strGroupBySQL = " select  * from ( " + strGroupBySQL + " ) bb   ";

                    if (!string.IsNullOrEmpty(OrderFieldName))
                        strGroupBySQL += " order by " + OrderFieldName + AscOrDesc;
                }

                //保存到布局表
                strSQL = " update t_syChartLayout set  ExecSQL='" + strGroupBySQL + "' where  module_serialno='" + _Serialno + "'";
                strSQL += " and buser='" + Common.Comm._user.UserCode + "'";

                DataService.Data.ExecuteNonQuery(strSQL);
            }
            catch (Exception ex)
            {
                Common.Message.MsgAlert(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 保存图表配置
        /// </summary>
        private void SaveChartOption()
        {
            string strSQL = "", SeriesFieldName = "", FieldNameX = "";
            string ChartType = "", FieldNameY = "";
            string topCount = "", OrderFieldName = "";
            int OrderType = 0;

            try
            {
                SeriesFieldName = cbeSeriesFieldName.EditValue.ToString();
                FieldNameX = cbeChartX.EditValue.ToString();
                FieldNameY = cbeChartY.EditValue.ToString();
                ChartType = cbeChartType.EditValue.ToString();
                topCount = teTopCount.EditValue.ToString();
                OrderFieldName = cbeOrderFieldName.EditValue.ToString();
                //排序方式
                OrderType = rdoOrderBy.SelectedIndex;

                strSQL = " update  t_syChartLayout set SeriesFieldName='" + SeriesFieldName + "',";
                strSQL += " FieldNameY='" + FieldNameY + "', OrderType=" + OrderType + ",";
                strSQL += " TopCount=" + topCount + ", OrderFieldName='" + OrderFieldName + "',";
                strSQL += " FieldNameX='" + FieldNameX + "',ChartType='" + ChartType + "' where Module_serialno='" + _Serialno + "'";
                strSQL += " and buser='" + Common.Comm._user.UserCode + "' ";

                DataService.Data.ExecuteNonQuery(strSQL);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                this.MsgError(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 检查配置
        /// </summary>
        private bool CheckOption()
        {
            string SeriesFieldName = "", FieldNameX = "";
            string FieldNameY = "", FieldName = "";
            string OrderFieldName = "";
            bool blFlag = false;

            SeriesFieldName = cbeSeriesFieldName.EditValue.ToString();
            FieldNameX = cbeChartX.EditValue.ToString();
            FieldNameY = cbeChartY.EditValue.ToString();
            OrderFieldName = cbeOrderFieldName.EditValue.ToString();

            DataTable dtGroup = (gcGroupBy.DataSource as BindingSource).DataSource as DataTable;
            DataTable dtQuery = (gcQuery.DataSource as BindingSource).DataSource as DataTable;

            //判断统计角度是否为空
            if (dtGroup != null)
            {
                DataRow[] drG = dtGroup.Select(" GroupBy='True' or SumType <> '' ");
                if (drG != null && drG.Length > 0)
                    blFlag = true;
            }

            if (blFlag)
            {
                foreach (DataRow drQuery in dtQuery.Rows)
                {
                    FieldName = drQuery["FieldName"].ToString();
                    if (FieldName != "")
                    {
                        DataRow[] ArrQuery = dtGroup.Select("GroupBy =false and  SumType = '' and FieldName='" + FieldName + "'");

                        if (ArrQuery != null && ArrQuery.Length > 0)
                        {
                            this.MsgWarn("由于在过滤查询您选中了：“" + FieldName + "”字段\n所以您需要在统计角度选中：“" + FieldName + "”");
                            return false;
                        }
                    }
                }


                DataRow[] Arrdr = dtGroup.Select("GroupBy =false and  SumType = ''");

                if (Arrdr == null || Arrdr.Length <= 0) return true;

                foreach (DataRow dr in Arrdr)
                {
                    if (dr["FieldName"].ToString().Trim() == SeriesFieldName)
                    {
                        this.MsgWarn("请在统计角度选中：“" + SeriesFieldName + "”");
                        return false;

                    }
                    if (dr["FieldName"].ToString().Trim() == FieldNameX)
                    {
                        this.MsgWarn("请在统计角度选中：“" + FieldNameX + "”");
                        return false;
                    }
                    if (dr["FieldName"].ToString().Trim() == FieldNameY)
                    {
                        this.MsgWarn("请在统计角度选中：“" + FieldNameY + "”");
                        return false;
                    }

                    if (dr["FieldName"].ToString().Trim() == OrderFieldName)
                    {
                        this.MsgWarn("请在统计角度选中：“" + OrderFieldName + "”");
                        return false;
                    }
                }
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGroupClose_Click(object sender, EventArgs e)
        {
            //关闭
            this.Close();
        }
        private void gvQuery_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            //过淲条件
            if (e.Column.FieldName.ToString() == "FieldName")
            {
                RepositoryItemComboBox rep = new RepositoryItemComboBox();
                rep.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

                if (ArrFieldName == null) return;

                foreach (string s in ArrFieldName)
                {
                    rep.Items.Add(s.Trim());
                }

                rep.Items.Add(" ");
                e.RepositoryItem = rep;

            }
            else if (e.Column.FieldName.ToString() == "AndOr")
            {
                RepositoryItemComboBox rep = new RepositoryItemComboBox();
                rep.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
                rep.Items.Add("And");
                rep.Items.Add("Or");
                rep.Items.Add("");
                e.RepositoryItem = rep;
            }
        }
        private void btnQueryOK_Click(object sender, EventArgs e)
        {
            //保存
            if (!CheckOption()) return;
            //过滤条件
            SaveQuery();
            //统计角度
            SaveGroupBy();
            //图表配置
            SaveChartOption();
            //保存SQL
            SaveExecSQL();
        }
    }
}

