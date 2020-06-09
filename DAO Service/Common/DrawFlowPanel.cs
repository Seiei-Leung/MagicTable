using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using Common;
using Model;

namespace Common
{
    public class DrawFlowPanel : PanelControl
    {
        private bool Navigation = false;
        private bool MouseIsDown = false;   //鼠标在panel按下
        private int offset = 2; //框选框与控件边框的距离
        private Rectangle MouseRect = Rectangle.Empty;  //鼠标框选区域
        private IDictionary<string, Control> selectedControls = null;   //所有选中的控件
        private Color colorSelected = Color.Red;    //选中控件外框颜色
        private Color colorCurr = Color.Blue;   //选中的当前外框颜色
        private Control controlCurr = null; //当前选中控件

        const int Band = 5;
        const int MinWidth = 10;    //控件最小宽度
        const int MinHeight = 10;   //控件最小高度
        private EnumMousePointPosition m_MousePointPosition;    //鼠标形状
        private Point p, p1;    //记录控件位置

        private BindingSource bindingSourceConfig = new BindingSource();   //控件配置BindingSource
        private DataTable dtConfig = null;  //控件配置表

        private IDictionary<string, Control> Controls_ = new Dictionary<string, Control>();


        public DrawFlowPanel()
            : base()
        {
            selectedControls = new Dictionary<string, Control>();
            //this.VisibleChanged += new EventHandler(panel_VisibleChanged);
            this.ControlAdded += new ControlEventHandler(MyPanel_ControlAdded);
            this.MouseEnter += new EventHandler(MyPanel_MouseEnter);
            this.ControlRemoved += new ControlEventHandler(MyPanel_ControlRemoved);
        }

        public DrawFlowPanel(BindingSource bsConfig)
            : base()
        {
            selectedControls = new Dictionary<string, Control>();
            BindingSourceConfig = bsConfig;
        }

        public enum EnumMousePointPosition
        {
            /// <summary>
            /// 无
            /// </summary>
            MouseSizeNone = 0,
            /// <summary>
            /// 拉伸右边框
            /// </summary>
            MouseSizeRight = 1,
            /// <summary>
            /// 拉伸左边框
            /// </summary>
            MouseSizeLeft = 2,
            /// <summary>
            /// 拉伸下边框
            /// </summary>
            MouseSizeBottom = 3,
            /// <summary>
            /// 拉伸上边框
            /// </summary>
            MouseSizeTop = 4,
            /// <summary>
            /// 拉伸左上角
            /// </summary>
            MouseSizeTopLeft = 5,
            /// <summary>
            /// 拉伸右上角
            /// </summary>
            MouseSizeTopRight = 6,
            /// <summary>
            /// 拉伸左下角
            /// </summary>
            MouseSizeBottomLeft = 7,
            /// <summary>
            /// 拉伸右下角
            /// </summary>
            MouseSizeBottomRight = 8,
            /// <summary>
            /// 鼠标拖动
            /// </summary>
            MouseDrag = 9
        }


        #region 属性

        /// <summary>
        /// 控件配置BindingSource
        /// </summary>
        public BindingSource BindingSourceConfig
        {
            get { return bindingSourceConfig; }
            set
            {
                bindingSourceConfig = value;
                if (bindingSourceConfig == null)
                    return;
                dtConfig = bindingSourceConfig.DataSource as DataTable;

                if (dtConfig.Columns.Contains("ItemType"))
                    Navigation = true;

                //用于方法SetControlCurr
                //bindingSourceConfig.Sort = "SNo ASC";
                if (dtConfig.Columns.Contains("PageNo") && dtConfig.Columns.Contains("SNo"))
                    bindingSourceConfig.Sort = "PageNo,SNo";
                else if (!dtConfig.Columns.Contains("PageNo") && dtConfig.Columns.Contains("SNo"))
                    bindingSourceConfig.Sort = "SNo ASC";
            }
        }

        /// <summary>
        /// 所有选中的控件集合（key：控件名称）
        /// </summary>
        public IDictionary<string, Control> SelectedControls
        {
            get { return selectedControls; }
        }
        /// <summary>
        /// 获取当前选中激活的控件
        /// </summary>
        public Control ControlCurr { get { return controlCurr; } }
        /// <summary>
        /// 是否设计中
        /// </summary>
        public bool IsDesign { get; set; }

        #endregion

        public void DeleteSelectControl()
        {
            foreach (Control c in SelectedControls.Values)
            {
                DataRow row = (c.Tag as TagObjects).RowConfig;
                row.Delete();
                DrawBound(c, this.BackColor, ButtonBorderStyle.Solid);
                this.Controls.Remove(c);
            }
            selectedControls.Clear();
            this.Invalidate();
            this.Update();
        }

        /// <summary>
        /// 解锁控件
        /// </summary>
        public void UnLock()
        {
            this.MouseDown += new MouseEventHandler(panel_MouseDown);
            this.MouseMove += new MouseEventHandler(panel_MouseMove);
            this.MouseUp += new MouseEventHandler(panel_MouseUp);
            this.FindForm().KeyPreview = true;

            foreach (Control ctl in this.Controls)
            {
                //if (!ctl.Visible)
                //    continue;
                if (ctl is TWControls.MyLine && Navigation)
                {
                    (ctl as TWControls.MyLine).pictureArrow.MouseDown += new MouseEventHandler(Control_MouseDown);
                    (ctl as TWControls.MyLine).pictureArrow.MouseLeave += new EventHandler(Control_MouseLeave);
                    (ctl as TWControls.MyLine).pictureArrow.MouseMove += new MouseEventHandler(Control_MouseMove);
                    (ctl as TWControls.MyLine).pictureArrow.MouseUp += new MouseEventHandler(Control_MouseUp);
                    (ctl as TWControls.MyLine).pictureLine.MouseDown += new MouseEventHandler(Control_MouseDown);
                    (ctl as TWControls.MyLine).pictureLine.MouseLeave += new EventHandler(Control_MouseLeave);
                    (ctl as TWControls.MyLine).pictureLine.MouseMove += new MouseEventHandler(Control_MouseMove);
                    (ctl as TWControls.MyLine).pictureLine.MouseUp += new MouseEventHandler(Control_MouseUp);
                }
                else
                {
                    ctl.MouseDown += new MouseEventHandler(Control_MouseDown);
                    ctl.MouseLeave += new EventHandler(Control_MouseLeave);
                    ctl.MouseMove += new MouseEventHandler(Control_MouseMove);
                    ctl.MouseUp += new MouseEventHandler(Control_MouseUp);
                }
                if (ctl is BaseEdit)
                {
                    ((BaseEdit)ctl).Properties.ReadOnly = true;
                }
            }

            IsDesign = true;
        }
        /// <summary>
        /// 锁定控件
        /// </summary>
        public void Lock()
        {
            this.MouseDown -= new MouseEventHandler(panel_MouseDown);
            this.MouseMove -= new MouseEventHandler(panel_MouseMove);
            this.MouseUp -= new MouseEventHandler(panel_MouseUp);
            this.FindForm().KeyPreview = false;

            foreach (Control ctl in this.Controls)
            {
                //if (!ctl.Visible)
                //    continue;
                if (ctl is TWControls.MyLine && Navigation)
                {
                    (ctl as TWControls.MyLine).pictureArrow.MouseDown -= new MouseEventHandler(Control_MouseDown);
                    (ctl as TWControls.MyLine).pictureArrow.MouseLeave -= new EventHandler(Control_MouseLeave);
                    (ctl as TWControls.MyLine).pictureArrow.MouseMove -= new MouseEventHandler(Control_MouseMove);
                    (ctl as TWControls.MyLine).pictureArrow.MouseUp -= new MouseEventHandler(Control_MouseUp);
                    (ctl as TWControls.MyLine).pictureLine.MouseDown -= new MouseEventHandler(Control_MouseDown);
                    (ctl as TWControls.MyLine).pictureLine.MouseLeave -= new EventHandler(Control_MouseLeave);
                    (ctl as TWControls.MyLine).pictureLine.MouseMove -= new MouseEventHandler(Control_MouseMove);
                    (ctl as TWControls.MyLine).pictureLine.MouseUp -= new MouseEventHandler(Control_MouseUp);
                }
                else
                {
                    ctl.MouseDown -= new MouseEventHandler(Control_MouseDown);
                    ctl.MouseLeave -= new EventHandler(Control_MouseLeave);
                    ctl.MouseMove -= new MouseEventHandler(Control_MouseMove);
                    ctl.MouseUp -= new MouseEventHandler(Control_MouseUp);
                }

                if (ctl is BaseEdit)
                {
                    DataRow row = (ctl.Tag as TagObjects).RowConfig;
                    ((BaseEdit)ctl).Properties.ReadOnly = Convert.ToBoolean(row["ReadOnly"]);
                }
            }
            removeAll();
            this.Invalidate();
            this.Update();
            IsDesign = false;
        }

        #region 容器事件

        void MyPanel_ControlRemoved(object sender, ControlEventArgs e)
        {
            Controls_.Remove(e.Control.Name);
        }

        //把控件存放到字典
        void MyPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            Controls_.Add(e.Control.Name, e.Control);
        }


        void MyPanel_MouseEnter(object sender, EventArgs e)
        {
            //暂时注释
            //if (this.Visible)
            //    Comm.PanelCurr = this;
        }

        void panel_MouseUp(object sender, MouseEventArgs e)
        {
            this.Capture = false;
            Cursor.Clip = Rectangle.Empty;
            MouseIsDown = false;
            DrawRectangle();
            MouseRect = Rectangle.Empty;
        }
        void panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseIsDown)
            {
                ResizeToRectangle(e.Location);

                Rectangle recCurr = GetRectangle(e.Location);
                ChangeControlsState(recCurr);
            }
        }
        void panel_MouseDown(object sender, MouseEventArgs e)
        {
            controlCurr = null;
            MouseIsDown = true;
            DrawStart(e.Location);
        }
        #endregion

        #region 控件事件

        private void Control_MouseDown(object sender, MouseEventArgs e)
        {
            if (!IsDesign)
                return;

            p.X = e.X;
            p.Y = e.Y;
            p1.X = e.X;
            p1.Y = e.Y;

            Control ctl;
            if (Navigation && sender is PictureBox)
                ctl = (sender as PictureBox).Parent;
            else
                ctl = sender as Control;
            //MyPanel p = ctl.Parent as MyPanel;

            if (Control.ModifierKeys == Keys.Control)   //按下Ctrl
            {
                if (this.SelectedControls.ContainsKey(ctl.Name))
                {
                    this.RemoveSelected(ctl);
                    if (this.ControlCurr != null && this.ControlCurr.Equals(ctl))
                    {
                        this.SetControlCurr(this.First);
                    }
                }
                else
                {
                    this.AddSelected(ctl);
                    if (this.ControlCurr == null)
                        this.SetControlCurr(ctl);
                }
            }
            else
            {

                if (this.SelectedControls.ContainsKey(ctl.Name))
                {
                    this.SetControlCurr(ctl);
                }
                else
                {
                    this.removeAll();
                    this.AddSelected(ctl);
                    this.SetControlCurr(ctl);
                }
            }
        }

        private void Control_MouseLeave(object sender, EventArgs e)
        {
            if (!IsDesign)
                return;

            m_MousePointPosition = EnumMousePointPosition.MouseSizeNone;
            this.Cursor = Cursors.Arrow;
        }

        private void Control_MouseMove(object sender, MouseEventArgs e)
        {
            if (!IsDesign)
                return;

            //Control ctl_Sel = (sender as Control);

            Control ctl_Sel;
            if (Navigation && sender is PictureBox)
                ctl_Sel = (sender as PictureBox).Parent;
            else
                ctl_Sel = sender as Control;

            if (e.Button == MouseButtons.Left)
            {
                foreach (Control ctl in selectedControls.Values)
                {
                    DrawBound(ctl, this.BackColor, ButtonBorderStyle.Solid);

                    switch (m_MousePointPosition)
                    {
                        case EnumMousePointPosition.MouseDrag:
                            this.Cursor = Cursors.SizeAll;
                            ctl.Left = ctl.Left + e.X - p.X;//子窗体与父窗体间距+当前移动的X点-点击按钮X点
                            ctl.Top = ctl.Top + e.Y - p.Y;//同上
                            break;
                        case EnumMousePointPosition.MouseSizeBottom:
                            ctl.Height = ctl.Height + e.Y - p1.Y;
                            //'记录光标拖动的当前点
                            if (ctl.Equals(ctl_Sel))
                            {
                                p1.X = e.X;
                                p1.Y = e.Y;
                            }
                            break;
                        case EnumMousePointPosition.MouseSizeBottomRight:
                            ctl.Width = ctl.Width + e.X - p1.X;
                            ctl.Height = ctl.Height + e.Y - p1.Y;
                            //'记录光标拖动的当前点
                            if (ctl.Equals(ctl_Sel))
                            {
                                p1.X = e.X;
                                p1.Y = e.Y;
                            }
                            break;
                        case EnumMousePointPosition.MouseSizeRight:
                            ctl.Width = ctl.Width + e.X - p1.X;
                            //'记录光标拖动的当前点
                            if (ctl.Equals(ctl_Sel))
                            {
                                p1.X = e.X;
                                p1.Y = e.Y;
                            }
                            break;
                        case EnumMousePointPosition.MouseSizeTop:
                            ctl.Top = ctl.Top + (e.Y - p.Y);
                            ctl.Height = ctl.Height - (e.Y - p.Y);
                            break;
                        case EnumMousePointPosition.MouseSizeLeft:
                            ctl.Left = ctl.Left + e.X - p.X;
                            ctl.Width = ctl.Width - (e.X - p.X);
                            break;
                        case EnumMousePointPosition.MouseSizeBottomLeft:
                            ctl.Left = ctl.Left + e.X - p.X;
                            ctl.Width = ctl.Width - (e.X - p.X);
                            ctl.Height = ctl.Height + e.Y - p1.Y;
                            //'记录光标拖动的当前点
                            if (ctl.Equals(ctl_Sel))
                            {
                                p1.X = e.X;
                                p1.Y = e.Y;
                            }
                            break;
                        case EnumMousePointPosition.MouseSizeTopRight:
                            ctl.Top = ctl.Top + (e.Y - p.Y);
                            ctl.Width = ctl.Width + (e.X - p1.X);
                            ctl.Height = ctl.Height - (e.Y - p.Y);
                            //'记录光标拖动的当前点
                            if (ctl.Equals(ctl_Sel))
                            {
                                p1.X = e.X;
                                p1.Y = e.Y;
                            }
                            break;
                        case EnumMousePointPosition.MouseSizeTopLeft:
                            ctl.Left = ctl.Left + e.X - p.X;
                            ctl.Top = ctl.Top + (e.Y - p.Y);
                            ctl.Width = ctl.Width - (e.X - p.X);
                            ctl.Height = ctl.Height - (e.Y - p.Y);
                            break;
                        default:
                            break;
                    }
                    if (ctl.Width < MinWidth) ctl.Width = MinWidth;
                    if (ctl.Height < MinHeight) ctl.Height = MinHeight;

                    Color color = colorSelected;
                    if (ControlCurr.Equals(ctl))
                        color = colorCurr;
                    DrawBound(ctl, color, ButtonBorderStyle.Solid);
                }
            }
            else
            {
                m_MousePointPosition = MousePointPosition(ctl_Sel.Size, e);  //'判断光标的位置状态
                //标签控件只能移动
                if (ctl_Sel is Label)
                    m_MousePointPosition = EnumMousePointPosition.MouseDrag;
                //备注、图片控件以外的控件都只能移动和调整宽度
                else if (!(ctl_Sel is MemoEdit) &
                    !(ctl_Sel is PictureEdit) &
                    !(ctl_Sel is GroupControl) &
                    !(ctl_Sel is CheckedListBoxControl) &
                    !(ctl_Sel is TWControls.MyLine) &
                    !(ctl_Sel is SimpleButton) &
                    m_MousePointPosition != EnumMousePointPosition.MouseSizeLeft &
                    m_MousePointPosition != EnumMousePointPosition.MouseSizeRight)

                    m_MousePointPosition = EnumMousePointPosition.MouseDrag;


                switch (m_MousePointPosition)  //'改变光标
                {
                    case EnumMousePointPosition.MouseSizeNone:
                        this.Cursor = Cursors.Arrow;       //'箭头
                        break;
                    case EnumMousePointPosition.MouseDrag:
                        this.Cursor = Cursors.SizeAll;     //'默认
                        break;
                    case EnumMousePointPosition.MouseSizeBottom:
                        this.Cursor = Cursors.SizeNS;      //'南北
                        break;
                    case EnumMousePointPosition.MouseSizeTop:
                        this.Cursor = Cursors.SizeNS;      //'南北
                        break;
                    case EnumMousePointPosition.MouseSizeLeft:
                        this.Cursor = Cursors.SizeWE;      //'东西
                        break;
                    case EnumMousePointPosition.MouseSizeRight:
                        this.Cursor = Cursors.SizeWE;      //'东西
                        break;
                    case EnumMousePointPosition.MouseSizeBottomLeft:
                        this.Cursor = Cursors.SizeNESW;    //'东北到南西
                        break;
                    case EnumMousePointPosition.MouseSizeBottomRight:
                        this.Cursor = Cursors.SizeNWSE;    //'东南到西北
                        break;
                    case EnumMousePointPosition.MouseSizeTopLeft:
                        this.Cursor = Cursors.SizeNWSE;    //'东南到西北
                        break;
                    case EnumMousePointPosition.MouseSizeTopRight:
                        this.Cursor = Cursors.SizeNESW;    //'东北到南西
                        break;
                    default:
                        break;
                }
            }
        }

        private void Control_MouseUp(object sender, MouseEventArgs e)
        {
            if (!IsDesign)
                return;

            if (e.Button == MouseButtons.Left)
            {
                ToDataTable();
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

        #endregion

        #region 数据表、BinDingSource

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-10</para>
        /// <para>把控件的位置、大小赋值到对应的配置数据表里面</para>
        /// </summary>
        /// <param name="ctl"></param>
        private void ToDataTable()
        {
            if (bindingSourceConfig == null)
                return;
            try
            {
                foreach (Control ctl in selectedControls.Values)
                {
                    DataRow row = (ctl.Tag as TagObjects).RowConfig;
                    string controlName = Convert.ToString(row["ControlName"]);

                    if (ctl.Name.Equals(controlName))
                    {
                        row["Width"] = ctl.Size.Width;
                        row["Height"] = ctl.Size.Height;
                        row["locatex"] = ctl.Location.X;
                        row["locatey"] = ctl.Location.Y;
                    }
                }
            }
            catch (Exception e) { this.FindForm().MsgError("(错误代码行: " + Comm.CurrMethod + ") " + e.Message); }
        }

        /// <summary>
        /// 创建人：郑志冲
        /// <para>日期：2012-10-11</para>
        /// <para>设置BindingSource的当前行</para>
        /// </summary>
        /// <param name="ctl"></param>
        private DataRow SetCurrRow(Control ctl)
        {
            int position = ctl.TabIndex - 1;
            if (position > bindingSourceConfig.Count - 1 || position < 0)
                return null;

            bindingSourceConfig.Position = position;
            DataRow row = (ctl.Tag as TagObjects).RowConfig;
            Comm.ChangeControlProperties(Comm._vGridControlProperties, Comm._dtControlProperties, row, "主档控件");
            return row;
        }

        #endregion

        #region 选中控件集合操作
        /// <summary>
        /// 添加到集合
        /// </summary>
        /// <param name="ctl"></param>
        public void AddSelected(Control ctl)
        {
            if (selectedControls.ContainsKey(ctl.Name))
                return;

            selectedControls.Add(ctl.Name, ctl);
            DrawBound(ctl, colorSelected, ButtonBorderStyle.Solid);

        }

        /// <summary>
        /// 删除集合里面的控件
        /// </summary>
        /// <param name="ctl"></param>
        public void RemoveSelected(Control ctl)
        {
            if (!selectedControls.ContainsKey(ctl.Name))
                return;

            selectedControls.Remove(ctl.Name);
            DrawBound(ctl, this.BackColor, ButtonBorderStyle.Solid);

        }

        public void removeAll()
        {

            foreach (Control ctl in selectedControls.Values)
            {
                DrawBound(ctl, this.BackColor, ButtonBorderStyle.Solid);
            }
            selectedControls.Clear();
        }

        /// <summary>
        /// 设置当前激活的控件
        /// </summary>
        /// <param name="ctl"></param>
        public void SetControlCurr(Control ctl)
        {
            CancelControlCurr();
            if (ctl == null)
                return;

            if (!selectedControls.ContainsKey(ctl.Name))
                return;
            //DrawBound(ctl, selectedColor, ButtonBorderStyle.Solid);
            DrawBound(ctl, colorCurr, ButtonBorderStyle.Dashed);
            controlCurr = ctl;
            DataRow row = null;
            if (!Navigation)
                row = SetCurrRow(ctl);
 
            //设置当前编辑框和标签
            //string controlName = Convert.ToString(row["ControlName"]);
            string controlName = ctl.Name;
            Comm.EditCurr = Controls_[controlName];
            if (Controls_.ContainsKey(controlName + "_Label") && Controls_[controlName + "_Label"] is LabelControl)
                Comm.LabelCurr = Controls_[controlName + "_Label"];
            else
                Comm.LabelCurr = null;
        }

        /// <summary>
        /// 取消当前激活控件
        /// </summary>
        public void CancelControlCurr()
        {
            if (controlCurr == null)
                return;

            if (selectedControls.ContainsKey(controlCurr.Name))
                DrawBound(controlCurr, colorSelected, ButtonBorderStyle.Dashed);
            else
                DrawBound(controlCurr, this.BackColor, ButtonBorderStyle.Solid);
            controlCurr = null;
        }

        /// <summary>
        /// 集合里面的第一个控件
        /// </summary>
        public Control First
        {
            get
            {
                if (selectedControls.Count > 0)
                    return selectedControls.First().Value;

                return null;
            }
        }

        #endregion

        #region 框选方法

        /// <summary>
        /// 获取鼠标框选的区域的矩形
        /// </summary>
        /// <param name="mouseLocation"></param>
        /// <returns></returns>
        private Rectangle GetRectangle(Point mouseLocation)
        {
            int width = Math.Abs(MouseRect.Width);
            int height = Math.Abs(MouseRect.Height);

            int x = Math.Min(MouseRect.X, mouseLocation.X);
            int y = Math.Min(MouseRect.Y, mouseLocation.Y);

            Rectangle rec = new Rectangle(x, y, width, height);

            return rec;
        }

        /// <summary>
        /// 画框选矩形
        /// </summary>
        private void DrawRectangle()
        {
            Rectangle rect = this.RectangleToScreen(MouseRect);
            ControlPaint.DrawReversibleFrame(rect, Color.White, FrameStyle.Dashed);

            //Graphics g = this.CreateGraphics();
            //g.DrawRectangle(new Pen(Color.Black), rect);
            //g.Flush();
        }

        /// <summary>
        /// 根据鼠标位置设置框选矩形大小
        /// </summary>
        /// <param name="mouseLocation"></param>
        private void ResizeToRectangle(Point mouseLocation)
        {
            DrawRectangle();
            MouseRect.Width = mouseLocation.X - MouseRect.Left;
            MouseRect.Height = mouseLocation.Y - MouseRect.Top;
            DrawRectangle();
        }

        /// <summary>
        /// 鼠标按下时，初始化工作区和框选矩形
        /// </summary>
        /// <param name="StartPoint"></param>
        private void DrawStart(Point StartPoint)
        {
            this.Refresh();
            this.Capture = true;
            //Cursor.Clip = this.RectangleToScreen(new Rectangle(0, 0, ClientSize.Width, ClientSize.Height));
            Cursor.Clip = this.RectangleToScreen(this.ClientRectangle);
            MouseRect = new Rectangle(StartPoint.X, StartPoint.Y, 0, 0);
            //selectedControls.Clear();
            //DrawBound(controlCurr, this.BackColor, ButtonBorderStyle.Solid);
            //controlCurr = null;

            ChangeControlsState(MouseRect);
        }

        /// <summary>
        /// 改变容器里面控件状态
        /// </summary>
        /// <param name="recCurr"></param>
        private void ChangeControlsState(Rectangle recCurr)
        {
            foreach (Control ctl in this.Controls)
            {
                if (!ctl.Visible)
                    continue;
                if (recCurr.IntersectsWith(ctl.Bounds))
                {
                    AddSelected(ctl);
                    if (controlCurr == null)
                        SetControlCurr(ctl);
                }
                else
                {
                    RemoveSelected(ctl);
                    if (selectedControls.Count == 0)
                        controlCurr = null;

                    if (controlCurr != null && controlCurr.Equals(ctl))
                        SetControlCurr(First);
                    //DrawBound(ctl, this.BackColor);
                }
            }
        }

        /// <summary>
        /// 给控件增加外框
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="color"></param>
        public void DrawBound(Control ctl, Color color, ButtonBorderStyle borderStyle)
        {
            if (ctl == null)
                return;

            borderStyle = ButtonBorderStyle.Solid;

            Rectangle rec = new Rectangle(ctl.Location.X - offset, ctl.Location.Y - offset, ctl.Size.Width + 2 * offset, ctl.Size.Height + 2 * offset);
            ControlPaint.DrawBorder(this.CreateGraphics(), rec, color, borderStyle);

            //ctl.Tag = rec;

            //if (color == selectedColor)
            //{
            //    if (borderStyle == ButtonBorderStyle.Dashed)
            //    {
            //        ctl.BackColor = Color.Yellow;
            //    }
            //    else
            //    {
            //        ctl.BackColor = Color.Blue;
            //    }
            //}
            //else
            //{
            //    ctl.BackColor = (Color)ctl.Tag;
            //}
        }

        #endregion


        #region 对齐方法

        public void MoveControlAtSelected(Keys key)
        {
            if (this.selectedControls.Count == 0 ||
                key != Keys.Right && key != Keys.Left && key != Keys.Right && key != Keys.Up && key != Keys.Down)
                return;
            foreach (Control ctl in this.selectedControls.Values)
            {
                this.MoveControl(ctl, key);
            }
            ToDataTable();
        }
        /// <summary>
        /// 键盘方向键移动选中的控件
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="key"></param>
        public void MoveControl(Control ctl, Keys key)
        {
            Color color = colorSelected;
            if (ControlCurr.Equals(ctl))
                color = colorCurr;

            DrawBound(ctl, this.BackColor, ButtonBorderStyle.Solid);

            switch (key)
            {
                case Keys.Right:
                    ctl.Left = ctl.Location.X + 1;
                    break;
                case Keys.Left:
                    ctl.Left = ctl.Location.X - 1;
                    break;
                case Keys.Up:
                    ctl.Top = ctl.Location.Y - 1;
                    break;
                case Keys.Down:
                    ctl.Top = ctl.Location.Y + 1;
                    break;
            }

            DrawBound(ctl, color, ButtonBorderStyle.Solid);
        }

        /// <summary>
        /// 左对齐
        /// </summary>
        public void Align_Left()
        {
            if (controlCurr == null || selectedControls.Count < 2)
                return;

            int x = controlCurr.Location.X;

            foreach (Control ctl in selectedControls.Values)
            {
                if (controlCurr.Equals(ctl))
                    continue;
                Color color = colorSelected;
                if (ControlCurr.Equals(ctl))
                    color = colorCurr;

                DrawBound(ctl, this.BackColor, ButtonBorderStyle.Solid);
                ctl.Left = x;
                DrawBound(ctl, color, ButtonBorderStyle.Solid);
            }
            ToDataTable();
        }

        /// <summary>
        /// 右对齐
        /// </summary>
        public void Align_Right()
        {
            if (controlCurr == null || selectedControls.Count < 2)
                return;

            foreach (Control ctl in selectedControls.Values)
            {
                if (controlCurr.Equals(ctl))
                    continue;
                Color color = colorSelected;
                if (ControlCurr.Equals(ctl))
                    color = colorCurr;

                DrawBound(ctl, this.BackColor, ButtonBorderStyle.Solid);
                ctl.Left += controlCurr.Right - ctl.Right;
                DrawBound(ctl, color, ButtonBorderStyle.Solid);
            }
            ToDataTable();
        }

        /// <summary>
        /// 垂直对齐
        /// </summary>
        public void Align_VCenter()
        {
            if (controlCurr == null || selectedControls.Count < 2)
                return;

            int x = controlCurr.Location.X;
            int center = x + Convert.ToInt16(Math.Floor(controlCurr.Width / 2f));

            foreach (Control ctl in selectedControls.Values)
            {
                if (controlCurr.Equals(ctl))
                    continue;
                Color color = colorSelected;
                if (ControlCurr.Equals(ctl))
                    color = colorCurr;

                DrawBound(ctl, this.BackColor, ButtonBorderStyle.Solid);
                int centerCtl = x + Convert.ToInt16(Math.Floor(ctl.Width / 2f));
                ctl.Left = x + center - centerCtl;
                DrawBound(ctl, color, ButtonBorderStyle.Solid);
            }
            ToDataTable();
        }

        /// <summary>
        /// 上对齐
        /// </summary>
        public void Align_Top()
        {
            if (controlCurr == null || selectedControls.Count < 2)
                return;

            int y = controlCurr.Location.Y;
            foreach (Control ctl in selectedControls.Values)
            {
                if (controlCurr.Equals(ctl))
                    continue;
                Color color = colorSelected;
                if (ControlCurr.Equals(ctl))
                    color = colorCurr;

                DrawBound(ctl, this.BackColor, ButtonBorderStyle.Solid);
                ctl.Top = y;
                DrawBound(ctl, color, ButtonBorderStyle.Solid);
            }
            ToDataTable();
        }

        /// <summary>
        /// 下对齐
        /// </summary>
        public void Align_Bottom()
        {
            if (controlCurr == null || selectedControls.Count < 2)
                return;

            int y = controlCurr.Location.Y;
            foreach (Control ctl in selectedControls.Values)
            {
                if (controlCurr.Equals(ctl))
                    continue;
                Color color = colorSelected;
                if (ControlCurr.Equals(ctl))
                    color = colorCurr;

                DrawBound(ctl, this.BackColor, ButtonBorderStyle.Solid);
                ctl.Top += controlCurr.Bottom - ctl.Bottom; ;
                DrawBound(ctl, color, ButtonBorderStyle.Solid);
            }
            ToDataTable();
        }

        /// <summary>
        /// 水平对齐
        /// </summary>
        public void Align_HCenter()
        {
            if (controlCurr == null || selectedControls.Count < 2)
                return;

            int y = controlCurr.Location.Y;
            int center = y + Convert.ToInt16(Math.Floor(controlCurr.Height / 2f));

            foreach (Control ctl in selectedControls.Values)
            {
                if (controlCurr.Equals(ctl))
                    continue;
                Color color = colorSelected;
                if (ControlCurr.Equals(ctl))
                    color = colorCurr;

                DrawBound(ctl, this.BackColor, ButtonBorderStyle.Solid);
                int centerCtl = y + Convert.ToInt16(Math.Floor(ctl.Height / 2f));
                ctl.Top = y + center - centerCtl;
                DrawBound(ctl, color, ButtonBorderStyle.Solid);
            }
            ToDataTable();
        }

        /// <summary>
        /// 使宽度相同
        /// </summary>
        public void Same_Width()
        {
            if (controlCurr == null || selectedControls.Count < 2)
                return;

            int width = controlCurr.Size.Width;
            foreach (Control ctl in selectedControls.Values)
            {
                if (controlCurr.Equals(ctl))
                    continue;
                Color color = colorSelected;
                if (ControlCurr.Equals(ctl))
                    color = colorCurr;

                DrawBound(ctl, this.BackColor, ButtonBorderStyle.Solid);
                ctl.Size = new Size(width, ctl.Size.Height);
                DrawBound(ctl, color, ButtonBorderStyle.Solid);
            }
            ToDataTable();
        }

        /// <summary>
        /// 使高度相同
        /// </summary>
        public void Same_Height()
        {
            if (controlCurr == null || selectedControls.Count < 2)
                return;

            int height = controlCurr.Size.Height;
            foreach (Control ctl in selectedControls.Values)
            {
                if (controlCurr.Equals(ctl))
                    continue;
                Color color = colorSelected;
                if (ControlCurr.Equals(ctl))
                    color = colorCurr;

                DrawBound(ctl, this.BackColor, ButtonBorderStyle.Solid);
                ctl.Size = new Size(ctl.Size.Width, height);
                DrawBound(ctl, color, ButtonBorderStyle.Solid);
            }
            ToDataTable();
        }

        /// <summary>
        /// 使大小相同
        /// </summary>
        public void Same_Size()
        {
            try
            {
                if (controlCurr == null || selectedControls.Count < 2)
                    return;

                int width = controlCurr.Size.Width;
                int height = controlCurr.Size.Height;
                foreach (Control ctl in selectedControls.Values)
                {
                    if (controlCurr.Equals(ctl))
                        continue;
                    Color color = colorSelected;
                    if (ControlCurr.Equals(ctl))
                        color = colorCurr;

                    DrawBound(ctl, this.BackColor, ButtonBorderStyle.Solid);
                    ctl.Size = new Size(width, height);
                    DrawBound(ctl, color, ButtonBorderStyle.Solid);
                }
                ToDataTable();
            }
            catch (Exception e) { this.FindForm().MsgError("(错误代码行: " + Comm.CurrMethod + ") " + e.Message); }
        }

        #endregion
    }
}
