using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFBR_MsgForwardingService.Msg
{
    public class BaseMsg<T> where T : new()
    {
        public static T _Instace;
        public Logger logger = LogManager.GetLogger(nameof(BaseMsg<T>));
        static BaseMsg()
        {
            _Instace = new T();
        }
        protected BaseMsg() { }
        private SocketMessageMng socket;
        /// <summary>
        /// 实例化转化发消息
        /// </summary>
        /// <param name="port"></param>
        public virtual void InitUdp(int port, string desc)
        {
            try
            {
                socket = new SocketMessageMng(port);
                socket.UdpStartListen();
                socket.SetTextEvent += Socket_SetTextEvent;
                logger.Info("Udp初始化成功：{0}" + (string.IsNullOrEmpty(desc) ? string.Empty : "," + desc), port);
            }
            catch (Exception ex)
            {
                logger.Error("Udp异常：{0}", ex.ToString());
            }
        }
        private void Socket_SetTextEvent(string msg)
        {
            try
            {
                logger.Debug("Socket_SetTextEvent接收消息：{0}", msg);
                if (string.IsNullOrEmpty(msg) || msg.Length <= 0)
                {
                    return;
                }
                var newmsg = Received(msg);
                if (!string.IsNullOrEmpty(newmsg))
                {
                    SendInfo(msg);
                }
            }
            catch (Exception ex)
            {
                logger.Error("Udp异常：{0}", ex.ToString());
            }
        }
        public virtual string Received(string msg)
        {
            return msg;
        }
        public virtual void SendInfo(string msg)
        {

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
