using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using JKVideo.Core.Http;
using JKVideo.WPFClient.CoomonCore;
using Newtonsoft.Json;
using _12306.Domain.Entities;

namespace JKVideo.WPFClient
{
    /// <summary>
    /// Interaction logic for PassengersWindow.xaml
    /// </summary>
    public partial class PassengersWindow : Window
    {
        /// <summary>
        /// Http请求者
        /// </summary>
        protected IHttpProvider HttpProvider { get; private set; }

        public PassengersWindow()
        {
            HttpProvider = new HttpProvider();
            CheckLogin();
            InitializeComponent();
            Init();
        }

        void Init()
        {
            var dicPostData = new Dictionary<string, string>();

            dicPostData.Add("_json_att", "");
            dicPostData.Add("REPEAT_SUBMIT_TOKEN", CookieStoreInstance.Repeat_Submit_Token);

            HttpResponseParameter responseParameter2 = HttpProvider.Excute(new HttpRequestParameter
            {
                Url = "https://kyfw.12306.cn/otn/confirmPassenger/getPassengerDTOs",
                IsPost = true,
                Parameters = dicPostData,
                Cookie = CookieStoreInstance.CurrentCookie
            });

            HttpJsonResultEntity<QueryPassengerResult> passengerResult = JsonConvert.DeserializeObject<HttpJsonResultEntity<QueryPassengerResult>>(responseParameter2.Body);

            var list = passengerResult.Data.normal_passengers;
            var _list = new ObservableCollection<Passengers>();
            foreach (var item in list)
            {
                _list.Add(item);
            }

            DataGrid1.ItemsSource = _list;
        }

        void CheckLogin()
        {
            if (!RandCodeCookieHelper.CheckUserLogin(CookieStoreInstance.CurrentCookie))
            {
                Login12306 login = new Login12306();
                login.ShowDialog();
            }
        }
    }
}
