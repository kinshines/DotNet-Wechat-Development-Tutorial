using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplicationWeixin.Areas.area2.Controllers
{
    public class HelloController : Controller
    {
        // GET: area2/Hello
        public ActionResult Index()
        {
            return Content("Hello World");
        }
    }
}