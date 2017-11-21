using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace onlinepayBase.PinganBank.Messages
{
    public class tcvMessagePay: ReceiveMessage
    {
        /// <summary>
        /// 订单状态，只返回‘01’，表示成功
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 支付完成时间
        /// </summary>
        public string date { get; set; }
        /// <summary>
        /// 订单手续费金额
        /// </summary>
        public decimal charge { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        public string masterId { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderId { get; set; }
        /// <summary>
        /// 币种，目前只支持RMB
        /// </summary>
        public string currency { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        public string paydate { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public decimal amount { get; set; }
        /// <summary>
        /// 款项描述
        /// </summary>
        public string objectName { get; set; }
        /// <summary>
        /// 订单有效期(秒)，0不生效
        /// </summary>
        public int validtime { get; set; }
        /// <summary>
        /// 备注字段
        /// </summary>
        public string remark { get; set; }
    }
}
