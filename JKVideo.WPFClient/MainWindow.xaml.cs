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
using System.Windows.Navigation;
using System.Windows.Shapes;
using JKVideo.Core.Common;
using JKVideo.Core.Http;
using Newtonsoft.Json;
using _12306.Domain.Entities;

namespace JKVideo.WPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Http请求者
        /// </summary>
        protected IHttpProvider httpProvider { get; private set; }

        /// <summary>
        /// 查询票数
        /// </summary>
        protected List<QeryTicketsData> TicketsDataList;

        public MainWindow()
        {
            httpProvider = new HttpProvider();
            CookieStoreInstance.Repeat_Submit_Token = Guid.NewGuid().ToString("N");
            InitializeComponent();
            BindingData();
        }

        private void BindingData()
        {
            var url =
                string.Format(
                    "https://kyfw.12306.cn/otn/leftTicket/queryT?leftTicketDTO.train_date={0}&leftTicketDTO.from_station={1}&leftTicketDTO.to_station={2}&purpose_codes={3}", "2016-02-04", "SHH", "FYH", "ADULT");
            HttpResponseParameter responseParameter = httpProvider.Excute(new HttpRequestParameter
            {
                Url = url,
                IsPost = false
            });

            HttpJsonResult<QeryTicketsData> loginResult = JsonConvert.DeserializeObject<HttpJsonResult<QeryTicketsData>>(responseParameter.Body);
            var list = loginResult.Data.Select(x => x.QueryLeftNewDTO).ToList();
            TicketsDataList = loginResult.Data.ToList();

            var _list = new ObservableCollection<QueryLeftNewDTO>();
            foreach (var item in list)
            {
                _list.Add(item);
            }
            DataGrid1.ItemsSource = _list;
        }

        protected void btn_OnClick(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.ShowDialog();
        }

        protected void btn_12306Click(object sender, RoutedEventArgs e)
        {
            Login12306 login = new Login12306();
            login.ShowDialog();
        }

        void CheckOrderInfo()
        {
            var postDicData = new Dictionary<string, string>();
            postDicData.Add("cancel_flag", "2");
            postDicData.Add("bed_level_order_num", "000000000000000000000000000000");
            postDicData.Add("passengerTicketStr", "1,0,1,张志强,1,341204198809062655,15221848167,N");
            postDicData.Add("oldPassengerStr", "张志强,1,341204198809062655,1_");
            postDicData.Add("tour_flag", "dc");
            postDicData.Add("randCode", "110,111.95999145507812,103,63.959999084472656");
            postDicData.Add("_json_att", "");
            postDicData.Add("REPEAT_SUBMIT_TOKEN", CookieStoreInstance.Repeat_Submit_Token);

            var url = "https://kyfw.12306.cn/otn/confirmPassenger/checkOrderInfo";

            var method = "POST";
        }

        void CheckSubmitOrderRequest()
        {
            var secretStr =
                TicketsDataList.Where(x => x.QueryLeftNewDTO.station_train_code.Equals("K5566"))
                    .Select(x => x.QueryLeftNewDTO.station_train_code).FirstOrDefault();
            var postDicData = new Dictionary<string, string>();
            postDicData.Add("secretStr", secretStr);
            postDicData.Add("train_date", "2016-02-04");
            postDicData.Add("back_train_date", "2015-12-30");
            postDicData.Add("tour_flag", "dc");
            postDicData.Add("purpose_codes", "ADULT");
            postDicData.Add("query_from_station_name", "上海");
            postDicData.Add("query_to_station_name", "阜阳");

            var url = "https://kyfw.12306.cn/otn/leftTicket/submitOrderRequest";

            var method = "POST";
        }

        void GetQueueCount()
        {
            var postDicData = new Dictionary<string, string>();
            postDicData.Add("train_date", "Thu Feb 04 2016 00:00:00 GMT+0800 (中国标准时间)");
            postDicData.Add("train_no", "55000K556610");
            postDicData.Add("stationTrainCode", "K5566");
            postDicData.Add("seatType", "1");
            postDicData.Add("fromStationTelecode", "SHH");
            postDicData.Add("toStationTelecode", "FYH");
            postDicData.Add("leftTicket", "10093037711009300000");
            postDicData.Add("purpose_codes", "00");
            postDicData.Add("_json_att", "");
            postDicData.Add("REPEAT_SUBMIT_TOKEN", CookieStoreInstance.Repeat_Submit_Token);

            var url = "https://kyfw.12306.cn/otn/confirmPassenger/getQueueCount";

            var method = "POST";
        }

        void ConfirmSingForQueue()
        {
            var postDicData = new Dictionary<string, string>();
            postDicData.Add("passengerTicketStr", "1,0,1,张志强,1,341204198809062655,15221848167,N");
            postDicData.Add("oldPassengerStr", "张志强,1,341204198809062655,1_");
            postDicData.Add("randCode", "110,111.95999145507812,103,63.959999084472656");
            postDicData.Add("purpose_codes", "00");
            postDicData.Add("key_check_isChange", "B507D2AC2DB90E6B870FE0C7680B03A4B40CEEEDB16D4C08ACD9E20F");
            postDicData.Add("leftTicketStr", "10093037711009300000");
            postDicData.Add("train_location", "H2");
            postDicData.Add("roomType", "00");
            postDicData.Add("dwAll", "N");
            postDicData.Add("_json_att", "");
            postDicData.Add("REPEAT_SUBMIT_TOKEN", CookieStoreInstance.Repeat_Submit_Token);

            var url = "https://kyfw.12306.cn/otn/confirmPassenger/confirmSingleForQueue";

            var method = "POST";
        }

        protected void btn_YDClick(object sender, RoutedEventArgs e)
        {
            var trainCode = string.Empty;

            for (var i = 0; i < DataGrid1.Items.Count; i++)
            {
                var cntr = DataGrid1.ItemContainerGenerator.ContainerFromIndex(i);
                DataGridRow ObjROw = (DataGridRow)cntr;
                if (ObjROw != null)
                {
                    FrameworkElement objElement = DataGrid1.Columns[0].GetCellContent(ObjROw);

                    if (objElement != null)
                    {
                        QueryLeftNewDTO entity = (QueryLeftNewDTO)objElement.DataContext;
                        CheckBox objChk = (CheckBox)objElement;
                        if (objChk.IsChecked == true)
                        {
                            trainCode += "," + entity.station_train_code;
                        }
                    }
                }
            }
            YuDingWindow yuDing = new YuDingWindow(trainCode);
            yuDing.ShowDialog();
        }

        protected void JiKeLogin_Click(object sender, RoutedEventArgs e)
        {
            JiKeWindow jikewindow = new JiKeWindow();
            jikewindow.Show();
        }

        protected void Lianxiren_Click(object sender, RoutedEventArgs e)
        {
            PassengersWindow window = new PassengersWindow();
            window.Show();
        }
    }
}
