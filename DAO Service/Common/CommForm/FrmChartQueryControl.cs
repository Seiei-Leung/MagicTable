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
    public partial class FrmChartQueryControl : Form
    {
        private string _Serialno = "";
        string[] ArrFieldName = null;
        public string FieldNameX = "", FieldNameY = "";
        private Dictionary<string, string> _dicSumType = new Dictionary<string, string>();
        private DataTable _dtChartOption = new DataTable();
        public string _strGroupByFieldName = "";
        public string _showFieldName = "";


        public FrmChartQueryControl()
        {
            InitializeComponent();
        }

        public FrmChartQueryControl(string serialno, string QueryOrSum, string showFieldName)
        {
            InitializeComponent();
            this._Serialno = serialno;
            this._showFieldName = showFieldName;
            //配置界面
            GetFieldName();

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
            bool blFlag = false;
            try
            {
                BindingSource bs = new BindingSource();

                ArrFieldName = _showFieldName.Split(';');

                strSQL = " select  * from t_syChartOptionsCtl where Serialno='" + _Serialno + "'";

                DataTable dtOptionsCtl = DataService.Data.OpenDataSingle(strSQL, "t_syChartOptionsCtl");

                //检查有没有多余的字段
                foreach (DataRow dr in dtOptionsCtl.Rows)
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
                        delSQL += "  delete from   t_syChartOptionsCtl  where guid='" + dr["Guid"].ToString() + "'";
                    }
                }

                if (!string.IsNullOrEmpty(delSQL))
                    DataService.Data.ExecuteNonQuery(delSQL);

                strSQL = " select  * from t_syChartOptionsCtl where Serialno='" + _Serialno + "'";

                dtOptionsCtl = DataService.Data.OpenDataSingle(strSQL, "t_syChartOptionsCtl");

                //新增字段
                foreach (string s in ArrFieldName)
                {
                    blFlag = false;

                    foreach (DataRow drr in dtOptionsCtl.Rows)
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
                        DataRow drrr = dtOptionsCtl.NewRow();
                        drrr["FieldName"] = s.Trim();
                        drrr["Serialno"] = this._Serialno;
                        drrr["BUser"] = "System";
                        drrr["IsQuery"] = false; 

                        dtOptionsCtl.Rows.Add(drrr);
                    }
                }
                 
                bs.DataSource = dtOptionsCtl;
                gridControl1.DataSource = bs;

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
            DataTable dtGroupBy = (gridControl1.DataSource as BindingSource).DataSource as DataTable;
            dtGroupBy.TableName = "t_syChartOptionsCtl";

            DataService.DataServer.proxy.SaveData(dtGroupBy);
            //DataTable dtChange = dtGroupBy.GetChanges();

            //if (dtChange != null)
            //{
            //    DataService.DataServer.proxy.SaveData(dtChange);
            //}

            _strGroupByFieldName = "";
            //分组过滤
            foreach (DataRow dr in dtGroupBy.Rows)
            {
                if (dr.RowState == DataRowState.Deleted) continue;

                if (dr["IsQuery"].ToString() == "True")
                {
                    //是否分组                     
                    if (string.IsNullOrEmpty(_strGroupByFieldName))
                        _strGroupByFieldName = dr["FieldName"].ToString();
                    else
                        _strGroupByFieldName += "," + dr["FieldName"].ToString();

                    //多选，则重复一个字段
                    if (dr["IsMultSelect"].ToString() == "True")
                    {
                        _strGroupByFieldName += "," + dr["FieldName"].ToString();
                    }
                }
            }

            string strSQL = " update  t_syChartOptions set  CustomQuery='" + _strGroupByFieldName + "'";
            strSQL += "  where guid='" + _Serialno + "'";
            DataService.Data.ExecuteNonQuery(strSQL);

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        /// <summary>
        /// 保存需要执行的SQL
        /// </summary>
        private void SaveExecSQL()
        {


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

        private void btnOptionOK_Click(object sender, EventArgs e)
        {
            //统计角度
            SaveGroupBy();
            //SaveExecSQL();
        }


    }
}

