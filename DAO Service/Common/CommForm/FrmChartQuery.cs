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
    public partial class FrmChartQuery : Form
    {
        private string _strSQL = "", _Serialno = "";
        private DataTable _dtColumn = new DataTable();
        public string FieldNameX = "", FieldNameY = "";

        public FrmChartQuery()
        {
            InitializeComponent();
        }

        public FrmChartQuery(string strSQL, string serialno)
        {
            InitializeComponent();
            this._strSQL = strSQL;
            this._Serialno = serialno;

            GetFieldName();
            //统计角度
            GroupData();
        }

        /// <summary>
        /// 获取
        /// </summary>
        private void GetFieldName()
        {
            string strSQL = "";
            int iCount = 0;
            strSQL = " select * from (" + this._strSQL + ") aa where 1=2 ";

            _dtColumn = DataService.Data.OpenDataSingle(this._strSQL, "t_syChartLayout");

            if (_dtColumn == null) return;

            strSQL = " select  * from t_syChartQuery where buser='" + Common.Comm._user.UserCode + "'  ";
            strSQL += " and Serialno_Module='"+_Serialno+"'  order by  OrderID ";

            DataTable dt = DataService.Data.OpenDataSingle(strSQL, "t_syChartLayout");
            dt.TableName = "t_syChartLayout";

            iCount = dt.Rows.Count;
             
            for (int i = 1; i <= (5 - iCount); i++)
            {
                DataRow dr = dt.NewRow();
                dr["OrderID"] = i;
                dr["Guid"] = Guid.NewGuid().ToString();
                dr["BUser"] = Common.Comm._user.UserCode;
                dr["Serialno_Module"] = _Serialno;
                dt.Rows.Add(dr);
            }

            BindingSource bs = new BindingSource();
            bs.DataSource = dt;
            gridControl1.DataSource = bs;
        }

        /// <summary>
        /// 统计角度 
        /// </summary>
        private void GroupData()
        {

            if (_dtColumn == null) return;

            foreach (DataColumn dc in _dtColumn.Columns)
            {
                if (dc.DataType == typeof(string))
                    cbeX.Properties.Items.Add(dc.ColumnName.ToString());

                if (dc.DataType == typeof(decimal) ||
                     dc.DataType == typeof(Int32) || dc.DataType == typeof(double))
                    cbeY.Properties.Items.Add(dc.ColumnName.ToString());

            }
        }
        
        /// <summary>
        /// 查询条件的SQL
        /// </summary>
        private void GetSQL()
        {
            string strSQL = "", AndOr = "", strSaveSQL = "";
            DataTable dt = new DataTable();

            try
            {
                dt = (gridControl1.DataSource as BindingSource).DataSource as DataTable;

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["FieldName"].ToString().Trim() != ""
                        && dr["Compare"].ToString().Trim() != "" && dr["InputValue"].ToString().Trim() != "")
                    {
                        if (string.IsNullOrEmpty(strSQL))
                        {
                            strSQL = " where " + dr["FieldName"].ToString().Trim() + dr["Compare"].ToString().Trim();
                            strSQL += "''" + dr["InputValue"].ToString().Trim() + "''";
                        }
                        else
                        {

                            if (string.IsNullOrEmpty(AndOr))
                                AndOr = " and ";

                            strSQL += "  " +AndOr + "  " + dr["FieldName"].ToString().Trim() + dr["Compare"].ToString().Trim();
                            strSQL += "''" + dr["InputValue"].ToString().Trim() + "''";
                        }


                        AndOr = dr["AndOr"].ToString().Trim();
                    }
                }

                strSaveSQL = " update t_syChartLayout set condiction='" + strSQL + "' ";
                strSaveSQL += " where module_serialno='" + _Serialno + "' and  buser='" + Common.Comm._user.UserCode + "'";

                DataService.Data.ExecuteNonQuery(strSaveSQL);

                DataTable dtTemp = (gridControl1.DataSource as BindingSource).DataSource as DataTable;
                dtTemp.TableName = "t_syChartQuery";

                DataTable dtLayOut = dtTemp.GetChanges();

                if (dtLayOut != null)
                    DataService.DataServer.proxy.SaveData(dtLayOut);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;

            }
            catch (Exception ex)
            {
                Common.Message.MsgError(ex.Message.ToString());
            }
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            GetSQL();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void gridView1_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            if (e.Column.FieldName.ToString() == "FieldName")
            {
                RepositoryItemComboBox rep = new RepositoryItemComboBox();
                rep.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;

                foreach (DataColumn dc in _dtColumn.Columns)
                {
                    rep.Items.Add(dc.ColumnName.ToString().Trim());
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

        private void btnGroupOK_Click(object sender, EventArgs e)
        {
            if (cbeX.EditValue != null)
                FieldNameX = cbeX.EditValue.ToString();

            if (cbeY.EditValue != null)
                FieldNameY = cbeY.EditValue.ToString();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void btnGroupClose_Click(object sender, EventArgs e)
        {
            //关闭
            this.Close();
        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

