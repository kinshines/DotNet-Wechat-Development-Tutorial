using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;
using wxBase.Model.poi;

namespace WebApplicationWeixin.Areas.area11.Controllers
{
    public class HomeController : Controller
    {
        // GET: area11/Home
        public ActionResult Index()
        {
            string str_json = "{\"begin\":0,\"limit\":20}";
            string url = "https://api.weixin.qq.com/cgi-bin/poi/getpoilist?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);

            wxPoilist poilist = JSONHelper.JSONToObject<wxPoilist>(result);
            ViewBag.poilist = poilist;

            return View();
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
            Response.Redirect("~/area11/Home");

            return View();
        }

    }
}