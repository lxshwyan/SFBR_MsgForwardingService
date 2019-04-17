
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace SFBR_MsgForwardingService
{
    class Program
    {  
        static void Main(string[] args)
        {    
            HostFactory.Run(x =>                                
            {
                x.Service<Startup>(s =>                       
                {
                    s.ConstructUsing(name => new Startup());     
                    s.WhenStarted(tc => tc.Start());              
                    s.WhenStopped(tc => tc.Stop());               
                });
                x.RunAsLocalSystem();                         

                x.SetDescription("SFBR消息转发服务");      
                x.SetDisplayName("SFBR消息转发服务");                    
                x.SetServiceName("SFBR_MsgForwardingService");                    
            });
        }
    }
}
