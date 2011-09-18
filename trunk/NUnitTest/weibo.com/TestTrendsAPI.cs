using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MiniNet.OAuthAPI;
using MiniNet.OAuthAPI.Util;

namespace NUnitTest.weibo.com
{
    /// <summary>
    /// 话题接口
    /// </summary>
    [TestFixture]
    public class TestTrendsAPI
    {
        private IOAuthAPI oauthAPI = null;

        [TestFixtureSetUp]
        public void Init()
        {
            oauthAPI = OAuthAPIFactory.CreateOAuthAPI();
            oauthAPI.AppKey = "1992241176";
            oauthAPI.AppSecret = "f6b0007ab6757928d70c4961d04e2606";
            oauthAPI.Token = "ad8b1dc2cd84a2bdb70783f6b369f199";
            oauthAPI.TokenSecret = "637ecb48f715b3a9f1f8f9c516062e0c";
        }

        /// <summary>
        /// trends
        /// </summary>
        [Test]
        public void TestTrends()
        {
            //trends
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/trends.json", "user_id=1035923322");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// trends/statuses
        /// </summary>
        [Test]
        public void TestTrendsStatuses()
        {
            var keyword = "%E7%BE%8e%e5%a5%b3";
            //trends/statuses
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/trends/statuses.json", string.Format("trend_name={0}", keyword));
            Assert.IsNotNull(JSON.JsonDecode(result));
        }
    }
}
