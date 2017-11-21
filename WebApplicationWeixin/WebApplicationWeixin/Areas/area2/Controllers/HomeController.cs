using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationWeixin.Models;

namespace WebApplicationWeixin.Areas.area2.Controllers
{
    public class HomeController : Controller
    {
        // GET: area2/Home
        public ActionResult Index()
        {
            Person p = new Person();
            p.name = "张三";
            p.sex = "男";
            p.Age = 18;
            return View(p);
        }
    }
}