using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace wxBase
{
    public static class HttpService
    {
        public static string Post(string uri, string postData)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(uri));
            webRequest.Method = "post";
            webRequest.ContentType = "text/html";
            webRequest.ContentLength = byteArray.Length;
            System.IO.Stream newStream = webRequest.GetRequestStream();
            newStream.Write(byteArray, 0, byteArray.Length);
            newStream.Close();
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            string data = new System.IO.StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8")).ReadToEnd();

            return data;
        }

        public static string Get(string uri)
        {
            string strLine = "", data = "";
            using (WebClient wc = new WebClient())
            {
                try
                {
                    using (Stream stream = wc.OpenRead(uri))
                    {
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            while ((strLine = sr.ReadLine()) != null)
                            {
                                data += strLine;
                            }
                            sr.Close();
                        }
                    }

                }
                catch (System.Exception ex)
                {
                    return ex.Message;
                }
                wc.Dispose();
            }

            return data;
        }


    }


}
