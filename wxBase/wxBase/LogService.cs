using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wxBase
{
    public static class LogService
    {
        /// <summary>
        /// 保存日志文件的文件夹
        /// </summary>
        static string logDir = "log";
        /// <summary>
        /// 日志文件
        /// </summary>
        static string logFile = "";

        /// <summary>
        /// 追加一条信息
        /// </summary>
        /// <param name="text"></param>
        public static void Write(string text)
        {
            #region 如果日志文件夹不存在，则创建之
            string dir = AppDomain.CurrentDomain.BaseDirectory + "//" + logDir;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            #endregion
            // 日志文件名包含日期
            logFile = dir + "//" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            // 记录日志
            File.AppendAllText(logFile, "\r\n"+DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]：  ") + text+"\r\n");
        }

    }
}
