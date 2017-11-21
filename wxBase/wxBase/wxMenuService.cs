using System.IO;
using System.Text;

namespace wxBase
{
    public static class wxMenuService
    {

        public static string Create(string menufile)
        {
            string menu_content = File.ReadAllText(menufile, Encoding.GetEncoding("GB2312"));
            string url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token="+weixinService.Access_token;
            string result = HttpService.Post(url, menu_content);

            return result;
        }

        public static string Get()
        {
            string token = weixinService.Access_token;
            string url = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token=" + token;
            string result = HttpService.Get(url);

            return result;
        }

        public static string delete()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token=" + weixinService.Access_token;
            string result = HttpService.Get(url);

            return result;
        }

        public static string GetConfig()
        {
            string url = " https://api.weixin.qq.com/cgi-bin/get_current_selfmenu_info?access_token=" + weixinService.Access_token;
            string result = HttpService.Get(url);

            return result;
        }

        public static string addconditional(string menufile)
        {
            string menu_content = File.ReadAllText(menufile, Encoding.GetEncoding("GB2312"));
            string url = "https://api.weixin.qq.com/cgi-bin/menu/addconditional?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, menu_content);

            return result;
        }

        public static string delconditional(string menuid)
        {
            string json = "{ \"menuid\":\"" + menuid + "\"}";
            string url = "https://api.weixin.qq.com/cgi-bin/menu/delconditional?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, json);

            return result;
        }

        public static string trymatch(string userid)
        {
            string json = "{ \"user_id\":\"" + userid + "\"}";
            string url = "https://api.weixin.qq.com/cgi-bin/menu/trymatch?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, json);

            return result;
        }

    }
}
