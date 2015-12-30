using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JKVideo.Core.Http
{
    /// <summary>
    /// 存储当前 会话Cookie 类
    /// </summary>
    public sealed class CookieStoreInstance
    {
        /// <summary>
        /// 当前登录成功后的 会话Cookie
        /// </summary>
        public static HttpCookieType CurrentCookie;

        public static string Repeat_Submit_Token;
    }
}
