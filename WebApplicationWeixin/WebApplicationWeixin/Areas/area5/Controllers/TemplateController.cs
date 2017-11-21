using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;

namespace WebApplicationWeixin.Areas.area5.Controllers
{
    public class TemplateController : Controller
    {
        // GET: area5/Template
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetTemplateID(FormCollection form)
        {
            string template_id_short = form["template_id_short"];
            string str_json = "{ \"template_id_short\": \"" + template_id_short + "\"}";
            string url = "https://api.weixin.qq.com/cgi-bin/template/api_add_template?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);

            return View();
        }

    }
}