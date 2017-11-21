using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace wxBase
{
    public static class wxMediaService
    {
        /// <summary>
        /// 将图片Post到微信
        /// </summary>
        /// <param name="url">上传素材开发接口</param>
        /// <param name="path">本地图片路径</param>
        /// <returns></returns>
        public static string HttpUploadFile(string url, string path)
        {
            // 设置参数
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            //设置请求随重定向响应
            request.AllowAutoRedirect = true;
            // 提交方式
            request.Method = "POST";
            string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
            request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            int pos = path.LastIndexOf("\\");
            string fileName = path.Substring(pos + 1);

            //请求头部信息
            StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", fileName));
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            byte[] bArr = new byte[fs.Length];
            fs.Read(bArr, 0, bArr.Length);
            fs.Close();

            Stream postStream = request.GetRequestStream();
            postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
            postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
            postStream.Write(bArr, 0, bArr.Length);
            postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            postStream.Close();

            //发送请求并获取相应回应数据
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求
            Stream instream = response.GetResponseStream();
            StreamReader sr = new StreamReader(instream, Encoding.UTF8);
            //返回结果网页（html）代码
            string content = sr.ReadToEnd();
            return content;
        }


        /// <summary>
        /// 获取临时素材
        /// </summary>
        /// <param name="media_id">临时素材id</param>
        /// <param name="path">保存临时素材的文件的绝对路径</param>
        public static void Get(string media_id, string path)
        {
            string token = weixinService.Access_token;
            //准备获取临时素材的接口url
            string url = "https://api.weixin.qq.com/cgi-bin/media/get?access_token=" + token + "&media_id=" + media_id;
            //提交请求
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
            // 处理返回数据
            using (WebResponse wr = req.GetResponse())
            {
                //在这里对接收到的素材内容进行处理 
                System.IO.Stream strm = wr.GetResponseStream();
                byte[] buffer = new byte[2048];// 缓冲区，每次读取2K数据
                byte[] result = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    while (true)
                    {
                        int read = strm.Read(buffer, 0, buffer.Length);

                        // 直到读取完最后的2K数据就可以返回结果了 
                        if (read <= 0)
                        {
                            result = ms.ToArray();
                            break;
                        }
                        ms.Write(buffer, 0, read);
                    }
                }
                //保存到文件
                File.WriteAllBytes(path, result);
            }
        }

        public static string add_news(string content)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, content);
            return result;
        }

        /// <summary>
        /// 将永久素材Post到微信
        /// </summary>
        /// <param name="url">上传永久素材开发接口</param>
        /// <param name="path">本地图片路径</param>
        /// <returns></returns>

        public static string add_material(string url, string path)
        {
            var boundary = "fbce142e-4e8e-4bf3-826d-cc3cf506cccc"; // 分隔符
            var client = new HttpClient();// 使用HttpClient对象实现文件上传
            // 设置默认请求包头
            client.DefaultRequestHeaders.Add("User-Agent", "KnowledgeCenter");
            client.DefaultRequestHeaders.Remove("Expect");
            client.DefaultRequestHeaders.Remove("Connection");
            client.DefaultRequestHeaders.ExpectContinue = false;
            client.DefaultRequestHeaders.ConnectionClose = true;
            // 设置默认请求包体
            var content = new MultipartFormDataContent(boundary);
            content.Headers.Remove("Content-Type");
            content.Headers.TryAddWithoutValidation("Content-Type", "multipart/form-data; boundary=" + boundary);
            //处理图片
            Image image = Image.FromFile("path");
            byte[] ImageByte = ImageToBytes(image);
            var contentByte = new ByteArrayContent(ImageByte);
            content.Add(contentByte);
            string filename = Path.GetFileName(path);
            string ext = Path.GetExtension(path).ToLower();
            string content_type = "";
            if (ext == ".jpeg")
                content_type = "image/jpeg";
            if (ext == ".png")
                content_type = "image/png";
            if (ext == ".gif")
                content_type = "image/gif";

            contentByte.Headers.Remove("Content-Disposition");
            contentByte.Headers.TryAddWithoutValidation("Content-Disposition", "form-data; name=\"media\";filename=\""+filename+"\"" + "");
            contentByte.Headers.Remove("Content-Type");
            contentByte.Headers.TryAddWithoutValidation("Content-Type", content_type);
            // 上传文件
            try
            {
                var result = client.PostAsync(url, content);
                if (result.Result.StatusCode != HttpStatusCode.OK)
                    throw new Exception(result.Result.Content.ReadAsStringAsync().Result);
                if (result.Result.Content.ReadAsStringAsync().Result.Contains("media_id"))
                {
                    var resultContent = result.Result.Content.ReadAsStringAsync().Result;
                    return resultContent;
                }
                throw new Exception(result.Result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException.Message);
            }
        }

        /// <summary>
        /// 将Image对象转换为byte[]
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image image)
        {
            ImageFormat format = image.RawFormat;
            using (MemoryStream ms = new MemoryStream())
            {
                if (format.Equals(ImageFormat.Jpeg))
                {
                    image.Save(ms, ImageFormat.Jpeg);
                }
                else if (format.Equals(ImageFormat.Png))
                {
                    image.Save(ms, ImageFormat.Png);
                }
                else if (format.Equals(ImageFormat.Bmp))
                {
                    image.Save(ms, ImageFormat.Bmp);
                }
                else if (format.Equals(ImageFormat.Gif))
                {
                    image.Save(ms, ImageFormat.Gif);
                }
                else if (format.Equals(ImageFormat.Icon))
                {
                    image.Save(ms, ImageFormat.Icon);
                }
                byte[] buffer = new byte[ms.Length];
                //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }

        }

        /// <summary>
        /// 获取永久素材
        /// </summary>
        /// <param name="media_id">永久素材id</param>
        /// <param name="path">保存永久素材的文件的绝对路径</param>
        public static void Get_material(string media_id, string path)
        {
            string token = weixinService.Access_token;
            //准备获取临时素材的接口url
            string url = "https://api.weixin.qq.com/cgi-bin/material/get_material?access_token=" + token;             //提交请求

            string media = "{\"media_id\":\""+ media_id+"\"}";

            //转换输入参数的编码类型，获取bytep[]数组 
            byte[] byteArray = Encoding.UTF8.GetBytes(media);
            //初始化新的webRequst
            //1． 创建httpWebRequest对象
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
            //2． 初始化HttpWebRequest对象
            webRequest.Method = "POST";
            webRequest.ContentType = "text/html";
            webRequest.ContentLength = byteArray.Length;
            //3． 附加要POST给服务器的数据到HttpWebRequest对象(附加POST数据的过程比较特殊，它并没有提供一个属性给用户存取，需要写入HttpWebRequest对象提供的一个stream里面。)
            Stream newStream = webRequest.GetRequestStream();//创建一个Stream,赋值是写入HttpWebRequest对象提供的一个stream里面
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();
            // 处理返回数据
            using (WebResponse wr = webRequest.GetResponse())
            {
                //在这里对接收到的素材内容进行处理 
                System.IO.Stream strm = wr.GetResponseStream();
                byte[] buffer = new byte[2048];// 缓冲区，每次读取2K数据
                byte[] result = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    while (true)
                    {
                        int read = strm.Read(buffer, 0, buffer.Length);

                        // 直到读取完最后的2K数据就可以返回结果了 
                        if (read <= 0)
                        {
                            result = ms.ToArray();
                            break;
                        }
                        ms.Write(buffer, 0, read);
                    }
                }
                //保存到文件
                File.WriteAllBytes(path, result);
            }
        }

        public static string delete(string media_id)
        {
            string str_json = "{\"media_id\":\"" + media_id + "\"}";
            string url = " https://api.weixin.qq.com/cgi-bin/material/del_material?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);
            return result;
        }

        public static string get_materialcount()
        {
            string url = " https://api.weixin.qq.com/cgi-bin/material/get_materialcount?access_token=" + weixinService.Access_token;
            string result = HttpService.Get(url);

            return result;
        }

        public static string batchget_material(string materialtype)
        {
            string json = "{\"type\":\"" + materialtype + "\",\"offset\":0,\"count\":10 }";

            string url = " https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, json);

            return result;
        }


    }
}