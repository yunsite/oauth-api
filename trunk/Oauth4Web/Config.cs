using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace Oauth4Web
{
    public class Config
    {
        public static string CallbackUrl = ConfigurationManager.AppSettings["callbackUrl"];

        public static string TwitterConnectionString = ConfigurationManager.ConnectionStrings["TwitterConnectionString"].ConnectionString;
    }
}
