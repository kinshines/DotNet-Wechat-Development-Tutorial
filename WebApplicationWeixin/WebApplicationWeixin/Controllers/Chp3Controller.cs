using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;

namespace WebApplicationWeixin.Controllers
{
    public class Chp3Controller : Controller
    {
        // GET: Chp3
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getcallbackip()
        {
            List<string> iplist = weixinService.GetCallbackip();
            return View(iplist);
        }

        public ActionResult get_access_token()
        {
          ViewBag.Access_token= weixinService.Access_token;

            return View();
        }
    }
}