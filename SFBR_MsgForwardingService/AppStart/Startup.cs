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

        public string SignalRUrl { get; set; } = "http://192.168.137.171:8893";
        public int ListenPort { get; set; } = 8893;
        public int ABDoorPort { get; set; } = 8887;

        public Logger logger = LogManager.GetLogger(nameof(Startup));
        private void InitSinalR(string SignalRURI = "http://192.168.137.171:8893")
        {
            try
            {
                try
                {
                    using (WebApp.Start(SignalRURI, builder =>
                    {
                        builder.Map("/signalr", map =>
                        {
                            map.UseCors(CorsOptions.AllowAll);   //运行跨域
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

        private void ReadConfig()
        {

            try
            {
                if (!string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["SignalRUrl"]))
                {
                    SignalRUrl = System.Configuration.ConfigurationManager.AppSettings["SignalRUrl"].ToString();
                }
                if (!string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["ListenPort"]))
                {
                    ListenPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ListenPort"].ToString());
                }
                if (!string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings["ABDoorPort"]))
                {
                    ABDoorPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings["ABDoorPort"].ToString());
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "读取参数错误");

            }


        }
        /// <summary>
        ///  开启服务
        /// </summary>
        public void Start()
        {
            ReadConfig();
            PlatAlarmMsg._Instace.InitUdp(ListenPort, "平台和清点消息");
            ABDoorMsg._Instace.InitUdp(ABDoorPort, "AB相关消息");

            InitSinalR(SignalRUrl);


        }
        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            PlatAlarmMsg._Instace.Dis();
            ABDoorMsg._Instace.Dis();
            logger.Info("已停止服务");
        }
    }
}