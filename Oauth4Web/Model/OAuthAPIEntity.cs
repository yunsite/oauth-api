﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Oauth4Web.Model
{
    public class OAuthAPIEntity
    {
        public string AccessTokenUrl { get; set; }
        public string AuthorizeUrl { get; set; }
        public string RequestTokenUrl { get; set; }
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        public string Token { get; set; }
        public string TokenSecret { get; set; }
        public int Site { get; set; }
    }
}