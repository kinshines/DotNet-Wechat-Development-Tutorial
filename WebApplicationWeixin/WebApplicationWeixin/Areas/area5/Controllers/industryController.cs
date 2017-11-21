using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;

namespace WebApplicationWeixin.Areas.area5.Controllers
{
    public class industryController : Controller
    {
        // GET: area5/industry
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult set(FormCollection form)
        {
            string industry_id1 = form["industry_id1"];
            string industry_id2 = form["industry_id2"];
            string str_json = "{\" industry_id1\":\"" + industry_id1 + "\", \"industry_id1\"= \"" + industry_id1 + "\"}";
            string url = " https://api.weixin.qq.com/cgi-bin/template/api_set_industry?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);

            return View();
        }

    }
}