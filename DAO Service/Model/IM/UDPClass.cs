using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Model.IM
{
    public partial class UDPClass : Component
    {
        public delegate void DataArrivedHandler(DataArrivedEventArgs e);
        public event DataArrivedHandler onDataArrived;

        public UDPClass()
        {
            InitializeComponent();
        }

        public UDPClass(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private UdpClient _server;
        private Thread thread;
        private IPEndPoint _clientEp;
        private int _port;

        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        public void Listen()
        {
            _server = new UdpClient(this.Port);
            thread = new Thread(new ThreadStart(GetData));
            thread.IsBackground = true;  //后台运行
            this.thread.Start();  //另外个线程启动，与主线程不同，多线程
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        private void GetData()
        {
            while (true)//无限循环
            {
                try
                {
                    byte[] buf = _server.Receive(ref _clientEp);
                    if (this.onDataArrived != null)
                    {
                        this.onDataArrived(new DataArrivedEventArgs(buf,_clientEp));
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        /// <summary>
        /// 发送数据1
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ep"></param>
        private void Send(byte[] data, IPEndPoint ep)
        {
            try
            {
                this._server.Send(data, data.Length, ep);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 发送数据2
        /// </summary>
        /// <param name="data">发送的数据</param>
        /// <param name="ip">ip</param>
        /// <param name="port">端口号</param>
        private void Send(byte[] data, IPAddress ip, int port)
        {
            IPEndPoint ep = new IPEndPoint(ip, port);
            this.Send(data, ep);
        }
        /// <summary>
        /// 发送数据3
        /// </summary>
        /// <param name="data"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        private void Send(byte[] data, string ip, int port)
        {
            IPAddress ipa = IPAddress.Parse(ip);
            Send(data, ipa, port);
        }
        /// <summary>
        /// 退出，关闭线程
        /// </summary>
        public void Close()
        {
            try
            {
                this._server.Close();//udp服务关闭
                this.thread.Abort();//结束线程
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
