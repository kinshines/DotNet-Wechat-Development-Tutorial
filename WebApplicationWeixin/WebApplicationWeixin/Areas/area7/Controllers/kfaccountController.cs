using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;

namespace WebApplicationWeixin.Areas.area7.Controllers
{
    public class kfaccountController : Controller
    {
        // GET: area7/kfaccount_edit
        public ActionResult Index()
        {
            string oldname = Request.QueryString["oldname"];

            if (!string.IsNullOrEmpty(oldname))
                ViewBag.oldname = oldname.Substring(0, oldname.IndexOf('@'));
            return View();
        }

        [HttpPost]
        public ActionResult edit(FormCollection form)
        {
            string kfaccount = form["kfaccount"] + "@公众号";
            string nickname = form["nickname"];
            string password = form["password"];
            string str_json = "{ \"kf_account\": \"" + kfaccount + "\",\"nickname\":\"" + nickname + "\", \"password\":\"" + password + "\"}";
            string url = "https://api.weixin.qq.com/customservice/kfaccount/update?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);

            Response.Redirect("/area7/Kefu");
            return View();
        }

        [HttpPost]
        public ActionResult del(FormCollection form)
        {
            string kfaccount = form["kfaccount"] + "@deyuyanxue";
            string url = "https://api.weixin.qq.com/customservice/kfaccount/del?access_token=" + weixinService.Access_token + "&kf_account=" + kfaccount;
            string result = HttpService.Get(url);

            return View();
        }

    }
}