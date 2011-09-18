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
            oauthAPI.AppKey = "1992241176";
            oauthAPI.AppSecret = "f6b0007ab6757928d70c4961d04e2606";
            oauthAPI.Token = "ad8b1dc2cd84a2bdb70783f6b369f199";
            oauthAPI.TokenSecret = "637ecb48f715b3a9f1f8f9c516062e0c";
        }

        /// <summary>
        /// statuses/public_timeline
        /// </summary>
        [Test]
        public void TestPublicTimeline()
        {
            //statuses/public_timeline
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/statuses/public_timeline.json", "count=50");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// statuses/friends_timeline
        /// </summary>
        [Test]
        public void TestFriendsTimeline()
        {
            //statuses/friends_timeline
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/statuses/friends_timeline.json", "count=50&page=1");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// statuses/friends_timeline
        /// </summary>
        [Test]
        public void TestUserTimeline()
        {
            //statuses/user_timeline
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/statuses/user_timeline/timyang.json", "");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// statuses/mentions
        /// </summary>
        [Test]
        public void TestMentions()
        {
            //statuses/mentions
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/statuses/mentions.json", "count=50&page=1");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// statuses/comments_timeline
        /// </summary>
        [Test]
        public void TestCommentsTimeline()
        {
            //statuses/comments_timeline
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/statuses/comments_timeline.json", "count=50&page=1");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// statuses/comments_by_me
        /// </summary>
        [Test]
        public void TestCommentsByMe()
        {
            //statuses/comments_by_me
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/statuses/comments_by_me.json", "count=50&page=1");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// statuses/comments_to_me
        /// </summary>
        [Test]
        public void TestCommentsToMe()
        {
            //statuses/comments_to_me
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/statuses/comments_to_me.json", "count=50&page=1");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// statuses/comments
        /// </summary>
        [Test]
        public void TestComments()
        {
            //statuses/comments
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/statuses/comments.json", "id=3355667822313167&count=50&page=1");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// statuses/counts
        /// </summary>
        [Test]
        public void TestCounts()
        {
            //statuses/counts
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/statuses/counts.json", "ids=3355667822313167,3355667822691604,3355667822691750,3355667823349187,3355667826367633");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// statuses/repost_timeline
        /// </summary>
        [Test]
        public void TestRepostTimeline()
        {
            //statuses/repost_timeline
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/statuses/repost_timeline.json", "id=3355650084584979&count=50&page=1");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// statuses/repost_by_me
        /// </summary>
        [Test]
        public void TestRepostByMe()
        {
            //statuses/repost_by_me
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/statuses/repost_by_me.json", "id=1771615175&count=50&page=1");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// statuses/unread
        /// </summary>
        [Test]
        public void TestUnread()
        {
            //statuses/unread
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/statuses/unread.json", "");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// statuses/reset_count
        /// </summary>
        [Test]
        public void TestResetCount()
        {
            //statuses/reset_count
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/statuses/reset_count.json", "type=1");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// emotions
        /// </summary>
        [Test]
        public void Testemotions()
        {
            //emotions
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/emotions.json", "");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }
    }
}
