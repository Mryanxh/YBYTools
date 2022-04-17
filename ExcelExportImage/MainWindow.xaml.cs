using System.Diagnostics;
using System.IO;
using System.Windows;

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

        private void excelimg_Click(object sender, RoutedEventArgs e)
        {
            centerGrid.Children.Clear();
            centerGrid.Children.Add(new ExportExcelImg());
        }

        private void pdfimg_Click(object sender, RoutedEventArgs e)
        {
            centerGrid.Children.Clear();
            centerGrid.Children.Add(new ExportPdfImg());
        }

        private void CheckUpdate_Click(object sender, RoutedEventArgs e)
        {
            string filepath = $"{Directory.GetCurrentDirectory()}\\Update.exe";
            Process.Start(filepath, $"{viewModel.ProgramName} {viewModel.VersionStr}");
            this.Close();
        }
        public void SimpleInvoke()
        {

        }

    }
}