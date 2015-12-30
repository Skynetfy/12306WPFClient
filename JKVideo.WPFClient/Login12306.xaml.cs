using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using JKVideo.Core.Enums;
using JKVideo.Core.Http;
using JKVideo.WPFClient.CoomonCore;
using Newtonsoft.Json;
using _12306.Domain.Entities;

namespace JKVideo.WPFClient
{
    /// <summary>
    /// Interaction logic for Login12306.xaml
    /// </summary>
    public partial class Login12306 : Window
    {
        /// <summary>
        /// Http请求者
        /// </summary>
        protected IHttpProvider HttpProvider { get; private set; }
        /// <summary>
        /// 会话Cookie
        /// </summary>
        protected HttpCookieType SessionCookie { get; private set; }

        public Login12306()
        {
            HttpProvider = new HttpProvider();
            InitializeComponent();

            if (RandCodeCookieHelper.CheckUserLogin(CookieStoreInstance.CurrentCookie))
                this.Close();
            ShowCode();
        }

        protected void btn_Click(object sender, RoutedEventArgs e)
        {
            var userName = UserName.Text.Trim();
            var passWord = PassWord.Password.Trim();
            var code = string.Empty;
            if (RandCodeImage1.IsChecked == true)
                code += "32,53";
            if (RandCodeImage2.IsChecked == true)
                code += ",103,45";
            if (RandCodeImage3.IsChecked == true)
                code += ",178,55";
            if (RandCodeImage4.IsChecked == true)
                code += ",243,44";
            if (RandCodeImage5.IsChecked == true)
                code += ",45,113";
            if (RandCodeImage6.IsChecked == true)
                code += ",107,114";
            if (RandCodeImage7.IsChecked == true)
                code += ",172,98";
            if (RandCodeImage8.IsChecked == true)
                code += ",249,117";
            code = code.Trim(',');
            CheckRandCode(code);

            var dicPostData = new Dictionary<string, string>();

            dicPostData.Add("loginUserDTO.user_name", userName);
            dicPostData.Add("userDTO.password", passWord);
            dicPostData.Add("randCode", code);

            HttpResponseParameter responseParameter = HttpProvider.Excute(new HttpRequestParameter
            {
                Url = "https://kyfw.12306.cn/otn/login/loginAysnSuggest",
                IsPost = true,
                Parameters = dicPostData,
                Cookie = SessionCookie
            });

            HttpJsonResultEntity<UserLoginResult> loginResut = JsonConvert.DeserializeObject<HttpJsonResultEntity<UserLoginResult>>(responseParameter.Body);
            SessionCookie.CookieCollection.Add(responseParameter.Cookie.CookieCollection);
            SessionCookie.CookieString = SessionCookie.CookieString + "," +
                                                             responseParameter.Cookie.CookieString;
            CookieStoreInstance.CurrentCookie = SessionCookie;
            this.Close();
            //PostUserLogin();
            //CheckUserLogin();

            //var dicPostData2 = new Dictionary<string, string>();

            //dicPostData.Add("_json_att", "");
            //dicPostData.Add("REPEAT_SUBMIT_TOKEN", "b4ed83a3647be9dfb24f3fbfe8f351b5");

            //HttpResponseParameter responseParameter2 = HttpProvider.Excute(new HttpRequestParameter
            //{
            //    Url = "https://kyfw.12306.cn/otn/confirmPassenger/getPassengerDTOs",
            //    IsPost = true,
            //    Parameters = dicPostData2,
            //    Cookie = SessionCookie
            //});

            //HttpJsonResultEntity<QueryPassengerResult> passengerResult = JsonConvert.DeserializeObject<HttpJsonResultEntity<QueryPassengerResult>>(responseParameter2.Body);
            //LoginResultEntity loginResult = responseParameter.Body.DeserializeObject<LoginResultEntity>();

            //if (loginResult.status == 1)
            //{
            //    // 2.登录成功，保存cookie
            //    // CookieStoreInstance.CurrentCookie = responseParameter.Cookie;


        }

        void CheckUserLogin()
        {
            var dicPostData2 = new Dictionary<string, string>();
            dicPostData2.Add("_json_att", "");
            HttpResponseParameter responseParameter2 = HttpProvider.Excute(new HttpRequestParameter
            {
                Url = "https://kyfw.12306.cn/otn/login/checkUser",
                IsPost = true,
                Parameters = dicPostData2,
                Cookie = SessionCookie
            });
        }

        void PostUserLogin()
        {
            var dicPostData2 = new Dictionary<string, string>();
            dicPostData2.Add("_json_att", "");
            HttpResponseParameter responseParameter2 = HttpProvider.Excute(new HttpRequestParameter
            {
                Url = "https://kyfw.12306.cn/otn/login/userLogin",
                IsPost = true,
                Parameters = dicPostData2,
                Cookie = CookieStoreInstance.CurrentCookie
            });
        }

        void CheckRandCode(string randCode)
        {
            var dicPostData = new Dictionary<string, string>();
            dicPostData.Add("rand", "sjrand");
            dicPostData.Add("randCode", randCode);

            HttpResponseParameter responseParameter = HttpProvider.Excute(new HttpRequestParameter
            {
                Url = "https://kyfw.12306.cn/otn/passcodeNew/checkRandCodeAnsyn",
                IsPost = true,
                Parameters = dicPostData,
                Cookie = SessionCookie
            });
        }

        void ShowCode()
        {
            GetLoginSessionCookie();

            var request = new HttpRequestParameter();
            request.Url = string.Format("https://kyfw.12306.cn/otn/passcodeNew/getPassCodeNew?module=login&rand=sjrand&{0}",
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

                viefCode.Source = bmp;

                ms.Close();
            };

            var response = HttpProvider.Excute(request);

            SessionCookie.CookieCollection.Add(response.Cookie.CookieCollection);
            SessionCookie.CookieString = SessionCookie.CookieString + "," + response.Cookie.CookieString;
        }

        /// <summary>
        /// 获取登录的Cookie
        /// </summary>
        void GetLoginSessionCookie()
        {
            var httpResponse = HttpProvider.Excute(new HttpRequestParameter()
            {
                Url = "https://kyfw.12306.cn/otn/login/init",
                IsPost = false
            });

            SessionCookie = httpResponse.Cookie;
        }
    }
}
