using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using DevExpress.XtraEditors;
using Common;

namespace Common
{
    /// <summary>
    /// 创建人：郑志冲
    /// <para>日期：2012-10-10</para>
    /// <para>设置容器中控件可移动、调整大小(按下鼠标左键)</para>
    /// </summary>
    public class ControlMove : IDisposable
    {
        public void Dispose()
        {
            if (bindingSourceConfig != null)
            {
                bindingSourceConfig.Clear();
                bindingSourceConfig.Dispose();
            }
            if (dtConfig != null)
            {
                dtConfig.Clear();
                dtConfig.Dispose();
                dtConfig = null;
            }
        }

        const int Band = 5;
        const int MinWidth = 10;
        const int MinHeight = 10;
        private EnumMousePointPosition m_MousePointPosition;
        private Point p, p1;
        //Control Pc = null;
        Control[] Pcs = null;


        private DataTable dtConfig = null;
        private BindingSource bindingSourceConfig = null;

        Color[] localColors = null;   // 容器原本的颜色
        Color moveStatusColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));  //解锁状态时容器的颜色
        Color notMoveStatusColor = SystemColors.Control; //锁定状态时容器的颜色  InactiveCaptionText

        int step = 1;   //用于在BindingSource里面查找控件
        private Control curControl; //当前控件
        private Color curBackColor;    //当前控件原来的背景色
        private Color curBackColorSet = Color.Yellow; //当前控件设计时的背景色

        public BindingSource BindingSourceConfig
        {
            get { return bindingSourceConfig; }
            set
            {
                bindingSourceConfig = value;                
                dtConfig = bindingSourceConfig.DataSource as DataTable;
                //用于方法SetCurrRow
                bindingSourceConfig.Sort = "SNo ASC";
                //if (dtConfig.Select("SNo=0").Length > 0)
                //    step = 0;
            }
        }

        public ControlMove()
        {
        }

        public ControlMove(BindingSource dsConfig)
        {
            BindingSourceConfig = dsConfig;
        }

        /// <summary>
        /// 是否处于设计状态
        /// </summary>
        public bool IsDesign
        {
            get
            {
                if (Pcs == null)
                    return false;
                else if (Pcs[0].BackColor == moveStatusColor)
                    return true;
                else
                    return false;
            }
        }

        public enum EnumMousePointPosition
        {
            MouseSizeNone = 0, //'无
            MouseSizeRight = 1, //'拉伸右边框
            MouseSizeLeft = 2, //'拉伸左边框
            MouseSizeBottom = 3, //'拉伸下边框
            MouseSizeTop = 4, //'拉伸上边框
            MouseSizeTopLeft = 5, //'拉伸左上角
            MouseSizeTopRight = 6, //'拉伸右上角
            MouseSizeBottomLeft = 7, //'拉伸左下角
            MouseSizeBottomRight = 8, //'拉伸右下角
            MouseDrag = 9  // '鼠标拖动
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-10</para>
        /// <para>把容器中控件的位置、大小赋值到对应的配置数据表里面</para>
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="dt">配置数据表</param>
        public void GetConfigToTable(Control container, DataTable dt)
        {
            foreach (Control ctl in container.Controls)
            {
                ToDataTable(ctl);
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-10</para>
        /// <para>把控件的位置、大小赋值到对应的配置数据表里面</para>
        /// </summary>
        /// <param name="ctl"></param>
        private void ToDataTable(Control ctl)
        {
            foreach (DataRow row in dtConfig.Rows)
            {
                if (ctl.Name.EndsWith("_Label"))
                {
                    if (ctl.Name.Substring(0, ctl.Name.Length - 6).Equals(Convert.ToString(row["ControlName"])))
                    {
                        row["LabelWidth"] = ctl.Size.Width;
                        row["LabelHeight"] = ctl.Size.Width;
                        row["Labellocatex"] = ctl.Location.X;
                        row["Labellocatey"] = ctl.Location.Y;

                        break;
                    }
                    
                }
                else
                {
                    if (ctl.Name.Equals(Convert.ToString(row["ControlName"])))
                    {
                        row["Width"] = ctl.Size.Width;
                        row["Height"] = ctl.Size.Height;
                        row["locatex"] = ctl.Location.X;
                        row["locatey"] = ctl.Location.Y;

                        break;
                    }
                }
            }
            }
        

        /// <summary>
        /// 创建人：方君业
        /// <para>日期：2012-10-10</para>
        /// <para>把DataTable中控件的位置、大小赋值到对应的控件属性</para>
        /// </summary>
        /// <param name="ctl"></param>
        public void ToControl()
        {
            DataRow row = TableHandler.GetCurrentDataRow(BindingSourceConfig);
            for (int i = 0; i < Pcs.Length; i++)
            {
                foreach (Control ctl in Pcs[i].Controls)
                {
                    if (ctl.Name == row["ControlName"].ToString())
                    {
                        ctl.Size = new System.Drawing.Size(Convert.ToInt32(row["width"]), Convert.ToInt32(row["height"]));
                        ctl.Location = new System.Drawing.Point(Convert.ToInt32(row["locatex"]), Convert.ToInt32(row["locatey"]));
                        if (ctl.GetType() == typeof(System.Windows.Forms.Label))
                        {
                            ((System.Windows.Forms.Label)ctl).Text = row["Caption"].ToString();
                            if ((Validator.IsHtmlColor(row["FontColor"].ToString())))
                                ((System.Windows.Forms.Label)ctl).ForeColor = System.Drawing.ColorTranslator.FromHtml(row["FontColor"].ToString());
                            if ((Validator.IsHtmlColor(row["BackColor"].ToString())))
                                ((System.Windows.Forms.Label)ctl).BackColor = System.Drawing.ColorTranslator.FromHtml(row["BackColor"].ToString());
                        }
                        if (ctl.GetType() == typeof(BaseEdit))
                        {
                            if ((Validator.IsHtmlColor(row["FontColor"].ToString())))
                                ((BaseEdit)ctl).ForeColor = System.Drawing.ColorTranslator.FromHtml(row["FontColor"].ToString());
                            if ((Validator.IsHtmlColor(row["BackColor"].ToString())))
                                ((BaseEdit)ctl).BackColor = System.Drawing.ColorTranslator.FromHtml(row["BackColor"].ToString());
                        }
                        //break;
                    }

                    if (ctl.Name == row["ControlName"].ToString()+"_Label")
                    {
                        ctl.Size = new System.Drawing.Size(Convert.ToInt32(row["Labelwidth"]), Convert.ToInt32(row["Labelheight"]));
                        ctl.Location = new System.Drawing.Point(Convert.ToInt32(row["Labellocatex"]), Convert.ToInt32(row["Labellocatey"]));
                        if (ctl.GetType() == typeof(System.Windows.Forms.Label))
                        {
                            ((System.Windows.Forms.Label)ctl).Text = row["Caption"].ToString();
                            if ((Validator.IsHtmlColor(row["FontColor"].ToString())))
                                ((System.Windows.Forms.Label)ctl).ForeColor = System.Drawing.ColorTranslator.FromHtml(row["FontColor"].ToString());
                            if ((Validator.IsHtmlColor(row["BackColor"].ToString())))
                                ((System.Windows.Forms.Label)ctl).BackColor = System.Drawing.ColorTranslator.FromHtml(row["BackColor"].ToString());
                        }
                        if (ctl.GetType() == typeof(BaseEdit))
                        {
                            if ((Validator.IsHtmlColor(row["FontColor"].ToString())))
                                ((BaseEdit)ctl).ForeColor = System.Drawing.ColorTranslator.FromHtml(row["FontColor"].ToString());
                            if ((Validator.IsHtmlColor(row["BackColor"].ToString())))
                                ((BaseEdit)ctl).BackColor = System.Drawing.ColorTranslator.FromHtml(row["BackColor"].ToString());
                        }
                        //break;
                    }

                }
            }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-11</para>
        /// <para>设置BindingSource的当前行</para>
        /// </summary>
        /// <param name="ctl"></param>
        private void SetCurrRow(Control ctl)
        {
            //按照控件的TabIndex属性直接找bindingsource的位置
            int position = ctl.TabIndex - step;
            if (position > bindingSourceConfig.Count-1 || position < 0)
                return;

            bindingSourceConfig.Position = position;
            DataRow row = TableHandler.GetCurrentDataRow(bindingSourceConfig);
            Comm.ChangeControlProperties(Comm._vGridControlProperties, Comm._dtControlProperties, row, "主档控件");

            //设置控件颜色
            if (curControl != null)
            {
                curControl.BackColor = curBackColor;
            }
            if (Validator.IsHtmlColor(row["BackColor"].ToString()))
                curBackColor = System.Drawing.ColorTranslator.FromHtml(row["BackColor"].ToString());
            
            ctl.BackColor = curBackColorSet;
            curControl = ctl;


            //bindingSourceConfig.MoveFirst();
            
            //while (bindingSourceConfig.Position < bindingSourceConfig.Count)
            //{
            //    DataRow row = TableHandler.GetCurrentDataRow(bindingSourceConfig);

            //    if (row == null)
            //        return;

            //    if (ctl.Name.Equals(Convert.ToString(row["ControlName"])))
            //    {
            //        Comm.ChangeControlProperties(Comm._vGridControlProperties, Comm._dtControlProperties, row,"主档控件");
            //        break;
            //    }

            //    bindingSourceConfig.MoveNext();
            //}
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-10</para>
        /// <para>解锁容器中的控件</para>
        /// </summary>
        /// <param name="container">容器</param>
        public void SetCtrlMove(Control[] container)
        {
            int clength = container.Length;
            if (container[0].BackColor == moveStatusColor)
                return;
            localColors = new Color[clength];
            Pcs = new Control[clength];
            for (int i=0; i < container.Length; i++)
            {
                localColors[i] = container[i].BackColor;
                Pcs[i] = container[i];
                Pcs[i].BackColor = moveStatusColor;

                foreach (Control ctl in Pcs[i].Controls)
                {
                    ctl.MouseDown += new MouseEventHandler(Ctrl_MouseDown);
                    ctl.MouseLeave += new EventHandler(Ctrl_MouseLeave);
                    ctl.MouseMove += new MouseEventHandler(Ctrl_MouseMove);
                    ctl.MouseUp += new MouseEventHandler(Ctrl_MouseUp);
                    

                    if (ctl is BaseEdit)
                    {
                        ((BaseEdit)ctl).Properties.ReadOnly = true;
                    }
                }
            }

        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-10</para>
        /// <para>锁定容器中的控件（容器颜色设置为：SystemColors.Control）</para>
        /// </summary>
        /// <param name="container">容器</param>
        public void RemoveCtrlMove(Control[] container)
        {
            if (container[0].BackColor == notMoveStatusColor)
                return;

            if (curControl != null)
            {
                curControl.BackColor = curBackColor;
            }
            
            for (int i=0; i < container.Length; i++)
            {
                Pcs[i] = container[i];
                //notMoveStatusColor = container.BackColor;
                Pcs[i].BackColor = localColors[i];

                foreach (Control ctl in Pcs[i].Controls)
                {
                    ctl.MouseDown -= new MouseEventHandler(Ctrl_MouseDown);
                    ctl.MouseLeave -= new EventHandler(Ctrl_MouseLeave);
                    ctl.MouseMove -= new MouseEventHandler(Ctrl_MouseMove);
                    ctl.MouseUp -= new MouseEventHandler(Ctrl_MouseUp);

                    if (ctl is BaseEdit)
                    {
                        //按照控件的TabIndex属性直接找bindingsource的位置
                        int position = ctl.TabIndex - step;
                        if (position > bindingSourceConfig.Count - 1 || position < 0)
                            return;

                        bindingSourceConfig.Position = position;
                        DataRow row = TableHandler.GetCurrentDataRow(bindingSourceConfig);

                        ((BaseEdit)ctl).Properties.ReadOnly = Convert.ToBoolean(row["ReadOnly"]);
                    }
                }
            }
        }

        private void Ctrl_MouseDown(object sender, MouseEventArgs e)
        {
            p.X = e.X;
            p.Y = e.Y;
            p1.X = e.X;
            p1.Y = e.Y;

            Control lCtrl = (sender as Control);
            SetCurrRow(lCtrl);
        }

        private void Ctrl_MouseLeave(object sender, EventArgs e)
        {
            m_MousePointPosition = EnumMousePointPosition.MouseSizeNone;
            for (int i = 0; i < Pcs.Length; i++)
            {
                Pcs[i].Cursor = Cursors.Arrow;
            }
        }

        private EnumMousePointPosition MousePointPosition(Size size, System.Windows.Forms.MouseEventArgs e)
        {

            if ((e.X >= -1 * Band) | (e.X <= size.Width) | (e.Y >= -1 * Band) | (e.Y <= size.Height))
            {
                if (e.X < Band)
                {
                    if (e.Y < Band)
                    {
                        return EnumMousePointPosition.MouseSizeTopLeft;
                    }
                    else
                    {
                        if (e.Y > -1 * Band + size.Height)
                        {
                            return EnumMousePointPosition.MouseSizeBottomLeft;
                        }
                        else
                        {
                            return EnumMousePointPosition.MouseSizeLeft;
                        }
                    }
                }
                else
                {
                    if (e.X > -1 * Band + size.Width)
                    {
                        if (e.Y < Band)
                        {
                            return EnumMousePointPosition.MouseSizeTopRight;
                        }
                        else
                        {
                            if (e.Y > -1 * Band + size.Height)
                            {
                                return EnumMousePointPosition.MouseSizeBottomRight;
                            }
                            else
                            {
                                return EnumMousePointPosition.MouseSizeRight;
                            }
                        }
                    }
                    else
                    {
                        if (e.Y < Band)
                        {
                            return EnumMousePointPosition.MouseSizeTop;
                        }
                        else
                        {
                            if (e.Y > -1 * Band + size.Height)
                            {
                                return EnumMousePointPosition.MouseSizeBottom;
                            }
                            else
                            {
                                return EnumMousePointPosition.MouseDrag;
                            }
                        }
                    }
                }
            }
            else
            {
                return EnumMousePointPosition.MouseSizeNone;
            }
        }


        void Ctrl_MouseUp(object sender, MouseEventArgs e)
        {
            if (bindingSourceConfig == null)
                return;

            Control ctl = (sender as Control);

            if (e.Button == MouseButtons.Left)
            {
                ToDataTable(ctl);
            }
        }

        private void Ctrl_MouseMove(object sender, MouseEventArgs e)
        {
            Control lCtrl = (sender as Control);

            if (e.Button == MouseButtons.Left)
            {
                switch (m_MousePointPosition)
                {
                    case EnumMousePointPosition.MouseDrag:
                        lCtrl.Left = lCtrl.Left + e.X - p.X;//子窗体与父窗体间距+当前移动的X点-点击按钮X点
                        lCtrl.Top = lCtrl.Top + e.Y - p.Y;//同上
                        break;
                    case EnumMousePointPosition.MouseSizeBottom:
                        lCtrl.Height = lCtrl.Height + e.Y - p1.Y;
                        p1.X = e.X;
                        p1.Y = e.Y; //'记录光标拖动的当前点
                        break;
                    case EnumMousePointPosition.MouseSizeBottomRight:
                        lCtrl.Width = lCtrl.Width + e.X - p1.X;
                        lCtrl.Height = lCtrl.Height + e.Y - p1.Y;
                        p1.X = e.X;
                        p1.Y = e.Y; //'记录光标拖动的当前点
                        break;
                    case EnumMousePointPosition.MouseSizeRight:
                        lCtrl.Width = lCtrl.Width + e.X - p1.X;
                        //      lCtrl.Height = lCtrl.Height + e.Y - p1.Y;
                        p1.X = e.X;
                        p1.Y = e.Y; //'记录光标拖动的当前点
                        break;
                    case EnumMousePointPosition.MouseSizeTop:
                        lCtrl.Top = lCtrl.Top + (e.Y - p.Y);
                        lCtrl.Height = lCtrl.Height - (e.Y - p.Y);
                        break;
                    case EnumMousePointPosition.MouseSizeLeft:
                        lCtrl.Left = lCtrl.Left + e.X - p.X;
                        lCtrl.Width = lCtrl.Width - (e.X - p.X);
                        break;
                    case EnumMousePointPosition.MouseSizeBottomLeft:
                        lCtrl.Left = lCtrl.Left + e.X - p.X;
                        lCtrl.Width = lCtrl.Width - (e.X - p.X);
                        lCtrl.Height = lCtrl.Height + e.Y - p1.Y;
                        p1.X = e.X;
                        p1.Y = e.Y; //'记录光标拖动的当前点
                        break;
                    case EnumMousePointPosition.MouseSizeTopRight:
                        lCtrl.Top = lCtrl.Top + (e.Y - p.Y);
                        lCtrl.Width = lCtrl.Width + (e.X - p1.X);
                        lCtrl.Height = lCtrl.Height - (e.Y - p.Y);
                        p1.X = e.X;
                        p1.Y = e.Y; //'记录光标拖动的当前点
                        break;
                    case EnumMousePointPosition.MouseSizeTopLeft:
                        lCtrl.Left = lCtrl.Left + e.X - p.X;
                        lCtrl.Top = lCtrl.Top + (e.Y - p.Y);
                        lCtrl.Width = lCtrl.Width - (e.X - p.X);
                        lCtrl.Height = lCtrl.Height - (e.Y - p.Y);
                        break;
                    default:
                        break;
                }
                if (lCtrl.Width < MinWidth) lCtrl.Width = MinWidth;
                if (lCtrl.Height < MinHeight) lCtrl.Height = MinHeight;

            }
            else
            {
                m_MousePointPosition = MousePointPosition(lCtrl.Size, e);  //'判断光标的位置状态
                //标签控件只能移动
                if (lCtrl is Label)
                    m_MousePointPosition = EnumMousePointPosition.MouseDrag;
                //备注、图片控件以外的控件都只能移动和调整宽度
                else if (!(lCtrl is MemoEdit) & 
                    !(lCtrl is PictureEdit) & 
                    m_MousePointPosition != EnumMousePointPosition.MouseSizeLeft & 
                    m_MousePointPosition != EnumMousePointPosition.MouseSizeRight)

                    m_MousePointPosition = EnumMousePointPosition.MouseDrag;

                for (int i = 0; i < Pcs.Length; i++)
                {
                    switch (m_MousePointPosition)  //'改变光标
                    {
                        case EnumMousePointPosition.MouseSizeNone:
                            Pcs[i].Cursor = Cursors.Arrow;       //'箭头
                            break;
                        case EnumMousePointPosition.MouseDrag:
                            Pcs[i].Cursor = Cursors.Default;     //'默认
                            break;
                        case EnumMousePointPosition.MouseSizeBottom:
                            Pcs[i].Cursor = Cursors.SizeNS;      //'南北
                            break;
                        case EnumMousePointPosition.MouseSizeTop:
                            Pcs[i].Cursor = Cursors.SizeNS;      //'南北
                            break;
                        case EnumMousePointPosition.MouseSizeLeft:
                            Pcs[i].Cursor = Cursors.SizeWE;      //'东西
                            break;
                        case EnumMousePointPosition.MouseSizeRight:
                            Pcs[i].Cursor = Cursors.SizeWE;      //'东西
                            break;
                        case EnumMousePointPosition.MouseSizeBottomLeft:
                            Pcs[i].Cursor = Cursors.SizeNESW;    //'东北到南西
                            break;
                        case EnumMousePointPosition.MouseSizeBottomRight:
                            Pcs[i].Cursor = Cursors.SizeNWSE;    //'东南到西北
                            break;
                        case EnumMousePointPosition.MouseSizeTopLeft:
                            Pcs[i].Cursor = Cursors.SizeNWSE;    //'东南到西北
                            break;
                        case EnumMousePointPosition.MouseSizeTopRight:
                            Pcs[i].Cursor = Cursors.SizeNESW;    //'东北到南西
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
