using System;
using System.Collections.Generic;
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

namespace AlphaYanTools.Kaoqin
{
    /// <summary>
    /// DayControl.xaml 的交互逻辑
    /// </summary>
    public partial class DayControl : UserControl
    {
        public string Day { get; set; }

        public DayControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public void SetDK(List<DateTime> source)
        {
            source = source.OrderBy(x => x.Hour).ToList();
            foreach (DateTime dk in source)
            {
                TextBlock txt = new TextBlock();
                txt.Text = dk.ToString("HH:mm:ss");
                dktime.Children.Add(txt);
            }
        }
    }
}
