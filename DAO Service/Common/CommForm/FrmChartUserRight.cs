using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraTreeList.Nodes;
using Common;

namespace Common
{
    public partial class FrmChartUserRight : FrmBaseExten
    {
        DataTable dtGroup = null;
        BindingSource bsGroup = new BindingSource();
        DataTable dtUser = null;
        BindingSource bsUser = new BindingSource();
        BindingSource bsChart = new BindingSource();

        DataTable _dtChartUser = new DataTable();
        DataTable _dtChart = new DataTable();

        public FrmChartUserRight()
        {
            InitializeComponent();

            //组
            InitGroup();
            //用户
            InitUsers();
            //图表
            InitChart();
            //图表用户
            InitChartUser();
        }

        private void InitUsers()
        {
            dtUser = DataService.Data.OpenDataSingle("select * from v_SYUsers where Del<>1", "v_SYUsers");
            bsUser.DataSource = dtUser;
            bsUser.Filter = "Del=0";
            gridUser.DataSource = bsUser;
        }

        private void InitGroup()
        {
            dtGroup = DataService.DataServer.proxy.GetUserGroup();
            bsGroup.DataSource = dtGroup;
            bsGroup.Filter = "Del=0";
            treeListGroup.DataSource = bsGroup;
            //收起所有，只展开根节点
            if (treeListGroup.Nodes.Count > 0)
            {
                treeListGroup.CollapseAll();
                treeListGroup.Nodes[0].Expanded = true;
            }
        }

        /// <summary>
        /// 图表列表
        /// </summary>
        private void InitChart()
        {
            try
            {
                DataRow thisRow = TableHandler.GetCurrentDataRow(bsUser);

                string strSQL = "";
                strSQL += " select  *,cast(0 as bit) Sel  from t_syChartOptions  where IsUse=1 order by btime desc";

                _dtChart = DataService.Data.OpenDataSingle(strSQL, "t_syChartOptions");

                strSQL = " select  * from t_syChartUser order by btime desc ";
                _dtChartUser = DataService.Data.OpenDataSingle(strSQL, "t_syChartUser");

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
        /// 图表用户
        /// </summary>
        private void InitChartUser()
        {
            string strSQL = " select  * from t_syChartUser order by btime desc ";
            _dtChartUser = DataService.Data.OpenDataSingle(strSQL, "t_syChartUser");
        }

        /// <summary>
        /// 根据角色选择图表配置信息
        /// </summary>
        private void SelectChartByUserId(string UserCode)
        {
            string where = "";

            if (_dtChart != null)
            {
                foreach (DataRow dr in _dtChart.Rows)
                {
                    dr["Sel"] = false;

                    where = " UserCode='" + UserCode + "' and Serialno_Chart='" + dr["Guid"].ToString() + "'";

                    DataRow[] ArrSelect = _dtChartUser.Select(where);

                    if (ArrSelect != null && ArrSelect.Length > 0)
                        dr["Sel"] = true;
                }
            }
        }

        void _dtChart_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            SetState(true);
        } 

        private void SetState(bool IsEnabled)
        {
            barSave.Enabled = IsEnabled;
            barCancel.Enabled = IsEnabled;
        }

        private void chkReadOnly_CheckedChanged(object sender, EventArgs e)
        {
            SetState(true);
        }

        private void gridView2_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            //点击用户列表时
            if (e.RowHandle >= 0)
            {
                DataRow dr = gridView2.GetDataRow(e.RowHandle);

                if (dr != null)
                    SelectChartByUserId(dr["Code"].ToString());
            }

        }
         
        private void treeListGroup_Click(object sender, EventArgs e)
        {
            DataRow drGroup = TableHandler.GetCurrentDataRow(bsGroup);              
            bsUser.Filter = " Del=0 and typecode like '" + drGroup["TreeCode"].ToString() + "%'";           
        }


        //保存
        protected override void BarSaveClick()
        {
            try
            {
                DataRow thisRow = TableHandler.GetCurrentDataRow(bsUser);
                string strSQL = "", guid = "";
                strSQL = " delete from  t_syChartUser where   UserCode='" + thisRow["Code"].ToString() + "'";

                BindingSource bs = gridControl1.DataSource as BindingSource;

                gridView1.CloseEditor();

                DataTable dtChart = bs.DataSource as DataTable;

                for (int i = 0; i < dtChart.Rows.Count; i++)
                {
                    if (dtChart.Rows[i]["Sel"].ToString() == "True")
                    {
                        guid = dtChart.Rows[i]["Guid"].ToString();

                        strSQL += " insert  t_syChartUser (UserCode,Serialno_Chart,BUser) ";
                        strSQL += " select  '" + thisRow["Code"].ToString() + "','" + guid + "',";
                        strSQL += "'" + Common.Comm._user.UserCode + "'";
                    }
                }

                DataService.Data.ExecuteNonQuery(strSQL);

                //重新用户绑定图表的值
                InitChartUser();

                SetState(false);
            }
            catch (Exception ex)
            {
                Common.Message.MsgError(ex.Message.ToString());
            }
        }
    }
}
