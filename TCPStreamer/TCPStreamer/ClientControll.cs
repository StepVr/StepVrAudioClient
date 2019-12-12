using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TCPStreamer
{
    public class ClientControll
    {
        public ClientControll()
        {

        }

        public delegate void ReceiveNewOrder(string NewOrder);
        public event ReceiveNewOrder mReceiveNewOrder;

        private UdpClient mListen;

        //接收控制信息
        public void StartMonitor()
        {
            try
            {
                if (mListen == null)
                {
                    mListen = new UdpClient(7778);
                    
                    //创建接收线程
                    Thread RecivceThread = new Thread(RecivceMsg);
                    RecivceThread.Name = "ClientControll";
                    RecivceThread.IsBackground = true;
                    RecivceThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //接收数据线程
        private void RecivceMsg()
        {
            IPEndPoint remote = new IPEndPoint(IPAddress.Any, 7778);
            while (true)
            {
                try
                {
                    byte[] recivcedata = mListen.Receive(ref remote);
                    string strMsg = Encoding.ASCII.GetString(recivcedata, 0, recivcedata.Length);
                    mReceiveNewOrder(strMsg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
