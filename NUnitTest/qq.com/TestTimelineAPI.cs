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
            oauthAPI.AppKey = "ebf7336b13e244c888e67ef33c6a07be";
            oauthAPI.AppSecret = "7b27dd3e8e85af7558961c5bc9a297b4";
            oauthAPI.Token = "f71f4c3c13be441ea658171649cdb68a";
            oauthAPI.TokenSecret = "e5f386111aff1908f67df4c763c9efbc";
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
            var result = oauthAPI.Call(HttpMethod.GET, "http://open.t.qq.com/api/t/re_list", "format=json&flag=1&rootid=14660084825938");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// t/re_count
        /// </summary>
        [Test]
        public void TestReCount()
        {
            //t/re_count
            var result = oauthAPI.Call(HttpMethod.GET, "http://open.t.qq.com/api/t/re_count", "format=json&ids=16165010368331,94545127182506&flag=2");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }
    }
}
