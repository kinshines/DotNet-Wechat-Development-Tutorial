using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wxBase
{
    public static class wxMessageService
    {
        public static void SendnewsByGroup(int groupid, string mediaid)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token=" + weixinService.Access_token;

            string json = "{ \"filter\":{ \"is_to_all\":false,\"group_id\":" + groupid
                             + "},\"mpnews\":{\"media_id\":\"" + mediaid + "\"},\"msgtype\":\"mpnews\"}";
            string result = HttpService.Post(url, json);
        }

        public static void SendtextByGroup(int groupid, string content)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token=" + weixinService.Access_token;

            string json = "{ \"filter\":{ \"is_to_all\":false,\"group_id\":" + groupid
                             + "},\"text\":{\"content\":\"" + content + "\"},\"msgtype\":\"text\"}";
            string result = HttpService.Post(url, json);
        }

        public static void SendtextByOpenids(List<string> openidlist, string content)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token=" + weixinService.Access_token;
            string json = "{ \"touser\":[";
            for (int i = 0; i < openidlist.Count; i++)
            {
                json+="\""+ openidlist[i]+"\"";
                if (i < openidlist.Count - 1)
                    json += ",";
            }
            json+="],\"text\":{\"content\":\"" + content + "\"},\"msgtype\":\"text\"}";
            string result = HttpService.Post(url, json);
        }

        public static void send_template(string url, string touser, string template_id, string data)
        {

            string posturl = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token=" + weixinService.Access_token;
            string postdata = "{\"touser\":\"" + touser + "\",\"template_id\":\"" + template_id + "\",\"url\":\"" + url + "\",\"data\":" + data + "}";
            string result = HttpService.Post(posturl, postdata);
        }

        public static void Sendkftext(string kfacount, string touser, string content)
        {
            string url = "https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token=" + weixinService.Access_token;

            string json = "{\"touser\":\"" + touser + "\", \"msgtype\":\"text\", \"customservice\":" +
                                "{\"kf_account\": \"" + kfacount + "\" },\"text\":{\"content\":\"" + content +
                                "\"}}";
            string result = HttpService.Post(url, json);
        }

    }
}
