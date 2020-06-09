using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace Model.IM
{
    //public delegate void DataArrivedEventHandler(object sender,DataArrivedEventArgs e);//委托(数据到达就要用，非常重要)
    public class DataArrivedEventArgs:System.EventArgs
    {
        private byte[] _data;

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }
        private int _port;

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }
        private IPAddress _ip;

        public IPAddress Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }
        public DataArrivedEventArgs()
        {
        }
        public DataArrivedEventArgs(byte[] data)
        {
            this._data = data;
        }
        public DataArrivedEventArgs(IPAddress ip, int port)
        {
            this._ip = ip;
            this._port = port;
        }
        public DataArrivedEventArgs(byte[] data, int port, IPAddress ip)
        {
            this._data = data;
            this._ip = ip;
            this._port = port;
        }
        public DataArrivedEventArgs(byte[] data, IPEndPoint ep)
        {
            this._data = data;
            this._ip = ep.Address;
            this._port = ep.Port;
        }
    }
}
