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
        private readonly string BaseUrl = "http://222.187.120.186:8288/selfservice/att/get_data_info/?";
        private readonly string HolidayFileName = "holiday.txt";
        string Cookie = String.Empty;

        public ViewKaoQin()
        {
            InitializeComponent();
        }

        private void SetCookie(string cookie)
        {
            this.Cookie = cookie;
        }
        private void LoadKaoQin(int month)
        {
            DateTime StartTime = new DateTime(DateTime.Now.Year, month, DateTime.Now.Day).AddDays(-DateTime.Now.Day + 1);
            DateTime EndTime = new DateTime(DateTime.Now.Year, month, DateTime.Now.Day).AddMonths(1).AddDays(-DateTime.Now.AddMonths(1).Day);
            string startTime = StartTime.ToString("yyyy-MM-dd");
            string endTime = EndTime.ToString("yyyy-MM-dd");
            
            if (Cookie != null)
            {
                string Url = $"{BaseUrl}info_type=att_data_form&page=1&limit=100&st={startTime}&et={endTime}";
                string result = HttpPost(Url, Cookie);
                if (result != null && !string.IsNullOrEmpty(result))
                {
                    ResultModel resultModel = JsonConvert.DeserializeObject<ResultModel>(result);
                    List<DataModel> Source = resultModel.Data;
                    List<HolidayModel> Holidays = null;
                    if (File.Exists(HolidayFileName))
                    {
                        Holidays = JsonConvert.DeserializeObject<List<HolidayModel>>(File.ReadAllText(HolidayFileName));
                    }
                    PrintMonth(StartTime, EndTime, Source, Holidays);
                }
            }
        }

        void PrintMonth(DateTime StartTime, DateTime EndTime, List<DataModel> Source, List<HolidayModel> Holiday)
        {
            for (DateTime dt = StartTime; dt < EndTime; dt = dt.AddDays(1))
            {
                if ((int)dt.DayOfWeek == 6 || (int)dt.DayOfWeek == 0)
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{dt:MM-dd}\t");
                HolidayModel holiday = Holiday.Find(s => s.Date.Date == dt.Date);
                if (null != holiday && holiday.Holiday)
                {
                    Console.Write(holiday.Name);
                }
                Console.Write("\t");
                List<DataModel> OneDay = Source.FindAll(s => s.TTime.Day == dt.Day);
                DataModel up = OneDay.Find(s => s.TTime.Hour < 12);
                if (null != up)
                {
                    if (up.TTime < new DateTime(up.TTime.Year, up.TTime.Month, up.TTime.Day, 9, 30, 00))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.Write(up.TTime.ToString("HH:mm:ss"));
                    Console.ResetColor();
                }
                Console.Write("-");
                DataModel down = OneDay.Find(s => s.TTime.Hour > 12);
                if (null != down)
                {
                    if (down.TTime > new DateTime(down.TTime.Year, down.TTime.Month, down.TTime.Day, 17, 00, 00))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.Write(down.TTime.ToString("HH:mm:ss"));
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }

        string HttpPost(string posturl, string cookie)
        {
            Encoding encoding = Encoding.GetEncoding("utf-8");
            // 准备请求...
            try
            {
                HttpWebRequest request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = HttpMethod.Post.Method;
                request.Headers.Add("Cookie", cookie);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream instream = response.GetResponseStream();
                StreamReader sr = new StreamReader(instream);
                string content = sr.ReadToEnd();
                string err = string.Empty;
                return content;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
                return string.Empty;
            }
        }

        string HttpGet(string Url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

    }
}
