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
using DevExpress.XtraGrid.Views.Grid.ViewInfo;

namespace Common.CommForm
{
    public partial class FrmChartQueryAndSum : Form
    {
        private string _Serialno = "";
        string[] ArrFieldName = null;
        public string FieldNameX = "", FieldNameY = "";
        private Dictionary<string, string> _dicSumType = new Dictionary<string, string>();
        private DataTable _dtChartOption = new DataTable();

        public DevExpress.Utils.ToolTipController MyToolTipClt = null;
        DevExpress.Utils.ToolTipControllerShowEventArgs args = null;

        public string _strFilter = "", _strGroupBy = "";
        public string _showFieldName = "", _GroupName = "";


        public FrmChartQueryAndSum()
        {
            InitializeComponent();
        }

        public FrmChartQueryAndSum(string serialno, string QueryOrSum, string showFieldName)
        {
            InitializeComponent();
            this._Serialno = serialno;
            this._showFieldName = showFieldName;
            //过滤条件
            GetQueryFieldName();
            //分组界面
            GetFieldName();
            //注册事件
            RegesterControl();
            //统计类别
            AddSumType();

            if (QueryOrSum == "Query")
                xtraTabControl1.TabPages.RemoveAt(1);
            else
                xtraTabControl1.TabPages.RemoveAt(0);

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

            //btnQueryOK.MouseEnter -= new EventHandler(btnQueryOK_MouseEnter);
            //btnQueryOK.MouseEnter += new EventHandler(btnQueryOK_MouseEnter);

            //gridView1.MouseDown -= new MouseEventHandler(gridView1_MouseDown);
            //gridView1.MouseDown += new MouseEventHandler(gridView1_MouseDown);

        }

        void gridView1_MouseDown(object sender, MouseEventArgs e)
        {
            GridHitInfo downHitInfo = null;
            downHitInfo = gridView1.CalcHitInfo(new Point(e.X, e.Y));

            if (downHitInfo == null) return;
            if (downHitInfo.Column.FieldName != "FieldName") return;

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

        void repSumType2_SelectedValueChanged(object sender, EventArgs e)
        {
            //统计类别
            try
            {
                ComboBoxEdit cb = sender as ComboBoxEdit;
                int rowHandle = gridView1.FocusedRowHandle;

                DataRow dr = gridView1.GetDataRow(rowHandle);

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
            int rowHandle = gridView1.FocusedRowHandle;

            DataRow dr = gridView1.GetDataRow(rowHandle);

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
            string strSQL = "", delSQL = "";
            try
            {
                bool blFlag = false;

                BindingSource bs = new BindingSource();

                ArrFieldName = _showFieldName.Split(';');

                strSQL = " select  * from t_syChartOptionsUser where Serialno_Module='" + _Serialno + "' and buser='System' ";

                DataTable dtOptionsUser = DataService.Data.OpenDataSingle(strSQL, "t_syChartOptionsUser");

                //检查有没有多余的字段
                foreach (DataRow dr in dtOptionsUser.Rows)
                {
                    blFlag = false;
                    foreach (string ss in ArrFieldName)
                    {
                        if (ss == dr["FieldName"].ToString())
                        {
                            blFlag = true;
                            break;
                        }
                    }

                    //删除多余的列
                    if (!blFlag)
                    {
                        delSQL += "  delete from   t_syChartOptionsUser  where guid='" + dr["Guid"].ToString() + "'";
                    }
                }

                if (!string.IsNullOrEmpty(delSQL))
                    DataService.Data.ExecuteNonQuery(delSQL);

                strSQL = " select  * from t_syChartOptionsUser where Serialno_Module='" + _Serialno + "' and buser='System' ";

                dtOptionsUser = DataService.Data.OpenDataSingle(strSQL, "t_syChartOptionsUser");
                dtOptionsUser.TableName = "t_syChartOptionsUser";

                // dtOptionsUser.AcceptChanges();

                //新增字段
                foreach (string s in ArrFieldName)
                {
                    blFlag = false;

                    foreach (DataRow drr in dtOptionsUser.Rows)
                    {
                        if (s == drr["FieldName"].ToString())
                        {
                            blFlag = true;
                            break;
                        }
                    }

                    if (!blFlag)
                    {
                        //新增字段
                        DataRow drrr = dtOptionsUser.NewRow();
                        drrr["FieldName"] = s.Trim();
                        drrr["Serialno_Module"] = this._Serialno;
                        drrr["BUser"] = "System";
                        drrr["IsShow"] = true;
                        dtOptionsUser.Rows.Add(drrr);
                    }
                }

                bs.DataSource = dtOptionsUser;
                gridControl1.DataSource = bs;
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
            string strSQL = "", DelSQL = "";
            int iCount = 0;
            bool blFlag = false;

            try
            {
                ArrFieldName = _showFieldName.Split(';');

                strSQL = " select  * from t_syChartQuery where buser='System'  ";
                strSQL += " and Serialno_Module='" + _Serialno + "'  order by  OrderID ";

                DataTable dt = DataService.Data.OpenDataSingle(strSQL, "t_syChartLayout");
                dt.TableName = "t_syChartLayout";

                //检查有没有多余的字段
                foreach (DataRow dr in dt.Rows)
                {
                    blFlag = false;
                    foreach (string ss in ArrFieldName)
                    {
                        if (ss == dr["FieldName"].ToString())
                        {
                            blFlag = true;
                            break;
                        }
                    }

                    //删除多余的列
                    if (!blFlag)
                    {
                        DelSQL += "  delete from t_syChartQuery where guid='" + dr["Guid"].ToString() + "' ";
                    }
                }

                DelSQL += "  delete from  t_syChartQuery where serialno_module='" + _Serialno + "' and fieldname is null ";
                DataService.Data.ExecuteNonQuery(DelSQL);

                dt = DataService.Data.OpenDataSingle(strSQL, "t_syChartLayout");
                dt.TableName = "t_syChartLayout";

                iCount = dt.Rows.Count;

                for (int i = 1; i <= (5 - iCount); i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["OrderID"] = i;
                    dr["Guid"] = Guid.NewGuid().ToString();
                    dr["BUser"] = "System";
                    dr["Serialno_Module"] = _Serialno;
                    dr["Compare"] = "=";
                    dt.Rows.Add(dr);
                }

                BindingSource bs = new BindingSource();
                bs.DataSource = dt;
                gcQuery.DataSource = bs;

            }
            catch (Exception ex)
            {
                this.MsgError(ex.Message.ToString());
            }

        }

        /// <summary>
        /// 保存过滤条件
        /// </summary>
        private void SaveQuery()
        {
            try
            {
                string strQuerySQL = string.Empty, AndOr = "";
                string strSQL = "";

                DataTable dtQuery = (gcQuery.DataSource as BindingSource).DataSource as DataTable;

                dtQuery.TableName = "t_syChartQuery";
                DataTable dtChange = dtQuery.GetChanges();

                if (dtChange != null)
                {
                    DataService.DataServer.proxy.SaveData(dtChange);
                }

                //查询条件
                foreach (DataRow dr in dtQuery.Rows)
                {
                    if (dr["FieldName"].ToString().Trim() != ""
                        && dr["Compare"].ToString().Trim() != "" && dr["InputValue"].ToString().Trim() != "")
                    {
                        if (string.IsNullOrEmpty(strQuerySQL))
                        {
                            strQuerySQL = " where " + dr["FieldName"].ToString().Trim() + dr["Compare"].ToString().Trim();
                            strQuerySQL += "''" + dr["InputValue"].ToString().Trim() + "''";
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(AndOr))
                                AndOr = " and ";

                            strQuerySQL += "  " + AndOr + "  " + dr["FieldName"].ToString().Trim() + dr["Compare"].ToString().Trim();
                            strQuerySQL += "''" + dr["InputValue"].ToString().Trim() + "''";
                        }
                        AndOr = dr["AndOr"].ToString().Trim();
                    }
                }

                strSQL = " update  t_syChartOptions set Filter='" + strQuerySQL + "' where guid='" + _Serialno + "'";
                DataService.Data.ExecuteNonQuery(strSQL);
                _strFilter = strQuerySQL;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                Common.Message.MsgError(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        private void SaveGroupBy()
        {
            string strGroupByFieldName = "", strSumFieldName = "";

            DataTable dtGroupBy = (gridControl1.DataSource as BindingSource).DataSource as DataTable;
            dtGroupBy.TableName = "t_syChartOptionsUser";

            //DataTable dtChange = dtGroupBy.GetChanges();

            //if (dtChange != null)
            //{
            DataService.DataServer.proxy.SaveData(dtGroupBy);
            //}

            //分组过滤
            foreach (DataRow dr in dtGroupBy.Rows)
            {
                if (dr.RowState == DataRowState.Deleted) continue;


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
                //else
                //{
                //    //是否分组 
                //    if (string.IsNullOrEmpty(strGroupByFieldName))
                //        strGroupByFieldName = dr["FieldName"].ToString();
                //    else
                //        strGroupByFieldName += "," + dr["FieldName"].ToString();
                //}
            }

            string strSQL = " update  t_syChartOptions set SumFieldName='" + strSumFieldName + "',GroupName='" + strGroupByFieldName + "'";
            strSQL += "  where guid='" + _Serialno + "'";
            DataService.Data.ExecuteNonQuery(strSQL);

            _strGroupBy = strSumFieldName;
            _GroupName = strGroupByFieldName;

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>
        /// 保存需要执行的SQL
        /// </summary>
        private void SaveExecSQL()
        {
            string strTotalSQL = "", AndOr = "", strQuerySQL = "", TableName = "";
            string strGroupByFieldName = "", strSumFieldName = "", strGroupBySQL = "";
            string strSQL = " select  * from  t_syChartOptions where Guid='" + _Serialno + "'";

            try
            {
                _dtChartOption = DataService.Data.OpenDataSingle(strSQL, "t_syChartOptions");

                DataTable dtQuery = (gcQuery.DataSource as BindingSource).DataSource as DataTable;
                DataTable dtGroupBy = (gridControl1.DataSource as BindingSource).DataSource as DataTable;

                //查询条件
                foreach (DataRow dr in dtQuery.Rows)
                {
                    if (dr["FieldName"].ToString().Trim() != ""
                        && dr["Compare"].ToString().Trim() != "" && dr["InputValue"].ToString().Trim() != "")
                    {
                        if (string.IsNullOrEmpty(strQuerySQL))
                        {
                            strQuerySQL = " where " + dr["FieldName"].ToString().Trim() + dr["Compare"].ToString().Trim();
                            strQuerySQL += "''" + dr["InputValue"].ToString().Trim() + "''";
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(AndOr))
                                AndOr = " and ";

                            strQuerySQL += "  " + AndOr + "  " + dr["FieldName"].ToString().Trim() + dr["Compare"].ToString().Trim();
                            strQuerySQL += "''" + dr["InputValue"].ToString().Trim() + "''";
                        }
                        AndOr = dr["AndOr"].ToString().Trim();
                    }
                }

                TableName = _dtChartOption.Rows[0]["TableName"].ToString();

                //图表总的配置表
                if (_dtChartOption != null)
                {
                    strTotalSQL = " select  " + _dtChartOption.Rows[0]["ShowFieldName"].ToString() + " from ";
                    strTotalSQL += TableName + strQuerySQL;
                }

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
            //过滤条件
            SaveQuery();
            //SaveExecSQL();
        }

        private void btnOptionOK_Click(object sender, EventArgs e)
        {
            //统计角度
            SaveGroupBy();
            //SaveExecSQL();
        }


    }
}

