using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wxBase.wxPay
{
    public class NativePay
    {
        /**
* 生成直接支付url，支付url有效期为2小时,模式二
* @param productId 商品ID
* @return 模式二URL
*/
        public string GetPayUrl(string productCode, string productName, string productDesc, string orderno, int total_fee)
        {
            LogService.Write( "Native pay mode 2 url is producing...");

            if (total_fee <= 0)
                total_fee = 1;
            WxPayData data = new WxPayData();
            data.SetValue("body", productDesc);//商品描述
            data.SetValue("attach", productName);//附加数据
            data.SetValue("out_trade_no", orderno);//随机字符串
            data.SetValue("total_fee", total_fee);//总金额
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
            data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
            data.SetValue("goods_tag", productName);//商品标记
            data.SetValue("trade_type", "NATIVE");//交易类型
            data.SetValue("product_id", productCode);//商品ID

            WxPayData result = WxPayApi.UnifiedOrder(data);//调用统一下单接口
            LogService.Write("调用统一下单接口的返回结果：" + result.ToXml());
            string url = result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码链接

            LogService.Write("Get native pay mode 2 url : " + url);
            return url;
        }

    }
}
