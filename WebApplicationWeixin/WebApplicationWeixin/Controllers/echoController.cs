using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using wxBase;
using wxBase.MessageEncryption;
using wxBase.Model;

namespace WebApplicationWeixin.Controllers
{
    public class echoController : Controller
    {
        // GET: echo
        public ActionResult Index()
        {
            // 从请求中获取timestamp参数、nonce参数和signature参数值
            string echoString = Request.QueryString["echoStr"];
            string signature = Request.QueryString["signature"];
            string timestamp = Request.QueryString["timestamp"];
            string encrypt_type = Request.QueryString["encrypt_type"];
            string msg_signature = Request.QueryString["msg_signature"];
            string nonce = Request.QueryString["nonce"];
            //记录日志
            LogService.Write("msg_signature：" + msg_signature);
            LogService.Write("signature：" + signature);
            LogService.Write("timestamp：" + timestamp);
            LogService.Write("nonce：" + nonce);
            LogService.Write("encrypt_type：" + encrypt_type);

            if (Request.RequestType.ToUpper() == "POST")//如果是POST的数据，则记录内容
            {
                string message = PostInput();
                wxModelMessage mm = new wxModelMessage();
                LogService.Write("收到信息：" + message);
                mm.ParseXML(message);

                if (encrypt_type != "")
                {
                    LogService.Write("密文信息：" + mm.Encrypt);
                    WXBizMsgCrypt wxcpt = new WXBizMsgCrypt(weixinService.token, weixinService.EncodingAESKey, weixinService.appid);
                    int ret = 0;
                    string sMsg = "";
                    ret = wxcpt.DecryptMsg(msg_signature, timestamp, nonce, mm.Encrypt, ref sMsg);
                    if (ret != 0)
                    {
                        LogService.Write("ERR: Decrypt fail, ret: " + ret);
                        //return;
                    }
                    LogService.Write("解析后的明文：" + sMsg);

                    mm.ParseXML(sMsg);
                }
                else
                {
                    mm.ParseXML(message);
                }
                switch (mm.MsgType.ToLower())
                {
                    case "text":
                        LogService.Write("--收到来自【" + mm.FromUserName + "】的消息。消息类型：" + mm.MsgType + ", 消息id:" + mm.MsgId + "，Content:" + mm.Content + "。时间:" + mm.CreateTime);
                        break;
                    case "image":
                        LogService.Write("收到来自【" + mm.FromUserName + "】的消息。消息类型：" + mm.MsgType + ", 消息id:" + mm.MsgId + "，PicUrl:" + mm.PicUrl + "，media_id:" + mm.media_id + "。时间:" + mm.CreateTime);
                        break;
                    case "voice":
                        LogService.Write("收到来自【" + mm.FromUserName + "】的消息。消息类型：" + mm.MsgType + ", 消息id:" + mm.MsgId + "，Format:" + mm.Format + "，media_id:" + mm.media_id + "。时间:" + mm.CreateTime);
                        break;
                    case "video":
                        LogService.Write("收到来自【" + mm.FromUserName + "】的消息。消息类型：" + mm.MsgType + ", 消息id:" + mm.MsgId + "，ThumbMediaId:" + mm.ThumbMediaId + "，media_id:" + mm.media_id + "。时间:" + mm.CreateTime);
                        break;
                    case "shortvideo":
                        LogService.Write("收到来自【" + mm.FromUserName + "】的消息。消息类型：" + mm.MsgType + ", 消息id:" + mm.MsgId + "，ThumbMediaId:" + mm.ThumbMediaId + "，media_id:" + mm.media_id + "。时间:" + mm.CreateTime);
                        break;
                    case "location":
                        LogService.Write("收到来自【" + mm.FromUserName + "】的消息。消息类型：" + mm.MsgType + ", 消息id:" + mm.MsgId + "，地址:" + mm.Label + "(" + mm.Location_X + "," + mm.Location_Y + "), scale:" + mm.Scale + "时间:" + mm.CreateTime);
                        break;
                    case "link":
                        LogService.Write("收到来自【" + mm.FromUserName + "】的消息。消息类型：" + mm.MsgType + ", 消息id:" + mm.MsgId + "，标题:" + mm.Title + "消息描述" + mm.Description + "Url:" + mm.Url + "时间:" + mm.CreateTime);
                        break;
                    case "event":
                        LogService.Write("收到来自【" + mm.FromUserName + "】的消息。消息类型：" + mm.MsgType + ", 消息id:" + mm.MsgId + "事件类型:" + mm.Event + ", 时间:" + mm.CreateTime);
                        Response.Write("");
                        break;

                }
                if (mm.Content == "文本")
                    wxModelMessage.sendMessage(mm.FromUserName, "您好，欢迎光临！");
                else if (mm.Content == "图片")
                    wxModelMessage.sendImageMessage(mm.FromUserName, "D1gMtCf2t2HK8-iPBHVGV77b120z9M6J6L0jn0K8Zaw");
                else if (mm.Content == "音乐")
                    wxModelMessage.sendMusicMessage(mm.FromUserName, "水边的阿迪丽娜", "好听的钢琴曲", "http://mstar9.com/media/%E6%B0%B4%E8%BE%B9%E7%9A%84%E9%98%BF%E8%92%82%E4%B8%BD%E5%A8%9C.mp3", "http://mstar9.com/media/%E6%B0%B4%E8%BE%B9%E7%9A%84%E9%98%BF%E8%92%82%E4%B8%BD%E5%A8%9C.mp3", "zetp9PuX5LisLjqXI8ZKHOiQ2Sr7uB6U6MbtWQK2QEZzcflSSDMEjnopEIb19Nwj");
               else if(mm.Content == "客服")
                {
                    wxMessageService.Sendkftext("xiaoqiang@deyuyanxue", mm.FromUserName, "客服小强为您服务");
                }
            }
            else//如果是GET的数据，则验证消息
            {
                LogService.Write("echoString:" + echoString);
                LogService.Write("signature:" + signature);
                LogService.Write("timestamp:" + timestamp);
                LogService.Write("nonce:" + nonce);
                // 对token、timestamp、nonce三个参数进行加密，得到临时signature字符串
                string tmp_signature = weixinService.make_signature(timestamp, nonce);
                LogService.Write("tmp_signature:" + tmp_signature);
                if (tmp_signature == signature && !string.IsNullOrEmpty(echoString))
                {
                    Response.Write(echoString);
                    Response.End();
                }
                else
                {
                    Response.Write("Invalid request!");
                    Response.End();
                }
            }
            return View();
        }

        private string make_signature(string token, string timestamp, string nonce)
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

        // 获取POST返回来的数据
        private string PostInput()
        {
            try
            {
                System.IO.Stream s = Request.InputStream;
                int count = 0;
                byte[] buffer = new byte[1024];
                StringBuilder builder = new StringBuilder();
                while ((count = s.Read(buffer, 0, 1024)) > 0)
                {
                    builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
                }
                s.Flush();
                s.Close();
                s.Dispose();
                return builder.ToString();
            }
            catch (Exception ex)
            { throw ex; }
        }

    }
}