using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;
using wxBase.Model.OAuth2;

namespace WebApplicationWeixin.Controllers
{
    public class Auth2Controller : Controller
    {
        // GET: Auth2
        public ActionResult Index()
        {
            string code = Request.QueryString["code"];
            if (!string.IsNullOrEmpty(code))
            {
                LogService.Write("收到用户授权code:" + code);
                UserAccessToken t = OAuth2Service.get_accesstoken_bycode(code);

                LogService.Write("获取到了用户accesstoken:" + t.access_token + ", refresh token:" + t.refresh_token + ", openid=" + t.openid + ", scope:" + t.scope + ", expires_in:" + t.expires_in);
                return View();

            }
            return Content("NO CODE");
        }
    }
}