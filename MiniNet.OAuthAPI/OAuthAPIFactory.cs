using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace MiniNet.OAuthAPI
{
    public class OAuthAPIFactory
    {
        public static IOAuthAPI CreateOAuthAPI()
        {
            return new OAuthAPI();
        }

        public static IOAuthAPI CreateOAuthAPI(IWebProxy proxy)
        {
            OAuthAPI api= new OAuthAPI();

            api.Proxy = proxy;

            return api;
        }
    }
}
