using MY.Utils.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using youxuewangGenerated.Tables;
using yxw.SQLServerBase;

namespace PayOnLine.Controllers
{
    public class LineController : Controller
    {
        //
        // GET: /Line/1209
        public ActionResult Index(int id)
        {
            string regid = "";
            string htid = "";
            string cpcode = "";
            string username = "";
            htid = id.ToString();
            ViewData["htid"] = htid;
            //htid = "9421";
            try
            {
                string regid1 = Session["regid"].ToString();
                //string regid1 = "77426";
                if (string.IsNullOrEmpty(regid1))
                {
                    Response.Redirect("http://www.youxuewang.com.cn");
                }
                else 
                {
                    regid = Session["regid"].ToString();
                }
                username = Session["username"].ToString();
                LogService.Debug("用户id和用户账户：",regid+","+username);
            }
            catch (Exception)
            {
            }

            Ht_version ht = new Ht_version();
            List<Ht_version> list = ht.Select("where id='"+htid+"'");
            if(list.Count>0)
            {
                ViewBag.Version = list;
                if (!string.IsNullOrEmpty(list[0].cpno))
                {
                    cpcode = list[0].cpno;
                }
            }

            #region 处理特殊情况下的支付
            //Ht_version ht1 = new Ht_version();
            //List<Ht_version> list1 = ht1.Select("where id='" + htid + "'");
            //if(list1.Count>0)
            //{
            //    ViewBag.Chengren = list1[0].adultprice;
            //    ViewBag.Student = list1[0].studentprice;
            //}

            
            //ht_bmzf bmzf = new ht_bmzf();
            //List<ht_bmzf> listzf = bmzf.Select("where regid='"+regid+"'");
            //if(listzf.Count>0)
            //{
            //    if (listzf[0].flag == 0)
            //    {
            //        ViewData["flag"] = "(余款)";
            //    }
            //    else 
            //    {
            //        ViewData["flag"] = "(全款)";
            //    }
            //}
            #endregion

            SQLBase sb = new SQLBase();
            string sqlad = "select * from Ht_version v left join ht_bd_map h on v.id=h.hetong_id left join Baodan b on h.baodan_id=b.id where b.regid='" + regid + "' and v.id='" + htid + "' and ischild=1";
            List<List<string>> listbd = sb.ExecuteQuery(sqlad);

            string sqlstu = "select * from Ht_version v left join ht_bd_map h on v.id=h.hetong_id left join Baodan b on h.baodan_id=b.id where b.regid='" + regid + "' and v.id='" + htid + "' and ischild=0";
            List<List<string>> listbd1 = sb.ExecuteQuery(sqlstu);

            int count = listbd.Count;
            int count1 = listbd1.Count;
            //int count = 1;
            //int count1 = 0;
            if (listbd1.Count > 0)
            {
                ViewBag.Baodan1 = count1;
            }
            if(listbd.Count>0)
            {
                ViewBag.Baodan = count;
            }

            //支付信息确认
            SQLBase sqlbase=new SQLBase();
            string sqlquery = "select r.truename,o.name from RegisterPresonal r left join org o on r.orgid=o.id where r.id='"+regid+"'";
            List<List<string>> listreg = sqlbase.ExecuteQuery(sqlquery);
            if(listreg.Count>0)
            {
                ViewData["username"]=listreg[0][0];
                ViewData["classname"]=listreg[0][1];
            }

            return View("");
        }

        public ActionResult Pay(string cpname, string cpdesc, float price, int num, int paytype, float sumprice/*1-支付宝,2-微信*/,int htid)
        {
            string regid = ""; 
            string regid1 =Session["regid"].ToString();
            if (string.IsNullOrEmpty(regid1))
            {
                Response.Redirect("http://www.youxuewang.com.cn");
            }
            else 
            {
                regid = Session["regid"].ToString();
            }
            string cpcode = "";
            order_pay o = new order_pay();
            o.orderno = generate_orderno(1, 1);
            o.product_code = cpcode;
            o.product_name = cpname;
            o.product_desc = cpdesc;
            o.price = price.ToString();
            try
            {
                o.htid = Convert.ToInt32(htid);//改过htid
            }
            catch (Exception)
            {
            }

            try
            {
                o.regid = Convert.ToInt32(regid);
            }
            catch (Exception)
            {
            }

            LogService.Debug("用户id：", regid);
            //o.num = num;
            if (paytype == 1)
                o.paytype = "支付宝";
            else
                o.paytype = "微信支付";
            o.sumittime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            o.Insert();
            Session["htid"] = null;
            float f=1f;
            if (f < 1)
            {
                f = f * 100.0F * num;
            }
            else 
            {
                f = sumprice * 100.0F;
            }
            int total_fee = (int)f;
            LogService.LOG_LEVENL = 3;    
            LogService.Debug(this.GetType().ToString(), "total_fee=" + f);

            switch (paytype)
            {
                case 1:
                    Response.Redirect(Url.Action("Index", "aliPay") + "?productCode=" + cpcode + "&productName=" + cpname + "&productDesc=" + cpdesc + "&orderno=" + o.orderno + "&total_fee=" + total_fee);
                    break;
                case 2:
                    Response.Redirect(Url.Action("Index", "Weixin") + "?productCode=" + cpcode + "&productName=" + cpname + "&productDesc=" + cpdesc + "&orderno=" + o.orderno + "&total_fee=" + total_fee);
                    break;

            }
            return Content("");
        }

        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <param name="group_type">团队类型。0-散客、1-团队</param>
        /// <param name="product_type">产品类型。1-旅游线路，2-租车</ param>
        private string generate_orderno(int group_type, int product_type)
        {
            Random rand = new Random();

            string orderno = group_type.ToString() + product_type.ToString() + DateTime.Now.ToString("yyMMddHHmmssfff") + rand.Next(100).ToString();
            return orderno;
        }

        public ActionResult Orderlist()
        {
            return View("");
        }

        public ActionResult Zhuxiao(int count) 
        {
            if(count==1)
            {
                Session["regid"] = "";
                Session["username"] = "";
                Session["zhiwu"] = "";
                Session["usertype"] = "";
                Response.Redirect("http://www.youxuewang.com.cn");
            }
            return View();
        }
    }
}
