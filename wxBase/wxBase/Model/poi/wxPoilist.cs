using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wxBase.Model.poi
{
    public class wxPoilist
    {
        public int errcode;
        public string errmsg;
        public List<wxPoiBaseInfo> business_list;
        public int total_count;
    }
}
