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
using JKVideo.Core.Enums;
using JKVideo.Core.Http;

namespace JKVideo.WPFClient
{
    /// <summary>
    /// Interaction logic for YuDingWindow.xaml
    /// </summary>
    public partial class YuDingWindow : Window
    {
        private string _trainCode;
        /// <summary>
        /// Http请求者
        /// </summary>
        protected IHttpProvider httpProvider { get; private set; }
        /// <summary>
        /// 会话Cookie
        /// </summary>
        protected HttpCookieType SessionCookie { get; private set; }

        public YuDingWindow()
        {
            InitializeComponent();
            httpProvider = new HttpProvider();
            GetLoginSessionCookie();
            ShowLianXiRen();
            
        }

        public YuDingWindow(string tranCode) : this()
        {
            _trainCode = tranCode.Trim(',');
            trainCodeName.Content = _trainCode;
        }

        void ShowLianXiRen()
        {
            var dicPostData = new Dictionary<string, string>();

            dicPostData.Add("pageIndex", "1");
            dicPostData.Add("pageSize", "20");

            HttpResponseParameter responseParameter = httpProvider.Excute(new HttpRequestParameter
            {
                Url = "https://kyfw.12306.cn/otn/passengers/query",
                IsPost = true,
                Parameters = dicPostData,
                Cookie = SessionCookie
            });
        }

        protected void Yd_Click(object sender, RoutedEventArgs e)
        {

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

                randCodeImage.Source = bmp;

                ms.Close();
            };

            httpProvider.Excute(request);
        }
        /// <summary>
        /// 获取登录的Cookie
        /// </summary>
        void GetLoginSessionCookie()
        {
            var httpResponse = httpProvider.Excute(new HttpRequestParameter()
            {
                Url = "https://kyfw.12306.cn/otn/passengers/init",
                IsPost = false
            });

            SessionCookie = httpResponse.Cookie;
        }
    }
}
