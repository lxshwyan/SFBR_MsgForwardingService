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
    public class ABDoorMsg : BaseMsg<ABDoorMsg>
    {
        public ABDoorMsg()
        {
            logger = LogManager.GetLogger(nameof(ABDoorMsg));
        }
        /// <summary>
        /// 实例化转化发消息
        /// </summary>
        /// <param name="port"></param>
        public override void InitUdp(int port = 8887, string desc = "")
        {
            base.InitUdp(port, desc);
        }
        public override string Received(string msg)
        {
            logger.Debug("Socket_SetTextEvent接收消息：{0}", msg);
            if (string.IsNullOrEmpty(msg) || msg.Length <= 0)
            {
                return null;
            }
            try
            {
                var data = JsonConvert.DeserializeObject<DoorMessage>(msg);
                if (data!=null)
                {
                    if (data.TypeID != 4)
                    {
                        return null;
                    }
                    logger.Debug("Socket_SetTextEvent转发复核消息：{0}", msg);
                    return JsonConvert.SerializeObject(new AB_MsgBody() { abDoorType = ABDoorType.AB_Check, body = msg });
                }
                else
                {

                }
              
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return null;
            }
            return msg;

        }   
        public override void SendInfo(string msg)
        {
            try
            {
                var hub = GlobalHost.ConnectionManager.GetHubContext<MsgHub>();
                var data = JsonConvert.DeserializeObject<AB_MsgBody>(msg);    
                switch (data.abDoorType)
                {
                    case ABDoorType.AB_Check:
                        hub.Clients.All.abCheck(msg);
                        break;
                    case ABDoorType.AB_Register:
                        hub.Clients.All.abRegister(msg);
                        break;
                    default:
                        break;
                }    
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }  
    public enum ABDoorType
    {
        AB_Check, //复核消息
        AB_Register, //登记消息 
    }
    /// <summary>
    /// AB门消息主题
    /// </summary>
    public class AB_MsgBody
    {
        public ABDoorType abDoorType;
        public string body;
    }
}
