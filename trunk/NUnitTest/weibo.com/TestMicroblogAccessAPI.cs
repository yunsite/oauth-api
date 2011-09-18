using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using MiniNet.OAuthAPI;
using MiniNet.OAuthAPI.Util;
using System.IO;
using System.Collections;

namespace NUnitTest.weibo.com
{
    /// <summary>
    /// 微博访问接口 
    /// </summary>
    [TestFixture]
    public class TestMicroblogAccessAPI
    {
        private IOAuthAPI oauthAPI = null;

        private Double mid = 0;
        private Double cid = 0;

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
        /// statuses/show
        /// </summary>
        [Test]
        public void TestShow()
        {
            //statuses/show
            var result = oauthAPI.Call(HttpMethod.GET, "http://api.t.sina.com.cn/statuses/show/3355650084584979.json", "");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// statuses/update
        /// </summary>
        [Test]
        public void TestUpdate()
        {
            var content = "good，美好的一天啊。" + new Random().Next(10000, 999999);
            //statuses/update
            var result = oauthAPI.Call(HttpMethod.POST, "http://api.t.sina.com.cn/statuses/update.json", string.Format("status={0}", content));

            var obj = JSON.JsonDecode(result);

            if (obj != null)
            {
                var hash = obj as Hashtable;
                mid = (Double)hash["id"];
            }

            Assert.IsNotNull(obj);
        }

        /// <summary>
        /// statuses/upload
        /// </summary>
        [Test]
        public void TestUpload()
        {
            var status = "美图啊" + new Random().Next(10000, 999999);
            var filename = "044825_370_dgfpbnlh.jpg";
            var bytes = File.ReadAllBytes(@"E:\weibo\044825_370_dgfpbnlh.jpg");

            //statuses/upload
            var result = oauthAPI.Call("http://api.t.sina.com.cn/statuses/upload.json", string.Format("status={0}", status), filename, bytes);
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// statuses/destroy/:id 
        /// </summary>
        [Test]
        public void TestDestroy()
        {
            TestUpdate();

            Assert.IsTrue(mid > 0);
            //statuses/destroy/:id 
            var result = oauthAPI.Call(HttpMethod.POST, string.Format("http://api.t.sina.com.cn/statuses/destroy/{0}.json", mid.ToString("G16")), "");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// statuses/repost 
        /// </summary>
        [Test]
        public void TestRepost()
        {
            var id = "3357575112568056";
            var status = "不错" + new Random().Next(10000, 999999);
            var is_comment = 3;

            //statuses/repost 
            var result = oauthAPI.Call(HttpMethod.POST, "http://api.t.sina.com.cn/statuses/repost.json", string.Format("id={0}&status={1}&is_comment={2}", id, status, is_comment));
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// statuses/comment
        /// </summary>
        [Test]
        public void TestComment()
        {
            var id = "3357575112568056";
            var comment = "好" + new Random().Next(10000, 999999);

            //statuses/comment
            var result = oauthAPI.Call(HttpMethod.POST, "http://api.t.sina.com.cn/statuses/comment.json", string.Format("id={0}&comment={1}", id, comment));

            var obj = JSON.JsonDecode(result);

            if (obj != null)
            {
                var hash = obj as Hashtable;
                cid = (Double)hash["id"];
            }

            Assert.IsNotNull(obj);
        }

        /// <summary>
        /// statuses/reply (有问题，暂时没法解决)
        /// </summary>
        [Test]
        public void TestReply()
        {
            var cid = "202110915497479429";
            var id = "3355406366424701";
            var comment = "好" + new Random().Next(10000, 999999);

            //statuses/reply
            var result = oauthAPI.Call(HttpMethod.POST, "http://api.t.sina.com.cn/statuses/reply.json", string.Format("cid={0}&comment={1}&id={2}&without_mention=0", cid, comment, id));
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// statuses/comment_destroy/:id
        /// </summary>
        [Test]
        public void TestCommentDestroy()
        {
            TestComment();

            Assert.IsTrue(cid > 0);
            //statuses/comment_destroy/:id
            var result = oauthAPI.Call(HttpMethod.POST, string.Format("http://api.t.sina.com.cn/statuses/comment_destroy/{0}.json", cid.ToString("G16")), "");
            Assert.IsNotNull(JSON.JsonDecode(result));
        }

        /// <summary>
        /// statuses/comment/destroy_batch
        /// </summary>
        [Test]
        public void TestCommentDestroyBatch()
        {
            Double cid1 = 0, cid2 = 0;

            TestComment();
            Assert.IsTrue(cid > 0);

            cid1 = cid;

            TestComment();
            Assert.IsTrue(cid > 0);

            cid2 = cid;

            //statuses/comment/destroy_batch
            var result = oauthAPI.Call(HttpMethod.POST, "http://api.t.sina.com.cn/statuses/comment/destroy_batch.json", "ids=" + cid1.ToString("G16") + "," + cid2.ToString("G16"));
            Assert.IsNotNull(JSON.JsonDecode(result));
        }
    }
}
