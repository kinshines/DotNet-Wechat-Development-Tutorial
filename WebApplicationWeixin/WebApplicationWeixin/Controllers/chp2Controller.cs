using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationWeixin.Models;

namespace WebApplicationWeixin.Controllers
{
    public class chp2Controller : Controller
    {
        // GET: chp2
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