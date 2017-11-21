using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wxBase.Model.Users;

namespace wxBase
{
    public static class wxUsersService
    {
        public static wxUserGroups get_groups()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/groups/get?access_token=" + weixinService.Access_token;
            wxUserGroups group = JSONHelper.JSONToObject<wxUserGroups> (HttpService.Get(url));

            return group;
        }

        public static wxUsersSummary get_users()
        {
            string url = " https://api.weixin.qq.com/cgi-bin/user/get?access_token=" + weixinService.Access_token;
            wxUsersSummary user = JSONHelper.JSONToObject<wxUsersSummary>(HttpService.Get(url));
            return user;
        }

        public static string getupstreammsgdistweek(string begin_date, string end_date)
        {
            string json = "{\"begin_date\":\"" + begin_date + "\",\"end_date\":\"" + end_date + "\"}";
            string url = "https://api.weixin.qq.com/datacube/getupstreammsgdistweek?access_token=" + weixinService.Access_token;
            string result = HttpService.Post(url, json);
            return result;
        }

    }
}
