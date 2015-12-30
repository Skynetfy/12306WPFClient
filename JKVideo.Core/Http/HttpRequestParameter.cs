using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JKVideo.Core.Enums;

namespace JKVideo.Core.Http
{
    /// <summary>
    /// 请求参数类
    /// </summary>
    public class HttpRequestParameter
    {
        public HttpRequestParameter()
        {
            Encoding = Encoding.UTF8;
            ResponseEnum = HttpResponseEnum.String;
        }
        /// <summary>
        /// 请求方式：true表示post,false表示get
        /// </summary>
        public bool IsPost { get; set; }
        /// <summary>
        /// 是否启用代理：true表示启用,false表示禁用
        /// </summary>
        public bool IsProxy { get; set; }
        /// <summary>
        /// 请求地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 请求Cookie对象
        /// </summary>
        public HttpCookieType Cookie { get; set; }
        /// <summary>
        /// 请求编码
        /// </summary>
        public Encoding Encoding { get; set; }
        /// <summary>
        /// 请求参数-字典
        /// </summary>
        public IDictionary<string, string> Parameters { get; set; }
        /// <summary>
        /// 请求体-字符串
        /// </summary>
        public string PostData { get; set; }
        /// <summary>
        /// 引用页
        /// </summary>
        public string RefererUrl { get; set; }
        /// <summary>
        /// 主机域名
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// 最初请求发起地址
        /// </summary>
        public string Origin { get; set; }
        /// <summary>
        /// 用户浏览器
        /// </summary>
        public string UserAgent { get; set; }
        /// <summary>
        /// 请求接收类型
        /// </summary>
        public string Accept { get; set; }
        /// <summary>
        /// 响应读取方式
        /// </summary>
        public HttpResponseEnum ResponseEnum { get; set; }
        /// <summary>
        /// 流方式的操作
        /// </summary>
        public Action<Stream> StreamAction { get; set; }
    }
}
