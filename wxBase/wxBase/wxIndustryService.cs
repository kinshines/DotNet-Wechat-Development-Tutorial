using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wxBase.Model.Industry;

namespace wxBase
{
    public static class wxIndustryService
    {
        public static wxIndustryInfo get_industry()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/template/get_industry?access_token=" + weixinService.Access_token;
            wxIndustryInfo info = JSONHelper.JSONToObject<wxIndustryInfo>(HttpService.Post(url,""));
            return info;
        }

    }
}
