using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniNet.OAuthAPI
{
    public enum HttpMethod
    {
        GET,
        POST,
        Upload
    } ;
     
    public interface IOAuthAPI
    {
        string RequestTokenUrl { get; set; }
        string AuthorizeUrl { get; set; }
        string AccessTokenUrl { get; set; }
        string Token { get; set; }
        string TokenSecret { get; set; }
        string AppKey { get; set; }
        string AppSecret { get; set; }

        /// <summary>
        /// 获取未授权的Request Token
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appKeySecret"></param>
        /// <param name="callBackUrl"></param>
        /// <returns></returns>
        bool GetRequestToken(string appKey, string appKeySecret, string callBackUrl);

        /// <summary>
        /// 获取用户授权的Request Token。这里返回重定向的地址。
        /// </summary>
        /// <param name="callBackUrl"></param>
        /// <returns></returns>
        string getAuthorize(string callBackUrl);

        /// <summary>
        /// 用授权的Request Token换取Access Token
        /// </summary>
        /// <param name="verifier"></param>
        /// <returns></returns>
        bool getAccessToken(string verifier);

        /// <summary>
        /// 接口调用方法
        /// </summary>
        /// <param name="method">GET,POST,Upload</param>
        /// <param name="api">api接口地址</param>
        /// <param name="parameter">api接口需要的参数</param>
        /// <returns></returns>
        string Call(HttpMethod method, string api, string parameter);
    }
}
