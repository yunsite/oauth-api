using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniNet.OAuthAPI.Net;
using System.Web;

namespace MiniNet.OAuthAPI
{ 
    public class OAuthAPI : IOAuthAPI
    {
        private const string oauthTokenKey = "oauth_token";

        private const string oauthTokenSecretKey = "oauth_token_secret";

        private OAuthBase oauthBase = null;

        public OAuthAPI()
        {
            oauthBase = new OAuthBase();
        }

        #region 属性
        public string RequestTokenUrl
        {
            get
            {
                return oauthBase.RequestTokenUrl;
            }
            set
            {
                oauthBase.RequestTokenUrl = value;
            }
        }

        public string AuthorizeUrl
        {
            get
            {
                return oauthBase.AuthorizeUrl;
            }
            set
            {
                oauthBase.AuthorizeUrl = value;
            }
        }

        public string AccessTokenUrl
        {
            get
            {
                return oauthBase.AccessTokenUrl;
            }
            set
            {
                oauthBase.AccessTokenUrl = value;
            }
        }

        public string Token
        {
            get
            {
                return oauthBase.Token;
            }
            set
            {
                oauthBase.Token = value;
            }
        }

        public string TokenSecret
        {
            get
            {
                return oauthBase.TokenSecret;
            }
            set
            {
                oauthBase.TokenSecret = value;
            }
        }

        public string AppKey
        {
            get
            {
                return oauthBase.AppKey;
            }
            set
            {
                oauthBase.AppKey = value;
            }
        }

        public string AppSecret
        {
            get
            {
                return oauthBase.AppSecret;
            }
            set
            {
                oauthBase.AppSecret = value;
            }
        }
        #endregion

        public bool GetRequestToken(string appKey, string appKeySecret, string callBackUrl)
        {
            oauthBase.AppKey = appKey;
            oauthBase.AppSecret = appKeySecret;
            oauthBase.CallbackUrl = callBackUrl;

            var content = Call(HttpMethod.GET, this.RequestTokenUrl, "");

            if (string.IsNullOrEmpty(content))
            {
                return false;
            }

            var queryString = HttpUtility.ParseQueryString(content);

            oauthBase.Token = queryString[oauthTokenKey];
            oauthBase.TokenSecret = queryString[oauthTokenSecretKey];

            if (string.IsNullOrEmpty(oauthBase.Token) || string.IsNullOrEmpty(oauthBase.TokenSecret))
            {
                return false;
            }

            return true;
        }

        public string getAuthorize(string callBackUrl)
        {
            oauthBase.CallbackUrl = callBackUrl;

            var url = this.AuthorizeUrl;
            string outUrl;
            string querystring;

            var nonce = oauthBase.GenerateNonce();
            var timestamp = oauthBase.GenerateTimeStamp();
            var signature = oauthBase.GenerateSignature(
                    new Uri(url),
                    oauthBase.AppKey,
                    oauthBase.AppSecret,
                    oauthBase.Token,
                    oauthBase.TokenSecret,
                    "GET",
                    timestamp,
                    nonce,
                    out outUrl,
                    out querystring
                    );

            querystring += "&oauth_signature=" + HttpUtility.UrlEncode(signature);

            if (querystring.Length > 0)
            {
                outUrl += "?" + querystring;
            }

            return outUrl;
        }

        public bool getAccessToken(string verifier)
        {
            oauthBase.Verifier = verifier;

            var content = Call(HttpMethod.GET, this.AccessTokenUrl, "");

            if (string.IsNullOrEmpty(content))
            {
                return false;
            }

            var queryString = HttpUtility.ParseQueryString(content);

            oauthBase.Token = queryString[oauthTokenKey];
            oauthBase.TokenSecret = queryString[oauthTokenSecretKey];

            if (string.IsNullOrEmpty(oauthBase.Token) || string.IsNullOrEmpty(oauthBase.TokenSecret))
            {
                return false;
            }

            return true;
        }

        public string Call(HttpMethod method, string api, string parameter)
        {
            if (method == HttpMethod.GET)
            {
                if (!string.IsNullOrEmpty(parameter))
                {
                    api += "?source=" + oauthBase.AppKey + "&" + parameter;
                }
            }
            else if (method == HttpMethod.POST)
            {
                if (api.IndexOf("?") > 0)
                {
                    api += "&";
                }
                else
                {
                    api += "?";
                }

                var postdata = ParsePostData(parameter);

                api += postdata;
            }

            var url = api;
            string outUrl;
            string querystring;

            var nonce = oauthBase.GenerateNonce();
            var timestamp = oauthBase.GenerateTimeStamp();
            var signature = oauthBase.GenerateSignature(
                    new Uri(url),
                    oauthBase.AppKey,
                    oauthBase.AppSecret,
                    oauthBase.Token,
                    oauthBase.TokenSecret,
                    method.ToString(),
                    timestamp,
                    nonce,
                    out outUrl,
                    out querystring
                    );

            querystring += "&oauth_signature=" + HttpUtility.UrlEncode(signature);

            if (method == HttpMethod.GET && querystring.Length > 0)
            {
                outUrl += "?" + querystring;
            }

            var request = HttpRequestFactory.CreateHttpRequest();

            var content = "";

            if (method == HttpMethod.GET)
            {
                content = request.Get(outUrl);
            }
            else if (method == HttpMethod.POST)
            {
                content = request.Post(outUrl, querystring);
            }

            return content;
        }

        private string ParsePostData(string postData)
        {
            var appendedPostData = postData + "&source=" + oauthBase.AppKey;
            var queryString = HttpUtility.ParseQueryString(appendedPostData);
            var resultUrl = "";
            foreach (var key in queryString.AllKeys)
            {
                if (resultUrl.Length > 0)
                {
                    resultUrl += "&";
                }
                queryString[key] = (queryString[key]);
                resultUrl += (key + "=" + queryString[key]);
            }
            return resultUrl;
        }
    }
}
