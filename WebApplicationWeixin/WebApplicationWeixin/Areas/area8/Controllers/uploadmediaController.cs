using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;
using wxBase.Model.Media;

namespace WebApplicationWeixin.Areas.area8.Controllers
{
    public class uploadmediaController : Controller
    {
        // GET: area8/uploadmedia
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(FormCollection form)
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

                #region 讲图片上传至微信服务器                    
                string url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", weixinService.Access_token, "image");
                string json = wxMediaService.HttpUploadFile(url, path);
                #endregion
                UploadMediaResult um = JSONHelper.JSONToObject<UploadMediaResult>(json);
                Response.Write("上传成功。媒体id:" + um.media_id + "");
                return View();
                //JObject jb = (JObject)JsonConvert.DeserializeObject(json);//这里就能知道返回正确的消息了下面是个人的逻辑我就没写不用看省略的
            }
        }


    }
}