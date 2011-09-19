using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MiniNet.OAuthAPI;
using MiniNet.OAuthAPI.Util;

namespace NUnitTest.qq.com
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
            oauthAPI.AppKey = "ebf7336b13e244c888e67ef33c6a07be";
            oauthAPI.AppSecret = "7b27dd3e8e85af7558961c5bc9a297b4";
            oauthAPI.Token = "f71f4c3c13be441ea658171649cdb68a";
            oauthAPI.TokenSecret = "e5f386111aff1908f67df4c763c9efbc";
        }

        /// <summary>
        /// user/other_info
        /// </summary>
        [Test]
        public void TestUserInfo()
        {
            //user/other_info
            var result = oauthAPI.Call(HttpMethod.GET, "http://open.t.qq.com/api/user/other_info", "format=json&name=gantingting");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }
    }
}
