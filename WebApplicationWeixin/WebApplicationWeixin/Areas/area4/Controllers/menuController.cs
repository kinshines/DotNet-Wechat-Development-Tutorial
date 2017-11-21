using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;
using wxBase.Model;
using wxBase.Model.Menu;

namespace WebApplicationWeixin.Areas.area4.Controllers
{
    public class menuController : Controller
    {
        // GET: area4/menu
        public ActionResult Index()
        {
            return View();
        }

        // GET: Menu
        public ActionResult Create()
        {
            Response.Write(wxMenuService.Create(Server.MapPath("~/menu.txt")));
            return View();
        }
        public ActionResult Get()
        {
            Response.Write(wxMenuService.Get());
            return View();
        }

        public ActionResult delete()
        {
            wxResult result = JSONHelper.JSONToObject<wxResult>(wxMenuService.delete());
            if (result.errcode == "0")
                Response.Write("操作成功");
            else
                Response.Write("操作失败：" + result.errmsg);

            return View();
        }

        public ActionResult GetConfig()
        {
            wxModelMenuConfig menu_config = JSONHelper.JSONToObject<wxModelMenuConfig>(wxMenuService.GetConfig());

            return View(menu_config);
        }

        public ActionResult addconditional()
        {
            Response.Write(wxMenuService.addconditional(Server.MapPath("~/menu01.txt")));
            return View();
        }

        public ActionResult trymatch()
        {
            Response.Write(wxMenuService.trymatch("xxxxxxxxx"));
            return View();
        }

    }
}