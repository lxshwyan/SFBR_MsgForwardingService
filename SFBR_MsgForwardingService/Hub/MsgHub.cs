﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;                
using Microsoft.AspNet.SignalR.Hubs;

namespace SFBR_MsgForwardingService.Hub
{
  
    public class MsgHub : Microsoft.AspNet.SignalR.Hub
    {

        public static ConcurrentDictionary<string, string> dictionary = new ConcurrentDictionary<string, string>();

        public void Hello(string msg)
        {
           // Console.WriteLine($"当前在线一共{dictionary.Count}个连接");
            this.Clients.All.Welcome($"当前在线一共{dictionary.Count}个连接");
        }
        /// <summary>
        /// 编写发送信息的方法
        /// </summary>
        /// <param name="name"></param>
        /// <param name="message"></param>
        public void Send(string name, string message)
        {
            //调用所有客户注册的本地的JS方法(addMessage)
            Clients.All.addMessage(name, message);   
        }
        public void SendAll(string name, string message)
        {
           //调用所有客户注册的本地的JS方法(addMessage)
           Clients.All.allInfo(message);
        }
        public override Task OnConnected()
        {
            dictionary.TryAdd(this.Context.ConnectionId, DateTime.Now.ToString());
            Console.WriteLine($"当前在线一共{dictionary.Count}个连接");   
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            dictionary.TryRemove(this.Context.ConnectionId,out string value);
            return base.OnDisconnected(stopCalled);
        }
    }
}