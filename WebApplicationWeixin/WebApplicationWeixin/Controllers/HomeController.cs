using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using wxBase;
using wxBase.MessageEncryption;

namespace WebApplicationWeixin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string signature = "6f5b6afb295b63792cb410c67037090cd3301332";
            string timestamp = "1478407798";
            string nonce = "277864450";
            string Encrypt = "S+NEZo8UNKrJng3x/1usqmEnjDLIokcBYs3H4+HmJmSD69rqNIdYCgFIzjTkp4JKWSVGylxGWJ4GsFtaaf1nP63KqrSo2jCRfxXoGH6oMrp3LcBnYUFsQDeR4cX2aitKscxostDcAtT3gJrWKxgMmmzWvSm6yd7MW/8tRJmrXQZ8f9e6zLtrVTAndujCB8kSL1+ToQseVskqOZ85VbHb5cLh8PaGnc2myHmETvn75n2D9yNQS+rx771r7vcj6XhXRGb0x2oEbD5SBloejpu5AdTOuxGnGcEnUSLQQua99PIJbWx7bJbt3X8x/b/Coe3tEeFIeG/dj/K87n7Mg73wjDcLH9Xmhig3b7rKIEploNgDLPy5FLCZwjy/q0uAU8/xP+ev+BR6muX1HaqNeYSLL+VifSlGoxmum9+3Fcp0kP4PKQrjl06dle80lsV/0+3O2WiqdaD+XoHzKULFvSWl6Q==";

            WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(weixinService.token, weixinService.EncodingAESKey, weixinService.appid);

            int ret = 0;
            string sMsg = "";
            ret = wxcpt.DecryptMsg(signature, timestamp, nonce, Encrypt, ref sMsg);

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult GetCallbackip()
        {
            List<string> iplist = weixinService.GetCallbackip();
            Response.Write("微信服务器IP地址:<br/>" );
            for (int i = 0; i < iplist.Count; i++)
            {
                Response.Write(iplist[i]+"<br/>");
            }

            //string xml = "<xml><ToUserName><![CDATA[gh_fb05515d34d5]]></ToUserName>< FromUserName >< ![CDATAoJY1DwaACHhgnAi5UJ1PrG7q3WwU]] ></ FromUserName >< CreateTime > 1475069430 </ CreateTime >< MsgType >< ![CDATA[text]] ></ MsgType >< Content >< ![CDATA[测试]] ></ Content >< MsgId > 6335374961606071621 </ MsgId ></ xml > ";

            //string str = xml.Substring(59, xml.Length - 59);
            //if (!string.IsNullOrEmpty(xml))
            //{
            //    //封装请求类
            //    XmlDocument requestDocXml = new XmlDocument();
            //    requestDocXml.LoadXml(xml);
            //    XmlElement rootElement = requestDocXml.DocumentElement;

            //    wxModelMsgText WxXmlModel = new wxModelMsgText();
            //    WxXmlModel.ToUserName = rootElement.SelectSingleNode("ToUserName").InnerText;
            //    WxXmlModel.FromUserName = rootElement.SelectSingleNode("FromUserName").InnerText;
            //    WxXmlModel.CreateTime = rootElement.SelectSingleNode("CreateTime").InnerText;
            //    WxXmlModel.MsgType = rootElement.SelectSingleNode("MsgType").InnerText;

                //switch (WxXmlModel.MsgType)
                //{
                //    case "text":
                //        WxXmlModel.Content = rootElement.SelectSingleNode("Content").InnerText;
                //        break;
                //    case "image":
                //        WxXmlModel.PicUrl = rootElement.SelectSingleNode("PicUrl").InnerText;
                //        break;
                //    case "event":
                //        WxXmlModel.Event = rootElement.SelectSingleNode("Event").InnerText;
                //        if (WxXmlModel.Event == "subscribe")//关注类型
                //        {
                //            WxXmlModel.EventKey = rootElement.SelectSingleNode("EventKey").InnerText;
                //        }
                //        break;
                //    default:
                //        break;
                //}
                //sohovan.com.common.CommonMethod.WriteTxt(WxXmlModel.Content);//接收的文本消息
                //                                                             //回复消息
                //                                                             //
                //                                                             //
                //                                                             //12345678
                //                                                             //
                //                                                             //
                //                                                             //0
                //                                                             //
                //string XML = "" + sohovan.com.common.CommonMethod.ConvertDateTimeInt(DateTime.Now) + "0";
                ////ResponseXML(WxXmlModel);
                //sohovan.com.common.CommonMethod.WriteTxt(XML);
                //HttpContext.Current.Response.Write(XML);
                //HttpContext.Current.Response.End();
            //}
            return View();
        }

        [HttpPost]
        public ActionResult get_all_private_template()
        {
            string url = " https://api.weixin.qq.com/cgi-bin/template/get_all_private_template?access_token=" + weixinService.Access_token;
            string result = HttpService.Get(url);

            return View();
        }

    }
}