using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using JKVideo.Core;
using JKVideo.Core.Enums;
using JKVideo.Core.Http;
using Newtonsoft.Json;
using _12306.Domain.Entities;

namespace JKVideo.WPFClient.CoomonCore
{
    public class RandCodeCookieHelper
    {
        public static bool CheckUserLogin(HttpCookieType sessionCookie)
        {
            var dicPostData2 = new Dictionary<string, string>();
            dicPostData2.Add("_json_att", "");
            HttpResponseParameter responseParameter2 = new HttpProvider().Excute(new HttpRequestParameter
            {
                Url = _12306UrlConfig.CheckLoginUrl,
                IsPost = true,
                Parameters = dicPostData2,
                Cookie = sessionCookie
            });

            HttpJsonResultEntity<LoginCheckResult> logResult = JsonConvert.DeserializeObject<HttpJsonResultEntity<LoginCheckResult>>(responseParameter2.Body);

            return logResult.Data.Flag;

        }

        public static HttpCookieType GetSessionCookie()
        {
            var httpResponse = new HttpProvider().Excute(new HttpRequestParameter()
            {
                Url = _12306UrlConfig.LoginCookieUrl,
                IsPost = false
            });

            return httpResponse.Cookie;
        }

        public static HttpCookieType ShowRandCode(HttpCookieType sessionCookie, Image viefCode)
        {
            var SessionCookie = new HttpCookieType();
            SessionCookie = sessionCookie;
            //GetSessionCookie();
            var request = new HttpRequestParameter();
            request.Url = string.Format(_12306UrlConfig.RandCodeUrl,
                DateTime.Now.ToString("yyyyMMddHHmmsss"));
            request.ResponseEnum = HttpResponseEnum.Stream;
            request.Cookie = sessionCookie;
            request.StreamAction = x =>
            {
                MemoryStream ms = new MemoryStream();
                byte[] buffer = new byte[1024];
                while (true)
                {
                    int sz = x.Read(buffer, 0, buffer.Length);
                    if (sz == 0) break;
                    ms.Write(buffer, 0, sz);
                }
                ms.Position = 0;

                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = new MemoryStream(ms.ToArray());
                bmp.EndInit();

                viefCode.Source = bmp;

                ms.Close();
            };

            var response = new HttpProvider().Excute(request);

            SessionCookie.CookieCollection.Add(response.Cookie.CookieCollection);
            SessionCookie.CookieString = SessionCookie.CookieString + "," + response.Cookie.CookieString;

            return sessionCookie;
        }
    }
}
