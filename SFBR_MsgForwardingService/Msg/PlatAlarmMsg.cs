﻿/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：SFBR_MsgForwardingService.Msg
*文件名： PlatAlarmMsg
*创建人： Lxsh
*创建时间：2019/4/15 11:43:13
*描述
*=======================================================================
*修改标记
*修改时间：2019/4/15 11:43:13
*修改人：Lxsh
*描述：
************************************************************************/
using Microsoft.AspNet.SignalR;
using NLog;
using SFBR_MsgForwardingService.Hub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFBR_MsgForwardingService.Msg
{
   public  class PlatAlarmMsg
    {
        public static PlatAlarmMsg _Instace;
        public  Logger logger = LogManager.GetLogger(nameof(PlatAlarmMsg));
        static PlatAlarmMsg()
        {
            _Instace = new PlatAlarmMsg();
        }
        private PlatAlarmMsg() { }  
        private SocketMessageMng socket;
        /// <summary>
        /// 实例化转化发消息
        /// </summary>
        /// <param name="port"></param>
        public void InitUdp(int port = 8893)
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
            msg = string.Format("{0}:收到信息：{1}", DateTime.Now.ToString(), msg);  
            logger.Debug("Socket_SetTextEvent接收消息：{0}", msg);   
            if (string.IsNullOrEmpty(msg) || msg.Length < 2)
            {
                return;
            }
            SendInfo(msg);
           
        }
        public void SendInfo(string msg)
        {
           var hub = GlobalHost.ConnectionManager.GetHubContext<MsgHub>();
            hub.Clients.All.allInfo(msg);
            switch (msg.Substring(0, 2).ToUpper())
            {
                case "BJ"://刷新
                    {
                        hub.Clients.All.alarmInfo(msg);
                        break;
                    }

                case "MJ":
                    {
                        hub.Clients.All.mjInfo(msg);
                        break;
                    }
                case "Do":
                    {
                        hub.Clients.All.doInfo(msg);
                        break;
                    }
                case "CL":
                    {
                        hub.Clients.All.clInfo(msg);
                    }
                    break;
                case "FK":
                    {
                        hub.Clients.All.fkInfo(msg);
                        break;
                    }
                case "DW":
                    {
                        hub.Clients.All.dwInfo(msg);
                        break;
                    }
                case "DC":
                    {
                        hub.Clients.All.dcInfo(msg);
                        break;
                    }
                case "DU":
                    hub.Clients.All.duInfo(msg);
                    break;
                case "DR":
                    hub.Clients.All.drInfo(msg);
                    break;
                case "RC":
                    hub.Clients.All.rcInfo(msg);
                    break;
                case "QT":
                    hub.Clients.All.qtInfo(msg);
                    break;
                case "QD":
                    hub.Clients.All.qdInfo(msg);
                    break;
                default:
                    {
                        break;
                    }
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