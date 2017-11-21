using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationPayonlineDemo.Controllers
{
    public class OrderInfo
    {
        public string orderno { get; set; }
        public string product_code { get; set; }
        public string product_name { get; set; }
        public string product_desc { get; set; }
        public float price { get; set; }
        public int num { get; set; }
        public string submittime { get; set; }
        public string transaction_id { get; set; }
        public float cash_fee { get; set; }
        public string paytime { get; set; }
        public string paytype { get; set; }

        public string paystate { get; set; }

    }
}