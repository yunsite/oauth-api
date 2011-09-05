using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using MiniNet.Net.Common;

namespace MiniNet.Net
{
    public class NetClient
    {
        #region 私有字段
        private int maxLength = 1 * 1024 * 1024;

        private string userAgent = "Mozilla/5.0 (Windows NT 5.1; rv:5.0) Gecko/20100101 Firefox/5.0";

        private Encoding encode = Encoding.GetEncoding("gb2312");

        private CookieContainer cookieContauner = new CookieContainer();

        private bool isCompress = true;

        private bool allowAutoRedirect = true;

        private bool isKeepCookie = false;

        private bool isCharsetProbe = true;

        private bool expect100Continue = true;

        private WebHeaderCollection responseHeads = null;
        #endregion

        #region 属性
        /// <summary>
        /// 默认压缩传输。
        /// </summary>
        public bool IsCompress
        {
            get { return isCompress; }
            set { isCompress = value; }
        }

        /// <summary>
        /// 默认允许跳转。
        /// </summary>
        public bool AllowAutoRedirect
        {
            get { return allowAutoRedirect; }
            set { allowAutoRedirect = value; }
        }

        /// <summary>
        /// 默认保持cookie
        /// </summary>
        public bool IsKeepCookie
        {
            get { return isKeepCookie; }
            set { isKeepCookie = value; }
        }

        public bool Expect100Continue
        {
            get { return expect100Continue; }
            set { expect100Continue = value; }
        }

        /// <summary>
        /// 默认是gb2312
        /// </summary>
        public Encoding Encode
        {
            get
            {
                return encode;
            }
            set
            {
                isCharsetProbe = false;
                encode = value;
            }
        }
        #endregion

        public string Post(Uri uri, string postData, string referer, int timeOut)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;

            try
            {
                string content = null;

                request = GetRequest(HttpMethod.POST, uri, referer, timeOut);

                var bytes = encode.GetBytes(postData);

                Stream requestStream = request.GetRequestStream();

                requestStream.Write(bytes, 0, bytes.Length);

                requestStream.Close();

                response = (HttpWebResponse)request.GetResponse();

                bytes = ResponseAsBytes(response);

                if (bytes != null)
                {
                    if (CharsetProbe.IsValidUtf8(bytes))
                    {
                        content = Encoding.UTF8.GetString(bytes);
                    }
                    else
                    {
                        content = encode.GetString(bytes);
                    }
                }

                return content;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                    response = null;
                }

                request = null;
            }
        }

        public string GetPage(Uri uri, string referer, int timeOut)
        {
            try
            {
                string content = null;

                byte[] bytes = DownloadData(uri, referer, timeOut);

                if (bytes != null)
                {
                    if (isCharsetProbe && CharsetProbe.IsValidUtf8(bytes))
                    {
                        content = Encoding.UTF8.GetString(bytes);
                    }
                    else
                    {
                        content = encode.GetString(bytes);
                    }
                }

                return content;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public byte[] DownloadData(Uri uri, string referer, int timeOut)
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            byte[] bytes;

            try
            {
                request = GetRequest(HttpMethod.GET, uri, referer, timeOut);

                response = (HttpWebResponse)request.GetResponse();

                responseHeads = response.Headers;

                bytes = ResponseAsBytes(response);

                return bytes;
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(uri);
                return null;
            }
            catch (NotSupportedException ex2)
            {
                Console.WriteLine(ex2.Message);
                Console.WriteLine(uri);

                return null;
            }
            finally
            {
                bytes = null;

                if (response != null)
                {
                    response.Close();
                    response = null;
                }

                request = null;
            }
        }

        private HttpWebRequest GetRequest(HttpMethod method, Uri uri, string referer, int timeOut)
        {
            HttpWebRequest request = null;

            request = (HttpWebRequest)WebRequest.Create(uri);

            request.Timeout = timeOut;
            request.ReadWriteTimeout = timeOut;
            request.UserAgent = userAgent;
            request.Method = method.ToString();
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "text/xml,application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
            request.KeepAlive = true;
            request.Headers.Add("Accept-Language", "zh-cn,zh;q=0.5");
            request.Headers.Add("Accept-Charset", "gb2312,utf-8;q=0.7,*;q=0.7");
            request.Referer = referer;
            request.AllowAutoRedirect = allowAutoRedirect;

            if (!expect100Continue)
            {
                request.ServicePoint.Expect100Continue = false;
            }

            if (isCompress)
            {
                request.Headers.Add("Accept-Encoding", "gzip,deflate");
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            }

            if (isKeepCookie)
            {
                request.CookieContainer = cookieContauner;
            }

            if (allowAutoRedirect)
            {
                request.MaximumAutomaticRedirections = 3;
            }

            return request;
        }

        private byte[] ResponseAsBytes(HttpWebResponse response)
        {
            int DefaultDownloadBufferLength = 65536;
            long length = response.ContentLength;
            //bool unknownsize = false;
            int n;
            int offset = 0;

            Stream stream = null;
            byte[] buffer = null;
            byte[] newbuf = null;

            try
            {
                //超过大小限制返回NULL
                if (length > maxLength)
                {
                    throw new Exception("超过大小限制");
                }

                //返回-1表示未知长度
                if (length == -1)
                {
                    //unknownsize = true;
                    length = DefaultDownloadBufferLength;
                }

                buffer = new byte[(int)length];

                stream = response.GetResponseStream();

                do
                {
                    if (length > maxLength)
                    {
                        throw new Exception("超过大小限制");
                    }

                    n = stream.Read(buffer, offset, (int)length - offset);

                    offset += n;
                    //如果服务器返回未知长度
                    if (offset == length)
                    {
                        length += DefaultDownloadBufferLength;

                        newbuf = new byte[(int)length];

                        Buffer.BlockCopy(buffer, 0, newbuf, 0, offset);
                        buffer = newbuf;
                        newbuf = null;
                    }
                } while (n != 0);


                newbuf = new byte[offset];
                Buffer.BlockCopy(buffer, 0, newbuf, 0, offset);
                buffer = newbuf;
                newbuf = null;

                stream.Close();
                stream = null;

                return buffer;
            }
            catch
            {
                throw;
            }
            finally
            {
                newbuf = null;

                if (stream != null)
                {
                    stream.Close();
                    stream = null;
                }
            }
        }
    }
}
