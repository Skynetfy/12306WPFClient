using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using JKVideo.Core.Enums;

namespace JKVideo.Core.Http
{
    public class HttpUtil
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="requestParameter">请求报文</param>
        /// <returns>响应报文</returns>
        public static HttpResponseParameter Excute(HttpRequestParameter requestParameter)
        {
            // 1.实例化
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(requestParameter.Url, UriKind.RelativeOrAbsolute));
            SetProxy(webRequest, requestParameter);
            // 2.设置请求头
            SetHeader(webRequest, requestParameter);
            // 3.设置请求Cookie
            SetCookie(webRequest, requestParameter);
            // 4.ssl/https请求设置
            if (Regex.IsMatch(requestParameter.Url, "^https://"))
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            }
            // 5.设置请求参数[Post方式下]
            SetParameter(webRequest, requestParameter);

            // 6.返回响应报文
            return SetResponse(webRequest, requestParameter);
        }

        /// <summary>
        /// 设置请求头
        /// </summary>
        /// <param name="webRequest">HttpWebRequest对象</param>
        /// <param name="requestParameter">请求参数对象</param>
        static void SetHeader(HttpWebRequest webRequest, HttpRequestParameter requestParameter)
        {
            webRequest.Method = requestParameter.IsPost ? "POST" : "GET";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            if (!string.IsNullOrEmpty(requestParameter.Accept))
            {
                webRequest.Accept = requestParameter.Accept;
            }
            webRequest.KeepAlive = true;
            webRequest.UserAgent = !string.IsNullOrEmpty(requestParameter.UserAgent) ? requestParameter.UserAgent : "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.63 Safari/537.36";
            webRequest.AllowAutoRedirect = true;
            webRequest.ProtocolVersion = HttpVersion.Version11;
            webRequest.Timeout = 10000;
            //webRequest.AllowWriteStreamBuffering = false;
            //webRequest.IfModifiedSince = DateTime.Now;
            //webRequest.Headers["Accept-Encoding"] = "gzip,deflate,sdch";
            //webRequest.Headers["Accept-Language"] = "zh-CN,zh;q=0.8";
            //webRequest.Headers[HttpRequestHeader.IfModifiedSince] = DateTime.Now.Millisecond.ToString();
            webRequest.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            if (!string.IsNullOrEmpty(requestParameter.RefererUrl))
            {
                webRequest.Referer = requestParameter.RefererUrl;
            }
            if (!string.IsNullOrEmpty(requestParameter.Host))
            {
                webRequest.Host = requestParameter.Host;
            }
            if (!string.IsNullOrEmpty(requestParameter.Origin))
            {
                webRequest.Headers["Origin"] = requestParameter.Origin;
            }
        }

        /// <summary>
        /// 设置请求Cookie
        /// </summary>
        /// <param name="webRequest">HttpWebRequest对象</param>
        /// <param name="requestParameter">请求参数对象</param>
        private static void SetCookie(HttpWebRequest webRequest, HttpRequestParameter requestParameter)
        {
            // 必须实例化，否则响应中获取不到Cookie
            webRequest.CookieContainer = new CookieContainer();
            if (requestParameter.Cookie != null && !string.IsNullOrEmpty(requestParameter.Cookie.CookieString))
            {
                webRequest.Headers["Cookie"] = requestParameter.Cookie.CookieString;
                webRequest.Headers[HttpRequestHeader.Cookie] = requestParameter.Cookie.CookieString;
            }
            if (requestParameter.Cookie != null && requestParameter.Cookie.CookieCollection != null && requestParameter.Cookie.CookieCollection.Count > 0)
            {
                webRequest.CookieContainer.Add(requestParameter.Cookie.CookieCollection);
            }
        }

        /// <summary>
        /// ssl/https请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        /// <summary>
        /// 设置请求参数（只有Post请求方式才设置）
        /// </summary>
        /// <param name="webRequest">HttpWebRequest对象</param>
        /// <param name="requestParameter">请求参数对象</param>
        static void SetParameter(HttpWebRequest webRequest, HttpRequestParameter requestParameter)
        {
            if (requestParameter.IsPost)
            {
                string para;
                if (requestParameter.Parameters != null && requestParameter.Parameters.Count > 0)
                {
                    StringBuilder data = new StringBuilder(string.Empty);
                    foreach (KeyValuePair<string, string> keyValuePair in requestParameter.Parameters)
                    {
                        data.AppendFormat("{0}={1}&", keyValuePair.Key, HttpUtility.UrlEncode(keyValuePair.Value));
                    }
                    para = data.Remove(data.Length - 1, 1).ToString();
                }
                else
                {
                    para = requestParameter.PostData;
                }
                if (string.IsNullOrEmpty(para))
                {
                    para = string.Empty;
                }
                byte[] bytePosts = requestParameter.Encoding.GetBytes(para);
                webRequest.ContentLength = bytePosts.Length;
                using (Stream requestStream = webRequest.GetRequestStream())
                {
                    requestStream.Write(bytePosts, 0, bytePosts.Length);
                    requestStream.Close();
                }
            }
        }

        static void SetProxy(HttpWebRequest webRequest, HttpRequestParameter requestParameter)
        {
            if (requestParameter.IsProxy)
            {
                List<string> proxyIps = new List<string>
                {
                    "http://183.207.224.12:80",
                    "http://117.135.194.55:80",
                    "http://122.225.117.26:80",
                    "http://222.88.236.236:80",
                    "http://218.203.13.174:80",
                    "http://113.105.224.86:80",
                    "http://115.239.210.199:80",
                    "http://119.6.144.74:80",
                    "http://117.135.250.51:80",
                    "http://120.131.128.211:80",
                };
                Random random = new Random();
                string address = proxyIps[random.Next(proxyIps.Count)];
                webRequest.Proxy = new WebProxy("http://120.198.243.3:80", true);
                webRequest.Credentials = CredentialCache.DefaultCredentials;
            }
            else
            {
                webRequest.Proxy = null;
            }
        }

        /// <summary>
        /// 返回响应报文
        /// </summary>
        /// <param name="webRequest">HttpWebRequest对象</param>
        /// <param name="requestParameter">请求参数对象</param>
        /// <returns>响应对象</returns>
        static HttpResponseParameter SetResponse(HttpWebRequest webRequest, HttpRequestParameter requestParameter)
        {
            HttpResponseParameter responseParameter = new HttpResponseParameter();
            using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                responseParameter.Uri = webResponse.ResponseUri;
                responseParameter.StatusCode = webResponse.StatusCode;
                responseParameter.Cookie = new HttpCookieType
                {
                    CookieCollection = webResponse.Cookies,
                    CookieString = webResponse.Headers[HttpResponseHeader.SetCookie]
                    //CookieString = webResponse.Headers["Set-Cookie"]
                };
                responseParameter.ContentType = webResponse.ContentType;
                responseParameter.ContentLength = webResponse.ContentLength;
                if (requestParameter.ResponseEnum == HttpResponseEnum.String)
                {
                    using (StreamReader reader = new StreamReader(webResponse.GetResponseStream(), requestParameter.Encoding))
                    {
                        responseParameter.Body = reader.ReadToEnd();
                        reader.Close();
                    }
                }
                else
                {
                    //responseParameter.ResponseStream = webResponse.GetResponseStream();
                    if (requestParameter.StreamAction != null)
                    {
                        requestParameter.StreamAction(webResponse.GetResponseStream());
                    }
                }

                webResponse.Close();
            }
            //try
            //{
            //    using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
            //    {
            //        responseParameter.Uri = webResponse.ResponseUri;
            //        responseParameter.StatusCode = webResponse.StatusCode;
            //        responseParameter.Cookie = new HttpCookieType
            //        {
            //            CookieCollection = webResponse.Cookies,
            //            CookieString = webResponse.Headers["Set-Cookie"]
            //        };
            //        using (StreamReader reader = new StreamReader(webResponse.GetResponseStream(), requestParameter.Encoding))
            //        {
            //            responseParameter.Body = reader.ReadToEnd();
            //        }
            //    }
            //}
            //catch (WebException ex)
            //{
            //    using (StreamReader reader = new StreamReader(ex.Response.GetResponseStream(), requestParameter.Encoding))
            //    {
            //        responseParameter.Body = reader.ReadToEnd();
            //    }
            //}
            return responseParameter;
        }
    }
}
