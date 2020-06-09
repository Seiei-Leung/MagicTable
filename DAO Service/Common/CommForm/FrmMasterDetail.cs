using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;

namespace Common
{
    public partial class FrmMasterDetail : FrmBase
    {

        public DataTable SysModM;
        public FrmMasterDetail()
        {
            InitializeComponent();
        }

        private void FormMasterDetail_Load(object sender, EventArgs e)
        {

            ////SysModM = Utility.Helper.GetTable("select b.Modname,b.classname,a.* from tSysModManagedl_M a left join tSysModManage b on a.serialno=b.serialno where b.classname='" + this.Name.ToString() + "'");
            //FormMainDetailLoadLayOut LoadLayOut = new FormMainDetailLoadLayOut(SysModM, this);
            //LoadLayOut.LoadControls();
            //foreach(Control ctrl in panelMaster.Controls)
            //{
            //    Utility.GetControl g = new Utility.GetControl(ctrl, panelMaster, this);
            //}    
        }
    }
}
