using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors.Repository;

namespace Common.CommForm
{
    public partial class FrmChartOption : FrmBase
    {
        private DataTable _dtFieldName = new DataTable();
        private string _Serialno = string.Empty;
        private string _TableName = "";

        public FrmChartOption()
        {
            InitializeComponent();
        }
        public FrmChartOption(string Serialno)
        {
            InitializeComponent();
            InitData(Serialno);
            RegeditControl();
        }

        void InitData(string Serialno)
        {
            string strSQL = "";
            this._Serialno = Serialno;

            strSQL = " select  * from t_syChartOptions where guid='" + Serialno + "'";
            DataTable dtData = DataService.Data.OpenDataSingle(strSQL, "t_syChartOptions");

            if (dtData == null || dtData.Rows.Count <= 0)
            {
                DataRow dr = dtData.NewRow();
                dr["Guid"] = Guid.NewGuid().ToString();
                dr["Buser"] = Common.Comm._user.UserCode;
                dtData.Rows.Add(dr);
            }
            else
            {
                _TableName = dtData.Rows[0]["TableName"].ToString();
                DataBind(_TableName);
            }

            BindingSource bs = new BindingSource();
            bs.DataSource = dtData;
            vGrid.DataSource = bs;

        }

        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegeditControl()
        {
            repBtnQuery.ButtonClick -= new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(repBtnQuery_ButtonClick);
            repBtnQuery.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(repBtnQuery_ButtonClick);

            repBtnGroup.ButtonClick -= new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(repBtnGroup_ButtonClick);
            repBtnGroup.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(repBtnGroup_ButtonClick);

        }

        void repBtnGroup_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //打开统计角度

            if (string.IsNullOrEmpty(_Serialno))
            {
                Common.Message.MsgAlert("请先保存配置");
                return;
            }

            //先保存配置
            Save("");

            FrmChartQueryAndSum frm = new FrmChartQueryAndSum(_Serialno, "SumType","");
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                InitData(_Serialno);
            }
        }

        void repBtnQuery_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //打开过滤条件
            if (string.IsNullOrEmpty(_Serialno))
            {
                Common.Message.MsgAlert("请先保存配置");
                return;
            }

            //先保存配置
            Save("");
            FrmChartQueryAndSum frm = new FrmChartQueryAndSum(_Serialno, "Query","");

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                InitData(_Serialno);
            }
        }

        private void DataBind(string TableName)
        {
            string strSQL = " select  * from  " + TableName + "  where   1=2";
            _dtFieldName = DataService.Data.OpenDataSingle(strSQL, TableName);
        }

        /// <summary>
        /// 保存单据
        /// </summary>
        private void Save(string type)
        {
            //DataTable dtData = vGrid.DataSource

            BindingSource bs = new BindingSource();
            string strSQL = "";

            bs = vGrid.DataSource as BindingSource;
            bs.EndEdit();
            DataTable dtData = (bs.DataSource as DataTable).GetChanges();

            if (dtData != null)
            {
                if (dtData.Rows[0]["TableName"].ToString().Trim() == "")
                {
                    Common.Message.MsgAlert("请输入数据表或者视图名称!");
                    return;
                }

                //组装查询SQL 

                DataService.DataServer.proxy.SaveData(dtData);

                if (type == "save")
                {
                    Common.Message.MsgAlert("保存成功!");
                }
            }

            string ShowFieldName = vGrid.Rows["ShowFieldName"].Properties.Value.ToString();
            string TableName = vGrid.Rows["TableName"].Properties.Value.ToString();
            string Filter = vGrid.Rows["Filter"].Properties.Value.ToString();

            strSQL = " select  " + ShowFieldName + "  from " + TableName;

            if (Filter != "")
                strSQL += Filter;
            else
                strSQL += " where  1=1 ";

            DataTable dtTemp = new DataTable();
            dtTemp = DataService.Data.OpenDataSingle(" select  * from t_syChartOptions where guid='" + _Serialno + "'", "t_syChartOptions");

            dtTemp.TableName = "t_syChartOptions";

            if (dtTemp.Rows.Count <= 0)
            {
                DataRow drTemp = dtTemp.NewRow();
                drTemp["ExecSQL"] = strSQL;
                _Serialno=Guid.NewGuid().ToString();
                drTemp["Guid"] = _Serialno;
                dtTemp.Rows.Add(drTemp);
            }
            else
            {
                dtTemp.Rows[0]["ExecSQL"] = strSQL;              
            }
            
            DataService.DataServer.proxy.SaveData(dtTemp);

            dtTemp.AcceptChanges();

            if (type == "save")
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        /// <summary>
        /// 清空配置
        /// </summary>
        private void ClearOption()
        {
            string strSQL = "";

            strSQL = "  delete from  t_syChartOptionsUser where Serialno_Module='" + _Serialno + "' ";
            strSQL += " delete from  t_syChartQuery where Serialno_Module='" + _Serialno + "' ";
            strSQL += "  delete  t_syChartLayout where Module_Serialno='" + _Serialno + "'";

            DataService.Data.ExecuteNonQuery(strSQL);
        }

        /// <summary>
        /// 保存前检查
        /// </summary>
        /// <returns></returns>
        private bool BeforeSave()
        {
            DataTable dt = (vGrid.DataSource as BindingSource).DataSource as DataTable;

            if (dt != null)
            {
                if (dt.Rows[0]["ShowFieldName"].ToString() == "")
                {
                    Common.Message.MsgError("请选择显示字段!");
                    return false;
                }
                //图表X轴字段
                if (dt.Rows[0]["ChartFieldName"].ToString() == "")
                {
                    Common.Message.MsgError("请选择图表X轴字段!");
                    return false;
                }
                //图表Y轴字段
                if (dt.Rows[0]["ChartFieldValue"].ToString() == "")
                {
                    Common.Message.MsgError("请选择图表Y轴字段!");
                    return false;
                }
            }

            return true;
        }

        private void vGrid_CustomRecordCellEdit(object sender, DevExpress.XtraVerticalGrid.Events.GetCustomRowCellEditEventArgs e)
        {
            if (e.Row.Properties.FieldName == "ShowFieldName")
            {
                //串联字段
                RepositoryItemCheckedComboBoxEdit repChk = new RepositoryItemCheckedComboBoxEdit();

                if (_dtFieldName != null && _dtFieldName.Columns.Count > 0)
                {
                    foreach (DataColumn dc in _dtFieldName.Columns)
                    {
                        repChk.Items.Add(dc.ColumnName.Trim());
                    }
                }

                repChk.EditValueChanged -= new EventHandler(repChk_EditValueChanged);
                repChk.EditValueChanged += new EventHandler(repChk_EditValueChanged);
                e.RepositoryItem = repChk;
            }

            if (e.Row.Properties.FieldName == "SeriesFieldName")
            {
                //串联字段
                RepositoryItemComboBox repChk = new RepositoryItemComboBox();
                repChk.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

                if (_dtFieldName != null && _dtFieldName.Columns.Count > 0)
                {
                    foreach (DataColumn dc in _dtFieldName.Columns)
                    {
                        repChk.Items.Add(dc.ColumnName.Trim());
                    }
                }
                e.RepositoryItem = repChk;
            }

            if (e.Row.Properties.FieldName == "ChartFieldName")
            {
                //图表名称绑定
                //RepositoryItemCheckedComboBoxEdit repChkSum = new RepositoryItemCheckedComboBoxEdit();
                RepositoryItemComboBox repChkSum = new RepositoryItemComboBox();
                repChkSum.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

                if (_dtFieldName != null && _dtFieldName.Columns.Count > 0)
                {
                    foreach (DataColumn dc in _dtFieldName.Columns)
                    {
                        //if (dc.DataType == typeof(string))
                            repChkSum.Items.Add(dc.ColumnName.Trim());
                    }
                }
                e.RepositoryItem = repChkSum;
            }

            if (e.Row.Properties.FieldName == "ChartFieldValue")
            {
                //图表值绑定
                //RepositoryItemCheckedComboBoxEdit repChkSum = new RepositoryItemCheckedComboBoxEdit();
                RepositoryItemComboBox repChkSum = new RepositoryItemComboBox();
                repChkSum.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;


                if (_dtFieldName != null && _dtFieldName.Columns.Count > 0)
                {
                    foreach (DataColumn dc in _dtFieldName.Columns)
                    {
                        //if (dc.DataType == typeof(double) || dc.DataType == typeof(decimal) || dc.DataType == typeof(Int32))
                            repChkSum.Items.Add(dc.ColumnName.Trim());
                    }
                }
                e.RepositoryItem = repChkSum;
            }

            if (e.Row.Properties.FieldName == "OrderFieldName")
            {
                //排序字段
                //RepositoryItemCheckedComboBoxEdit repChkSum = new RepositoryItemCheckedComboBoxEdit();
                RepositoryItemComboBox repChkSum = new RepositoryItemComboBox();
                repChkSum.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

                repChkSum.Items.Add("");
                if (_dtFieldName != null && _dtFieldName.Columns.Count > 0)
                {
                    foreach (DataColumn dc in _dtFieldName.Columns)
                    {
                        repChkSum.Items.Add(dc.ColumnName.Trim());
                    }
                }
                e.RepositoryItem = repChkSum;
            }
            //if (e.Row.Properties.FieldName == "Category")
            //{
            //    e.RepositoryItem = repCategory;
            //}
        }

        void repChk_EditValueChanged(object sender, EventArgs e)
        {
            //显示字段
            string strSQL = "";
            DataTable dt = (vGrid.DataSource as BindingSource).DataSource as DataTable;

            if (dt != null)
            {
                dt.Rows[0]["Filter"] = "";
                dt.Rows[0]["SumFieldName"] = "";
                dt.Rows[0]["ShowFieldName"] = "";
                dt.Rows[0]["Filter"] = "";
                dt.Rows[0]["SumFieldName"] = "";
                //图表X轴字段
                dt.Rows[0]["ChartFieldName"] = "";
                //图表Y轴字段
                dt.Rows[0]["ChartFieldValue"] = "";

                ClearOption();
                //先保存配置
                Save("");
            }
        }

        /// <summary>
        /// 自定义查询
        /// </summary>
        private void CustomQueryNew()
        {
            //自定义查询
            string SelectFieldName = "", strSQL = "";
            DataTable dtQuery = new DataTable();
            dtQuery.Columns.Add("字段名", typeof(string));

            foreach (DataColumn dc in _dtFieldName.Columns)
            {
                DataRow d = dtQuery.NewRow();
                d[0] = dc.Caption;
                dtQuery.Rows.Add(d);

                DataRow dd = dtQuery.NewRow();
                dd[0] = dc.Caption;
                dtQuery.Rows.Add(dd);
            }

            DataTable dtSelected = Common.Comm.showSelectText(dtQuery, "选择自定义查义字段");

            foreach (DataRow drr in dtSelected.Rows)
            {
                SelectFieldName += drr["字段名"].ToString() + ";";
            }

            strSQL = " update t_syChartOptions set CustomQuery='" + SelectFieldName + "' where Guid='" + _Serialno + "'";

            DataService.Data.ExecuteNonQuery(strSQL);

            InitData(_Serialno);
        }


        private void vGrid_CellValueChanged(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.Row.Properties.FieldName == "TableName")
                {
                    //视图和数据表
                    DataRow dr = TableHandler.GetCurrentDataRow(vGrid.DataSource as BindingSource);

                    dr["ShowFieldName"] = "";
                    dr["Filter"] = "";
                    dr["SumFieldName"] = "";
                    //图表X轴字段
                    dr["ChartFieldName"] = "";
                    //图表Y轴字段
                    dr["ChartFieldValue"] = "";

                    ClearOption();

                    DataBind(e.Value.ToString());
                }
            }
            catch (Exception ex)
            {
                Common.Message.MsgError(ex.Message.ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (BeforeSave())
                Save("save");
        }

        private void repCustomQuery_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //自定义查询
            CustomQueryNew();
        }
    }
}
