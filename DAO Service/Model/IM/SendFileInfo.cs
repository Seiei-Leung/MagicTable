using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.IM
{
    [Serializable]
    public class SendFileInfo
    {
        private byte _msgInfoType = 0;//文件发送消息类别

        public byte MsgInfoType
        {
            get { return _msgInfoType; }
            set { _msgInfoType = value; }
        }
        private int _pSendPos = 0;//上次发送的文件块的位置

        public int PSendPos
        {
            get { return _pSendPos; }
            set { _pSendPos = value; }
        }
        private byte[] _fileBlock = null;//当前发送的文件块

        public byte[] FileBlock
        {
            get { return _fileBlock; }
            set { _fileBlock = value; }
        }
        public SendFileInfo(byte msgInfoType, int pSendPos, byte[] fileBlock)
        {
            this._msgInfoType = msgInfoType;
            this._pSendPos = pSendPos;
            this._fileBlock = fileBlock;
        }
    }
}
