using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using wxBase.Model;

namespace wxBase
{
    public static class JssdkService
    {
        public static string nonceStr = "b4Vi0P1184GxXT3ZTN9uL6WCB43cRlf9pKGS9ORjj3p";
        /// <summary>
        /// access_token的有效期
        /// </summary>
        public static DateTime jsapi_ticket_validate_time = DateTime.Now.AddDays(-1);

        private static string jsapi_ticket;
        public static string Jsapi_ticket
        {
            get
            {
                string result = "";
                // 过期时再重新获取
                if (jsapi_ticket_validate_time <= DateTime.Now)
                {
                    string url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=" + weixinService.Access_token + "&type=jsapi";
                    result = HttpService.Get(url);
                    wxJsapi_ticket jt = JSONHelper.JSONToObject<wxJsapi_ticket>(result);
                    jsapi_ticket_validate_time = DateTime.Now.AddSeconds(jt.expires_in);
                    jsapi_ticket = jt.ticket;
                }
            return jsapi_ticket;
            }
        }

        /// <summary>
        /// 签名算法
        /// </summary>
        /// <param name="jsapi_ticket">jsapi_ticket</param>
        /// <param name="noncestr">随机字符串(必须与wx.config中的nonceStr相同)</param>
        /// <param name="timestamp">时间戳(必须与wx.config中的timestamp相同)</param>
        /// <param name="url">当前网页的URL，不包含#及其后面部分(必须是调用JS接口页面的完整URL)</param>
        /// <returns></returns>
        public static string GetSignature(string jsapi_ticket, string noncestr, long timestamp, string url)
        {
            var string1Builder = new StringBuilder();
            string1Builder.Append("jsapi_ticket=").Append(jsapi_ticket).Append("&")
                          .Append("noncestr=").Append(noncestr).Append("&")
                          .Append("timestamp=").Append(timestamp).Append("&")
                          .Append("url=").Append(url.IndexOf("#") >= 0 ? url.Substring(0, url.IndexOf("#")) : url);
            string string1 = string1Builder.ToString();
            return SHA1(string1);
        }

        //public static string nonceStr = "b4Vi0P1184GxXT3ZTN9uL6WCB43cRlf9pKGS9ORjj3p";
        /// <summary> /// 创建时间戳
        ///本代码来自开源微信SDK项目：https://github.com/night-king/weixinSDK
        /// </summary> /// <returns></returns> 
        public static long CreateTimestamp()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        /// <summary>  
        /// SHA1 加密，返回大写字符串  
        /// </summary>  
        /// <param name="content">需要加密字符串</param>  
        /// <returns>返回40位UTF8 大写</returns>  
        public static string SHA1(string content)
        {
            return SHA1(content, Encoding.UTF8);
        }
        /// <summary>  
        /// SHA1 加密，返回大写字符串  
        /// </summary>  
        /// <param name="content">需要加密字符串</param>  
        /// <param name="encode">指定加密编码</param>  
        /// <returns>返回40位大写字符串</returns>  
        public static string SHA1(string content, Encoding encode)
        {
            try
            {
                SHA1 sha1 = new SHA1CryptoServiceProvider();
                byte[] bytes_in = encode.GetBytes(content);
                byte[] bytes_out = sha1.ComputeHash(bytes_in);
                sha1.Dispose();
                string result = BitConverter.ToString(bytes_out);
                result = result.Replace("-", "");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("SHA1加密出错：" + ex.Message);
            }
        }
    }
}
