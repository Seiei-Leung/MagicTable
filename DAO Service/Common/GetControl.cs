using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;

namespace Common
{
    public class GetControl
    {

        #region 私有成员
        private Control ctrl = null;
        private ScrollableControl containe = null;
        private ScrollableControl formContaine = null;
        #endregion

        #region 构造函数
        /// <summary>
        /// 获取被选中的控件
        /// </summary>
        public GetControl(Control c, ScrollableControl parentContain, Form parentForm)
        {
            ctrl = c;
            this.containe = parentContain;
            this.formContaine = parentForm;
            ctrl.MouseDown += new MouseEventHandler(MouseDown);
        }

        #endregion


        private void MouseDown(object sender, MouseEventArgs e)
        {
            //if (Comm._isDesign == true)
            //{
            //    if (containe == null)
            //    {
            //        return;
            //    }
            //    if (e.Button == MouseButtons.Left)
            //    {
            //        //Debug.WriteLine("select * from tControltype where controlType='" + ctrl.GetType().ToString() + "'");
            //        Comm._dtControlAttribute = DataServer.proxy.GetControlAttributeValues(formContaine.Name.ToString(), ctrl.Name.ToString());
            //        Comm._bsControlAttribute.DataSource = Comm._dtControlAttribute;
            //        Comm._vGridControlProperties.DataSource = Comm._bsControlAttribute;
            //        Comm._selectedCtrl = ctrl;
            //        DataTable dtControltype = DataServer.proxy.GetControlTypebyType(ctrl.GetType().ToString());
            //        foreach (DevExpress.XtraVerticalGrid.Rows.EditorRow r in Comm._vGridControlProperties.Rows)
            //        {
            //            DataRow[] drs = dtControltype.Select("Attributes='" + r.Name + "'");
            //            if (drs.Length > 0)
            //            {
            //                r.Visible = true;
            //            }
            //            else
            //                r.Visible = false;
            //        }
            //    }
            //}
        }
    }
}
