using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;

namespace WebApplicationWeixin.Areas.area6.Controllers
{
    public class GetGroupIdController : Controller
    {
        // GET: area6/GetGroupId
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Get(FormCollection form)
        {
            string openid = form["openid"];
            string str_json = "{ \"openid\": \"" + openid + "\"}";
            string url = "https://api.weixin.qq.com/cgi-bin/groups/getid?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);

            return View();
        }

    }
}