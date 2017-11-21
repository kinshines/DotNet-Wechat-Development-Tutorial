using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using wxBase.Model;
using wxBase.Model.OAuth2;
using wxBase.Model.Pay;

namespace wxBase
{
    public static class wxPayService
    {
        public static readonly string APPID =weixinService.appid; //公众号的APPID
        public const string TENPAY = "1";
        public static readonly string PARTNER = ConfigurationManager.AppSettings["MCHID"].ToString();//商户号
        public static readonly string APPSECRET = ConfigurationManager.AppSettings["appsecret"].ToString();
        public static string PARTNER_KEY = ConfigurationManager.AppSettings["PARTNER_KEY"].ToString();
        public  static readonly string OAUTH2 = "https://open.weixin.qq.com/connect/oauth2/authorize";
        public  static readonly string OAUTH2_ACCESS_TOKEN = "https://api.weixin.qq.com/sns/oauth2/access_token";
        //支付密钥Key(原来的Paysignkey )
        public const string PAY_SIGNKEY = "dfasfffffffffffa";//paysignkey(非appkey) 

        //服务器异步通知页面路径
        public static readonly string TENPAY_NOTIFY = "http://www.youxuewang.com.cn/weixin/WXPayNotify_URL";
        //public static readonly string NOTIFY_URL_Card_Store = "/wx/WXPayNotify_URL.aspx";// ConfigurationManager.AppSettings["WXPayNotify_URL_CardStore"].ToString();
        //public static readonly string NOTIFY_URL_Card_User = "/wx/WXPayNotify_URL.aspx"; //ConfigurationManager.AppSettings["WXPayNotify_URL_CardUser"].ToString();
        //public static readonly string NOTIFY_URL_HB_Store = "/wx/WXPayNotify_URL.aspx";// ConfigurationManager.AppSettings["WXPayNotify_URL_CardStore"].ToString();

        /// <summary>
        /// 生成支付Json字符串（发送给财付通的报文）
        /// </summary>
        /// <param name="openid">支付用户的openid</param>
        /// <param name="Bill_No">订单号</param>
        /// <param name="Charge_Amt">支付金额</param>
        /// <param name="Body">支付描述</param>
        /// <returns>支付Json字符串</returns>
        public static wxpayPackage MakePayPackage(string openid, string Bill_No, decimal Charge_Amt, string Body)
        {
            LogService.Write("MakePayJsonstr:Bill_No:" + Bill_No+ ", Charge_Amt:" + Charge_Amt+ ", Body"+ Body);

            HttpContext Context = System.Web.HttpContext.Current;

            if (openid.Length == 0)
            {
                return null;
            }
            // *********** here ************
            //设置package订单参数
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();

            string total_fee = (Charge_Amt * 100).ToString("f0");// 支付金额单位为分
            string wx_timeStamp = "";   //时间戳
            string wx_nonceStr = getNoncestr();//随机字符串

            dic.Add("appid", APPID); //微信APPID

            dic.Add("mch_id", PARTNER);//商家的财付通帐号
            dic.Add("device_info", "1000");//设备号，可为空
            dic.Add("nonce_str", wx_nonceStr);  // 随机字符串
            dic.Add("trade_type", "JSAPI"); //交易类型
            dic.Add("attach", "att1");// 附加数据
            dic.Add("openid", openid); // 支付用户的openid
            dic.Add("out_trade_no", Bill_No);		//商家订单号
            dic.Add("total_fee", total_fee); //商品金额,以分为单位(money * 100).ToString()
            dic.Add("notify_url", TENPAY_NOTIFY.ToLower());//接收财付通通知的URL
            dic.Add("body", Body);//商品描述
            dic.Add("spbill_create_ip", Context.Request.UserHostAddress);   //用户的公网ip，不是商户服务器IP

            string get_sign = MakeSignstr(dic, PARTNER_KEY);

            string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";  // 统一支付接口
            // 支付接口
            string _req_data = "<xml>";
            _req_data += "<appid>" + APPID + "</appid>";
            _req_data += "<attach><![CDATA[att1]]></attach>";
            _req_data += "<body><![CDATA[" + Body + "]]></body> ";
            _req_data += "<device_info><![CDATA[1000]]></device_info> ";
            _req_data += "<mch_id><![CDATA[" + PARTNER + "]]></mch_id> ";
            _req_data += "<openid><![CDATA[" + openid + "]]></openid> ";
            _req_data += "<nonce_str><![CDATA[" + wx_nonceStr + "]]></nonce_str> ";
            _req_data += "<notify_url><![CDATA[" + TENPAY_NOTIFY.ToLower() + "]]></notify_url> ";
            _req_data += "<out_trade_no><![CDATA[" + Bill_No + "]]></out_trade_no> ";
            _req_data += "<spbill_create_ip><![CDATA[" + Context.Request.UserHostAddress + "]]></spbill_create_ip> ";
            _req_data += "<total_fee><![CDATA[" + total_fee + "]]></total_fee> ";
            _req_data += "<trade_type><![CDATA[JSAPI]]></trade_type> ";
            _req_data += "<sign><![CDATA[" + get_sign + "]]></sign> ";
            _req_data += "</xml>";

            //通知支付接口，拿到prepay_id
            wxPayReturnValue retValue = StreamReaderUtils.StreamReader(url, Encoding.UTF8.GetBytes(_req_data), System.Text.Encoding.UTF8, true);

            #region 获取prepay_id， 设置支付参数
            XmlDocument xmldoc = new XmlDocument();
            LogService.Write("retValue.Message="+ retValue.Message);

            xmldoc.LoadXml(retValue.Message);

            XmlNode Event = xmldoc.SelectSingleNode("/xml/prepay_id");

            wxpayPackage pp = new wxpayPackage();

            if (Event != null)
            {
                string _prepay_id = Event.InnerText;

                SortedDictionary<string, string> pay_dic = new SortedDictionary<string, string>();

                wx_timeStamp = getTimestamp();
                wx_nonceStr = getNoncestr();

                string _package = "prepay_id=" + _prepay_id;

                pay_dic.Add("appId", APPID);
                pay_dic.Add("timeStamp", wx_timeStamp);
                pay_dic.Add("nonceStr", wx_nonceStr);
                pay_dic.Add("package", _package);
                pay_dic.Add("signType", "MD5");
                LogService.Write("MakePayJsonstr:wx_timeStamp:" + wx_timeStamp);
                string get_PaySign = MakeSignstr(pay_dic, PARTNER_KEY);
                LogService.Write("MakePayJsonstr:wx_nonceStr:" + wx_nonceStr);
                LogService.Write("MakePayJsonstr:_package:" + _package);
                LogService.Write("MakePayJsonstr:get_PaySign:" + get_PaySign);


                pp.appId = APPID;
                pp.timeStamp = wx_timeStamp;
                pp.nonceStr = wx_nonceStr;
                pp.package = _package;
                pp.paySign = get_PaySign;
                pp.signType = "MD5";

            }
            #endregion
            return pp;
        }

        #region 取得OAuth2 URL地址
        /// <summary>
        ///  取得OAuth2 URL地址,格式为https://open.weixin.qq.com/connect/oauth2/authorize?appid=APPID&redirect_uri=REDIRECT_URI&response_type=code&scope=SCOPE&state=STATE #wechat_redirect
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="Scope"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string OAuth2_GetUrl_Pay(string URL, int Scope = 0, string state = "STATE")
        {
            StringBuilder sbCode = new StringBuilder(OAUTH2); // https://open.weixin.qq.com/connect/oauth2/authorize
            sbCode.Append("?appid=" + APPID);
            sbCode.Append("&scope=" + (Scope == 1 ? "snsapi_userinfo" : "snsapi_base"));
            sbCode.Append("&state=" + state);
            sbCode.Append("&redirect_uri=" + URL);// + Uri.EscapeDataString(URL));
            sbCode.Append("&response_type=code#wechat_redirect");

            return sbCode.ToString();
        }
        #endregion

        #region 取得OAuth2 Access_Token
        public static wxPayReturnValue OAuth2_Access_Token(string Code)
        {
            StringBuilder sbCode = new StringBuilder(OAUTH2_ACCESS_TOKEN);
            sbCode.Append("?appid=" + APPID);
            sbCode.Append("&secret=" + APPSECRET);
            sbCode.Append("&code=" + Code);
            sbCode.Append("&grant_type=authorization_code");

            wxPayReturnValue retValue = StreamReaderUtils.StreamReader(sbCode.ToString(), Encoding.UTF8);

            if (retValue.HasError)
            {
                return retValue;
            }

            try
            {
                UserAccessToken uat = JSONHelper.JSONToObject<UserAccessToken>(retValue.Message);
                retValue.PutValue("Weixin_OpenID", uat.openid);
                retValue.PutValue("Weixin_Token", uat.access_token);
                //retValue.PutValue("Weixin_ExpiresIn", intWeixin_ExpiresIn);
                //retValue.PutValue("Weixin_ExpiresDate", DateTime.Now.AddSeconds(intWeixin_ExpiresIn));
                //retValue.PutValue("refresh_token", StringUtils.GetJsonValue(retValue.Message, "refresh_token").ToString());
                //retValue.PutValue("scope", StringUtils.GetJsonValue(retValue.Message, "scope").ToString());
            }
            catch
            {
                retValue.HasError = true;
              //  retValue.Message = retValue.Message;
                retValue.ErrorCode = "";
            }

            return retValue;
        }
        #endregion

        public static string MakeRedPackPackage(wxRedPackPackage pack)
        {
            // 处理nonce_str随机字符串，不长于 32 位（本程序生成长度为16位的）
            pack.nonce_str = getNoncestr();
            #region 商户信息从config文件中读取
            //商户支付密钥key
            string key = ConfigurationManager.AppSettings["key"].ToString();
            //商户号
            pack.mch_id = ConfigurationManager.AppSettings["mch_id"].ToString();
            //商户 appid 
            pack.wxappid = ConfigurationManager.AppSettings["wxappid"].ToString();
            //提供方名称 
            pack.nick_name = ConfigurationManager.AppSettings["nick_name"].ToString();
            pack.act_id = "act_id";
            //红包収送者名称 
            pack.send_name = ConfigurationManager.AppSettings["send_name"].ToString();
            //红包収放总人数
            pack.total_num = int.Parse(ConfigurationManager.AppSettings["total_num"].ToString());
            //红包祝福语
            pack.wishing = ConfigurationManager.AppSettings["wishing"].ToString();
            //活动名称 
            pack.act_name = ConfigurationManager.AppSettings["act_name"].ToString();
            //备注信息 
            pack.remark = ConfigurationManager.AppSettings["remark"].ToString();
            //商户logo的url 
            pack.logo_imgurl = ConfigurationManager.AppSettings["logo_imgurl"].ToString();
            //分享文案 
            pack.share_content = ConfigurationManager.AppSettings["share_content"].ToString();
            //分享链接
            pack.share_url = ConfigurationManager.AppSettings["share_url"].ToString();
            //分享的图片url 
            pack.share_imgurl = ConfigurationManager.AppSettings["share_imgurl"].ToString();
            //调用接口的机器 Ip 地址
            pack.client_ip = "";
            #endregion

            #region 订单信息
            //生成订单号组成： mch_id+yyyymmdd+10 位一天内不能重复的数字
            //生成10位不重复的数字
            string num = "0123456789";
            string randomNum = RandomStr(num, 10);
            pack.mch_billno =  System.DateTime.Now.ToString("yyyyMMdd") + randomNum;
            #endregion
            string postData = @"<xml> 
                                 <mch_billno>{0}</mch_billno> 
                                 <mch_id>{1}</mch_id> 
                                 <wxappid>{2}</wxappid> 
                                 <nick_name>{3}</nick_name> 
                                 <send_name>{4}</send_name> 
                                 <re_openid>{5}</re_openid> 
                                 <total_amount>{6}</total_amount> 
                                 <min_value>{7}</min_value> 
                                 <max_value>{8}</max_value> 
                                 <total_num>{9}</total_num> 
                                 <wishing>{10}</wishing> 
                                 <client_ip>{11}</client_ip> 
                                 <act_name>{12}</act_name> 
                                 <act_id>{13}</act_id> 
                                 <remark>{14}</remark>
                                 <nonce_str>{15}</nonce_str>";
            postData = string.Format(postData,
                                            pack.mch_billno,
                                            pack.mch_id,
                                            pack.wxappid,
                                            pack.nick_name,
                                            pack.send_name,
                                            pack.re_openid,
                                            pack.total_amount,
                                            pack.min_value,
                                            pack.max_value,
                                            pack.total_num,
                                            pack.wishing,
                                            pack.client_ip,
                                            pack.act_name,
                                            pack.act_id,
                                            pack.remark,
                                            pack.nonce_str
                                );


            //原始传入参数
            string[] signTemp = { "mch_billno=" + pack.mch_billno, "mch_id=" + pack.mch_id, "wxappid=" + pack.wxappid, "nick_name=" + pack.nick_name, "send_name=" + pack.send_name, "re_openid=" + pack.re_openid, "total_amount=" + pack.total_amount, "min_value=" + pack.min_value, "max_value=" + pack.max_value, "total_num=" + pack.total_num, "wishing=" + pack.wishing, "client_ip=" + pack.client_ip, "act_name=" + pack.act_name, "act_id=" + pack.act_id, "remark=" + pack.remark, "nonce_str=" + pack.nonce_str };

            List<string> signList = signTemp.ToList();

            //拼接原始字符串
            if (!string.IsNullOrEmpty(pack.logo_imgurl))
            {
                postData += "<logo_imgurl>{0}</logo_imgurl> ";
                postData = string.Format(postData, pack.logo_imgurl);
                signList.Add("logo_imgurl=" + pack.logo_imgurl);
            }
            if (!string.IsNullOrEmpty(pack.share_content))
            {
                postData += "<share_content>{0}</share_content> ";
                postData = string.Format(postData, pack.share_content);
                signList.Add("share_content=" + pack.share_content);
            }
            if (!string.IsNullOrEmpty(pack.share_url))
            {
                postData += "<share_url>{0}</share_url> ";
                postData = string.Format(postData, pack.share_url);
                signList.Add("share_url=" + pack.share_url);
            }
            if (!string.IsNullOrEmpty(pack.share_imgurl))
            {
                postData += "<share_imgurl>{0}</share_imgurl> ";
                postData = string.Format(postData, pack.share_imgurl);
                signList.Add("share_imgurl=" + pack.share_imgurl);
            }

            #region 处理支付签名
            //对signList按照ASCII码从小到大的顺序排序
            signList.Sort();

            string signOld = string.Empty;
            string payForWeiXinOld = string.Empty;
            int i = 0;
            foreach (string temp in signList)
            {
                signOld += temp + "&";
                i++;
            }
            signOld = signOld.Substring(0, signOld.Length - 1);
            //拼接Key
            signOld += "&key=" + key;
            //处理支付签名
            pack.sign = GetMD5(signOld).ToUpper();
            #endregion
            postData += "<sign>{0}</sign></xml>";
            postData = string.Format(postData, pack.sign);
            return postData;
        }

        public static string RandomStr(string str, int Length)
        {
            string result = string.Empty;
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                result += str[rd.Next(str.Length)];
            }
            return result;
        }

        public static string getNoncestr()
        {
            Random random = new Random();
            return GetMD5(random.Next(1000).ToString(), "GBK");
        }

        public static string getTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        /// <summary>
        /// 生成签名字符串
        /// </summary>
        /// <param name="sParaTemp">签名的数据</param>
        /// <param name="key">密钥</param>
        /// <returns>签名字符串</returns>
        public static string MakeSignstr(SortedDictionary<string, string> sParaTemp, string key)
        {
            //获取过滤后的数组
            Dictionary<string, string> dicPara = new Dictionary<string, string>();
            dicPara = FilterPara(sParaTemp);// 过滤掉数组中的空值和签名参数并以字母a到z的顺序排序

            //组合参数数组
            string prestr = CreateLinkString(dicPara);
            //拼接支付密钥
            string stringSignTemp = prestr + "&key=" + key;

            //获得加密结果
            string myMd5Str = GetMD5(stringSignTemp);

            //返回转换为大写的加密串
            return myMd5Str.ToUpper();
        }

        /// <summary>
        /// 除去数组中的空值和签名参数并以字母a到z的顺序排序
        /// </summary>
        /// <param name="dicArrayPre">过滤前的参数组</param>
        /// <returns>过滤后的参数组</returns>
        public static Dictionary<string, string> FilterPara(SortedDictionary<string, string> dicArrayPre)
        {
            Dictionary<string, string> dicArray = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> temp in dicArrayPre)
            {
                if (temp.Key != "sign" && !string.IsNullOrEmpty(temp.Value))
                {
                    dicArray.Add(temp.Key, temp.Value);
                }
            }

            return dicArray;
        }

        //组合参数数组，将数组中数据拼接成url格式
        public static string CreateLinkString(Dictionary<string, string> dicArray)
        {
            StringBuilder prestr = new StringBuilder();
            foreach (KeyValuePair<string, string> temp in dicArray)
            {
                prestr.Append(temp.Key + "=" + temp.Value + "&");
            }

            // 去掉最后一个&
            int nLen = prestr.Length;
            prestr.Remove(nLen - 1, 1);

            return prestr.ToString();
        }

        //加密
        public static string GetMD5(string pwd)
        {
            MD5 md5Hasher = MD5.Create();

            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(pwd));

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        /** 获取大写的MD5签名结果 */
        public static string GetMD5(string encypStr, string charset)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch (Exception ex)
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }

        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <param name="group_type">团队类型。0-散客、1-团队</param>
        /// <param name="product_type">产品类型。1-旅游线路，2-租车</param>
        public static string generate_orderno(int group_type, int product_type)
        {
            Random rand = new Random();

            string orderno = group_type.ToString() + product_type.ToString() + DateTime.Now.ToString("yyMMddHHmmssfff") + rand.Next(100).ToString();
            return orderno;
        }

    }

}
