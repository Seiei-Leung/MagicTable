using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.IM
{
    /// <summary>
    /// 文件传输实体类
    /// </summary>
    public class FileTransmitEventArgs
    {
        private bool _isSender;

        public bool IsSender
        {
            get { return _isSender; }
            set { _isSender = value; }
        }
        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { _errorMessage = value; }
        }
        private string _fullName;

        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }
        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }
        private long _fileLen;

        public long FileLen
        {
            get { return _fileLen; }
            set { _fileLen = value; }
        }
        private long _currTransmitedLen;

        public long CurrTransmitedLen
        {
            get { return _currTransmitedLen; }
            set { _currTransmitedLen = value; }
        }
        public FileTransmitEventArgs() { }
        public FileTransmitEventArgs(bool isSender, string errorMessage, string fullName, string fileName, long fileLen, long currTransmitedLen)
        {
            this._isSender = isSender;
            this._errorMessage = errorMessage;
            this._fullName = fullName;
            this._fileName = fileName;
            this._fileLen = fileLen;
            this._currTransmitedLen = currTransmitedLen;
        }
    }
}
