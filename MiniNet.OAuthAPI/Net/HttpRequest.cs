using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniNet.Net;

namespace MiniNet.OAuthAPI.Net
{
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

        public string Upload(string url, string postdata, byte[] bytes)
        {
            throw new NotImplementedException();
        }
    }
}
