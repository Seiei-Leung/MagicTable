using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Model.IM
{
    [Serializable]
    public class MyImage
    {
        private int _position;

        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }
        private Image _image;

        public Image Image
        {
            get { return _image; }
            set { _image = value; }
        }
    }
}
