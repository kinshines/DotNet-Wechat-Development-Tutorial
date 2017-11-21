using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wxBase.Model.Menu
{
    public class wxModelMenuConfig
    {
        /// <summary>
        /// 指定是否开启菜单，0代表未开启，1代表开启
        /// </summary>
        public int is_menu_open;
        /// <summary>
        /// 菜单信息类 
        /// </summary>
        public wxModelMenuInfo selfmenu_info;

    }
}
