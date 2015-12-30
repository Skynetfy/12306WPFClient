using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        /// <summary>
        /// Http请求者
        /// </summary>
        protected IHttpProvider HttpProvider { get; private set; }
        /// <summary>
        /// 会话Cookie
        /// </summary>
        protected HttpCookieType SessionCookie { get; private set; }

        public Login()
        {
            HttpProvider = new HttpProvider();
            InitializeComponent();
            ViewCode();
        }

        protected void Login_Click(object sender, RoutedEventArgs e)
        {
            var userName = UserName.Text.Trim();
            var passWord = PassWord.Password.Trim();
            var code = VerCode.Text.Trim();
            var dicPostData = new Dictionary<string, string>();

            dicPostData.Add("client", "www");
            dicPostData.Add("referer", HttpUtility.UrlEncode("http://www.jikexueyuan.com/"));
            dicPostData.Add("uname", userName);
            dicPostData.Add("password", passWord);
            dicPostData.Add("verify", code);

            HttpResponseParameter responseParameter = HttpProvider.Excute(new HttpRequestParameter
            {
                Url = "http://passport.jikexueyuan.com/submit/login?is_ajax=1",
                IsPost = true,
                Parameters = dicPostData,
                Cookie = SessionCookie
            });

            LoginResultEntity loginResult = responseParameter.Body.DeserializeObject<LoginResultEntity>();

            if (loginResult.status == 1)
            {
                // 2.登录成功，保存cookie
                // CookieStoreInstance.CurrentCookie = responseParameter.Cookie;
            }
        }

        /// <summary>
        /// 显示验证码
        /// </summary>
        void ViewCode()
        {
            GetLoginSessionCookie();

            var request = new HttpRequestParameter();
            request.Url = string.Format("http://passport.jikexueyuan.com/sso/verify?t={0}",
                DateTime.Now.ToString("yyyyMMddHHmmsss"));
            request.ResponseEnum = HttpResponseEnum.Stream;
            request.Cookie = SessionCookie;
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

                VerifyCode.Source = bmp;

                ms.Close();
            };

            HttpProvider.Excute(request);
        }

        /// <summary>
        /// 获取登录的Cookie
        /// </summary>
        void GetLoginSessionCookie()
        {
            var httpResponse = HttpProvider.Excute(new HttpRequestParameter()
            {
                Url = "http://passport.jikexueyuan.com/sso/login",
                IsPost = false
            });

            SessionCookie = httpResponse.Cookie;
        }
    }
}
