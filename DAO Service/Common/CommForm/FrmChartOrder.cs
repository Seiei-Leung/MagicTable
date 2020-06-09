using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Common.CommForm
{
    public partial class FrmChartOrder : FrmBase
    {
        public FrmChartOrder()
        {
            InitializeComponent();
            BindData();
            RegeditControl();

            //int i = 0;
            //foreach (Image img in imgChart.Images)
            //{
            //    img.Save("d:\\" + i.ToString() + ".png");
            //    i++;
            //}
        }

        #region 注册控件
        /// <summary>
        /// 注册控件
        /// </summary>
        private void RegeditControl()
        {
            repCheck.CheckedChanged -= new EventHandler(repCheck_CheckedChanged);
            repCheck.CheckedChanged += new EventHandler(repCheck_CheckedChanged);
        }

        void repCheck_CheckedChanged(object sender, EventArgs e)
        {
            //是否选中
            try
            {
                BindingSource bs = gridControl1.DataSource as BindingSource;

                CheckEdit ch = sender as CheckEdit;
                if (!ch.Checked) return;

                layoutView1.CloseEditor();
                bs.EndEdit();

                DataTable dtData = bs.DataSource as DataTable;

                if (dtData == null) return;

                DataRow[] ArrDR = dtData.Select(" IsSelect=1");

                DataRow dr = TableHandler.GetCurrentDataRow(bs);

                if (ArrDR != null && ArrDR.Length > 4)
                {
                    Common.Message.MsgAlert("选择的图表不能超过4个");
                    dr["IsSelect"] = 0;
                }

            }
            catch (Exception ex)
            {
                Common.Message.MsgError(ex.Message.ToString());
            }
        }

        #endregion

        #region 数据绑定

        /// <summary>
        /// 数据绑定
        /// </summary>
        private void BindData()
        {
            try
            {
                string strSQL = "";
                DataTable dtTemp = new DataTable();
                DataTable dtChart;
                //strSQL = " select  * from t_syChartOptions ";
                //strSQL = " exec sp_Chart_GetByRole '" + Common.Comm._user.UserCode + "'";

                strSQL = " exec sp_Chart_GetAllChart '" + Common.Comm._user.UserCode + "'";

                dtTemp = DataService.Data.OpenDataSingle(strSQL, "t_syChartOptions");

                dtChart = dtTemp.Clone();

                DataRow[] ArrDr = dtTemp.Select(" guid <> ''");

                foreach (DataRow dr in ArrDr)
                    dtChart.Rows.Add(dr.ItemArray);

                dtChart.Columns.Add("pic", typeof(Image));
                //dtChart.Columns.Add("IsSelect", typeof(bool));


                ////已选中的布局
                //strSQL = " select * from  t_syChartLayout where buser='" + Common.Comm._user.UserCode + "'";
                //DataTable dtLayOut = DataService.Data.OpenDataSingle(strSQL, "t_syChartLayout");

                foreach (DataRow drChart in dtChart.Rows)
                {
                    drChart["pic"] = imgChart.Images[drChart["ChartType"].ToString()];
                }

                //foreach (DataRow drLayout in dtLayOut.Rows)
                //{
                //    DataRow[] Arr = dtChart.Select(" guid='" + drLayout["module_serialno"].ToString() + "' ");

                //    if (Arr.Length > 0)
                //        Arr[0]["IsSelect"] = true;
                //}

                BindingSource bs = new BindingSource();
                bs.DataSource = dtChart;

                gridControl1.DataSource = bs;
            }
            catch (Exception ex)
            {
                Common.Message.MsgError(ex.Message.ToString());
            }
        }

        #endregion

        #region 保存
        /// <summary>
        /// 保存
        /// </summary>
        private void Save()
        {
            try
            {
                BindingSource bs = gridControl1.DataSource as BindingSource;
                layoutView1.CloseEditor();
                bs.EndEdit();
                string strSQL = "";
                int IsSelect = 0;


                DataTable dt = bs.DataSource as DataTable;
                strSQL = " delete from  t_syChartLayout where buser='" + Common.Comm._user.UserCode + "'";

                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["IsSelect"].ToString().ToLower() == "true")
                        IsSelect = 1;
                    else
                        IsSelect = 0;

                    strSQL += "  insert  t_syChartLayout (module_serialno,buser,IsSelect) ";
                    strSQL += " select  '" + dr["guid"].ToString() + "','" + Common.Comm._user.UserCode + "'," + IsSelect + "";
                }

                DataService.Data.ExecuteNonQuery(strSQL);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;

            }
            catch (Exception ex)
            {
                this.MsgAlert(ex.Message.ToString());
            }
        }

        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            //保存
            Save();
        }

    }
}
