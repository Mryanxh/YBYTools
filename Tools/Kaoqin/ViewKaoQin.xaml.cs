using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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

namespace AlphaYanTools.Kaoqin
{
    /// <summary>
    /// ViewKaoQin.xaml 的交互逻辑
    /// </summary>
    public partial class ViewKaoQin : UserControl
    {
        public event Action<string> LogPrint;

        private readonly string BaseUrl = "http://222.187.120.186:8288/selfservice/att/get_data_info/?";
        private readonly string HolidayFileName = "holiday.txt";
        ViewKaoQinViewModel VModel;
        public ViewKaoQin()
        {
            InitializeComponent();
            VModel = new ViewKaoQinViewModel();
            this.DataContext = VModel;
            txt_cookie.Text = "csrftoken=VS6rzoYY5oFEV6i6oK7eIvnRVaY1wLFvyMJIgdT62RdKwp9qjKyzW8lqEwUHUt0E; sessionid=bkatvojhl29acpvrcudpdmfkg4hitvyg";
        }
        
        private void btn_check_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txt_cookie.Text))
            {
                MessageBox.Show("请输入Cookie");
                return;
            }
            else
            {
                LoadKaoQin(DateTime.Now.Month);
            }
        }
        private void LoadKaoQin(int month)
        {
            DateTime StartTime = new DateTime(DateTime.Now.Year, month, DateTime.Now.Day).AddDays(-DateTime.Now.Day + 1);
            DateTime EndTime = new DateTime(DateTime.Now.Year, month, DateTime.Now.Day).AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day);
            string startTime = StartTime.ToString("yyyy-MM-dd");
            string endTime = EndTime.ToString("yyyy-MM-dd");
            
            if (txt_cookie.Text != null)
            {
                string Url = $"{BaseUrl}info_type=att_data_form&page=1&limit=100&st={startTime}&et={endTime}";
                string result = HttpPost(Url, txt_cookie.Text);
                if (result != null && !string.IsNullOrEmpty(result))
                {
                    ResultModel resultModel = JsonConvert.DeserializeObject<ResultModel>(result);
                    List<DataModel> Source = resultModel.Data;
                    List<HolidayModel> Holidays = null;
                    if (File.Exists(HolidayFileName))
                    {
                        Holidays = JsonConvert.DeserializeObject<List<HolidayModel>>(File.ReadAllText(HolidayFileName));
                    }
                }
                else
                {
                    MessageBox.Show("读取失败");
                }
            }
        }
        private void LoadDate(int month)
        {
            DateTime Month = new DateTime(DateTime.Now.Year, month, DateTime.Now.Day);
            int days = (Month.AddMonths(1).AddDays(-Month.AddMonths(1).Day) - Month.AddDays(-Month.Day)).Days;
            int indexday = 0;
            int indexrow = 0;
            int indexcol = (int)Month.AddDays(-Month.Day).DayOfWeek;
            do
            {
                DayControl day = new DayControl
                {
                    Day = Month.AddDays(-Month.Day + indexday + 1).ToString("yyyy-MM-dd")
                };
                Grid.SetRow(day, indexrow);
                Grid.SetColumn(day, indexcol++);
                grid_Center.Children.Add(day);
                if (indexcol == 7)
                {
                    indexrow++;
                    indexcol = 0;
                }
                indexday++;
            } while (indexday < days);

            //if (source.Count > 0)
            //{
            //    txt_name.Text = source[0].Username;
            //    txt_id.Text = source[0].Pin;
            //}
            
            
            //int indexrow = 0;
            //for (int i = 0; i < days; i++)
            //{
                
            //    if (startindex == 7)
            //    {
            //        startindex = 0;
            //    }
            //    List<DataModel> kaoqin = source.FindAll(s => s.TTime.Day == DateTime.Now.AddDays(-DateTime.Now.Day + i + 1).Day);
            //    if (kaoqin.Count > 0)
            //    {
            //        List<DateTime> dksj = new List<DateTime>();
            //        foreach (DataModel dm in kaoqin)
            //        {
            //            dksj.Add(dm.TTime);
            //        }
            //        day.SetDK(dksj);
            //    }
                
                
            //    if (DateTime.Now.AddDays(i).DayOfWeek == DayOfWeek.Saturday)
            //    {
            //        indexrow++;
            //    }
            //    startindex++;
            //}
        }

        string HttpPost(string posturl, string cookie)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(posturl) as HttpWebRequest;
                request.Method = HttpMethod.Get.Method;
                request.Headers.Add("Cookie", cookie);
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/102.0.5005.63 Safari/537.36 Edg/102.0.1245.30";
                request.Referer = "http://222.187.120.186:8288/selfservice/att/att_data_form/";
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream instream = response.GetResponseStream();
                StreamReader sr = new StreamReader(instream);
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                LogPrint(ex.Message);
                return string.Empty;
            }
        }
        private void com_month_Selected(object sender, RoutedEventArgs e)
        {
            LoadDate(com_month.SelectedIndex + 1);
        }
    }

    class MonthComb
    {
        public int Index { get; set; }

        public string Month { get; set; }

        public string Name { get; set; }
    }
}
