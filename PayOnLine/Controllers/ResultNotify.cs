using MY.Utils.Service;
using onlinepayBase.weixin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using yxw.SQLServerBase;

namespace PayOnLine.Controllers
{
    /// <summary>
    /// 支付结果通知回调处理类
    /// 负责接收微信支付后台发送的支付结果并对订单有效性进行验证，将验证结果反馈给微信支付后台
    /// </summary>
    public class ResultNotify : Notify
    {
        public ResultNotify(System.IO.Stream _s)
            : base(_s)
        {
        }

        public override void ProcessNotify()
        {
            WxPayData notifyData = GetNotifyData();

            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                LogService.Error(this.GetType().ToString(), "The Pay result is error : " + res.ToXml());
            }

            string transaction_id = notifyData.GetValue("transaction_id") + "";

            LogService.Debug(this.GetType().ToString(), "transaction_id:" + transaction_id);
            //查询订单，判断订单真实性
            if (!QueryOrder(transaction_id))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                LogService.Error(this.GetType().ToString(), "Order query failure : " + res.ToXml());

            }
            //查询订单成功
            else
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                LogService.Info(this.GetType().ToString(), "order query success : " + res.ToXml());
                LogService.Info(this.GetType().ToString(), "回调信息【out_trade_no】: " + notifyData.GetValue("out_trade_no") + "; time_end：" + notifyData.GetValue("time_end") + "; cash_fee：" + notifyData.GetValue("cash_fee"));
                SQLBase s = new SQLBase();

                float fee = 0;
                try
                {
                    fee = float.Parse(notifyData.GetValue("cash_fee").ToString()) / 100.0F;
                }
                catch (Exception)
                {
                    throw;
                }
                string sql = "update order_pay set paytime='" + notifyData.GetValue("time_end") + "',cash_fee=" + fee + ",transaction_id='" + notifyData.GetValue("transaction_id") + "' WHERE orderno='" + notifyData.GetValue("out_trade_no") + "'";
                LogService.Error(this.GetType().ToString(), sql);
                s.ExecuteNonQuery(sql);

                string sql1 = "select htid,regid from order_pay WHERE orderno='" + notifyData.GetValue("out_trade_no") + "'";
                List<List<string>> listord = s.ExecuteQuery(sql1);
                if (listord.Count > 0)
                {
                    //string slq2 = "select baodan_id from ht_bd_map where hetong_id='" + listord[0][0] + "'";
                    //List<List<string>> listht = s.ExecuteQuery(slq2);
                    //if (listht.Count > 0)
                    //{
                    //    string sqlbd = "update baodan set state=5 where id='" + listht[0][0] + "' and regid='" + listord[0][1] + "'";
                    //    s.ExecuteNonQuery(sqlbd);
                    //}
                }
            }
        }

        //查询订单
        private bool QueryOrder(string transaction_id)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}