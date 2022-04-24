using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace AlphaYanTools
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MainWindowViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainWindowViewModel();
            this.DataContext = viewModel;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menu)
            {
                switch (menu.Tag)
                {
                    case "ExportPDFImg":
                        ExportPDFImg(); 
                        break;
                    case "ExportExcelImg":
                        ExportExcelImg();
                        break;
                    case "WWWXZYBY":
                        OpenPrivateUrl("http://www.xzyby.com");
                        break;
                    case "FILESXZYBY":
                        OpenPrivateUrl("http://files.xzyby.com");
                        break;
                    case "CheckUpdate":
                        CheckUpdate();
                        break;
                    default:
                        MessageBox.Show($"{menu.Header}功能还没写呢");
                        break;
                }
            }
        }
        private void ExportExcelImg()
        {
            centerGrid.Children.Clear();
            centerGrid.Children.Add(new ExportExcelImg());
        }

        private void ExportPDFImg()
        {
            centerGrid.Children.Clear();
            centerGrid.Children.Add(new ExportPdfImg());
        }

        private void CheckUpdate()
        {
            string filepath = $"{Directory.GetCurrentDirectory()}\\Update.exe";
            Process.Start(filepath, $"{viewModel.ProgramName} {viewModel.VersionStr}");
            this.Close();
        }
        /// <summary>
        /// 打开内网URL
        /// </summary>
        /// <param name="url"></param>
        private void OpenPrivateUrl(string url)
        {
            //必须在园博园内网才能打开
            //后续可使用内网地址的API接口判断是否在内网
            if (AlphaUtil.GetIP().StartsWith("61.177.215."))
            {
                Process.Start(url);
            }
            else
            {
                MessageBox.Show("您必须在园博园内网才能够打开该网址");
            }
        }
    }
}