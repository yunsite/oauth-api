using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniNet.OAuthAPI
{
    public class OAuthAPIFactory
    {
        public static IOAuthAPI CreateOAuthAPI()
        {
            return new OAuthAPI();
        }
    }
}
