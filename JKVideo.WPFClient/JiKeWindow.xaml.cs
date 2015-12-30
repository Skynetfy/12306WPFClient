using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using JKVideo.Core.Common;
using JKVideo.Core.Entities;
using JKVideo.Core.Enums;
using JKVideo.Core.Http;

namespace JKVideo.WPFClient
{
    /// <summary>
    /// Interaction logic for JiKeWindow.xaml
    /// </summary>
    public partial class JiKeWindow : Window
    {
        /// <summary>
        /// Http请求者
        /// </summary>
        protected IHttpProvider httpProvider { get; private set; }
        /// <summary>
        /// 会话Cookie
        /// </summary>
        protected HttpCookieType SessionCookie { get; private set; }

        public JiKeWindow()
        {
            InitializeComponent();
            httpProvider = new HttpProvider();
            GetVerifyCode();
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        void GetVerifyCode()
        {
            GetLoginSessionCookie();

            ImageRandCode.Source = null;
            // 2.带上会话cookie请求验证码图片（保证是当前用户）
            httpProvider.Excute(new HttpRequestParameter
            {
                Url = string.Format("http://passport.jikexueyuan.com/sso/verify?t={0}", DateTime.Now.ToString("yyyyMMddHHmmsss")),
                Cookie = SessionCookie,
                ResponseEnum = HttpResponseEnum.Stream,
                StreamAction = x =>
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

                    ImageRandCode.Source = bmp;

                    ms.Close();
                }
            });
        }

        /// <summary>
        /// 获取登录的Cookie
        /// </summary>
        void GetLoginSessionCookie()
        {
            var httpResponse = httpProvider.Excute(new HttpRequestParameter()
            {
                Url = "http://passport.jikexueyuan.com/sso/login",
                IsPost = false
            });

            SessionCookie = httpResponse.Cookie;
        }

        protected void Login_Click(object sender, RoutedEventArgs e)
        {
            var userName = UserName.Text.Trim();
            var passWord = PassWord.Password.Trim();
            var randCode = RandCode.Text.Trim();

            var postDic = new Dictionary<string, string>();
            postDic.Add("expire","7");
            postDic.Add("referer", "http://www.jikexueyuan.com/");
            postDic.Add("uname", userName);
            postDic.Add("password", passWord);
            postDic.Add("verify", randCode);

            HttpResponseParameter responseParameter = httpProvider.Excute(new HttpRequestParameter
            {
                Url = "http://passport.jikexueyuan.com/submit/login?is_ajax=1",
                IsPost = true,
                Parameters = postDic,
                Cookie = SessionCookie
            });

            LoginResultEntity loginResult = responseParameter.Body.DeserializeObject<LoginResultEntity>();
            if (loginResult.status == 1)
            {
                // 2.登录成功，保存cookie
                CookieStoreInstance.CurrentCookie = responseParameter.Cookie;
            }
        }

        protected void Check_Click(object sender, RoutedEventArgs e)
        {

            HttpResponseParameter responseParameter = httpProvider.Excute(new HttpRequestParameter
            {
                Url = "http://www.jikexueyuan.com/course/video_download?seq=1&course_id=228",
                IsPost = false,
                Cookie = CookieStoreInstance.CurrentCookie
            });
        }
    }
}
