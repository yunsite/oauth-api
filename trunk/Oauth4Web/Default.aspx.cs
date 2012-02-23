using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oauth4Web.Model;
using MiniNet.OAuthAPI;
using System.IO;
using MiniNet.OAuthAPI.Util;
using System.Collections;
using Oauth4Web.DataAccess;
using MiniNet.Net;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;
using MiniNet.Utility.Security;
using MiniNet.Utility;

namespace Oauth4Web
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["oauthAPIObj"] != null)
                {
                    OAuthAPIEntity oauthAPI = Session["oauthAPIObj"] as OAuthAPIEntity;

                    drpSite.SelectedIndex = oauthAPI.Site;

                    txtAppKey.Text = oauthAPI.AppKey;
                    txtAppSecret.Text = oauthAPI.AppSecret;
                    txtUserName.Text = oauthAPI.UserName;
                }

                if (Request["oauth_verifier"] != null || drpSite.SelectedIndex == 2)
                {
                    if (!string.IsNullOrEmpty(txtToken.Text))
                    {
                        return;
                    }

                    var verifier = Request["oauth_verifier"];

                    IOAuthAPI oauthAPI = Session["oauthAPI"] as IOAuthAPI;

                    if (oauthAPI.GetAccessToken(verifier))
                    {
                        txtToken.Text = oauthAPI.Token;
                        txtTokenSecret.Text = oauthAPI.TokenSecret;
                        this.lblErrorMsg.Text = "授权成功";

                        Session["oauthAPI"] = oauthAPI;

                        OAuthAPIEntity oauthAPIEntity = Session["oauthAPIObj"] as OAuthAPIEntity;

                        oauthAPIEntity.Token = oauthAPI.Token;
                        oauthAPIEntity.TokenSecret = oauthAPI.TokenSecret;

                        Session["oauthAPIObj"] = oauthAPIEntity;
                    }
                }
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAppKey.Text) || string.IsNullOrEmpty(txtAppSecret.Text))
            {
                lblErrorMsg.Text = "请输入appkey和appsecret";
                return;
            }

            Session.Clear();

            var site = int.Parse(drpSite.SelectedValue);

            if (site == 5)
            {
                site = 0;
            }

            OAuthAPIEntity entity = OAuthAPIDAL.Load(txtAppKey.Text, txtUserName.Text, site);

            if (entity != null)
            {
                lblErrorMsg.Text = "已经存在";
                txtToken.Text = entity.Token;
                txtTokenSecret.Text = entity.TokenSecret;

                IOAuthAPI oauthAPI2 = OAuthAPIFactory.CreateOAuthAPI();
                oauthAPI2.RequestTokenUrl = entity.RequestTokenUrl;
                oauthAPI2.AuthorizeUrl = entity.AuthorizeUrl;
                oauthAPI2.AccessTokenUrl = entity.AccessTokenUrl;
                oauthAPI2.AppKey = entity.AppKey;
                oauthAPI2.AppSecret = entity.AppSecret;
                oauthAPI2.Token = entity.Token;
                oauthAPI2.TokenSecret = entity.TokenSecret;

                Session["oauthAPI"] = oauthAPI2;
                return;
            }

            OAuthAPIEntity oauthAPIEntity = GetOAuthAPI(site);
            oauthAPIEntity.AppKey = txtAppKey.Text;
            oauthAPIEntity.AppSecret = txtAppSecret.Text;
            oauthAPIEntity.UserName = txtUserName.Text;
            oauthAPIEntity.Password = txtPassword.Text;
            oauthAPIEntity.Site = site;

            Session["oauthAPIObj"] = oauthAPIEntity;

            if (int.Parse(drpSite.SelectedValue) >= 5)
            {
                IHttpForm http = HttpFormFactory.DefaultHttpForm();

                string authorizeFormat = "https://api.weibo.com/oauth2/authorize?client_id={0}&redirect_uri={1}&response_type=code";

                string authorize = string.Format(authorizeFormat, oauthAPIEntity.AppKey, "http://barefoot.3322.org/queryservice.svc/query");

                HttpFormGetRequest getRequest = new HttpFormGetRequest();

                getRequest.Cookies = Login(oauthAPIEntity.UserName, oauthAPIEntity.Password);
                getRequest.Url = authorize;

                HttpFormResponse response = http.Get(getRequest);

                Match m = null;

                if (!response.Response.StartsWith("\"code="))
                {
                    m = Regex.Match(response.Response, "<input\\stype=\"hidden\"\\sname=\"regCallback\"\\svalue=\"(?<regCallback>[^\"]+)\"/>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

                    string regCallback = m.Groups["regCallback"].Value;

                    string regPostData = "action=submit&response_type=code&regCallback=" + regCallback + "&redirect_uri=http://barefoot.3322.org/queryservice.svc/query&client_id=" + oauthAPIEntity.AppKey + "&state=&from=";

                    HttpFormPostRawRequest regRequest = new HttpFormPostRawRequest();

                    regRequest.Data = regPostData;
                    regRequest.Url = "https://api.weibo.com/oauth2/authorize";
                    regRequest.Cookies = response.Cookies;

                    response = http.Post(regRequest);
                }

                string code = response.Response.Trim('\"').Substring(5);

                HttpFormPostRawRequest request = new HttpFormPostRawRequest();

                request.Url = "https://api.weibo.com/oauth2/access_token";

                string postDataFormat = "client_id={0}&client_secret={1}&grant_type=authorization_code&code={2}&redirect_uri=http://barefoot.3322.org/queryservice.svc/query";

                string postData = string.Format(postDataFormat, oauthAPIEntity.AppKey, oauthAPIEntity.AppSecret, code);

                request.Data = postData;

                response = http.Post(request);

                m = Regex.Match(response.Response, "{\"access_token\":\"(?<token>[^\"]+)\",");

                string token = m.Groups["token"].Value;

                txtToken.Text = token;
                txtTokenSecret.Text = code;
                this.lblErrorMsg.Text = "授权成功";

                oauthAPIEntity.Token = token;
                oauthAPIEntity.TokenSecret = code;
                oauthAPIEntity.Version = 2;

                Session["oauthAPIObj"] = oauthAPIEntity;
            }
            else
            {
                IOAuthAPI oauthAPI = OAuthAPIFactory.CreateOAuthAPI();
                oauthAPI.RequestTokenUrl = oauthAPIEntity.RequestTokenUrl;
                oauthAPI.AuthorizeUrl = oauthAPIEntity.AuthorizeUrl;
                oauthAPI.AccessTokenUrl = oauthAPIEntity.AccessTokenUrl;

                if (oauthAPI.GetRequestToken(oauthAPIEntity.AppKey, oauthAPIEntity.AppSecret, Config.CallbackUrl))
                {
                    var authorizationUrl = oauthAPI.GetAuthorize(Config.CallbackUrl);

                    Session["oauthAPI"] = oauthAPI;

                    if (!string.IsNullOrEmpty(authorizationUrl))
                    {
                        Response.Redirect(authorizationUrl);
                    }
                }
            }
        }

        private OAuthAPIEntity GetOAuthAPI(int site)
        {
            OAuthAPIEntity oauthAPI = new OAuthAPIEntity();

            switch (site)
            {
                case 0:
                    //新浪微博
                    oauthAPI.AccessTokenUrl = "http://api.t.sina.com.cn/oauth/access_token";
                    oauthAPI.AuthorizeUrl = "http://api.t.sina.com.cn/oauth/authorize";
                    oauthAPI.RequestTokenUrl = "http://api.t.sina.com.cn/oauth/request_token";
                    break;
                case 5:
                    //新浪微博2.0
                    oauthAPI.AccessTokenUrl = "https://api.weibo.com/oauth2/access_token";
                    oauthAPI.AuthorizeUrl = "https://api.weibo.com/oauth2/authorize";
                    oauthAPI.RequestTokenUrl = "";
                    break;
                case 1:
                    //腾讯微博
                    oauthAPI.AccessTokenUrl = "https://open.t.qq.com/cgi-bin/access_token";
                    oauthAPI.AuthorizeUrl = "https://open.t.qq.com/cgi-bin/authorize";
                    oauthAPI.RequestTokenUrl = "https://open.t.qq.com/cgi-bin/request_token";
                    break;
                case 2:
                    //网易微博
                    oauthAPI.AccessTokenUrl = "http://api.t.163.com/oauth/access_token";
                    oauthAPI.AuthorizeUrl = "http://api.t.163.com/oauth/authenticate";
                    oauthAPI.RequestTokenUrl = "http://api.t.163.com/oauth/request_token";
                    break;
                case 3:
                    //搜狐微博
                    oauthAPI.AccessTokenUrl = "http://api.t.sohu.com/oauth/access_token";
                    oauthAPI.AuthorizeUrl = "http://api.t.sohu.com/oauth/authorize";
                    oauthAPI.RequestTokenUrl = "http://api.t.sohu.com/oauth/request_token";
                    break;
                case 4:
                    //开心网
                    oauthAPI.AccessTokenUrl = "http://api.kaixin001.com/oauth/access_token";
                    oauthAPI.AuthorizeUrl = "http://api.kaixin001.com/oauth/authorize";
                    oauthAPI.RequestTokenUrl = "http://api.kaixin001.com/oauth/request_token";
                    break;
            }

            return oauthAPI;
        }

        protected void butReset_Click(object sender, EventArgs e)
        {
            txtAppKey.Text = "";
            txtAppSecret.Text = "";
            txtToken.Text = "";
            txtTokenSecret.Text = "";
            txtContent.Text = "";
            txtUserName.Text = "";

            Session.Clear();
        }

        protected void btnGet_Click(object sender, EventArgs e)
        {
            if (Session["oauthAPI"] == null)
            {
                lblErrorMsg.Text = "请先提供授权";
                return;
            }

            OAuthAPI oauthAPI = Session["oauthAPI"] as OAuthAPI;

            var methodVal = drpMethod.SelectedValue;

            MiniNet.OAuthAPI.HttpMethod method = MiniNet.OAuthAPI.HttpMethod.GET;

            if (methodVal.Equals("0"))
            {
                method = MiniNet.OAuthAPI.HttpMethod.GET;
            }
            else if (methodVal.Equals("1"))
            {
                method = MiniNet.OAuthAPI.HttpMethod.POST;
            }

            var content = "";

            if (FileUpload1.HasFile)
            {
                var file = Request.Files[0];
                var filename = file.FileName;
                byte[] bytes = new byte[file.ContentLength];
                file.InputStream.Read(bytes, 0, file.ContentLength);
                content = oauthAPI.Call(txtApi.Text, txtApiParameter.Text, filename, bytes);
            }
            else
            {
                content = oauthAPI.Call(method, txtApi.Text, txtApiParameter.Text);
            }

            txtContent.Text = content;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            OAuthAPIEntity oauthAPI = Session["oauthAPIObj"] as OAuthAPIEntity;

            try
            {
                OAuthAPIDAL.Save(oauthAPI);

                lblErrorMsg.Text = "保存成功";
            }
            catch (Exception ex)
            {
                lblErrorMsg.Text = "保存失败。";
            }
        }

        /// <summary>
        /// v1.3.17 2011-12-15
        /// </summary>
        private CookieContainer Login(string name, string pass)
        {
            IHttpForm http = HttpFormFactory.DefaultHttpForm();

            name = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(name));
            pass = HttpUtility.UrlEncode(pass);

            var preloginUrl = string.Format("http://login.sina.com.cn/sso/prelogin.php?entry=weibo&callback=sinaSSOController.preloginCallBack&su={0}&client=ssologin.js(v1.3.17)&_={1}", name, DateTimeHelper.GetTimestamp());

            HttpFormResponse response = http.Get(preloginUrl);

            Match m = Regex.Match(response.Response, "\"retcode\":(?<retcode>\\d),\"servertime\":(?<servertime>\\d+),\"pcid\":\"[\\w]+\",\"nonce\":\"(?<nonce>[0-9a-zA-Z]+)\"", RegexOptions.IgnoreCase);

            var servertime = m.Groups["servertime"].Value;
            var nonce = m.Groups["nonce"].Value;

            JSSha1Util jsMD5 = new JSSha1Util();

            var password = jsMD5.Hex_sha1("" + jsMD5.Hex_sha1(jsMD5.Hex_sha1(pass)) + servertime + nonce);

            var postData = string.Format("entry=weibo&gateway=1&from=&savestate=7&useticket=1&ssosimplelogin=1&vsnf=1&vsnval=&su={0}&service=miniblog&servertime={1}&nonce={2}&pwencode=wsse&sp={3}&encoding=UTF-8&url=http%3A%2F%2Fweibo.com%2Fajaxlogin.php%3Fframelogin%3D1%26callback%3Dparent.sinaSSOController.feedBackUrlCallBack&returntype=META", name, servertime, nonce, password);

            HttpFormPostRawRequest postRequest = new HttpFormPostRawRequest();

            postRequest.Data = postData;
            postRequest.Url = "http://login.sina.com.cn/sso/login.php?client=ssologin.js(v1.3.17)";
            postRequest.Referer = "http://weibo.com/";
            postRequest.Cookies = new CookieContainer();

            response = http.Post(postRequest);

            var content = response.Response;

            m = Regex.Match(content, "location\\.replace\\(['\"](?<url>.*?)['\"]\\)", RegexOptions.IgnoreCase);

            var nextUrl = m.Groups["url"].Value;

            response.Cookies = CookieHelper.UpdateDomain(response.Cookies, "weibo.com");

            HttpFormGetRequest request = new HttpFormGetRequest();
            request.Cookies = response.Cookies;
            request.Referer = "http://weibo.com/";
            request.Url = nextUrl;

            response = http.Get(request);

            bool isLogin = response.Response.Contains("\"result\":true,\"");

            return response.Cookies;
        }
    }
}
