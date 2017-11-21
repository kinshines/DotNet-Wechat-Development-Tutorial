using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using ThoughtWorks.QRCode.Codec;
using wxBase;
using wxBase.Model;
using wxBase.Model.Pay;
using wxBase.wxPay;

namespace WebApplicationWeixin.Areas.area12.Controllers
{
    public class HomeController : Controller
    {
        // GET: area12/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pay(string productCode, string productName, string productDesc, float price, int num, int paytype/*1-支付宝,2-微信,3-银联*/)
        {
            string orderno = wxPayService.generate_orderno(1, 1);
            #region 保存订单记录，以便接收支付成功消息时更新订单的状态
            /// insert 到order表
            //orders o = new orders();
            //o.orderno = generate_orderno(1, 1);
            //o.product_code = cpcode;
            //o.product_name = cpname;
            //o.product_desc = cpdesc;
            //o.price = price;
            //o.num = num;
            //switch (paytype)
            //{
            //    case 1:
            //        o.paytype = "支付宝";
            //        break;
            //    case 2:
            //        o.paytype = "微信支付";
            //        break;
            //    case 3:
            //        o.paytype = "银联支付";
            //        break;
            //}

            //o.sumittime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //o.Insert();
            #endregion
            float f = price * 100.0F * num;
            int total_fee = (int)f;
            ////LogService.LOG_LEVENL = 3;
            ////LogService.Debug(this.GetType().ToString(), "total_fee=" + f);

            switch (paytype)
            {
                case 1: //支付宝
                    //Response.Redirect(Url.Action("Index", "aliPay") + "?productCode=" + cpcode + "&productName=" + cpname + "&productDesc=" + cpdesc + "&orderno=" + o.orderno + "&total_fee=" + total_fee);
                    break;
                case 2:// 微信支付
                       //    Response.Redirect(Url.Action("Index", "Weixin") + "?productCode=" + cpcode + "&productName=" + cpname + "&productDesc=" + cpdesc + "&orderno=" + orderno + "&total_fee=" + total_fee);
                    NativePay nativePay = new NativePay();
                    //生成扫码支付模式二url
                    string url = StringService.ToHexString(nativePay.GetPayUrl(productCode, productName, productDesc, orderno, total_fee));

                    Response.Redirect(Url.Action("ScanQRCodeImage", "Home") + "?url=" + HttpUtility.UrlEncode(url) + "&orderno=" + orderno);

                    break;
                case 3: // 银联
                    //Response.Redirect(Url.Action("Index", "Bank") + "?productCode=" + cpcode + "&productName=" + cpname + "&productDesc=" + cpdesc + "&orderno=" + o.orderno + "&total_fee=" + total_fee);
                    break;

            }
            return Content("");
        }

        public ActionResult ScanQRCodeImage(string url)
        {

            return View();

        }

        public ActionResult MakeQRCodeImage(string url)
        {
            string str = StringService.FromHexString(url);
            //初始化二维码生成工具
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            qrCodeEncoder.QRCodeVersion = 0;
            qrCodeEncoder.QRCodeScale = 4;

            //将字符串生成二维码图片
            Bitmap image = qrCodeEncoder.Encode(str, Encoding.Default);

            //保存为PNG到内存流  
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);

            //输出二维码图片
            byte[] bytes = ms.GetBuffer();

            return File(bytes, @"image/jpeg");
        }
        /// <summary>
        /// 支付成功后的回调函数
        /// </summary>
        /// <returns></returns>
        public ActionResult ResultNotify()
        {
            ResultNotify resultNotify = new ResultNotify(Request.InputStream);
            resultNotify.ProcessNotify();

            return View();
        }

        /// <summary>
        /// 查询订单状态
        /// </summary>
        /// <param name="orderno"></param>
        /// <returns></returns>
        public ActionResult QueryOrderStatus(string orderno)
        {
            //string sql = "SELECT paytime FROM orders WHERE orderno='" + orderno + "'";
            //SQLBase s = new SQLBase();
            //List<List<string>> result = s.ExecuteQuery(sql);
            //if (result.Count > 0)
            //    return (Content(result[0][0]));

            return Content("");
        }


        public ActionResult redpack()
        {
            string strBillNo = wxPayService.getTimestamp(); // 订单号
            // 接收微信认证服务器发送来的code
            string strCode = Request.QueryString["code"] == null ? "" : Request.QueryString["code"]; 

            LogService.Write("code:" + strCode);
            ViewBag.OpenId = "";
            if (string.IsNullOrEmpty(strCode)) //如果接收到code，则说明是OAuth2服务器回调
            {
                //进行OAuth2认证，获取code
                string _OAuth_Url = wxPayService.OAuth2_GetUrl_Pay(Request.Url.ToString());

                LogService.Write("_OAuth_Url:" + _OAuth_Url);
                Response.Redirect(_OAuth_Url);
                return View();
            }
            else
            {
                //根据返回的code，获得Access_Token
                wxPayReturnValue retValue = wxPayService.OAuth2_Access_Token(strCode);

                if (retValue.HasError)
                {
                    Response.Write("获取code失败：" + retValue.Message);
                    return Content("");
                }
                LogService.Write("retValue.Message:" + retValue.Message);

                string strWeixin_OpenID = retValue.GetStringValue("Weixin_OpenID");
                string strWeixin_Token = retValue.GetStringValue("Weixin_Token");
                LogService.Write("strWeixin_OpenID:" + strWeixin_OpenID);

                ViewBag.OpenId = strWeixin_OpenID;
            }

            return View();
        }

        public ActionResult sendredpack()
        {
            // 当前用户的openid
            string strWeixin_OpenID = Request.QueryString["openId"] == null ? "" : Request.QueryString["openId"];

            if (string.IsNullOrEmpty(strWeixin_OpenID))
                return Content("");

            PayForWeiXinHelp PayHelp = new PayForWeiXinHelp();
            wxRedPackPackage model = new wxRedPackPackage();
            //接叐收红包的用户 用户在wxappid下的openid 
            model.re_openid = strWeixin_OpenID;
            //付款金额，单位分 
            int amount = 20000;
            model.total_amount = amount;
            //最小红包金额，单位分 
            model.min_value = amount;
            //最大红包金额，单位分 
            model.max_value = amount;
            //调用接口的机器 Ip 地址
            model.client_ip = Request.UserHostAddress;
            //调用方法得到POST到发放红包借口的数据
            string postData = PayHelp.DoDataForPayWeiXin(model);
            string result = "";
            try
            {
                result = PayHelp.SendRedPack(postData);
            }
            catch (Exception ex)
            {
                //写日志
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            string jsonResult = JsonConvert.SerializeXmlNode(doc);
            //Response.ContentType = "application/json";
            //Response.Write(jsonResult);

            return Content(jsonResult);
        }
    }
}