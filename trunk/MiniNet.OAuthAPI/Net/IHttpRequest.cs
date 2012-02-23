using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace MiniNet.OAuthAPI.Net
{
    public interface IHttpRequest
    {
        string Get(string url);
        string Post(string url, string postData);
        string Post(string url, string postData,WebHeaderCollection header);
        IWebProxy Proxy { get; set; }
        Encoding Encode { get; set; }
    }
}
