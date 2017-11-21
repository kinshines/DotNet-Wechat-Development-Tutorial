using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wxBase.Model.Menu
{
    public class wxModelMenuButton
    {
        /// <summary>
        /// 菜单按钮类型
        /// </summary>
        public string type;
        /// <summary>
        /// 菜单按钮名字
        /// </summary>
        public string name;
        /// <summary>
        /// 菜单按钮的关键字
        /// </summary>
        public string key;
        /// <summary>
        /// 当按钮的类型为view时，用于定义单击菜单按钮的跳转地址
        /// </summary>
        public string url;
        /// <summary>
        /// 对于官网上设置的自定义菜单，value字段用于保存菜单文本；对于Img和voice类型的菜单，value字段用于保存素材的mediaID；对于Video类型的菜单，value字段用于保存视频的下载链接
        /// </summary>
        public string value;
        public wxModelMenuSubButtons sub_button;
    }
}
