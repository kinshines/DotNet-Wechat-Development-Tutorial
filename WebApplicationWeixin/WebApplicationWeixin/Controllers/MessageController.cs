using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using wxBase;
using wxBase.Model.Message;

namespace WebApplicationWeixin.Controllers
{
    public class MessageController : Controller
    {
        // GET: Message
        public ActionResult Index()
        {

            return View();
        }

        // GET: Message
        public ActionResult sendall()
        {
            wxMessageService.SendtextByGroup(0, "测试消息");

            //wxMessageService.SendnewsByGroup(0, "6plPqFmrpkCBAH2d04r3FIckbnO89v9NzvU48qMwAexx-JdKdpjh5V8_12AOJ-AP");
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

        //[HttpPost]
        public ActionResult GetTemplateID(FormCollection form)
        {
            string template_id_short = form["template_id_short"];
            string str_json = "{ \"template_id_short\": \"" + template_id_short + "\"}";
            string url = "https://api.weixin.qq.com/cgi-bin/template/api_add_template?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, str_json);

            return View();
        }

        public ActionResult get_all_private_template()
        {
            string url = " https://api.weixin.qq.com/cgi-bin/template/get_all_private_template?access_token=" + weixinService.Access_token;
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