using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraTreeList.Nodes;
using Common;

namespace Common
{
    public partial class FrmChartRoleRight : Common.FrmBaseExten
    {
        DataTable dtRole = null;
        BindingSource bsRole = new BindingSource();
        BindingSource bsChart = new BindingSource();
        DataTable _dtChartUser = new DataTable();
        DataTable _dtChart = new DataTable();

        public FrmChartRoleRight()
        {
            InitializeComponent();
            //角色
            InitUsers();
            //图表配置
            InitChart();

            SetState(false);
        }

        private void InitUsers()
        {
            try
            {
                string strSQL = "";
                strSQL = " select Id Guid,[Name],Code,Del from t_SYRole  where Del<>1 order by  [Name] ";
                dtRole = DataService.Data.OpenDataSingle(strSQL, "t_SYRole");
                bsRole.DataSource = dtRole;
                bsRole.Filter = "Del=0";
                gcRole.DataSource = bsRole;
            }
            catch (Exception ex)
            {
                Common.Message.MsgError(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 图表列表
        /// </summary>
        private void InitChart()
        {
            try
            {
                DataRow thisRow = TableHandler.GetCurrentDataRow(bsRole);

                string strSQL = "";
                strSQL += " select  *,cast(0 as bit) Sel  from t_syChartOptions  where IsUse=1  order by btime desc";

                _dtChart = DataService.Data.OpenDataSingle(strSQL, "t_syChartOptions");

                BindingUser();

                bsChart.DataSource = _dtChart;
                gridControl1.DataSource = bsChart;

                _dtChart.RowChanged -= new DataRowChangeEventHandler(_dtChart_RowChanged);
                _dtChart.RowChanged += new DataRowChangeEventHandler(_dtChart_RowChanged);

            }
            catch (Exception ex)
            {
                Common.Message.MsgError(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 绑定客户
        /// </summary>
        private void BindingUser()
        {

            string strSQL = " select  * from t_syChartRole where buser='" + Common.Comm._user.UserCode + "' ";

            _dtChartUser = DataService.Data.OpenDataSingle(strSQL, "t_syChartRole");

        }

        void _dtChart_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            SetState(true);
        }

        /// <summary>
        /// 根据角色选择图表配置信息
        /// </summary>
        private void SelectChartByRole(string Serialno_Role)
        {
            string where = "";

            if (_dtChart != null)
            {
                foreach (DataRow dr in _dtChart.Rows)
                {
                    dr["Sel"] = false;

                    where = " Serialno_Role='" + Serialno_Role + "' and Serialno_Chart='" + dr["Guid"].ToString() + "'";

                    DataRow[] ArrSelect = _dtChartUser.Select(where);

                    if (ArrSelect != null && ArrSelect.Length > 0)
                        dr["Sel"] = true;
                }
            }
        }

        //保存
        protected override void BarSaveClick()
        {
            try
            {
                DataRow thisRow = TableHandler.GetCurrentDataRow(bsRole);
                string strSQL = "", guid = "";
                strSQL = " delete from  t_syChartRole where  Serialno_Role='" + thisRow["Guid"].ToString() + "'";

                BindingSource bs = gridControl1.DataSource as BindingSource;

                gridView1.CloseEditor();

                DataTable dtChart = bs.DataSource as DataTable;

                for (int i = 0; i < dtChart.Rows.Count; i++)
                {
                    if (dtChart.Rows[i]["Sel"].ToString() == "True")
                    {
                        guid = dtChart.Rows[i]["Guid"].ToString();

                        strSQL += " insert  t_syChartRole (Serialno_Role,Serialno_Chart,BUser) ";
                        strSQL += " select  '" + thisRow["Guid"].ToString() + "','" + guid + "',";
                        strSQL += "'" + Common.Comm._user.UserCode + "'";
                    }
                }

                DataService.Data.ExecuteNonQuery(strSQL);

                //重新用户绑定图表的值
                BindingUser();

                SetState(false);
            }
            catch (Exception ex)
            {
                Common.Message.MsgError(ex.Message.ToString());
            }
        }


        private void treeList_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            SetState(true);
        }

        private void SetState(bool IsEnabled)
        {
            barSave.Enabled = IsEnabled;
            barCancel.Enabled = IsEnabled;
        }

        private void gvRole_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.RowHandle >= 0)
            {
                DataRow dr = gvRole.GetDataRow(e.RowHandle);

                if (dr != null)
                    SelectChartByRole(dr["Guid"].ToString());
            }
        }
    }
}
