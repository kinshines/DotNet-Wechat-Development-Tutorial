using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wxBase.Model.weidian
{
    public class ProductInfo
    {
        public List<string> category_id;
        public List<ProductProperty> property;
        public string name;
        public List<SkuInfo> sku_info;
        public string main_img;
        public List<string> img;

    }
}
