
using Microsoft.AspNet.SignalR.Client;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace SignalRConsole
{
    class Program
    {
        //定义代理,广播服务连接相关
        private static IHubProxy HubProxy { get; set; }
        private static  string ServerUrl="http://192.168.137.111:61781/signalr"; 
        //定义一个连接对象
        public static HubConnection Connection { get; set; }

        static void Main(string[] args)
        {
            ConnectAsync();
        }
        //异步连接服务器
        private static void ConnectAsync()
        {
            Connection = new HubConnection(ServerUrl);
            Connection.Closed += Connection_Closed;
            Connection.StateChanged += Connection_StateChanged;
            Connection.Reconnected += Connection_Reconnected;
            HubProxy = Connection.CreateHubProxy("MsgHub");
            try
            {    
                Connection.Start();
                
                HubProxy.On<string>("Welcome", Welcome);
                HubProxy.On<string, string>("addMessage", RecvMsg);
                Thread.Sleep(1000);   //等待连接成功
                HubProxy.Invoke("Hello", "aaa");
                Console.ReadKey();
            }
            catch (Exception EX)
            {
                Console.WriteLine(EX.Message);
                return;
            }
        }

        private static void Connection_StateChanged(StateChange obj)
        {
       
        }

        private static void Connection_Reconnected()
        {
           
        }

        private static void Welcome(string msg)
        {
            Console.WriteLine(msg);
        }
        private static void RecvMsg(string name,string msg)
        {
            Console.WriteLine(msg);
        }
        private static void Connection_Closed()
        {
           // Connection.Start();
            // throw new NotImplementedException();
        }
    }
}
