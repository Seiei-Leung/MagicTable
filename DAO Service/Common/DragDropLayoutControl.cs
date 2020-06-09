using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraLayout.Dragging;
using DevExpress.XtraLayout.Customization;

namespace DevExpress.XtraLayout.Demos.Modules {
    public interface IDragManager {
        LayoutControlItem DragItem { get; set;}
        void SetDragCursor(DragDropEffects effect);

        LayoutControlItem DragItemAfter { get; set; }      
    }
    public partial class DragDropLayoutControl : UserControl {
        public DragDropLayoutControl() {
            InitializeComponent();
        }
        IDragManager DragManager { get { return Parent.Parent.Parent as IDragManager; } }
        LayoutControlItem newDragItem = null;
        private void layoutControl2_MouseDown(object sender, MouseEventArgs e) {
            newDragItem = layoutControl2.CalcHitInfo(new Point(e.X, e.Y)).Item as LayoutControlItem;
        }
        private void layoutControl2_MouseMove(object sender, MouseEventArgs e) {
            if(newDragItem == null || e.Button != MouseButtons.Left) return;
            DragManager.DragItem = newDragItem;
            DragManager.DragItemAfter = newDragItem;

            layoutControl2.DoDragDrop(DragManager.DragItem, DragDropEffects.Move);
            newDragItem = null;
        }
        private void layoutControl2_DragDrop(object sender, DragEventArgs e) {
            if(dragController != null && DragManager.DragItem != null) {
                dragController = new LayoutItemDragController(DragManager.DragItem,dragController);
                dragControllerAfter = new LayoutItemDragController(DragManager.DragItemAfter, dragControllerAfter);

                if(DragManager.DragItem.Owner == null || DragManager.DragItem.Parent == null)
                    dragController.DragWildItem();
                else
                    dragController.Drag();
                
                dragControllerAfter.Drag();

            }
            HideDragHelper();
            Parent.Cursor = Cursors.Default;
            DragManager.DragItem = null;
            DragManager.DragItemAfter = null;

        }
        private void layoutControl2_DragEnter(object sender, DragEventArgs e) {
            dragController = null;
            dragControllerAfter = null;
            ShowDragHelper();
        }
        private void layoutControl2_DragLeave(object sender, EventArgs e) {
            HideDragHelper();
        }
        private void layoutControl2_DragOver(object sender, DragEventArgs e) {
            UpdateDragHelper(new Point(e.X, e.Y));
            e.Effect = DragDropEffects.Copy;
            DragManager.SetDragCursor(e.Effect);
        }
        private void layoutControl2_GiveFeedback(object sender, GiveFeedbackEventArgs e) {
            e.UseDefaultCursors = false;
        }
        //dragHelper
        DragFrameWindow window;
        LayoutItemDragController dragController = null;
        LayoutItemDragController dragControllerAfter = null;

        protected DragFrameWindow DragFrameWindow {
            get {
                if(window == null) window = new DragFrameWindow(layoutControl2);
                return window;
            }
        }
        protected void ShowDragHelper() {
            if(DragManager.DragItem == null) return;
            DragFrameWindow.Visible = true;
        }
        protected void HideDragHelper() {

            if (DragFrameWindow == null) return;
            DragFrameWindow.Reset();
            DragFrameWindow.Visible = false;
        }
        protected void UpdateDragHelper(Point p) {
            if(DragManager.DragItem == null) return;
            p = layoutControl2.PointToClient(p);

            dragController = new LayoutItemDragController(null, layoutControl2.Root, new Point(p.X, p.Y));
            dragControllerAfter = new LayoutItemDragController(null, layoutControl2.Root, new Point(p.X+100, p.Y+100));

            DragFrameWindow.DragController = dragController;
            DragFrameWindow.DragController = dragControllerAfter;
        }
        
    }
}
