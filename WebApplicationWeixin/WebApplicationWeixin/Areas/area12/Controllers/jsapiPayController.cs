using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;
using wxBase.Model;
using wxBase.Model.Pay;

namespace WebApplicationWeixin.Areas.area12.Controllers
{
    public class jsapiPayController : Controller
    {
        string wxPay_json = "";
        // GET: area12/jsapiPay
        public ActionResult Index()
        {
            string strBillNo = wxPayService.getTimestamp(); // 订单号
            string strWeixin_OpenID = "";  // 当前用户的openid
            string strCode = Request.QueryString["code"] == null ? "" : Request.QueryString["code"]; // 接收微信认证服务器发送来的code

            LogService.Write("code:" + strCode);

            if (string.IsNullOrEmpty(strCode)) //如果接收到code，则说明是OAuth2服务器回调
            {
                //进行OAuth2认证，获取code
                string _OAuth_Url = wxPayService.OAuth2_GetUrl_Pay(Request.Url.ToString());

                LogService.Write("_OAuth_Url:" + _OAuth_Url);
                Response.Redirect(_OAuth_Url);
                return Content("");
            }
            else
            {
                //根据返回的code，获得
                wxPayReturnValue retValue = wxPayService.OAuth2_Access_Token(strCode);

                if (retValue.HasError)
                {
                    Response.Write("获取code失败：" + retValue.Message);
                    return Content("");
                }
                LogService.Write("retValue.Message:" + retValue.Message);

                strWeixin_OpenID = retValue.GetStringValue("Weixin_OpenID");
                string strWeixin_Token = retValue.GetStringValue("Weixin_Token");
                LogService.Write("strWeixin_OpenID:" + strWeixin_OpenID);

                if (string.IsNullOrEmpty(strWeixin_OpenID))
                {
                    Response.Write("openid出错");
                    return Content("");
                }
            }
            if (string.IsNullOrEmpty(strWeixin_OpenID))
                return Content("");

            wxpayPackage pp = wxPayService.MakePayPackage(strWeixin_OpenID, strBillNo, 0.01M, "测试");
            //  LogService.Write("_Pay_json1:" + _Pay_json);
            ViewBag.appid = pp.appId;
            ViewBag.nonceStr = pp.nonceStr;
            ViewBag.package = pp.package;
            ViewBag.paySign = pp.paySign;
            ViewBag.signType = pp.signType;
            ViewBag.timeStamp = pp.timeStamp;

            return View();
        }
    }
}