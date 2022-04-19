using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlphaYanTools
{
    internal class AlphaUtil
    {
        /// <summary>
        /// 获取公网IP；需要访问公网sohu.com
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string tempip = "";
            WebRequest request = WebRequest.Create("http://pv.sohu.com/cityjson?ie=utf-8");
            request.Timeout = 10000;
            WebResponse response = request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(resStream, System.Text.Encoding.Default);
            string htmlinfo = sr.ReadToEnd();
            Regex r = new Regex("((25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]\\d|\\d)\\.){3}(25[0-5]|2[0-4]\\d|1\\d\\d|[1-9]\\d|[1-9])", RegexOptions.None);
            Match mc = r.Match(htmlinfo);
            tempip = mc.Groups[0].Value;
            resStream.Close();
            sr.Close();
            return tempip;
        }
        private static Assembly Assembly { get { return Assembly.GetExecutingAssembly(); } }
        /// <summary>
        /// 当前程序版本
        /// </summary>
        public static Version AppVersion { get { return Assembly.GetName().Version; } }
        /// <summary>
        /// 当前程序名称
        /// </summary>
        public static string AppName { get { return Assembly.GetName().Name; } }
    }
}
