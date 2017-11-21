using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;
using wxBase.Model.DataCube;

namespace WebApplicationWeixin.Areas.area9.Controllers
{
    public class DataCubeController : Controller
    {
        private object wxResultUsersummary;

        // GET: area9/DataCube
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult getusersummary(FormCollection form)
        {
            string begindate = form["begindate"];
            string enddate = form["enddate"];

            string r=  wxDatacubeService.getusersummary(begindate, enddate);
            wxResultUsersummary result = JSONHelper.JSONToObject<wxResultUsersummary>(r);
            ViewBag.Result = result;
            return View();

        }

        [HttpPost]
        public ActionResult getusercumulate(FormCollection form)
        {
            string begindate = form["begindate"];
            string enddate = form["enddate"];

            wxResultUserCumulate result = JSONHelper.JSONToObject<wxResultUserCumulate>(wxDatacubeService.getusercumulate(begindate, enddate));
            ViewBag.Result = result;
            return View();

        }

        [HttpPost]
        public ActionResult getarticlesummary(FormCollection form)
        {
            string begindate = form["begindate"];
            string enddate = form["enddate"];
            string result = wxDatacubeService.getarticlesummary(begindate, enddate);
            return Content(result);

        }

        [HttpPost]
        public ActionResult getarticletotal(FormCollection form)
        {
            string begindate = form["begindate"];
            string enddate = form["enddate"];

            string result = wxDatacubeService.getarticletotal(begindate, enddate);
            return Content(result);

        }

        [HttpPost]
        public ActionResult getuserread(FormCollection form)
        {
            string begindate = form["begindate"];
            string enddate = form["enddate"];

            string result = wxDatacubeService.getuserread(begindate, enddate);
            return Content(result);

        }

        [HttpPost]
        public ActionResult getuserreadhour(FormCollection form)
        {
            string begindate = form["begindate"];
            string enddate = form["enddate"];

            string result = wxDatacubeService.getuserreadhour(begindate, enddate);
            return Content(result);

        }

        [HttpPost]
        public ActionResult getusershare(FormCollection form)
        {
            string begindate = form["begindate"];
            string enddate = form["enddate"];

            string result = wxDatacubeService.getusershare(begindate, enddate);
            return Content(result);
        }


        [HttpPost]
        public ActionResult getusersharehour(FormCollection form)
        {
            string begindate = form["begindate"];
            string enddate = form["enddate"];

            string result = wxDatacubeService.getusersharehour(begindate, enddate);
            return Content(result);
        }

        [HttpPost]
        public ActionResult getupstreammsg(FormCollection form)
        {
            string begindate = form["begindate"];
            string enddate = form["enddate"];

            string result = wxDatacubeService.getupstreammsg(begindate, enddate);
            return Content(result);

        }

        [HttpPost]
        public ActionResult getupstreammsgmonth(FormCollection form)
        {
            string begindate = form["begindate"];
            string enddate = form["enddate"];

            string result = wxDatacubeService.getupstreammsgmonth(begindate, enddate);
            return Content(result);

        }

        [HttpPost]
        public ActionResult getupstreammsgweek(FormCollection form)
        {
            string begindate = form["begindate"];
            string enddate = form["enddate"];

            string result = wxDatacubeService.getupstreammsgweek(begindate, enddate);
            return Content(result);

        }

        [HttpPost]
        public ActionResult getupstreammsghour(FormCollection form)
        {
            string begindate = form["begindate"];
            string enddate = form["enddate"];

            string result = wxDatacubeService.getupstreammsghour(begindate, enddate);
            return Content(result);

        }

        [HttpPost] 
        public ActionResult getupstreammsgdist(FormCollection form)
        {
            string begindate = form["begindate"];
            string enddate = form["enddate"];

            string result = wxDatacubeService.getupstreammsgdist(begindate, enddate);
            return Content(result);

        }

        [HttpPost]
        public ActionResult getupstreammsgdistmonth(FormCollection form)
        {
            string begindate = form["begindate"];
            string enddate = form["enddate"];

            string result = wxDatacubeService.getupstreammsgdistmonth(begindate, enddate);
            return Content(result);

        }
        [HttpPost]
        public ActionResult getupstreammsgdistweek(FormCollection form)
        {
            string begindate = form["begindate"];
            string enddate = form["enddate"];

            string result = wxDatacubeService.getupstreammsgdistweek(begindate, enddate);
            return Content(result);

        }

    }
}