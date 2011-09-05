using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniNet.OAuthAPI.Net
{
    public class HttpRequestFactory
    {
        public static IHttpRequest CreateHttpRequest()
        {
            return new HttpRequest();
        }
    }
}
