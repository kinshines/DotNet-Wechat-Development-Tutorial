using MY.Utils.Service;
using onlinepayBase.weixin;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ThoughtWorks.QRCode.Codec;
using yxw.SQLServerBase;

namespace WebApplicationPayonlineDemo.Controllers
{
    public class WeixinController : Controller
    {

        // GET: Weixin
        public ActionResult Index(string productCode, string productName, string productDesc, string orderno, int total_fee)
        {
            LogService.LOG_LEVENL = 3;
            NativePay nativePay = new NativePay();
            //生成扫码支付模式二url
            string url = StringService.StringToHexString( nativePay.GetPayUrl(productCode,productName, productDesc,orderno,total_fee));
            
            Response.Redirect(Url.Action("ScanQRCodeImage", "weixin")+"?url="+ HttpUtility.UrlEncode(url)+"&orderno="+orderno);


            return View();
        }

        public ActionResult ScanQRCodeImage( string url)
        {

            return View();

        }

        public ActionResult MakeQRCodeImage(string url)
        {
            string str = StringService.HexStringToString(url);
            //初始化二维码生成工具
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            qrCodeEncoder.QRCodeVersion = 0;
            qrCodeEncoder.QRCodeScale = 4;

            //将字符串生成二维码图片
            Bitmap image = qrCodeEncoder.Encode(str, Encoding.Default);

            //保存为PNG到内存流  
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Png);

            //输出二维码图片
            byte[] bytes =ms.GetBuffer();

            return File(bytes, @"image/jpeg");
        }
        /// <summary>
        /// 支付成功后的回调函数
        /// </summary>
        /// <returns></returns>
        public ActionResult ResultNotify()
        {
            ResultNotify resultNotify = new ResultNotify(Request.InputStream);
            resultNotify.ProcessNotify();

            return View();
        }

        public ActionResult QueryOrderStatus(string orderno)
        {
             string sql = "SELECT paytime FROM orders WHERE orderno='"+orderno+"'";
            SQLBase s = new SQLBase();
            List<List<string>> result = s.ExecuteQuery(sql);
            if (result.Count > 0)
                return (Content(result[0][0]));

            return Content("");
        }

        public ActionResult Orderlist()
        {


            return View("");
        }

    }
}