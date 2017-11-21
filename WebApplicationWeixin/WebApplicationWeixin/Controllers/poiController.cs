using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;
using wxBase.Model.Media;
using wxBase.Model.poi;

namespace WebApplicationWeixin.Controllers
{
    public class poiController : Controller
    {
        // GET: poi
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult add(FormCollection form)
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
                ViewBag.imgurl= ui.url;
                return View();
            }
            //{ "url":"http:\/\/mmbiz.qpic.cn\/mmbiz_jpg\/crL5YN9AycYvxK3y2plSTdhFpWHWfIOAqJUcQh740mwZzJK16SGuStYia44tcmosWiak3wfO3NjxQSZ5r9via1qKw\/0"}
        }

        public ActionResult getpoilist(FormCollection form)
        {

            string str_json = "{\"begin\":0,\"limit\":20}";
            string url = "https://api.weixin.qq.com/cgi-bin/poi/getpoilist?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);

            wxPoilist poilist = JSONHelper.JSONToObject < wxPoilist >( result);
            ViewBag.poilist = poilist;
            return View();
        }

        public ActionResult save()
        {
            string poi_id = Request.Form["poi_id"];
            string telephone = Request.Form["telephone"];
            string photo1 = Request.Form["photo1"];
            string photo2 = Request.Form["photo2"];
            string photo3 = Request.Form["photo3"];
            string photo4 = Request.Form["photo4"];
            string photo5 = Request.Form["photo5"];
            string recommend = Request.Form["recommend"];
            string special = Request.Form["special"];
            string introduction = Request.Form["introduction"];
            string open_time = Request.Form["open_time"];
            string avg_price = Request.Form["avg_price"];

            string json = "{\"business\":{\"base_info\":{" +
                "\"poi_id\":\"" + poi_id + "\"\"telephone\":\"" + telephone + "\"" +
                "\"photo_list\":[{\"photo_url\":\"" + photo1 + "\"}，{\"photo_url\":\"" + photo2 + "\"}，{\"photo_url\":\"" + photo3 + "\"}，{\"photo_url\":\"" + photo4 + "\"}，{\"photo_url\":\"" + photo5 + "\"}],\"recommend\":\"" + recommend + "\",\"special\":\"" + special + "\",\"introduction\":\"" + introduction + "\",\"open_time\":\"" + open_time + "\", \"avg_price\":" + avg_price + "}}}";

            string url = "	http://api.weixin.qq.com/cgi-bin/poi/getpoi?access_token=" + weixinService.Access_token;

            string result = HttpService.Post(url, json);
            wxReturnPoiInfo info = JSONHelper.JSONToObject<wxReturnPoiInfo>(result);
            Response.Redirect("/poi/getpoilist");

            return View(info);
        }

        public ActionResult viewinfo(string poi_id)
        {
            string json = "{\"poi_id\":\"" + poi_id + "\"}";
            string url = "	http://api.weixin.qq.com/cgi-bin/poi/getpoi?access_token=" + weixinService.Access_token;

            string result = HttpService.Post(url, json);
            wxReturnPoiInfo info = JSONHelper.JSONToObject<wxReturnPoiInfo>(result);

            return View(info);
        }

        public ActionResult delete(string poi_id)
        {
            string json = "{\"poi_id\":\"" + poi_id + "\"}";
            string url = "	http://api.weixin.qq.com/cgi-bin/poi/delpoi?access_token=" + weixinService.Access_token;

            string result = HttpService.Post(url, json);
            Response.Redirect("~/poi/getpoilist");

            return View();
        }

        public ActionResult addcard()
        {

            return View();
        }

        public ActionResult savecard()
        {
            string logo_url = Request.Form["logo_url"];
            string brand_name = Request.Form["brand_name"];
            string title = Request.Form["title"];
            string sub_title = Request.Form["sub_title"];
            string notice = Request.Form["notice"];
            string service_phone = Request.Form["service_phone"];
            string description = Request.Form["description"];
            string begin_date = Request.Form["begin_date"];
            string end_date = Request.Form["end_date"];
            string quantity = Request.Form["quantity"];
            string center_title = Request.Form["center_title"];
            string center_sub_title = Request.Form["center_sub_title"];
            string center_url = Request.Form["center_url"];
            string custom_url_name = Request.Form["custom_url_name"];
            string custom_url_sub_title = Request.Form["custom_url_sub_title"];
            string custom_url = Request.Form["custom_url"];
            string deal_detail = Request.Form["deal_detail"];

            DateTime bdate = DateTime.Now, edate= DateTime.Now;
            try
            {
                bdate = DateTime.Parse(begin_date);
            }
            catch (Exception)
            {

            }
            try
            {
                edate = DateTime.Parse(end_date);
            }
            catch (Exception)
            {

            }

            double begin_timestamp = weixinService.ConvertDateTimeInt(bdate);
            double end_timestamp = weixinService.ConvertDateTimeInt(edate);

            if (custom_url_sub_title.Length > 6)
                custom_url_sub_title = custom_url_sub_title.Substring(0, 5);
            if (custom_url_name.Length > 4)
                custom_url_name = custom_url_name.Substring(0, 4);

            string str_json = "{ \"card\": { \"card_type\": \"GROUPON\","+
                               "\"groupon\":{"+
                               "\"base_info\":{"+
                               "\"logo_url\": \""+logo_url+"\","+
                               "\"brand_name\": \""+brand_name+"\","+
                               "\"code_type\": \"CODE_TYPE_TEXT\","+
                               "\"title\": \""+title+"\","+"\"sub_title\": \""+sub_title+"\","+
                               "\"color\": \"Color010\",\"notice\": \""+notice+"\","+
                               "\"service_phone\": \""+service_phone+"\","+
                               "\"description\": \""+description+"\",\"date_info\": {"+
                                "\"type\": \"DATE_TYPE_FIX_TIME_RANGE\","+
                                "\"begin_timestamp\":"+ begin_timestamp + ",\"end_timestamp\":" + end_timestamp +
                                "}, \"sku\": { \"quantity\": 5000},\"use_custom_code\": false,\"bind_openid\": false," +
                               "\"can_share\": true, \"can_give_friend\": true,"+
                               "\"center_title\": \""+center_title+"\",\"center_sub_title\": \""+ center_sub_title + "\","+
                               "\"center_url\": \""+center_url+"\",\"custom_url_name\": \""+ custom_url_name + "\","+
                               "\"custom_url\": \""+ custom_url + "\",\"custom_url_sub_title\": \""+ custom_url_sub_title + "\""+
                               "}" +// end of base_info
                               ", \"deal_detail\": \""+ deal_detail + "\""+
                               "}" +// end of groupon
                               "}" +//  end of card
                               "}";  // end
            string url = "https://api.weixin.qq.com/card/create?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);

            wxReturnAddcard card_return =  JSONHelper.JSONToObject<wxReturnAddcard>(result);
            if (card_return.errcode == 0)
                ViewBag.cardid = card_return.card_id;
            return View();
        }

    }
}