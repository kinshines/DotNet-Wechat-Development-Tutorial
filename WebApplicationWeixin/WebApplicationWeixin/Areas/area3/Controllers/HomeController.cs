using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;

namespace WebApplicationWeixin.Areas.area3.Controllers
{
    public class HomeController : Controller
    {
        // GET: area3/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult get_access_token()
        {
            Response.Write("access_token:" + weixinService.Access_token);

            return View();
        }

        public ActionResult getcallbackip()
        {
            List<string> iplist = weixinService.GetCallbackip();
            return View(iplist);
        }

    }
}