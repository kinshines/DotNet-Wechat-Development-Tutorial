using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;
using wxBase.Model.Kefu;

namespace WebApplicationWeixin.Controllers
{
    public class KefuController : Controller
    {
        // GET: Kefu
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult set_industry(FormCollection form)
        {
            string industry_id1 = form["industry_id1"];
            string industry_id2 = form["industry_id2"];
            string str_json = "{\"industry_id1\":\"" + industry_id1 + "\", \"industry_id2\": \"" + industry_id2 + "\"}";
            string url = " https://api.weixin.qq.com/cgi-bin/template/api_set_industry?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);

            return View();
        }

        public ActionResult getkflist()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/customservice/getkflist?access_token=" + weixinService.Access_token;
            string result = HttpService.Get(url);
            wxModelKf_list kflist = JSONHelper.JSONToObject<wxModelKf_list>(result);

            return View(kflist);
        }

        [HttpPost]
        public ActionResult add_kfaccount(FormCollection form)
        {
            string kfaccount = form["kfaccount"] + "@deyuyanxue";
            string nickname = form["nickname"];
            string password = form["password"];
            string str_json = "{ \"kf_account\": \"" + kfaccount + "\",\"nickname\":\"" + nickname + "\", \"password\":\"" + password + "\"}";
            string url = "https://api.weixin.qq.com/customservice/kfaccount/add?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);

            return View();
        }

        [HttpPost]
        public ActionResult update_kfaccount(FormCollection form)
        {
            string kfaccount = form["kfaccount"] + "@deyuyanxue";
            string nickname = form["nickname"];
            string password = form["password"];
            string str_json = "{ \"kf_account\": \"" + kfaccount + "\",\"nickname\":\"" + nickname + "\", \"password\":\"" + password + "\"}";
            string url = "https://api.weixin.qq.com/customservice/kfaccount/update?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);

            return View();
        }

        [HttpPost]
        public ActionResult del_kfaccount(FormCollection form)
        {
            string kfaccount = form["kfaccount"] + "@deyuyanxue";
            string url = "https://api.weixin.qq.com/customservice/kfaccount/del?access_token=" + weixinService.Access_token + "&kf_account=" + kfaccount;
            string result = HttpService.Get(url);

            return View();
        }

        [HttpPost]
        public ActionResult Upload_headimg(FormCollection form)
        {
            string kfaccount = form["kfaccount"] + "@deyuyanxue";
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
                string url = string.Format("https://api.weixin.qq.com/customservice/kfaccount/uploadheadimg?access_token=" + weixinService.Access_token+ "&kf_account="+ kfaccount);
                string json = wxMediaService.HttpUploadFile(url, path);
                #endregion
                //UpoladThumbResult um = JSONHelper.JSONToObject<UpoladThumbResult>(json);
                //Response.Write("上传成功。媒体id:" + um.thumb_media_id + "");
                return View();
            }
        }

    }
}