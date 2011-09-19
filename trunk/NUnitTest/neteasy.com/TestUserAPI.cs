using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MiniNet.OAuthAPI;
using MiniNet.OAuthAPI.Util;

namespace NUnitTest.neteasy.com
{
    /// <summary>
    /// 用户接口
    /// </summary>
    [TestFixture]
    public class TestUserAPI
    {
        private IOAuthAPI oauthAPI = null;

        [TestFixtureSetUp]
        public void Init()
        {
            oauthAPI = OAuthAPIFactory.CreateOAuthAPI();
            oauthAPI.AppKey = "TeHso9yKTtSG5dVa";
            oauthAPI.AppSecret = "6sKtvqiZ6hD4zymed7uAiMLovx99nt9x";
            oauthAPI.Token = "a1c7f3f8741a7311cae7919f4aac1304";
            oauthAPI.TokenSecret = "8afb019d14ddf76dc12a031497f5916e";
        }

        /// <summary>
        /// users/show
        /// </summary>
        [Test]
        public void TestUserShow()
        {
            //users/show
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.163.com/users/show.json", "screen_name=senlinzhimei");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }
    }
}
