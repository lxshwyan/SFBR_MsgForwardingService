using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFBR_MsgForwardingService
{
    public class DoorMessage
    {
        public int AssertTypeID;

        public int DevType { get; set; }
        public string AssertGroupID { get; set; }
        public int TypeID { get; set; }
        public string Expression { get; set; }
        public string JY { get; set; }
        public string Dept { get; set; }
        public string Photo { get; set; }
        public string DateTime { get; set; }
        public string Address { get; set; }
        public string CardID { get; set; }
        public string AssertName { get; set; }
        public string AssertID { get; set; }
        public string DoorID { get; set; }
        public string StatuesID { get; set; }
        public int CheckResult { get; set; }
    }
}
