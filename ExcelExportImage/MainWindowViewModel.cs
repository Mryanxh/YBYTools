namespace AlphaYanTools
{
    internal class MainWindowViewModel : MyBindingBase
    {
        private string title { get; set; }
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }
        private string versionStr { get; set; }
        public string VersionStr
        {
            get { return versionStr; }
            set
            {
                versionStr = value;
                OnPropertyChanged("VersionStr");
            }
        }
        private string programName { get; set; }
        public string ProgramName
        {
            get { return programName; }
            set
            {
                programName = value;
                OnPropertyChanged("ProgramName");
            }
        }




        public MainWindowViewModel()
        {
            Title = "AlphaYan's Tools";
            VersionStr = GetVersion();
            ProgramName = GetProgramName();
        }

        private string GetVersion()
        {
            return "1.0.0.0";
        }

        private string GetProgramName()
        {
            return "AlphaYanTools";
        }
    }
}