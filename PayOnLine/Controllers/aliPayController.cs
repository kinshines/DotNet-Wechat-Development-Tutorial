using MY.Utils.Service;
using onlinepayBase.aliPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using yxw.SQLServerBase;

namespace PayOnLine.Controllers
{
    public class aliPayController : Controller
    {
        //回显修改状态
        [HttpPost, ValidateInput(false)]
        public ActionResult Notify(FormCollection coll)
        {
            LogService.LOG_LEVENL = 3;
            LogService.Error(this.GetType().ToString(), "notify parameter count:" + coll.Count);

            string alipayNotifyURL = "https://www.alipay.com/cooperate/gateway.do?service=notify_verify";
            //string alipayNotifyURL = "http://notify.alipay.com/trade/notify_query.do?";//此路径是在上面链接地址无法起作用时替换使用。
            string partner = aliPayService.Alipay_pid;         //partner合作伙伴id（必须填写）
            string key = aliPayService.Alipay_key; //partner 的对应交易安全校验码（必须填写）
            string _input_charset = "utf-8";//编码类型，完全根据客户自身的项目的编码格式而定，千万不要填错。否则极其容易造成MD5加密错误。
            LogService.Error(this.GetType().ToString(), "notify_id=" + coll["notify_id"]);
            alipayNotifyURL = alipayNotifyURL + "&partner=" + partner + "&notify_id=" + coll["notify_id"];

            //获取支付宝ATN返回结果，true是正确的订单信息，false 是无效的
            string responseTxt = aliPayService.Get_Http(alipayNotifyURL, 120000);

            //*******加密签名程序开始*******
            int i;
            //Load Form variables into NameValueCollection variable.

            // Get names of all forms into a string array.
            String[] requestarr = coll.AllKeys;

            //进行排序；
            string[] Sortedstr = aliPayService.BubbleSort(requestarr);


            //构造待md5摘要字符串 ；
            StringBuilder prestr = new StringBuilder();

            for (i = 0; i < Sortedstr.Length; i++)
            {
                if (Request.Form[Sortedstr[i]] != "" && Sortedstr[i] != "sign" && Sortedstr[i] != "sign_type")
                {
                    if (i == Sortedstr.Length - 1)
                    {
                        prestr.Append(Sortedstr[i] + "=" + Request.Form[Sortedstr[i]]);
                    }
                    else
                    {
                        prestr.Append(Sortedstr[i] + "=" + Request.Form[Sortedstr[i]] + "&");

                    }
                }
            }

            prestr.Append(key);

            string mysign = aliPayService.GetMD5(prestr.ToString(), _input_charset);
            //*******加密签名程序结束*******

            string sign = Request.Form["sign"];
            LogService.Debug(this.GetType().ToString(), "responseTxt=" + responseTxt);

            if (mysign == sign && responseTxt == "true")   //验证支付发过来的消息，签名是否正确，只要成功进如这个判断里，则表示该页面已被支付宝服务器成功调用
            //但判断内出现自身编写的程序相关错误导致通知给支付宝并不是发送success的消息或没有更新客户自身的数据库的情况，请自身程序编写好应对措施，否则查明原因时困难之极
            {
                LogService.LOG_LEVENL = 3;

                if (Request.Form["trade_status"] == "WAIT_BUYER_PAY")//   判断支付状态_等待买家付款（文档中有枚举表可以参考）            
                {
                    //更新自己数据库的订单语句，请自己填写一下
                    string strOrderNO = Request.Form["out_trade_no"];//订单号
                    string strPrice = Request.Form["total_fee"];//金额    如果你申请了商家购物卷功能，在返回信息里面请不要做金额的判断，否则会校验通过不了。

                    LogService.LOG_LEVENL = 3;

                    LogService.Debug(this.GetType().ToString(), "支付宝返回提示等待买家付款。订单号：" + strOrderNO + ";支付金额：" + strPrice);


                }
                else if (Request.Form["trade_status"] == "TRADE_FINISHED" || Request.Form["trade_status"] == "TRADE_SUCCESS")//   判断支付状态_交易成功结束（文档中有枚举表可以参考）   
                {
                    //更新自己数据库的订单语句，请自己填写一下
                    string strOrderNO = Request.Form["out_trade_no"];//订单号
                    string strPrice = Request.Form["total_fee"];//金额   
                    SQLBase s = new SQLBase();

                    string sql = "update order_pay set paytime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',cash_fee='" + strPrice + "', paytype='支付宝', transaction_id='" + Request.Form["trade_no"] + "'  WHERE orderno='" + strOrderNO + "'";
                    LogService.LOG_LEVENL = 3;
                    LogService.Error(this.GetType().ToString(), sql);
                    s.ExecuteNonQuery(sql);

                    string sql1 = "select htid,regid from order_pay WHERE orderno='" + strOrderNO + "'";
                    List<List<string>> listord = s.ExecuteQuery(sql1);
                    if(listord.Count>0)
                    {
                        //string slq2 = "select baodan_id from ht_bd_map where hetong_id='" + listord[0][0] + "'";
                        //List<List<string>> listht = s.ExecuteQuery(slq2);
                        //if(listht.Count>0)
                        //{
                        //    string sqlbd = "update baodan set state=5 where id='"+listht[0][0]+"' and regid='"+listord[0][1]+"'";
                        //    s.ExecuteNonQuery(sqlbd);
                        //}
                    }
                }
                else
                {
                    //更新自己数据库的订单语句，请自己填写一下

                }

                Response.Write("success");     //返回给支付宝消息，成功，请不要改写这个success
                //success与fail及其他字符的区别在于，支付宝的服务器若遇到success时，则不再发送请求通知（即不再调用该页面，让该页面再次运行起来），
                //若不是success，则支付宝默认没有收到成功的信息，则会反复不停地调用该页面直到失效，有效调用时间是24小时以内。

                //最好写TXT文件，以记录下是否异步返回记录。

                ////写文本，纪录支付宝返回消息，比对md5计算结果（如网站不支持写txt文件，可改成写数据库）
                string TOEXCELLR = "支付宝回调MD5结果:mysign=" + mysign + ",sign=" + sign + ",responseTxt=" + responseTxt;
                LogService.LOG_LEVENL = 3;
                LogService.Debug(this.GetType().ToString(), TOEXCELLR);
            }
            else
            {
                Response.Write("fail");

                //最好写TXT文件，以记录下是否异步返回记录。

                //写文本，纪录支付宝返回消息，比对md5计算结果（如网站不支持写txt文件，可改成写数据库）
                LogService.LOG_LEVENL = 3;
                string TOEXCELLR = "支付宝回调MD5结果:mysign=" + mysign + ",sign=" + sign + ",responseTxt=" + responseTxt;
                LogService.Debug(this.GetType().ToString(), TOEXCELLR);
            }
            return View();

        }
        //
        // GET: /aliPay/

        public ActionResult Index(string productCode, string productName, string productDesc, string orderno, int total_fee)
        {
            Encoding utf8 = Encoding.UTF8;
            Response.ContentEncoding = utf8;
            Request.ContentEncoding = utf8;
            LogService.LOG_LEVENL = 3;

            //string uname = Session.GetDataFromSession<string>("username");
            //LogService.Debug(this.GetType().ToString(), uname);

            //业务参数赋值； 
            string gateway = "https://www.alipay.com/cooperate/gateway.do?"; //支付接口 
            string service = "create_direct_pay_by_user"; //服务名称，这个是识别是何接口实现何功能的标识，请勿修改   string _input_charset = "utf-8"; //编码类型，完全根据客户自身的项目的编码格式而定，千万不要填错。否则极其容易造成MD5加密错误。 
            string key = aliPayService.Alipay_key; //安全校验码，与partner是一组，获取方式是：用签约时支付宝帐号登陆支付宝网站www.alipay.com，在商家服务我的商家里即可查到。 

            string show_url = "http://www.alipay.com/"; //展示地址，即在支付页面时，商品名称旁边的“详情”的链接地址。 

            string out_trade_no = orderno; //客户自己的订单号，订单号必须在自身订单系统中保持唯一性 
            string subject = productName; //商品名称，也可称为订单名称，该接口并不是单一的只能买一样东西，可把一次支付当作一次下订单 
            //string body = productDesc; //商品描述，即备注 
            string _input_charset = "utf-8"; //编码类型，完全根据客户自身的项目的编码格式而定，千万不要填错。否则极其容易造成MD5加密错误。 
            string sign_type = "MD5"; //加密类型,签名方式“不用改” 
            string partner = aliPayService.Alipay_pid; //商户ID，合作身份者ID，合作伙伴ID 
            string seller_email = aliPayService.Alipay_seller_email; //商家签约时的支付宝帐号，即收款的支付宝帐号 
            string fee = "";
            if (total_fee <= 10)
            {
                fee = (total_fee * 1.0F / 100.0).ToString();
            }
            else
            {
                fee = (total_fee * 1.0F/100.0f).ToString(); //商品价格，也可称为订单的总金额 
            }
            
            //服务器通知url（Alipay_Notify.aspx文件所在路经），必须是完整的路径地址 
            string notify_url = "http://www.youxuewang.com.cn/payonline/aliPay/Notify";
            //服务器返回url（Alipay_Return.aspx文件所在路经），必须是完整的路径地址 
            string return_url = "http://www.youxuewang.com.cn/teacher/bm/index";

            //构造数组； 
            //以下数组即是参与加密的参数，若参数的值不允许为空，若该参数为空，则不要成为该数组的元素 
            string[] para ={
                "service="+service,
                "partner=" + aliPayService.Alipay_pid,
                "seller_email=" + aliPayService.Alipay_seller_email,
                "out_trade_no=" + out_trade_no,
                "subject=" + subject,
                //"body=" + body,
                "total_fee=" + fee,
                "show_url=" + show_url,
                "payment_type=1",
                "notify_url=" + notify_url,
                "return_url=" + return_url,
                "_input_charset="+_input_charset
            };

            //支付URL生成 
            string aliay_url = aliPayService.CreatUrl(
                // gateway,//GET方式传递参数时请去掉注释 
            para,
            _input_charset,
            sign_type,
            key
            );

            // 以下是GET方式传递参数
            // Response.Redirect(aliay_url);

            //以下是POST方式传递参数 
            Response.Write("<form name='alipaysubmit' method='post' action='https://www.alipay.com/cooperate/gateway.do?_input_charset=utf-8'>");
            Response.Write("<input type='hidden' name='service' value=" + service + ">");
            Response.Write("<input type='hidden' name='partner' value=" + partner + ">");
            Response.Write("<input type='hidden' name='seller_email' value=" + seller_email + ">");
            Response.Write("<input type='hidden' name='out_trade_no' value=" + out_trade_no + ">");
            Response.Write("<input type='hidden' name='subject' value=" + subject + ">");
            //Response.Write("<input type='text' name='body' value=" + body + ">");
            Response.Write("<input type='hidden' name='total_fee' value=" + fee + ">");
            Response.Write("<input type='hidden' name='show_url' value=" + show_url + ">");
            Response.Write("<input type='hidden' name='return_url' value=" + return_url + ">");
            Response.Write("<input type='hidden' name='notify_url' value=" + notify_url + ">");
            Response.Write("<input type='hidden' name='payment_type' value=1>");
            Response.Write("<input type='hidden' name='sign' value=" + aliay_url + ">");
            Response.Write("<input type='hidden' name='sign_type' value=" + sign_type + ">");
            Response.Write("</form>");
            Response.Write("<script>");
            Response.Write("document.alipaysubmit.submit()");
            Response.Write("</script>");

            return Content("");
        }

        
    }
}
