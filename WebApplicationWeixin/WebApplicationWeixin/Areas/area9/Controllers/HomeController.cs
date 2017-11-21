using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationWeixin.Areas.area9.Controllers
{
    public class HomeController : Controller
    {
        // GET: area9/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getusersummary()
        {
            return View();
        }

        public ActionResult getusercumulate()
        {
            return View();
        }

        public ActionResult getarticlesummary()
        {
            return View();
        }
        public ActionResult getarticletotal()
        {
            return View();
        }
        public ActionResult getuserread()
        {
            return View();
        }
        public ActionResult getuserreadhour()
        {
            return View();
        }

        public ActionResult getusershare()
        {
            return View();
        }
        public ActionResult getusersharehour()
        {
            return View();
        }
        public ActionResult getupstreammsg()
        {
            return View();
        }
        public ActionResult getupstreammsgmonth()
        {
            return View();
        }

        public ActionResult getupstreammsgweek()
        {
            return View();
        }
        public ActionResult getupstreammsghour()
        {
            return View();
        }
        public ActionResult getupstreammsgdist()
        {
            return View();
        }
        public ActionResult getupstreammsgdistmonth()
        {
            return View();
        }
        public ActionResult getupstreammsgdistweek()
        {
            return View();
        }

    }
}