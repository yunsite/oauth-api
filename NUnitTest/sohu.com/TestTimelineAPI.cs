using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MiniNet.OAuthAPI;
using MiniNet.OAuthAPI.Util;

namespace NUnitTest.sohu.com
{
    /// <summary>
    /// 获取下行数据集(timeline)接口 
    /// </summary>
    [TestFixture]
    public class TestTimelineAPI
    {
        private IOAuthAPI oauthAPI = null;

        [TestFixtureSetUp]
        public void Init()
        {
            oauthAPI = OAuthAPIFactory.CreateOAuthAPI();
            oauthAPI.AppKey = "R59IF17nNl6PEt3EBbfM";
            oauthAPI.AppSecret = "Eb3)5g1PAqYD6UhrO1-O52CZ2tvkdioGxxf^7SCt";
            oauthAPI.Token = "380fa793579a7c3e27df2e3a425330ab";
            oauthAPI.TokenSecret = "5e664a38c22086564a3076dfa77f83cb";
        }

        /// <summary>
        /// statuses/friends_timeline
        /// </summary>
        [Test]
        public void TestUserTimeline()
        {
            //statuses/user_timeline
            var result = oauthAPI.Call(HttpMethod.GET, "http://open.t.qq.com/api/statuses/user_timeline", "format=json&pageflag=0&reqnum=20&name=dinglingtao");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// statuses/comments
        /// </summary>
        [Test]
        public void TestComments()
        {
            //statuses/comments
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sohu.com/statuses/comments/1920715746.json", "");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// statuses/counts
        /// </summary>
        [Test]
        public void TestCount()
        {
            //statuses/counts
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sohu.com/statuses/counts.json", "ids=194802546,194616681");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }
    }
}
