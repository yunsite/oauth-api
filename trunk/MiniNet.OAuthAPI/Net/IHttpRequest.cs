using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniNet.OAuthAPI.Net
{
    public interface IHttpRequest
    {
        string Get(string url);
        string Post(string url,string postData);
        string Upload(string url,string postdata,byte[] bytes);
    }
}
