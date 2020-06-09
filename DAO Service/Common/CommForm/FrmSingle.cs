using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Common
{
    public partial class FrmSingle : FrmBase
    {
        public bool isTreeForm;
        public DataTable dtSingle = new DataTable();
        public BindingSource bsSingle = new BindingSource();
        public DataTable dtTree = new DataTable();
        public BindingSource bsTree = new BindingSource();

        public FrmSingle()
        {
            InitializeComponent();
        }

        private void barNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (Common.Comm.HasRight(this.Text, "FInsert", "新建") == false)
            //    return;
            txtCode.Properties.ReadOnly = false;
            bsSingle.AddNew();
            DataRow thisRow = TableHandler.GetCurrentDataRow(bsSingle); 
            if (thisRow != null) 
            { 
                thisRow.BeginEdit();
                thisRow["BUser"] = Common.Comm._user.UserCode;
                thisRow["EUser"] = Common.Comm._user.UserCode;
                thisRow["BTime"] = DateTime.Now;
                thisRow["ETime"] = DateTime.Now;
                if (isTreeForm == true)
                {
                    DataRow dr = TableHandler.GetCurrentDataRow(bsTree);
                    thisRow["TypeCode"] = dr["Code"].ToString();
                }
                thisRow.EndEdit(); 
            }
            BarEnable(true);
        }

        private void barEdit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            //if (Common.Comm.HasRight(this.Text, "FEdit", "编辑") == false)
            //    return;
            txtCode.Properties.ReadOnly = true;
            BarEnable(true);
        }

        private void barSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridViewMain.CloseEditor();
            gridViewMain.UpdateCurrentRow();
            Common.DataServer.proxy.SaveData(dtSingle);
            dtSingle.AcceptChanges();
            BarEnable(false);
        }

        private void barSaveExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridViewMain.CloseEditor();
            gridViewMain.UpdateCurrentRow();
            Common.DataServer.proxy.SaveData(dtSingle);
            dtSingle.AcceptChanges();
            Close();
        }

        private void barExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Close();
        }

        private void barCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gridViewMain.CloseEditor();
            gridViewMain.UpdateCurrentRow();
            dtSingle.RejectChanges();
            BarEnable(false);
        }

        public void BarEnable(bool Eidtflag)
        {
            if (Eidtflag == true)
            {
                if (isTreeForm == true)
                    treeListGroup.Enabled = false;
                xtraTabPage1.PageVisible = false;
                xtraTabPage2.PageVisible = true;
                barSave.Enabled = true;
                barSaveExit.Enabled = true;
                barCancel.Enabled = true;
                barNew.Enabled = false;
                barCancellation.Enabled = false;
                barEdit.Enabled = false;
            }
            else
            {
                if (isTreeForm == true)
                    treeListGroup.Enabled = true;
                xtraTabPage1.PageVisible = true;
                xtraTabPage2.PageVisible = false;
                barSave.Enabled = false;
                barSaveExit.Enabled = false;
                barCancel.Enabled = false;
                barNew.Enabled = true;
                barCancellation.Enabled = true;
                barEdit.Enabled = true;
            }
        }

        private void barCancellation_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (Common.Comm.HasRight(this.Text, "FDel", "删除") == false)
            //    return;
            if(bsSingle.Current != null)
            {
                if (this.MsgConfirm("确实要删除记录吗？") == true)
                {
                    bsSingle.RemoveCurrent();
                    gridViewMain.CloseEditor();
                    gridViewMain.UpdateCurrentRow();
                    Common.DataServer.proxy.SaveData(dtSingle);
                    dtSingle.AcceptChanges();
                }
            }
        }

        private void FormSingle_Load(object sender, EventArgs e)
        {
            
            //dtSingle = proxy.GetUserGroup();
            
            formLoad();
            if (isTreeForm == true)
                panelLeft.Visible = true;
            else
                panelLeft.Visible = false;
            bsSingle.DataSource = dtSingle;
            gridMain.DataSource = bsSingle;
            gridBs.DataSource = bsSingle;
            bsTree.DataSource = dtTree;
            treeListGroup.DataSource = bsTree;
            txtCode.DataBindings.Add("EditValue", bsSingle, "Code", false, DataSourceUpdateMode.OnPropertyChanged);
            txtName.DataBindings.Add("EditValue", bsSingle, "Name", false, DataSourceUpdateMode.OnPropertyChanged);
            txtMnemonicCode.DataBindings.Add("EditValue", bsSingle, "MnemonicCode", false, DataSourceUpdateMode.OnPropertyChanged);
            
        }

        public virtual void formLoad()
        {
        }

        private void treeListGroup_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
        {
            if (isTreeForm == true)
            {
                DataRow drTree = TableHandler.GetCurrentDataRow(bsTree);
                bsSingle.Filter = " TypeCode like '" + drTree["Code"] + "%'";
            }
        }

        private void treeListGroup_CustomDrawNodeImages(object sender, DevExpress.XtraTreeList.CustomDrawNodeImagesEventArgs e)
        {
            e.SelectImageIndex = 7;
        }

        private void barPrintV_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (Common.Comm.HasRight(this.Text, "FPrint", "打印") == false)
            //    return;
        }

        private void barExportIn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (Common.Comm.HasRight(this.Text, "FImport", "导入") == false)
            //    return;
        }

        private void barExportOut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //if (Common.Comm.HasRight(this.Text, "FExport", "导出") == false)
            //    return;
        }
    }
}
