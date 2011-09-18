using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MiniNet.OAuthAPI;
using MiniNet.OAuthAPI.Util;
using System.Collections;

namespace NUnitTest.weibo.com
{
    [TestFixture]
    public class TestFriendshipsAPI
    {
        private IOAuthAPI oauthAPI = null;

        private bool isExists = true;

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
        /// friendships/create
        /// </summary>
        [Test]
        public void TestFriendshipsCreate()
        {
            TestFriendshipsDestroy();

            //friendships/create
            var result = oauthAPI.Call(HttpMethod.POST, "http://api.t.sina.com.cn/friendships/create.json", "user_id=1771615175");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// friendships/exists
        /// </summary>
        [Test]
        public void TestFriendshipsExists()
        {
            //friendships/exists
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/friendships/exists.json", "user_a=2281712542&user_b=1771615175");
            object obj = JSON.JsonDecode(result);

            Assert.IsNotNull(obj);

            var hash = obj as Hashtable;

            isExists = (Boolean)hash["friends"];
        }

        /// <summary>
        /// friendships/destroy
        /// </summary>
        [Test]
        public void TestFriendshipsDestroy()
        {
            TestFriendshipsExists();

            if (isExists)
            {
                //friendships/destroy
                var result = oauthAPI.Call(HttpMethod.POST, "http://api.t.sina.com.cn/friendships/destroy.json", "user_id=1771615175");
                Assert.IsNotNull(JSON.JsonDecode(result));
            }
        }

        /// <summary>
        /// friendships/show
        /// </summary>
        [Test]
        public void TestFriendsShow()
        {
            //friendships/show
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/friendships/show.json", "source_id=2281712542&target_id=2373552642");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }
    }
}
