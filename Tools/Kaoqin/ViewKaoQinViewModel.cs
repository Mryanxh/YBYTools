using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AlphaYanTools.Kaoqin
{
    internal class ViewKaoQinViewModel:MyBindingBase
    {
        private List<MonthComb> monthSource;
        public List<MonthComb> MonthSource
        {
            get { return monthSource; }
            set
            {
                monthSource = value;
                OnPropertyChanged("MonthSource");
            }
        }
        private MonthComb monthSelected;
        public MonthComb MonthSelected
        {
            get { return monthSelected; }
            set
            {
                monthSelected = value;
                OnPropertyChanged("MonthSelected");
            }
        }
        public ViewKaoQinViewModel()
        {
            InitMonth();
        }
        private void InitMonth()
        {
            MonthSource = new List<MonthComb>();
            for (int i = 1; i <= 12; i++)
            {
                MonthSource.Add(new MonthComb() { Index = i, Month = $"{i}", Name = $"{i}月" });
            }
            MonthSelected = MonthSource[DateTime.Now.Month - 1];
        }
    }
}