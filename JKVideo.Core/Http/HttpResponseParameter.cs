using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JKVideo.Core.Http
{
    /// <summary>
    /// 响应参数类
    /// </summary>
    public  class HttpResponseParameter
    {
        public HttpResponseParameter()
        {
            Cookie = new HttpCookieType();
        }
        /// <summary>
        /// 响应资源标识符
        /// </summary>
        public Uri Uri { get; set; }
        /// <summary>
        /// 响应状态码
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// 响应Cookie对象
        /// </summary>
        public HttpCookieType Cookie { get; set; }
        /// <summary>
        /// 响应长度
        /// </summary>
        public long ContentLength { get; set; }
        /// <summary>
        /// 响应类型
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// 响应体
        /// </summary>
        public string Body { get; set; }
        /// <summary>
        /// 响应流
        /// </summary>
        public Stream ResponseStream { get; set; }
    }
}
