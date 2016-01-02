<h3>实现了新浪、腾讯、网易、搜狐四大微博的统一OAuth认证、调用接口。</h3>

国内四大微博都有自己的API平台，都实现了oauth认证。各大微博提供的官方SDK也都各不相同。

而很多时候我只是需要一个简单的接口

例如像这样：
Call(HttpMethod method,string api,string parameter)

我调用任何一个微博的api接口，只需要传入3个参数：是GET还是POST,api地址，api需要的参数。

这本是一个挺容易实现的事，都是基于oauth实现的，只要这个接口实现了这个oauth协议就行了。

oauth是一个简单的协议，oauth认证的流程是这样的：

1.获取未授权的Request Token

2.获取用户授权的Request Token

3.用授权的Request Token换取Access Token

之后就是拿着access token去访问API了。

但，虽然都是oauth协议，四大微博实现的，......，各种坑爹啊。

在用过各种官方非官方的接口之后，我决定打造一个轮子，调试封装过程省略。

接口如下：

public interface IOAuthAPI
{
> string RequestTokenUrl { get; set; }<br />
> string AuthorizeUrl { get; set; }<br />
> string AccessTokenUrl { get; set; }<br />
> string Token { get; set; }<br />
> string TokenSecret { get; set; }<br />
> string AppKey { get; set; }<br />
> string AppSecret { get; set;}<br />

> bool GetRequestToken(string appKey, string appKeySecret, string callBackUrl);<br />
> string GetAuthorize(string callBackUrl);<br />
> bool GetAccessToken(string verifier);<br />

> string Call(HttpMethod method, string api, string parameter);
}

接口很简单，严格来说只有一个Call方法,

GetRequestToken，GetAuthorize，GetAccessToken等OAuth认证的三个步骤也是基于Call封装的。

API调用：

IOAuthAPI oauthAPI = new OAuthAPI();

//第一次实例化的时候需要设置一些东西。

//应用申请的key。

oauthAPI.AppKey = "";

oauthAPI.AppSecret = "";

//应用得到用户授权的Token

oauthAPI.Token = "";

oauthAPI.TokenSecret = "";

//然后就是调用各个API了：

Call(HttpMethod method,string api,string paramater)

至于OAuth认证，是调用GetRequestToken，getAuthorize，getAccessToken这三个方法。

详情请看项目里面的示例。

参考了官网的SDK的代码和一些博文:

1.官方SDK

2.http://sarlmolapple.is-programmer.com/posts/22435.html

3.http://wei.si/blog/2011/08/api-problems-of-sina-tencent-and-renren/

各大平台App申请页面：

新浪微博：http://open.weibo.com/development

腾讯微博：http://open.t.qq.com/develop.php

网易微博：http://open.t.163.com/apps/new

搜狐微博：http://open.t.sohu.com/apps/create.jsp
