using System;
using System.Collections.Generic;
using JKVideo.Core.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest2
    {
        /// <summary>
        /// Http请求者
        /// </summary>
        protected IHttpProvider httpProvider { get; private set; }
        /// <summary>
        /// 会话Cookie
        /// </summary>
        protected HttpCookieType SessionCookie { get; private set; }

        public UnitTest2()
        {
            //httpProvider = new HttpProvider();
            //GetSessionCookie();
        }
        [TestMethod]
        public void TestMethod2()
        {
            int len = "7a4f76a3bb33cabc5f2e8a530f5bfa26".Length;
            var guid = Guid.NewGuid().ToString("N");
            var le = guid.Length;
        }

        [TestMethod]
        public void TestMethod1()
        {
            var dicPostData = new Dictionary<string, string>();
            dicPostData.Add("username","skynetfy");
            dicPostData.Add("password", "3447983");
            dicPostData.Add("execution", "e4s1");
            dicPostData.Add("_eventId", "submit");
            dicPostData.Add("lt", "LT-47742-zooQnRZrkzqiz4KrTF61Xbdw1916Qx");

            HttpResponseParameter responseParameter = httpProvider.Excute(new HttpRequestParameter
            {
                Url = "http://passport.csdn.net/ajax/accounthandler.ashx?t=log&u=skynetfy&p=3447983&remember=0&callback=csdn.login_back&r=1451371217211",
                IsPost = false,
                Cookie = SessionCookie,
                Accept = "*/*",
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.93 Safari/537.36",
                RefererUrl = "http://download.csdn.net/download/sy_05/9381944"
            });

            SessionCookie = responseParameter.Cookie;

            var response = httpProvider.Excute(new HttpRequestParameter
            {
                Url = "http://dl.download.csdn.net/down11/20151229/eaa6473fcb255ba527cef1ad0ee1ae1e.rar?response-content-disposition=attachment%3Bfilename%3DCCC.rar&OSSAccessKeyId=9q6nvzoJGowBj4q1&Expires=1451375339&Signature=YYHS%2FTdgHSqv9WFq9ek2zGK2OlU%3D",
                IsPost = false,
                Cookie = SessionCookie,
                Accept = "*/*",
                UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.93 Safari/537.36",
            });
        }

        private void GetSessionCookie()
        {
            HttpResponseParameter responseParameter = httpProvider.Excute(new HttpRequestParameter
            {
                Url = "http://download.csdn.net/detail/sy_05/9381944",
                IsPost = false
            });
            SessionCookie = responseParameter.Cookie;
        }
    }
}
