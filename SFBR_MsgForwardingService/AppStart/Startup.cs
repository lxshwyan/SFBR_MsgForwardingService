/************************************************************************
* Copyright (c) 2019 All Rights Reserved.
*命名空间：Lxsh.Project.SignalRServer.Demo.AppStart
*文件名： Startup
*创建人： Lxsh
*创建时间：2019/1/18 14:57:51
*描述
*=======================================================================
*修改标记
*修改时间：2019/1/18 14:57:51
*修改人：Lxsh
*描述：
************************************************************************/

using SFBR_MsgForwardingService.Hub;
using System;
using System.Reflection;
using System.Threading;  
using Microsoft.AspNet.SignalR;
using Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using SFBR_MsgForwardingService.Msg;
using NLog;

namespace SFBR_MsgForwardingService
{

    public class Startup
    {   
        public  Logger logger = LogManager.GetLogger(nameof(PlatAlarmMsg));
        private  void InitSinalR(string SignalRURI = "http://localhost:6178")
        {
            try
            {
                try
                {
                    using (WebApp.Start(SignalRURI, builder =>
                    {
                        builder.Map("/signalr", map =>
                        {  
                            map.UseCors(CorsOptions.AllowAll);
                            var hubConfiguration = new HubConfiguration
                            { 
                                EnableJSONP = true
                            };  
                            map.RunSignalR(hubConfiguration);
                        });
                        builder.MapSignalR();

                    }))
                    {    
                        logger.Info("服务开启成功,运行在{0}", SignalRURI + "/signalr");
                        Console.ReadLine();
                    }
                }
                catch (TargetInvocationException)
                {
                    
                   logger.Info("服务开启失败. 已经有一个服务运行在{0}", SignalRURI);
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                logger.Info("服务开启异常：{0}", ex.ToString());
              
                Console.ReadLine();
            }
        }
      
      
        /// <summary>
        ///  开启服务
        /// </summary>
        public void Start()
        {  
             PlatAlarmMsg._Instace.InitUdp(); 

             InitSinalR();

         
        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {

            PlatAlarmMsg._Instace.Dis();
             logger.Info("已停止服务");
        }
    }
}