using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;
using wxBase.Model.Industry;
using wxBase.Model.Users;

namespace WebApplicationWeixin.Controllers
{
    public class UsersController : Controller
    {

        // GET: Users
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult addgroup(FormCollection form)
        {
            string groupname = form["groupname"];
            string str_json = "{ \"group\":{ \"name\":\"" + groupname + "\"}";
            string url = "https://api.weixin.qq.com/cgi-bin/groups/create?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);

            return View();
        }

        public ActionResult get_groups()
        {
            wxUserGroups mgroup = wxUsersService.get_groups();
            Response.Write("您的公众号共有" + mgroup.groups.Count + "个用户组。具体如下：<br/>");
            for (int i = 0; i < mgroup.groups.Count; i++)
            {
                Response.Write("<br/>id:" + mgroup.groups[i].id);
                Response.Write("<br/>用户组名:" + mgroup.groups[i].name);
                Response.Write("<br/>包含用户数:" + mgroup.groups[i].count);

            }
            return View();
        }

        [HttpPost]
        public ActionResult modifygroup(FormCollection form)
        {
            string groupid = form["groupid"];
            string groupname = form["groupname"];
            string str_json = "{ \"group\":{ \"id\":" + groupid + ",\"name\":\"" + groupname + "\"}";
            string url = " https://api.weixin.qq.com/cgi-bin/groups/update?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);
            Response.Write(result);
            return View();
        }

        [HttpPost]
        public ActionResult deletegroup(FormCollection form)
        {
            string groupid = form["groupid"];
            string str_json = "{ \"group\":{ \"id\":" + groupid + "}";
            string url = " https://api.weixin.qq.com/cgi-bin/groups/delete?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);
            Response.Write(result);
            return View();
        }

        public ActionResult get_users()
        {
            wxUsersSummary user = wxUsersService.get_users();
            return View(user);
        }

        [HttpPost]
        public ActionResult SetRemark(FormCollection form)
        {
            string openid = form["openid"];
            string remark = form["remark"];
            string str_json = "{ \"openid\": \"" + openid + "\", remark\":\"" + remark + "\"}";
            string url = " https://api.weixin.qq.com/cgi-bin/user/info/updateremark?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);

            return View();
        }

        [HttpPost]
        public ActionResult GetGroupId(FormCollection form)
        {
            string openid = form["openid"];
            string str_json = "{ \"openid\": \"" + openid + "\"}";
            string url = "https://api.weixin.qq.com/cgi-bin/groups/getid?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);

            return View();
        }


        public ActionResult get_industry()
        {
            wxIndustryInfo mindustry = wxIndustryService.get_industry();
            return View(mindustry);
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