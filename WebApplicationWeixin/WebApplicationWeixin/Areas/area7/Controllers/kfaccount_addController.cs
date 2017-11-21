using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;

namespace WebApplicationWeixin.Areas.area7.Controllers
{
    public class kfaccount_addController : Controller
    {
        // GET: area7/kfaccount_add
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult add(FormCollection form)
        {
            string kfaccount = form["kfaccount"] + "@公众号";
            string nickname = form["nickname"];
            string password = form["password"];
            string str_json = "{ \"kf_account\": \"" + kfaccount + "\",\"nickname\":\"" + nickname + "\", \"password\":\"" + password + "\"}";
            string url = "https://api.weixin.qq.com/customservice/kfaccount/add?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);

            return View();
        }

    }
}