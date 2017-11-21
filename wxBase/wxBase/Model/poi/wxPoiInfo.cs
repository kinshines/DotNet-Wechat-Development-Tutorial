using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wxBase.Model.poi
{
    public class wxPoiInfo
    {
        public string sid;
        public string business_name;
        public string branch_name;
        public string address;
        public string telephone;
        public List<string> categories;
        public string city;
        public string province;
        public int offset_type;
        public double longitude;
        public double latitude;
        public List<wxPoiPhotourl> photo_list;
        public string introduction;
        public string recommend;
        public string special;
        public string open_time;
        public int avg_price;
        public string poi_id;
        public int available_state;
        public string district;
        public int update_status;
    }
}
