using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wxBase.Model.Menu
{
    public class wxModelMenuNewsInfo
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string title;
        /// <summary>
        /// 作者
        /// </summary>
        public string author;
        /// <summary>
        /// 摘要
        /// </summary>
        public string digest;
        /// <summary>
        /// 指定图文消息是否显示封面，0为不显示，1为显示
        /// </summary>
        public int show_cover;
        /// <summary>
        /// 图文消息的封面图片的URL
        /// </summary>
        public string cover_url;
        /// <summary>
        /// 图文消息的正文URL
        /// </summary>
        public string content_url;
        /// <summary>
        /// 图文消息的原文的URL，若置空则没有查看原文入口
        /// </summary>
        public string source_url;
    }
}
