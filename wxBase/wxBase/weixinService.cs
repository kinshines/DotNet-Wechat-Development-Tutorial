using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using wxBase.Model;

namespace wxBase
{
    public static class weixinService
    {
        #region 属性       
        public static string token
        {
            get
            {
                return GetAppConfig("token");
            }
        }
        /// <summary>
        /// 微信公众平台开发者appid
        /// </summary>
        public static string appid
        {
            get {
                return GetAppConfig("appid");
            }
        }

        /// <summary>
        /// 微信公众平台开发者appid
        /// </summary>
        public static string appsecret
        {
            get
            {
                return GetAppConfig("appsecret");
            }
        }

        /// <summary>
        /// access_token的有效期
        /// </summary>
        public static DateTime token_validate_time = DateTime.Now.AddDays(-1);

        private static string access_token;
        public static string Access_token
        {    
            get        
            {
                // 过期时再重新获取
                if (token_validate_time <= DateTime.Now)
                {
                    string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid+ "&secret="+ appsecret;
                   access_token = HttpService.Get(url);
                }
                wxAccessToken token = JSONHelper.JSONToObject<wxAccessToken>(access_token);
                token_validate_time = DateTime.Now.AddSeconds(token.expires_in);
                return token.access_token;
            }
        }

        /// <summary>
        ///消息加解密密钥
        /// </summary>
        public static string EncodingAESKey
        {
            get
            {
                return GetAppConfig("EncodingAESKey");
            }
        }

        #endregion



        /// <summary>
        /// 微信服务器IP地址
        /// </summary>
        /// <returns>微信服务器IP地址</returns>
        public static List<string> GetCallbackip()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/getcallbackip?access_token=" + Access_token;
            string json = HttpService.Get(url);
            //解析JSON字符串
            wxCallbackip ip = JSONHelper.JSONToObject<wxCallbackip>(json);
            return ip.ip_list;
        }

        /// <summary>
        /// 读取配置文件
        /// </summary>
        /// <param name="strKey">配置项</param>
        /// <returns></returns>
        private static string GetAppConfig(string strKey)
        {
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key == strKey)
                {
                    return ConfigurationManager.AppSettings[strKey];
                }
            }
            return null;
        }

        public static string make_signature(string timestamp, string nonce)
        {
            //字典序排序
            var arr = new[] { token, timestamp, nonce }.OrderBy(z => z).ToArray();
            // 字符串连接
            var arrString = string.Join("", arr);
            // SHA1加密
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
            StringBuilder signature = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                signature.AppendFormat("{0:x2}", b);
            }
            return signature.ToString();
        }

        /// <summary>  
        /// xml字符串转类  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="key"></param>  
        /// <returns></returns>  
        public static T XmlStr2Class<T>(string msg)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringReader sr = new StringReader(msg);
            return (T)serializer.Deserialize(sr);
        }

        ///
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        ///
        public static double ConvertDateTimeInt(System.DateTime time)
        {
            double intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            TimeSpan ts = time - startTime;
            intResult = ts.TotalSeconds;
            return intResult;
        }
    }
}