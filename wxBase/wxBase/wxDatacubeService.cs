using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wxBase
{
    public static class wxDatacubeService
    {
        public static string getusersummary(string begin_date, string end_date)
        {
            string json = "{\"begin_date\":\"" + begin_date + "\",\"end_date\":\"" + end_date + "\"}";
            string url = " https://api.weixin.qq.com/datacube/getusersummary?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, json);

            return result;
        }

        public static string getusercumulate(string begin_date, string end_date)
        {
            string json = "{\"begin_date\":\"" + begin_date + "\",\"end_date\":\"" + end_date + "\"}";
            string url = " https://api.weixin.qq.com/datacube/getusercumulate?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, json);

            return result;
        }

        public static string getarticlesummary(string begin_date, string end_date)
        {
            string json = "{ \"begin_date\":\"" + begin_date + "\",\"end_date\":\"" + end_date + "\"}";
            string url = " https://api.weixin.qq.com/datacube/getarticlesummary?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, json);

            return result;
        }
        public static string getarticletotal(string begin_date, string end_date)
        {
            string json = "{\"begin_date\":\"" + begin_date + "\",\"end_date\":\"" + end_date + "\"}";
            string url = "https://api.weixin.qq.com/datacube/getarticletotal?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, json);
            return result;
        }

        public static string getuserread(string begin_date, string end_date)
        {
            string json = "{\"begin_date\":\"" + begin_date + "\",\"end_date\":\"" + end_date + "\"}";
            string url = "https://api.weixin.qq.com/datacube/getuserread?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, json);
            return result;
        }

        public static string getuserreadhour(string begin_date, string end_date)
        {
            string json = "{\"begin_date\":\"" + begin_date + "\",\"end_date\":\"" + end_date + "\"}";
            string url = "https://api.weixin.qq.com/datacube/getuserreadhour?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, json);
            return result;
        }

        public static string getusershare(string begin_date, string end_date)
        {
            string json = "{\"begin_date\":\"" + begin_date + "\",\"end_date\":\"" + end_date + "\"}";
            string url = "https://api.weixin.qq.com/datacube/getusershare?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, json);
            return result;
        }

        public static string getupstreammsg(string begin_date, string end_date)
        {
            string json = "{\"begin_date\":\"" + begin_date + "\",\"end_date\":\"" + end_date + "\"}";
            string url = "https://api.weixin.qq.com/datacube/getupstreammsg?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, json);
            return result;
        }

        public static string getupstreammsgmonth(string begin_date, string end_date)
        {
            string json = "{\"begin_date\": \"" + begin_date + "\",\"end_date\": \"" + end_date + "\"}";
            string url = "https://api.weixin.qq.com/datacube/getupstreammsgmonth?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, json);
            return result;
        }

        public static string getupstreammsgweek(string begin_date, string end_date)
        {
            string json = "{\"begin_date\":\"" + begin_date + "\",\"end_date\":\"" + end_date + "\"}";
            string url = "https://api.weixin.qq.com/datacube/getupstreammsgweek?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, json);
            return result;
        }

        public static string getupstreammsghour(string begin_date, string end_date)
        {
            string json = "{\"begin_date\":\"" + begin_date + "\",\"end_date\":\"" + end_date + "\"}";
            string url = "https://api.weixin.qq.com/datacube/getupstreammsghour?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, json);
            return result;
        }

        public static string getupstreammsgdist(string begin_date, string end_date)
        {
            string json = "{\"begin_date\":\"" + begin_date + "\",\"end_date\":\"" + end_date + "\"}";
            string url = " https://api.weixin.qq.com/datacube/getupstreammsgdist?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, json);
            return result;
        }

        public static string getupstreammsgdistmonth(string begin_date, string end_date)
        {
            string json = "{\"begin_date\":\"" + begin_date + "\",\"end_date\":\"" + end_date + "\"}";
            string url = "https://api.weixin.qq.com/datacube/getupstreammsgdistmonth?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, json);
            return result;
        }

        public static string getupstreammsgdistweek(string begin_date, string end_date)
        {
            string json = "{\"begin_date\":\"" + begin_date + "\",\"end_date\":\"" + end_date + "\"}";
            string url = "https://api.weixin.qq.com/datacube/getupstreammsgdistweek?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, json);
            return result;
        }

        public static string getusersharehour(string begin_date, string end_date)
        {
            string json = "{\"begin_date\":\"" + begin_date + "\",\"end_date\":\"" + end_date + "\"}";
            string url = "https://api.weixin.qq.com/datacube/getusersharehour?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, json);
            return result;
        }


    }
}