using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wxBase.Model.Pay
{
    public class wxpayPackage
    {
        public string appId;
        public string timeStamp;
        public string nonceStr;
        public string package;
        public string paySign;
        public string signType;

    }
}
