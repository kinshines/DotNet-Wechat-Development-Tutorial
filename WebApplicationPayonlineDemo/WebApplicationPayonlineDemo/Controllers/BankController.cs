using MerchantLibrary;
using MY.Utils.Service;
using onlinepayBase.PinganBank.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using youxuewangGenerated.Tables;

namespace WebApplicationPayonlineDemo.Controllers
{
    public class BankController : Controller
    {
        // GET: Bank
        public ActionResult Index()
        {
            Random rn = new Random();
            string ori = "";
            ori += "<kColl id=\"input\" append=\"false\">";
            ori += "<field id=\"masterId\" value=\"2000311146\"/>";
            ori += "<field id=\"orderId\" value=\"2000311146" + DateTime.Now.ToString("yyyyMMdd") + "" + GetRandNo() + "\"/>";
            ori += "<field id=\"currency\" value=\"RMB\"/>";
            ori += "<field id=\"amount\" value=\"0.05\"/>";
            ori += "<field id=\"paydate\" value=\"" + DateTime.Now.ToString("yyyyMMddHHmmss") + "\"/>";
            ori += "<field id=\"remark\" value=\"sdb\"/>";
            ori += "<field id=\"objectName\" value=\"KHpaygate\"/>";
            ori += "<field id=\"validtime\" value=\"0\"/>";
            ori += "</kColl>";
            #region gb2312编码

            string encodebase64 = Convert.ToBase64String(ASCIIEncoding.Default.GetBytes(ori));//base64转码
             string oristr = Server.UrlEncode(encodebase64);//url转码
            string jmq = MerchantLibrary.PaymentInterfaceUtil.merchantSignData_ABA(ori);
            string jmbase = Convert.ToBase64String(ASCIIEncoding.Default.GetBytes(jmq));//base64转码
            // 签名
            string signstr = Server.UrlEncode(jmbase);//url转码

            // post的数据
            string post_str = "orig=" + oristr + "&sign=" + signstr;
            string url = "https://testebank.sdb.com.cn/corporbank/khPayment.do";
            Post(url, oristr, signstr);
                                               //解码
                                               //string jmurlcode = Server.UrlDecode(txtorig.Value);
                                               //string jmbasecode = ASCIIEncoding.Default.GetString(Convert.FromBase64String(jmurlcode));
                                               //Response.Write(jmbasecode);
            #endregion
            return View();
        }

        public ActionResult Paysuccess()
        {
            string orig = Request["orig"];
            string sign = Request["sign"];


            string jmurlcode = Server.UrlDecode(orig);
            string jmbasecode = ASCIIEncoding.Default.GetString(Convert.FromBase64String(jmurlcode));
            string jmurlcode1 = Server.UrlDecode(sign);
            string jmbasecode1 = ASCIIEncoding.Default.GetString(Convert.FromBase64String(jmurlcode1));
            //Response.Write(jmbasecode);
            string jmq = MerchantLibrary.PaymentInterfaceUtil.merchantSignData_ABA(jmbasecode);
            string jmbase = Convert.ToBase64String(ASCIIEncoding.Default.GetBytes(jmq));//base64转码
            // 签名
            string signstr = Server.UrlEncode(jmbase);//url转码
            LogService.WriteLog("Pay Bank", this.GetType().ToString(), "orig:" + orig + ", sign:" + sign + ", jmbasecode=" + jmbasecode + ", jmbasecode1=" + jmbasecode1 + ",mysign_str:" + signstr);
            tcvMessagePay m = new tcvMessagePay();
            //string xml = "<kColl id=\"output\" append=\"false\"><field id=\"status\" value=\"01\"/><field id=\"date\" value=\"20160826082059\"/><field id=\"charge\" value=\"0.00\"/><field id=\"masterId\" value=\"2000311146\"/><field id=\"orderId\" value=\"20003111462016082608280649\"/><field id=\"currency\" value=\"RMB\"/><field id=\"amount\" value=\"0.05\"/><field id=\"paydate\" value=\"20160826082806\"/><field id=\"remark\" value=\"sdb\"/><field id=\"objectName\" value=\"KHpaygate\"/><field id=\"validtime\" value=\"0\"/></kColl>";
            m.FromXml(jmbasecode);
            LogService.WriteLog("Pay Bank", this.GetType().ToString(), "masterId:" + m.masterId + ", orderId:" + m.orderId + ", paydate=" + m.paydate + ", status=" + m.status + ",date:" + m.date + ", charge=" + m.charge + ",amount:" + m.amount + ", currency=" + m.currency + ",objectName:" + m.objectName + ",validtime:" + m.validtime + ",remark:" + m.remark);

            pabank_notify_pay p = new pabank_notify_pay();
            m.save(p);
            return View();

            //if (PaymentInterfaceUtil.merchantVerifyPayGate_ABA(orig, sign))
            //    LogService.WriteLog("Pay Bank", this.GetType().ToString(), "OK!!!");
            //else
            //    LogService.WriteLog("Pay Bank", this.GetType().ToString(), "Fail......");

        }

        /// <summary>
        /// 获取流水号
        /// </summary>
        /// <returns></returns>
        private string GetRandNo()
        {
            string result = "";
            result = (DateTime.Now.ToString("HHmmss")) + new Random().Next(10, 99).ToString();
            return result;
        }

        private void Post(string url, string orig, string sign)
        {
            //以下是POST方式传递参数 
            Response.Write("<form name='bankpaysubmit' method='post' action='"+url+"'>");
            Response.Write("<input type='hidden' name='orig' value='" + orig + "'>");
            Response.Write("<input type='hidden ' name='sign' value='" + sign + "'>");
            Response.Write("<input type='hidden' name='returnurl' value='http://www.youxuewang.com.cn/onlinepay/Bank/Paysuccess'>");
            Response.Write("<input type='hidden' name='NOTIFYURL' value='http://www.youxuewang.com.cn'>");
            Response.Write("</form>");
            Response.Write("<script>");
            Response.Write("document.bankpaysubmit.submit()");
            Response.Write("</script>");
        }
    }
}