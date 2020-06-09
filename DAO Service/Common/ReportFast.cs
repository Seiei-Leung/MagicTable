using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FastReport;
using FastReport.Design.StandardDesigner;
using FastReport.Editor.Syntax.Parsers;
using System.IO;

namespace Common
{
    public class ReportFast
    {
        Report rpt = null;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private string CMD = string.Empty;//操作类型
        private string strRptName = string.Empty;//报表名称
        private string code = string.Empty;//报表编码
        DataTable dtReportFile = new DataTable();
        BindingSource bsReport = new BindingSource();
        DataSet dsReportData = new DataSet();
        private string _className = string.Empty;
        private int _PrintType = 0;//打印类别(0:普通打印；1:草拟打印)

        public ReportFast(DevExpress.XtraGrid.Views.Grid.GridView gv)
        {
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            gridView1 = gv;
        }

        #region 报表设计

        private void Design()
        {
            rpt = new Report();
            DataRowView drv = (DataRowView)gridView1.GetFocusedRow();
            byte[] ff = (byte[])drv[3];
            code = drv[5].ToString();

            strRptName = drv[1].ToString();
            MemoryStream ms = new MemoryStream(ff);
            rpt.Load(ms);

           
           
        
        }

        #endregion
    }
}
