using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JKVideo.Core.Http;

namespace JKVideo.Core
{
    public class UserLoginProvider
    {

        /// <summary>
        /// Http请求者
        /// </summary>
        protected IHttpProvider httpProvider { get; private set; }
        /// <summary>
        /// 会话Cookie
        /// </summary>
        protected HttpCookieType sessionCookie { get; private set; }

        public UserLoginProvider()
        {
            httpProvider = new HttpProvider();
        }

        public HttpCookieType UserLogin(Dictionary<string, string> postData, HttpCookieType sessionCookie)
        {
            var _sessionCookie = new HttpCookieType();
            _sessionCookie = sessionCookie;
            HttpResponseParameter responseParameter = httpProvider.Excute(new HttpRequestParameter
            {
                Url = "https://kyfw.12306.cn/otn/login/loginAysnSuggest",
                IsPost = true,
                Parameters = postData,
                Cookie = sessionCookie
            });

            _sessionCookie.CookieCollection.Add(responseParameter.Cookie.CookieCollection);
            _sessionCookie.CookieString = _sessionCookie.CookieString + "," +
                                                             responseParameter.Cookie.CookieString;
            return _sessionCookie;
        }
    }
}
