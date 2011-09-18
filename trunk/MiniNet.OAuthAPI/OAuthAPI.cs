using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniNet.OAuthAPI.Net;
using System.Web;
using System.Globalization;

namespace MiniNet.OAuthAPI
{
    public class OAuthAPI : IOAuthAPI
    {
        private const string oauthTokenKey = "oauth_token";

        private const string oauthTokenSecretKey = "oauth_token_secret";

        private const string OAuthSignaturePattern = "OAuth oauth_consumer_key=\"{0}\",oauth_signature_method=\"HMAC-SHA1\",oauth_timestamp=\"{1}\",oauth_nonce=\"{2}\",oauth_version=\"1.0\",oauth_token=\"{3}\",oauth_signature=\"{4}\"";

        private const string ContentEncoding = "iso-8859-1";

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

        public string GetAuthorize(string callBackUrl)
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

        public bool GetAccessToken(string verifier)
        {
            oauthBase.Verifier = verifier;
            //这一步不需要callback,加上腾讯报错。
            oauthBase.CallbackUrl = null;

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
            if (!string.IsNullOrEmpty(parameter))
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

        public string Call(string api, string parameter, string filename, byte[] bytes)
        {
            var url = api;

            if (!string.IsNullOrEmpty(parameter))
            {
                if (api.IndexOf("?") > 0)
                {
                    url += "&";
                }
                else
                {
                    url += "?";
                }

                var postdata = ParsePostData(parameter);

                url += postdata;
            }

            var authorization = GetAuthorizationHead(url, "POST");

            var httpWebRequest = HttpRequestFactory.GetDefaultHttpWebRequest(HttpMethod.POST, new Uri(api), "", 60000);

            httpWebRequest.Headers.Add("Authorization", authorization);

            var boundary = "OAuthAPI";

            var cols = HttpUtility.ParseQueryString(parameter);
            var status = "";
            if (cols["status"] != null)
            {
                status = HttpUtility.UrlEncode(cols["status"]);
            }
            float? latVal = null;
            if (cols["lat"] != null)
            {
                latVal = float.Parse(cols["lat"]);
            }
            float? longVal = null;
            if (cols[""] != null)
            {
                longVal = float.Parse(cols["long"]);
            }

            var body = PackImage(boundary, filename, bytes, oauthBase.AppKey, status, latVal, longVal);

            httpWebRequest.ContentType = string.Format("multipart/form-data; boundary={0}", boundary);
            httpWebRequest.ContentLength = body.Length;
            httpWebRequest.PreAuthenticate = true;
            httpWebRequest.AllowWriteStreamBuffering = true;

            var request = HttpRequestFactory.CreateHttpRequest();

            var content = request.Post(httpWebRequest, body);

            return content;
        }

        /// <summary>
        /// Pack image from file into multipart-formdata post body
        /// </summary>
        /// <param name="boundary"></param>
        /// <param name="filename"></param>
        /// <param name="mimeTypes"></param>
        /// <param name="bytes"></param>
        /// <param name="source"></param>
        /// <param name="status"></param>
        /// <param name="latVal"></param>
        /// <param name="longVal"></param>
        /// <returns></returns>
        private byte[] PackImage(string boundary, string filename, byte[] bytes, string source, string status, float? latVal, float? longVal)
        {
            string header = string.Format("--{0}", boundary);
            string footer = string.Format("--{0}--", boundary);

            var body = new StringBuilder();

            if (!string.IsNullOrEmpty(status))
            {
                body.AppendLine(header);
                body.AppendLine(String.Format("Content-Disposition: form-data; name=\"{0}\"", "status"));
                body.AppendLine("Content-Type: text/plain; charset=US-ASCII");
                body.AppendLine("Content-Transfer-Encoding: 8bit");
                body.AppendLine();
                body.AppendLine(status);
            }

            if (!string.IsNullOrEmpty(source))
            {
                body.AppendLine(header);
                body.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"", "source"));
                body.AppendLine("Content-Type: text/plain; charset=US-ASCII");
                body.AppendLine("Content-Transfer-Encoding: 8bit");
                body.AppendLine();
                body.AppendLine(AppKey);
            }

            if (latVal.HasValue)
            {
                body.AppendLine(header);
                body.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"", "lat"));
                body.AppendLine("Content-Type: text/plain; charset=US-ASCII");
                body.AppendLine("Content-Transfer-Encoding: 8bit");
                body.AppendLine();
                body.AppendLine(latVal.Value.ToString());
            }

            if (longVal.HasValue)
            {
                body.AppendLine(header);
                body.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"", "long"));
                body.AppendLine("Content-Type: text/plain; charset=US-ASCII");
                body.AppendLine("Content-Transfer-Encoding: 8bit");
                body.AppendLine();
                body.AppendLine(longVal.Value.ToString());
            }

            string fileHeader = string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", "pic",
                                              filename);

            body.AppendLine(header);
            body.AppendLine(fileHeader);
            //body.AppendLine(string.Format("Content-Type: {0}", mimeTypes));
            body.AppendLine("Content-Type: application/octet-stream; charset=UTF-8");
            body.AppendLine("Content-Transfer-Encoding: binary");
            body.AppendLine();
            body.AppendLine(Encoding.GetEncoding(ContentEncoding).GetString(bytes));

            body.AppendLine(footer);

            return Encoding.GetEncoding(ContentEncoding).GetBytes(body.ToString());
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
                queryString[key] = HttpUtility.UrlEncode(queryString[key]);
                resultUrl += (key + "=" + queryString[key]);
            }
            return resultUrl;
        }

        private string GetAuthorizationHead(string url, string method)
        {
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

            signature = HttpUtility.UrlEncode(signature);

            return string.Format(
                CultureInfo.InvariantCulture,
                OAuthSignaturePattern,
                AppKey,
                timestamp,
                nonce,
                Token,
                signature);
        }
    }
}
