using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using wxBase;
using wxBase.Model;
using wxBase.Model.Media;

namespace WebApplicationWeixin.Areas.area8.Controllers
{
    public class HomeController : Controller
    {
        // GET: area8/Home
        public ActionResult Index()
        {
            return View();
        }


        public static string add_material(string url, string path)
        {
            var boundary = "fbce142e-4e8e-4bf3-826d-cc3cf506cccc";
            var client = new HttpClient();  //使用HttpClient对象将永久图片上传至微信服务器
            client.DefaultRequestHeaders.Add("User-Agent", "KnowledgeCenter");
            //  准备默认请求包头
            client.DefaultRequestHeaders.Remove("Expect");
            client.DefaultRequestHeaders.Remove("Connection");
            client.DefaultRequestHeaders.ExpectContinue = false;
            client.DefaultRequestHeaders.ConnectionClose = true;
            //将图片内容放入请求包体
            var content = new MultipartFormDataContent(boundary);
            content.Headers.Remove("Content-Type");
            content.Headers.TryAddWithoutValidation("Content-Type", "multipart/form-data; boundary=" + boundary);
            //处理图片
            Image image = Image.FromFile(path);
            byte[] ImageByte = ImageToBytes(image);
            var contentByte = new ByteArrayContent(ImageByte);
            content.Add(contentByte);
            string filename = Path.GetFileName(path);
            string ext = Path.GetExtension(path).ToLower();
            string content_type = "";
            if (ext == ".jpg" || ext == ".jpeg")
                content_type = "image/jpeg";
            if (ext == ".png")
                content_type = "image/png";
            if (ext == ".gif")
                content_type = "image/gif";

            contentByte.Headers.Remove("Content-Disposition");
            contentByte.Headers.TryAddWithoutValidation("Content-Disposition", "form-data; name=\"media\";filename=\"" + filename + "\"" + "");
            contentByte.Headers.Remove("Content-Type");
            contentByte.Headers.TryAddWithoutValidation("Content-Type", content_type);
            try
            {
                //提交至微信服务器
                var result = client.PostAsync(url, content);
                if (result.Result.StatusCode != HttpStatusCode.OK)
                    throw new Exception(result.Result.Content.ReadAsStringAsync().Result);
                // 返回结果
                if (result.Result.Content.ReadAsStringAsync().Result.Contains("media_id"))
                {
                    var resultContent = result.Result.Content.ReadAsStringAsync().Result;
                    //    var materialEntity = JsonConvert.DeserializeObject<MaterialImageReturn>(resultContent);

                    return resultContent;
                }
                throw new Exception(result.Result.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + ex.InnerException.Message);
            }

        }

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

        [HttpPost]
        public ActionResult add_material(FormCollection form)
        {

            //接收上传的文件
            if (Request.Files.Count == 0)
            {

                //Request.Files.Count 文件数为0上传不成功
                return View();
            }
            //上传的文件
            var file = Request.Files[0];
            if (file.ContentLength == 0)
            {
                //文件大小大（以字节为单位）为0时，做一些操作
                return View();
            }
            else
            {
                //文件大小不为0
                HttpPostedFileBase uploadfile = Request.Files[0];
                int pos = Request.Files[0].FileName.LastIndexOf('.');
                string ext = Request.Files[0].FileName.Substring(pos, Request.Files[0].FileName.Length - pos);
                //保存成自己的文件全路径,newfile就是你上传后保存的文件,
                string newFile = DateTime.Now.ToString("yyyyMMddHHmmss") + ext;
                string path = Server.MapPath("/upload");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                path += "//" + newFile;
                uploadfile.SaveAs(path);

                #region 将图片上传至微信服务器                    
                string url = string.Format("http://api.weixin.qq.com/cgi-bin/material/add_material?access_token={0}", weixinService.Access_token);
                string json = add_material(url, path);
                #endregion
                UpoladThumbResult um = JSONHelper.JSONToObject<UpoladThumbResult>(json);
                Response.Write("上传成功。媒体id:" + um.thumb_media_id + "");
                return View();
            }
        }

        [HttpPost]
        public ActionResult SetRemark(FormCollection form)
        {
            string openid = form["openid"];
            string remark = form["remark"];
            string str_json = "{ \" openid\": \"" + openid + "\", remark\":\"" + remark + "\"}";
            string url = " https://api.weixin.qq.com/cgi-bin/user/info/updateremark?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);

            return View();
        }

        [HttpPost]
        public ActionResult Get_Material(FormCollection form)
        {
            // 创建images目录
            string images_dir = Server.MapPath("~/images/");
            if (!Directory.Exists(images_dir))
                Directory.CreateDirectory(images_dir);
            string mediaid = form["Mediaid"];
            string filename = DateTime.Now.ToString("yyyyMMddHHmmddfff") + ".jpg";
            string path = images_dir + filename;
            wxMediaService.Get_material(mediaid, path);
            Response.Redirect("~/images/" + filename);
            return View();
        }

        [HttpPost]
        public ActionResult delete(string Mediaid)
        {
            wxResult result = JSONHelper.JSONToObject<wxResult>(wxMediaService.delete(Mediaid));
            if (result.errcode == "0")
                Response.Write("操作成功");
            else
                Response.Write("操作失败：" + result.errmsg);

            return View();
        }

        public ActionResult get_materialcount()
        {
            string json = wxMediaService.get_materialcount();
            wxMaterialcount mcount = JSONHelper.JSONToObject<wxMaterialcount>(json);
            Response.Write("您的公众号共有" + mcount.voice_count + "个语音消息，" + mcount.image_count + "个图片消息，" + mcount.news_count + "个图文消息，" + mcount.video_count + "个视频消息。");
            return View();
        }

        public ActionResult batchget_material()
        {
            string json = wxMediaService.batchget_material("image");
            wxMateriallist mlist = JSONHelper.JSONToObject<wxMateriallist>(json);
            Response.Write("您的公众号共有" + mlist.total_count + "个图片素材，本次获取" + mlist.item_count + "个素材信息。具体如下：<br/><br/>");
            for (int i = 0; i < mlist.item.Count; i++)
            {
                Response.Write("素材id:" + mlist.item[i].media_id + "<br/>");
                Response.Write("素材名字:" + mlist.item[i].name + "<br/>");
                Response.Write("最后更新时间:" + mlist.item[i].update_time + "<br/>");
                Response.Write("url:" + mlist.item[i].url + "<br/><br/>");
            }
            return View();
        }


    }
}