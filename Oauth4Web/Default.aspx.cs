using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oauth4Web.Model;
using MiniNet.OAuthAPI;

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
                }

                if (Request["oauth_verifier"] != null || drpSite.SelectedIndex == 2)
                {
                    if (!string.IsNullOrEmpty(txtToken.Text))
                    {
                        return;
                    }

                    var verifier = Request["oauth_verifier"];

                    OAuthAPI oauthAPI = Session["oauthAPI"] as OAuthAPI;

                    if (oauthAPI.GetAccessToken(verifier))
                    {
                        txtToken.Text = oauthAPI.Token;
                        txtTokenSecret.Text = oauthAPI.TokenSecret;
                        this.lblErrorMsg.Text = "授权成功";
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

            var site = drpSite.SelectedValue;

            OAuthAPIEntity oauthAPIEntity = GetOAuthAPI(site);
            oauthAPIEntity.AppKey = txtAppKey.Text;
            oauthAPIEntity.AppSecret = txtAppSecret.Text;
            oauthAPIEntity.Site = int.Parse(site);

            Session["oauthAPIObj"] = oauthAPIEntity;

            IOAuthAPI oauthAPI = new OAuthAPI();
            oauthAPI.RequestTokenUrl = oauthAPIEntity.RequestTokenUrl;
            oauthAPI.AuthorizeUrl = oauthAPIEntity.AuthorizeUrl;
            oauthAPI.AccessTokenUrl = oauthAPIEntity.AccessTokenUrl;

            if (oauthAPI.GetRequestToken(oauthAPIEntity.AppKey, oauthAPIEntity.AppSecret, "http://localhost:3668/default.aspx"))
            {
                var authorizationUrl = oauthAPI.GetAuthorize("http://localhost:3668/default.aspx");

                Session["oauthAPI"] = oauthAPI;

                if (!string.IsNullOrEmpty(authorizationUrl))
                {
                    Response.Redirect(authorizationUrl);
                }
            }
        }

        private OAuthAPIEntity GetOAuthAPI(string site)
        {
            OAuthAPIEntity oauthAPI = new OAuthAPIEntity();

            switch (site)
            {
                case "0":
                    //新浪微博
                    oauthAPI.AccessTokenUrl = "http://api.t.sina.com.cn/oauth/access_token";
                    oauthAPI.AuthorizeUrl = "http://api.t.sina.com.cn/oauth/authorize";
                    oauthAPI.RequestTokenUrl = "http://api.t.sina.com.cn/oauth/request_token";
                    break;
                case "1":
                    //腾讯微博
                    oauthAPI.AccessTokenUrl = "https://open.t.qq.com/cgi-bin/access_token";
                    oauthAPI.AuthorizeUrl = "https://open.t.qq.com/cgi-bin/authorize";
                    oauthAPI.RequestTokenUrl = "https://open.t.qq.com/cgi-bin/request_token";
                    break;
                case "2":
                    //网易微博
                    oauthAPI.AccessTokenUrl = "http://api.t.163.com/oauth/access_token";
                    oauthAPI.AuthorizeUrl = "http://api.t.163.com/oauth/authenticate";
                    oauthAPI.RequestTokenUrl = "http://api.t.163.com/oauth/request_token";
                    break;
                case "3":
                    //搜狐微博
                    oauthAPI.AccessTokenUrl = "http://api.t.sohu.com/oauth/access_token";
                    oauthAPI.AuthorizeUrl = "http://api.t.sohu.com/oauth/authorize";
                    oauthAPI.RequestTokenUrl = "http://api.t.sohu.com/oauth/request_token";
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

            HttpMethod method = HttpMethod.GET;

            if (methodVal.Equals("0"))
            {
                method = HttpMethod.GET;
            }
            else if (methodVal.Equals("1"))
            {
                method = HttpMethod.POST;
            }
            else
            {
                method = HttpMethod.Upload;
            }

            var content = oauthAPI.Call(method, txtApi.Text, txtApiParameter.Text);

            txtContent.Text = content;
        }
    }
}
