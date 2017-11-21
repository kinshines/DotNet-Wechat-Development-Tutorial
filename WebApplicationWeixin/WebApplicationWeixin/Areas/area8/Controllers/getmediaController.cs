using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;

namespace WebApplicationWeixin.Areas.area8.Controllers
{
    public class getmediaController : Controller
    {
        // GET: area8/getmedia
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Get(FormCollection form)
        {
            // 创建images目录
            string images_dir = Server.MapPath("~/images/");
            if (!Directory.Exists(images_dir))
                Directory.CreateDirectory(images_dir);
            string mediaid = form["Mediaid"];
            string filename = DateTime.Now.ToString("yyyyMMddHHmmddfff") + ".jpg";
            string path = images_dir + "\\" + filename;
            wxMediaService.Get(mediaid, path);
            Response.Redirect("~/images/" + filename);
            return View();
        }


    }
}