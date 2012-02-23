using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace MiniNet.OAuthAPI.Net
{
    public class HttpRequestFactory
    {
        public static IHttpRequest CreateHttpRequest()
        {
            return new HttpRequest();
        }

        public static IHttpRequest CreateHttpRequest(IWebProxy proxy)
        {
            HttpRequest request= new HttpRequest();
            request.Proxy = proxy;

            return request;
        }

        public static HttpWebRequest GetDefaultHttpWebRequest(HttpMethod method, Uri uri, string referer, int timeOut)
        {
            HttpWebRequest request = null;

            request = (HttpWebRequest)WebRequest.Create(uri);

            request.Timeout = timeOut;
            request.ReadWriteTimeout = timeOut;
            request.UserAgent = "Mozilla/5.0 (Windows NT 5.1; rv:5.0) Gecko/20100101 Firefox/5.0";
            request.Method = method.ToString();
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
            request.KeepAlive = true;
            request.Headers.Add("Accept-Language", "zh-cn,zh;q=0.5");
            request.Headers.Add("Accept-Charset", "gb2312,utf-8;q=0.7,*;q=0.7");
            request.Referer = referer;
            request.AllowAutoRedirect = true;
            request.Headers.Add("Accept-Encoding", "gzip,deflate");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.CookieContainer = new CookieContainer();
            request.MaximumAutomaticRedirections = 3;
            request.ServicePoint.Expect100Continue = false;

            return request;
        }
    }
}
