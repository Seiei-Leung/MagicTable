using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Common
{
    public class FormAutoDock
    {
        /// <summary>
        /// From停靠
        /// </summary>
        /// <param name="DockableForm"></param>
        /// <param name="DockFormHeight"></param>
        /// <param name="_dockTimer"></param>
        public static void SideHideOrShow(Form DockableForm, ref int DockFormHeight, Timer _dockTimer)
        {
            if (DockableForm.WindowState != FormWindowState.Minimized)
            {
                _dockTimer.Interval = 1500;
                if (Cursor.Position.X > DockableForm.Left - 1 && Cursor.Position.X < DockableForm.Right && Cursor.Position.Y > DockableForm.Top - 1 && Cursor.Position.Y < DockableForm.Bottom)
                {
                    if (DockableForm.Top <= 0 && DockableForm.Left > 5 && DockableForm.Left < Screen.PrimaryScreen.WorkingArea.Width - DockableForm.Width)
                    {
                        DockableForm.Top = 0;
                    }
                    else if (DockableForm.Left <= 0)
                    {
                        DockableForm.Left = 0;
                    }
                    else if (DockableForm.Left + DockableForm.Width >= Screen.PrimaryScreen.WorkingArea.Width)
                    {
                        DockableForm.Left = Screen.PrimaryScreen.WorkingArea.Width - DockableForm.Width;
                    }
                    else
                    {
                        if (DockFormHeight > 0)
                        {
                            DockableForm.Height = DockFormHeight;
                            DockFormHeight = 0;
                        }
                    }
                }
                else
                {
                    if (DockFormHeight < 1)
                    {
                        DockFormHeight = DockableForm.Height;
                    }
                    if (DockableForm.Top <= 4 && DockableForm.Left > 5 && DockableForm.Left < Screen.PrimaryScreen.WorkingArea.Width - DockableForm.Width)
                    {
                        DockableForm.Top = 3 - DockableForm.Height;
                        if (DockableForm.Left <= 4)
                        {
                            DockableForm.Left = -5;
                        }
                        else if (DockableForm.Left + DockableForm.Width >= Screen.PrimaryScreen.WorkingArea.Width - 4)
                        {
                            DockableForm.Left = Screen.PrimaryScreen.WorkingArea.Width - DockableForm.Width + 5;
                        }
                    }
                    else if (DockableForm.Left <= 4)
                    {
                        DockableForm.Left = 3 - DockableForm.Width;
                    }
                    else if (DockableForm.Left + DockableForm.Width >= Screen.PrimaryScreen.WorkingArea.Width - 4)
                    {
                        DockableForm.Left = Screen.PrimaryScreen.WorkingArea.Width - 3;
                    }
                    _dockTimer.Interval = 200;
                }
            }
        }
    }
}
