using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;
using wxBase.Model.Users;

namespace WebApplicationWeixin.Areas.area6.Controllers
{
    public class HomeController : Controller
    {
        // GET: area6/Home
        public ActionResult Index()
        {
            wxUserGroups mgroup = wxUsersService.get_groups();
            return View(mgroup);
        }

        public ActionResult addgroup()
        {
            return View();
        }

        public ActionResult editgroup(int groupid)
        {
            ViewBag.groupid = groupid;
            return View();
        }

        [HttpPost]
        public ActionResult modifygroup(FormCollection form)
        {
            string groupid = form["groupid"];
            string groupname = form["groupname"];
            string str_json = "{ \"group\":{ \"id\":" + groupid + ", \"name\":\", " + groupname + "\"}";
            string url = " https://api.weixin.qq.com/cgi-bin/groups/update?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);
            Response.Redirect("~/area6/Home/");
            return View();
        }

        public ActionResult deletegroup(int groupid)
        {
       //     string groupid = form["groupid"];
            string str_json = "{ \"group\":{ \"id\":" + groupid + "}";
            string url = " https://api.weixin.qq.com/cgi-bin/groups/delete?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);

            Response.Redirect("~/area6/Home/");
            return View();
        }

        public ActionResult get_users()
        {
            wxUsersSummary user = wxUsersService.get_users();
            return View(user);
        }

        public ActionResult Remark()
        {
            return View();
        }

        public ActionResult updatemembers()
        {
            return View();
        }

        public ActionResult GetGroupId()
        {
            return View();
        }        

        [HttpPost]
        public ActionResult updateGroup(FormCollection form)
        {
            string openid = form["openid"];
            string groupid = form["groupid"];
            string str_json = "{ \"openid\": \"" + openid + "\", \"groupid\"=\"" + groupid + "\"}";
            string url = "https://api.weixin.qq.com/cgi-bin/groups/members/update?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);

            return View();
        }

        public ActionResult batchupdate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult batchupdateGroup(FormCollection form)
        {
            string openid1 = form["openid1"];
            string openid2 = form["openid2"];
            string groupid = form["groupid"];
            string str_json = "{ \"openid_list\": [\"" + openid1 + "\", openid2\" ], \"to_groupid\"=\"" + groupid + "\"}";
            string url = "https://api.weixin.qq.com/cgi-bin/groups/members/batchupdate?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);

            return View();
        }

    }
}