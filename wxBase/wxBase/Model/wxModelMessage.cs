using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace wxBase.Model
{
    public class wxModelMessage
    {
        ///   
        /// 消息接收方微信号 
        /// 
        public string ToUserName { get; set; }
        ///   
        /// 消息发送方微信号 
        /// 
        public string FromUserName { get; set; }
        ///   
        /// 创建时间 
        ///   
        public string CreateTime { get; set; }
        ///
        /// 信息类型 地理位置:location,文本消息:text,消息类型:image ///   
        public string MsgType { get; set; }
        ///   
        /// 信息内容         
        public string Content { get; set; }
        ///   
        /// 地理位置纬度 
        ///   
        public string Location_X { get; set; }
        ///   
        /// 地理位置经度 
        ///   
        public string Location_Y { get; set; }
        ///   
        /// 地图缩放大小 
        /// ///   
        public string Scale { get; set; }
        /// 地理位置信息 
        ///
        public string Label { get; set; }
        ///   
        /// 图片链接，开发者可以用HTTP GET获取
        public string PicUrl { get; set; }
        ///   
        /// 事件类型，subscribe(订阅/扫描带参数二维码订阅)、unsubscribe(取消订阅)、CLICK(自定义菜单点击事件) 、SCAN（已关注的状态下扫描带参数二维码） 
        ///   
        public string Event { get; set; }
        /// 事件KEY值 
        public string EventKey { get; set; }
        /// 
        /// 二维码的ticket，可以用来换取二维码 
        ///
        public string Ticket { get; set; }

        /// <summary>
        /// 消息ID
        /// </summary>
        public string MsgId { get; set; }

        /// <summary>
        /// 图片消息媒体id，可以调用第8章介绍的多媒体文件下载接口获取数据
        /// </summary>
        public string media_id { get; set; }
        /// <summary>
        /// 图片消息媒体id，可以调用第8章介绍的多媒体文件下载接口获取数据
        /// </summary>
        public string Format { get; set; }
        /// <summary>
        /// 封面图消息媒体id，可以调用第8章介绍的多媒体文件下载接口获取数据
        /// </summary>
        public string ThumbMediaId { get; set; }

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title;
        /// <summary>
        /// 消息描述
        /// </summary>
        public string Description;
        /// <summary>
        /// 消息链接
        /// </summary>
        public string Url;

        /// <summary>
        /// 收到的密文
        /// </summary>
        public string Encrypt;
        /// <summary>
        /// 解析收到的消息
        /// </summary>
        /// <param name="requestStr"></param>
        public void ParseXML(string requestStr)
        {
            if (!string.IsNullOrEmpty(requestStr))
            {
                //封装请求类 
                try
                {
                    requestStr = requestStr.Replace("< ", "<").Replace(" >", ">").Replace("/ ", "/");
                    XmlDocument requestDocXml = new XmlDocument();
                    requestDocXml.LoadXml(requestStr);
                    XmlElement rootElement = requestDocXml.DocumentElement;
                    ToUserName = rootElement.SelectSingleNode("ToUserName").InnerText;
                    try
                    {
                        Encrypt = rootElement.SelectSingleNode("Encrypt").InnerText;
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        FromUserName = rootElement.SelectSingleNode("FromUserName").InnerText;
                    }
                    catch (Exception)
                    {

                    }
                    #region 将整数时间转换为yyyy-MM-dd格式
                    Int64 bigtime = 0;
                    try
                    {
                        CreateTime = rootElement.SelectSingleNode("CreateTime").InnerText;
                        bigtime = Convert.ToInt64(CreateTime) * 10000000;//100毫微秒为单位
                    }
                    catch (Exception)
                    {

                    }
                    // 1970-01-01 08:00:00是基准时间
                    DateTime dt_1970 = new DateTime(1970, 1, 1, 8, 0, 0);
                    long tricks_1970 = dt_1970.Ticks;//1970年1月1日刻度
                    long time_tricks = tricks_1970 + bigtime;//日志日期刻度
                    DateTime dt = new DateTime(time_tricks);//转化为DateTime
                    CreateTime = dt.ToString("yyyy-MM-dd HH:mm:ss");
                    #endregion
                    try
                    {
                        MsgId = rootElement.SelectSingleNode("MsgId").InnerText;
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        MsgType = rootElement.SelectSingleNode("MsgType").InnerText;
                    }
                    catch (Exception)
                    {

                    }
                    switch (MsgType)
                    {
                        case "text":
                            Content = rootElement.SelectSingleNode("Content").InnerText;
                            break;
                        case "image":
                            PicUrl = rootElement.SelectSingleNode("PicUrl").InnerText;
                            media_id = rootElement.SelectSingleNode("media_id").InnerText;
                            break;
                        case "voice":
                            try
                            {
                                Format = rootElement.SelectSingleNode("Format").InnerText;
                                media_id = rootElement.SelectSingleNode("MediaId").InnerText;

                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            break;
                        case "video":
                            try
                            {
                                media_id = rootElement.SelectSingleNode("MediaId").InnerText;
                                ThumbMediaId = rootElement.SelectSingleNode("ThumbMediaId").InnerText;

                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            break;
                        case "shortvideo":
                            try
                            {
                                media_id = rootElement.SelectSingleNode("MediaId").InnerText;
                                ThumbMediaId = rootElement.SelectSingleNode("ThumbMediaId").InnerText;
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            break;
                        case "location":
                            try
                            {
                                Location_X = rootElement.SelectSingleNode("Location_X").InnerText;
                                Location_Y = rootElement.SelectSingleNode("Location_Y").InnerText;
                                Scale = rootElement.SelectSingleNode("Scale").InnerText;
                                Label = rootElement.SelectSingleNode("Label").InnerText;
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                            break;
                        case "event":
                            Event = rootElement.SelectSingleNode("Event").InnerText;
                            break;
                        default: break;
                        case "link":
                            try
                            {
                                Title = rootElement.SelectSingleNode("Title").InnerText;
                                Description = rootElement.SelectSingleNode("Description ").InnerText;
                                Url = rootElement.SelectSingleNode("Url").InnerText;
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    LogService.Write("收到消息：" + requestStr + ",error:" + ex.Message);
                }
            }
        }

        private static void Send(string xmlMsg)
        {
            HttpContext context = HttpContext.Current;
            context.Response.Write(xmlMsg);
        }

        private static int GetCreateTime()
        {
            DateTime startDate = new DateTime(1970, 1, 1, 8, 0, 0);
            return (int)(DateTime.Now - startDate).TotalSeconds;
        }

        public static void sendMessage(string toUserName, string content)
        {
            string FromUserName = "deyuyanxue";
            string xmlMsg = "<xml>" +
            "<ToUserName><![CDATA[" + toUserName + "]]></ToUserName>" +
            "<FromUserName><![CDATA[" + FromUserName + "]]></FromUserName>" +
            "<CreateTime>"+GetCreateTime()+"</CreateTime>" +
            "<MsgType><![CDATA[text]]></MsgType>" +
            "<Content><![CDATA[" + content + "]]></Content>" +
            "</xml>";

            LogService.Write("回复信息:" + xmlMsg);
            Send(xmlMsg);
        }

        public static void sendImageMessage(string toUserName, string media_id)
        {
            string FromUserName = "deyuyanxue";
            string xmlMsg = "<xml>" +
            "<ToUserName><![CDATA[" + toUserName + "]]></ToUserName>" +
            "<FromUserName><![CDATA[" + FromUserName + "]]></FromUserName>" +
            "<CreateTime>" + GetCreateTime() + "</CreateTime>" +
            "<MsgType><![CDATA[image]]></MsgType>" +
            "<Image><MediaId><![CDATA[" + media_id + "]]></MediaId></Image>" +
            "</xml>";

            LogService.Write("回复信息:" + xmlMsg);
            Send(xmlMsg);
        }

        public static void sendMusicMessage(string toUserName, string title, string description, string MusicUrl, string HQMusicUrl, string ThumbMediaId)
        {
            string FromUserName = "deyuyanxue";
            string xmlMsg = "<xml>" +
            "<ToUserName><![CDATA[" + toUserName + "]]></ToUserName>" +
            "<FromUserName><![CDATA[" + FromUserName + "]]></FromUserName>" +
            "<CreateTime>" + GetCreateTime() + "</CreateTime>" +
          "<MsgType><![CDATA[music]]></MsgType>" +
           "<Music><Title><![CDATA[" + title+"]]></Title>" +
          "<Description><![CDATA["+description+"]]></Description>"+
            "<MusicUrl><![CDATA["+MusicUrl+"]]></MusicUrl>" +
"<HQMusicUrl><![CDATA["+HQMusicUrl +"]]></HQMusicUrl>" +
"<ThumbMediaId><![CDATA["+ ThumbMediaId + "]]></ThumbMediaId>" +
"</Music></xml>";

            LogService.Write("回复信息:" + xmlMsg);
            Send(xmlMsg);
        }

    }
}