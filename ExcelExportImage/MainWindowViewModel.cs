namespace AlphaYanTools
{
    internal class MainWindowViewModel : MyBindingBase
    {
        public string Title
        {
            get { return $"{ProgramName}-{VersionStr}"; }
        }
        public string VersionStr
        {
            get { return AlphaUtil.AppVersion.ToString(); }
        }
        public string ProgramName
        {
            get { return AlphaUtil.AppName; }
        }

        private string viewMessage;
        public string ViewMessage
        {
            get { return viewMessage; }
            set
            {
                viewMessage = value;
                OnPropertyChanged(nameof(ViewMessage));
            }
        }
        public MainWindowViewModel()
        {
            ViewMessage = "Hello World!";
        }
    }
}