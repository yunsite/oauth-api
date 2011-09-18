using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniNet.Net;
using System.Net;

namespace MiniNet.OAuthAPI.Net
{
    /// <summary>
    /// 下载接口的实现。
    /// </summary>
    public class HttpRequest : IHttpRequest
    {
        private NetClient netClient = null;

        public HttpRequest()
        {
            netClient = new NetClient();
        }

        public string Get(string url)
        {
            return netClient.GetPage(new Uri(url), "", 60000);
        }

        public string Post(string url, string postData)
        {
            return netClient.Post(new Uri(url), postData, "", 60000);
        }

        public string Get(HttpWebRequest request)
        {
            return netClient.GetPage(request);
        }

        public string Post(HttpWebRequest request, string postData)
        {
            return netClient.Post(request, postData);
        }

        public string Post(HttpWebRequest request, byte[] bytes)
        {
            return netClient.Post(request, bytes);
        }
    }
}
