using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;
using wxBase.Model.Kefu;

namespace WebApplicationWeixin.Areas.area7.Controllers
{
    public class KefuController : Controller
    {
        // GET: area7/Kefu
        public ActionResult Index()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/customservice/getkflist?access_token=" + weixinService.Access_token;
            string result = HttpService.Get(url);
            wxModelKf_list kflist = JSONHelper.JSONToObject<wxModelKf_list>(result);

            return View(kflist);

        }
    }
}