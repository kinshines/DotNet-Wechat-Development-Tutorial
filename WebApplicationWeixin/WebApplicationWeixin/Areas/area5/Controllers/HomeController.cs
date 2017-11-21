using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;
using wxBase.Model.Industry;
using wxBase.Model.Message;

namespace WebApplicationWeixin.Areas.area5.Controllers
{
    public class HomeController : Controller
    {
        // GET: area5/Home
        public ActionResult Index()
        {
            return View();
        }

        // GET: area5/Home
        public ActionResult sendall()
        {
            wxMessageService.SendtextByGroup(0, "测试消息");
            return View();
        }

        public ActionResult SendtextByOpenids()
        {
            List<string> openidslist = new List<string>();
            openidslist.Add("oD15RwLBbA4mr_-T9-2R7zE1mQSI");
            openidslist.Add("oD15RwLShriAALNLcis_mvWLm7BU");

            wxMessageService.SendtextByOpenids(openidslist, "测试消息");
            return View();
        }

        public ActionResult get_industry()
        {
            wxIndustryInfo minfo = wxIndustryService.get_industry();
            return View(minfo);
        }

        public ActionResult set_industry()
        {
            return View();
        }

        public ActionResult api_add_template()
        {
            return View();
        }

        public ActionResult get_all_private_template()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/template/get_all_private_template?access_token=" + weixinService.Access_token;
            wxModelTemplateList tlist = JSONHelper.JSONToObject<wxModelTemplateList>(HttpService.Get(url));

            return View();
        }

        public ActionResult send_template()
        {
            string touser = "o0e8Yw9IgsBt_yUh_uxv3uQilgS0";
            string url = "http://weixin.qq.com/download";
            string data = "{\"first\":{\"value\":\"恭喜你购买成功！\",\"color\":\"#173177\"},\"hotelName\":{\"value\":\"香格里拉大酒店\",\"color\":\"#ff0000\"},\"voucher number\":{\"value\":\"001123456\",\"color\":\"#ff0000\"},\"remark\":{\"value\":\"点击查看更多酒店详情；部分酒店已开通网上预约及退款服务。\",\"color\":\"#173177\"}}";
            string template_id = "Klo3t4clWlxMODm_yyfDAHc2wjONfcYBn2H3v51dMUE";

            wxMessageService.send_template(url, touser, template_id, data);
            return View();
        }

    }
}