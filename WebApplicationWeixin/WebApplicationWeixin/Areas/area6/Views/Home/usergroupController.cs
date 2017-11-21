using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;

namespace WebApplicationWeixin.Areas.area6.Views.Home
{
    public class usergroupController : Controller
    {
        // GET: area6/usergroup
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult add(FormCollection form)
        {
            string groupname = form["groupname"];
            string str_json = "{ \"group\":{ \"name\":\"" + groupname + "\"}";
            string url = "https://api.weixin.qq.com/cgi-bin/groups/create?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);

            return View();
        }

    }
}