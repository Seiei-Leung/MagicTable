using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Common
{/*
    public class MyLine:Panel
    {
        public System.Windows.Forms.PictureBox pictureLine = new System.Windows.Forms.PictureBox();
        public System.Windows.Forms.PictureBox pictureArrow = new System.Windows.Forms.PictureBox();
        private LineTypes myLineType;

        public LineTypes MyLineType
        {
            get { return myLineType; }
        } 

        public enum LineTypes
        {
            X,
            Y,
            Left,
            Right,
            Up,
            Down
        }

        public MyLine(LineTypes lineType, Point location)
        {
            SetPicture(lineType, 300, location);
            myLineType = lineType;
        }

        public MyLine(LineTypes lineType, int lineSize, Point location)
        {
            SetPicture(lineType, lineSize, location);
            myLineType = lineType;
        }

        public void SetPicture(LineTypes lineType, int lineSize, Point location)
        {
            if (lineType == LineTypes.Left || lineType == LineTypes.Right || lineType == LineTypes.X)
            {
                this.Width = lineSize;
                this.Height = 15;
                this.Location = location;

                if (lineType != LineTypes.X)
                    this.Controls.Add(pictureArrow);
                if (lineType == LineTypes.Right)
                {
                    pictureArrow.Dock = DockStyle.Right;
                    this.pictureArrow.Image = ((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("ArrowRight")));
                }
                else
                {
                    pictureArrow.Dock = DockStyle.Left;
                    this.pictureArrow.Image = ((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("ArrowLeft")));
                }
                this.pictureLine.Image = ((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("LineX")));              
                pictureArrow.Width = 20;
            }
            else
            {
                this.Width = 15;
                this.Height = lineSize;
                this.Location = location;
                if (lineType != LineTypes.Y)
                    this.Controls.Add(pictureArrow);
                if (lineType == LineTypes.Up)
                {
                    pictureArrow.Dock = DockStyle.Top;
                    this.pictureArrow.Image = ((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("ArrowUp")));
                }
                else
                {
                    pictureArrow.Dock = DockStyle.Bottom;
                    this.pictureArrow.Image = ((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("ArrowDown")));
                }
                this.pictureLine.Image = ((System.Drawing.Image)(Properties.Resources.ResourceManager.GetObject("LineY")));
                pictureArrow.Height = 20;
            }

            this.pictureLine.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureArrow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;

            this.Controls.Add(pictureLine);
            pictureLine.Dock = DockStyle.Fill;
        }
    }
  */
}
