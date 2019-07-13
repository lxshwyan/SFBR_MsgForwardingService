using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using NLog;
using SFBR_MsgForwardingService.Hub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFBR_MsgForwardingService.Msg
{
    public class ABDoorMsg
    {
        public static ABDoorMsg _Instace;
        public Logger logger = LogManager.GetLogger(nameof(ABDoorMsg));
        static ABDoorMsg()
        {
            _Instace = new ABDoorMsg();
        }
        private ABDoorMsg() { }
        private SocketMessageMng socket;
        /// <summary>
        /// 实例化转化发消息
        /// </summary>
        /// <param name="port"></param>
        public void InitUdp(int port = 8887)
        {
            try
            {
                socket = new SocketMessageMng(port);
                socket.UdpStartListen();
                socket.SetTextEvent += Socket_SetTextEvent;
                logger.Info("Udp初始化成功：{0}", port);
            }
            catch (Exception ex)
            {
                logger.Error("Udp异常：{0}", ex.ToString());
            }
        }
        public void Socket_SetTextEvent(string msg)
        {
            logger.Debug("Socket_SetTextEvent接收消息：{0}", msg);
            if (string.IsNullOrEmpty(msg) || msg.Length <= 0)
            {
                return;
            }
            try
            {
                var data = JsonConvert.DeserializeObject<DoorMessage>(msg);
                if (data.TypeID != 4)
                {
                    return;
                }
            }
            catch (Exception)
            {
                return;
            }
            SendInfo(msg);

        }
        public void SendInfo(string msg)
        {
            try
            {
                var hub = GlobalHost.ConnectionManager.GetHubContext<MsgHub>();
                hub.Clients.All.abCheck(msg);
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dis()
        {
            socket?.Dis();
        }
    }
}
