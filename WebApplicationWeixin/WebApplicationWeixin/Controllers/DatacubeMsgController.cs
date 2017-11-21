using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;

namespace WebApplicationWeixin.Controllers
{
    public class DatacubeMsgController : Controller
    {
        // GET: DatacubeMsg
        public ActionResult Index()
        {
            Response.Write(wxDatacubeService.getupstreammsg("2016-11-01", "2016-11-07"));
            return View();
        }
    }
}