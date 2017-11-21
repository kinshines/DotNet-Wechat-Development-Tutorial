using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;

namespace WebApplicationWeixin.Areas.area10.Controllers
{
    public class JsSdkController : Controller
    {
        // GET: area10/JsSdk
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getticket()
        {
            ViewBag.jsapi_ticket = JssdkService.Jsapi_ticket;
            ViewBag.jsapi_ticket_validate_time = JssdkService.jsapi_ticket_validate_time;

            return View();
        }

        public ActionResult jssdk_demo()
        {
            return View();
        }
    }
}