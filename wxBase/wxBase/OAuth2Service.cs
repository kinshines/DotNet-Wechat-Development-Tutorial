using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wxBase.Model.OAuth2;

namespace wxBase
{
    public static class OAuth2Service
    {
        public static UserAccessToken get_accesstoken_bycode(string code)
        {
            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + weixinService.appid + "&secret=" + weixinService.appsecret + "&code=" + code + "&grant_type=authorization_code";
            string result = HttpService.Get(url);
            UserAccessToken t = JSONHelper.JSONToObject<UserAccessToken>(result);

            return t;
        }
    }
}
