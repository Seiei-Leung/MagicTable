using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using System.ServiceModel;
using System.Xml;
using System.Drawing.Printing;


namespace Common
{
   public class ReportDev
    {

        XtraReport rept;
        private string CMD = string.Empty;//操作类型
        private string strRptName = string.Empty;//报表名称
        private string code = string.Empty;//报表编码
        DataTable dtReportFile = new DataTable();
        BindingSource bsReport = new BindingSource();
        DataSet dsReportData = new DataSet();
        private string _className = string.Empty;
        private int _PrintType = 0;//打印类别(0:普通打印；1:草拟打印)
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DataSet dsfastReport = new DataSet();

        public ReportDev(DevExpress.XtraGrid.Views.Grid.GridView gv)
        {
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            gridView1 = gv;
        }

        #region 报表设计
        /// <summary>
        /// 报表设计
        /// </summary>
        private void Design()
        {
            try
            {
                rept = new XtraReport();
                DataRowView drv = (DataRowView)gridView1.GetFocusedRow();

                //报表二进制
                byte[] ff = (byte[])drv[3];
                //报表GUID
                code = drv[5].ToString();

                //报表名称  
                strRptName = drv[1].ToString();
                MemoryStream ms = new MemoryStream(ff);
                rept.LoadLayout(ms);

                XRDesignFormEx designForm = new XRDesignFormEx();

                designForm.ReportStateChanged += new ReportStateEventHandler(designForm_ReportStateChanged);
                designForm.FormClosing += new FormClosingEventHandler(designForm_FormClosing);
                // 加载报表. 
                rept.DataSource = dsReportData;
                designForm.OpenReport(rept);
                // 打开设计器
                designForm.ShowDialog();
                designForm.Dispose();
            }
            catch (Exception ex)
            {
                //Message.MsgError(ex.Message.ToString());
                Message.MsgError(ex.Message.ToString());
            }

        }
        #endregion

        #region 保存新增
        /// <summary>
        /// 保存新增
        /// </summary>
        private void SaveAdd(byte[] ff, string RptName)
        {
            //string strSQL = " select * from t_SYReportTemp where  1=2  ";
            try
            {
                DataTable dtReport = DataService.DataServer.commonProxy.GetPrintTemplateColumn();
                DataRow dr = null;
                dr = dtReport.NewRow();

                dtReport.TableName = "t_SYReportTemp";
                dr["RptName"] = RptName;
                dr["ClassName"] = _className;
                dr["RptInfo"] = ff;
                dr["Code"] = System.Guid.NewGuid();
                dr["BTime"] = System.DateTime.Now;
                dr["ETime"] = System.DateTime.Now;
                dr["PrintType"] = _PrintType;

                dtReport.Rows.Add(dr);

                DataService.DataServer.proxy.SaveData(dtReport);
            }
            catch (Exception ex)
            {
                Message.MsgError(ex.Message.ToString());
            }
        }
        #endregion

        #region 保存修改
        /// <summary>
        /// 保存修改
        /// </summary>
        private void SaveUpdate(byte[] ff, string RptName, string code)
        {
            try
            {
                DataTable dtReport = DataService.DataServer.commonProxy.GetPrintTemplateByCode(code);

                DataRow dr = null;
                dr = dtReport.Rows[0];

                dr["RptName"] = RptName;
                dr["RptInfo"] = ff;
                dr["Code"] = code;
                dr["ETime"] = System.DateTime.Now;

                DataService.DataServer.proxy.SaveData(dtReport);
            }
            catch (Exception ex)
            {
                Message.MsgError(ex.Message.ToString());
            }

        }
        #endregion

        #region 打印预览
        /// <summary>
        /// 打印预览
        /// </summary>
        private void Preview()
        {
            try
            {
                rept = new XtraReport();
                DataRowView drv = (DataRowView)gridView1.GetFocusedRow();

                //报表二进制
                byte[] ff = (byte[])drv[3];
                //报表GUID
                code = drv[5].ToString();
                //报表名称
                strRptName = drv[1].ToString();
                MemoryStream ms = new MemoryStream(ff);
                rept.LoadLayout(ms);
                rept.DataSource = dsReportData;
                rept.ShowPreviewDialog();
                rept.Dispose();
            }
            catch (Exception ex)
            {
                Message.MsgError(ex.Message.ToString());
            }
        }
        #endregion

        #region 打印
        /// <summary>
        ///  打印
        /// </summary>
        private void Print()
        {
            try
            {
                rept = new XtraReport();
                DataRowView drv = (DataRowView)gridView1.GetFocusedRow();

                //报表二进制
                byte[] ff = (byte[])drv[3];

                //打印机名
                string strPrintName = drv[4].ToString();
                MemoryStream ms = new MemoryStream(ff);
                rept.LoadLayout(ms);
                if (strPrintName != "")
                    rept.Print(strPrintName);
                else
                    rept.PrintDialog();

                rept.Dispose();
            }
            catch (Exception ex)
            {
                Message.MsgError(ex.Message.ToString());
            }
        }
        #endregion

        void designForm_ReportStateChanged(object sender, ReportStateEventArgs e)
        {
            //只要报表发生改变就立即将状态设置为保存
            //避免系统默认保存对话框的出现
            if (e.ReportState == ReportState.Changed)
            {
                ((XRDesignFormEx)sender).DesignPanel.ReportState = ReportState.Saved;
            }
        }

        void designForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //在此处处理关闭设计器时的操作，主要用来自定义保存数据

            if (!Message.MsgConfirm("是否需要保存报表?")) return;

            System.IO.MemoryStream ms = new MemoryStream();
            rept.SaveLayout(ms);
            //FrmPromptName frm = new FrmPromptName("报表设计", "报表名称", strRptName);
            //frm.ShowDialog();

            byte[] ff = ms.ToArray();

            if (CMD == "ADD")
            {
                SaveAdd(ff, strRptName);
            }
            else
            {
                SaveUpdate(ff, strRptName, code);
            }

            //frm.Dispose();
        }

    }
}
