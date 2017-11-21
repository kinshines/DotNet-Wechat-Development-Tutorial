using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;

namespace WebApplicationWeixin.Areas.area7.Controllers
{
    public class HomeController : Controller
    {
        // GET: area7/Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult sendall()
        {
            wxMessageService.SendtextByGroup(0, "测试消息");
            return View();
        }

        public ActionResult SendtextByOpenids()
        {
            List<string> openidslist = new List<string>();
            openidslist.Add("oD15RwLBbA4mr_-T9-2R7zE1mQSI");
            openidslist.Add("oD15RwLShriAALNLcis_mvWLm7BU");

            wxMessageService.SendtextByOpenids(openidslist, "测试消息");
            return View();
        }

    }
}