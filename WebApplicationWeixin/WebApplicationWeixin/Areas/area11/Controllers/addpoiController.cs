using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;
using wxBase.Model.Media;
using wxBase.Model.poi;

namespace WebApplicationWeixin.Areas.area11.Controllers
{
    public class addpoiController : Controller
    {
        // GET: area11/addpoi
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult uploadimg()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload_poiimg(FormCollection form)
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
                string url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/uploadimg?access_token=" + weixinService.Access_token);
                string json = wxMediaService.HttpUploadFile(url, path);
                #endregion
                UploadImgResult ui = JSONHelper.JSONToObject<UploadImgResult>(json);
                ViewBag.imgurl = ui.url;
                return View();
            }
        }

        [HttpPost]
        public ActionResult save(FormCollection form)
        {
            string business_name = form["business_name"];
            string branch_name = form["branch_name"];
            string province = form["province"];
            string city = form["city"];
            string district = form["district"];
            string address = form["address"];
            string telephone = form["telephone"];
            string categories = form["categories"];
            string longitude = form["longitude"];
            string latitude = form["latitude"];
            string special = form["special"];
            string open_time = form["open_time"];
            string avg_price = form["avg_price"];
            string poi_id = form["poi_id"];
            string introduction = form["introduction"];
            string recommend = form["recommend"];
            string photo1 = form["photo1"];
            string photo2 = form["photo2"];
            string photo3 = form["photo3"];
            string photo4 = form["photo4"];
            string photo5 = form["photo5"];


            string str_json = "{\"business\":{\"base_info\":{\"sid\":\"" + poi_id + "\", \"business_name\":\"" + business_name + "\", \"branch_name\":\"" + branch_name + "\", \"province\":\"" + province + "\", \"city\":\"" + city + "\", \"district\":\"" + district + "\", \"address\":\"" + address + "\", \"telephone\":\"" + telephone + "\", \"categories\":[\"" + categories + "\"], \"offset_type\":1, \"longitude\":" + longitude + ", \"latitude\":" + latitude + ", \"photo_list\":[";
            str_json += "{\"photo_url\":\"" + photo1 + "\"},";
            str_json += "{\"photo_url\":\"" + photo2 + "\"},";
            str_json += "{\"photo_url\":\"" + photo3 + "\"},";
            str_json += "{\"photo_url\":\"" + photo4 + "\"},";
            str_json += "{\"photo_url\":\"" + photo5 + "\"}";

            str_json += "], \"recommend\":\"" + recommend + "\", \"special\":\"" + special + "\", \"introduction\":\"" + introduction + "\", \"open_time\":\"" + open_time + "\", \"avg_price\":" + avg_price + " } } }";
            string url = " http://api.weixin.qq.com/cgi-bin/poi/addpoi?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);
            wxReturnAddpoi rp = JSONHelper.JSONToObject<wxReturnAddpoi>(result);
            if (rp.errcode == 0)
                Response.Redirect("/area11/Home");
            else
                ViewBag.errmag = rp.errmsg;
            return View();
        }
    }
}