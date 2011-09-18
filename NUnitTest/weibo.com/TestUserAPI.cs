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
            oauthAPI.AppKey = "1992241176";
            oauthAPI.AppSecret = "f6b0007ab6757928d70c4961d04e2606";
            oauthAPI.Token = "ad8b1dc2cd84a2bdb70783f6b369f199";
            oauthAPI.TokenSecret = "637ecb48f715b3a9f1f8f9c516062e0c";
        }

        /// <summary>
        /// users/show
        /// </summary>
        [Test]
        public void TestUserShow()
        {
            //users/show
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/users/show.json", "user_id=1771615175");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// statuses/friends
        /// </summary>
        [Test]
        public void TestFriends()
        {
            //statuses/friends
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/statuses/friends.json", "user_id=1771615175&cursor=-1&count=50");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// statuses/followers
        /// </summary>
        [Test]
        public void TestFollowers()
        {
            //statuses/followers
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/statuses/followers.json", "user_id=1771615175&cursor=-1&count=50");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// users/hot
        /// </summary>
        [Test]
        public void TestUserHot()
        {
            //users/hot
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/users/hot.json", "category=model");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// users/suggestions
        /// </summary>
        [Test]
        public void TestSuggestions()
        {
            //users/suggestions
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/users/suggestions.json", "with_reason=1");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// user/friends/update_remark
        /// </summary>
        [Test]
        public void TestFriendsUpdateRemark()
        {
            //user/friends/update_remark
            var result = oauthAPI.Call(HttpMethod.POST, "http://api.t.sina.com.cn/user/friends/update_remark.json", "user_id=2373552642&remark=淫才");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }
    }
}
