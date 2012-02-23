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
        private IHttpForm http = null;

        public HttpRequest()
        {
            http = HttpFormFactory.DefaultHttpForm();
        }

        public string Get(string url)
        {
            HttpFormGetRequest request = new HttpFormGetRequest();
            request.Url = url;

            request.Proxy = Proxy;
            request.Encoding = Encode;

            HttpFormResponse response= http.Get(request);

            if (response == null)
            {
                return null;
            }

            return response.Response;
        }

        public string Post(string url, string postData)
        {
            HttpFormPostRawRequest request = new HttpFormPostRawRequest();

            request.Url = url;
            request.Data = postData;

            request.Proxy = Proxy;
            request.Encoding = Encode;

            HttpFormResponse response = http.Post(request);

            if (response == null)
            {
                return null;
            }

            return response.Response;
        }

        public string Post(string url, string postData,WebHeaderCollection header)
        {
            HttpFormPostRawRequest request = new HttpFormPostRawRequest();

            request.Url = url;
            request.Data = postData;
            request.Headers = header;

            request.Proxy = Proxy;
            request.Encoding = Encode;

            HttpFormResponse response = http.Post(request);

            if (response == null)
            {
                return null;
            }

            return response.Response;
        }

        #region IHttpRequest Members

        public IWebProxy Proxy { get; set; }

        #endregion

        #region IHttpRequest Members


        public Encoding Encode { get; set; }

        #endregion
    }
}
