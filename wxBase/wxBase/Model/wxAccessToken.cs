using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wxBase.Model
{
    //http://www.cnblogs.com/freeliver54/p/3725979.html

    public class wxAccessToken
    {
        /// <summary>
        /// 许可令牌
        /// </summary>
        public string access_token;
        /// <summary>
        /// 有效期时长（秒）
        /// </summary>
       public int expires_in; 

    }
}
