using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Model.IM
{
    [Serializable]
    public class IMMessage
    {
        private string _fromUser;

        public string FromUser
        {
            get { return _fromUser; }
            set { _fromUser = value; }
        }
        private string _fromIP;

        public string FromIP
        {
            get { return _fromIP; }
            set { _fromIP = value; }
        }
        private int _fromPort;

        public int FromPort
        {
            get { return _fromPort; }
            set { _fromPort = value; }
        }
        private string _toUser;

        public string ToUser
        {
            get { return _toUser; }
            set { _toUser = value; }
        }
        private string _toIP;

        public string ToIP
        {
            get { return _toIP; }
            set { _toIP = value; }
        }
        private int _toPort;

        public int ToPort
        {
            get { return _toPort; }
            set { _toPort = value; }
        }
        private string _txtMessage;

        public string TxtMessage
        {
            get { return _txtMessage; }
            set { _txtMessage = value; }
        }
        private string _fontName;

        #region 字体与颜色 以便在网络上传输能够序列化
        public string FontName
        {
            get { return _fontName; }
            set { _fontName = value; }
        }
        private float _fontSize;

        public float FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }
        private FontStyle _fontStyle;

        public FontStyle FontStyle
        {
            get { return _fontStyle; }
            set { _fontStyle = value; }
        }
        private Color _color;

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        #endregion

        private MyImage[] _images;

        public MyImage[] Images
        {
            get { return _images; }
            set { _images = value; }
        }
    }
}
