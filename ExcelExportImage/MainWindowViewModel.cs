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

        public MainWindowViewModel()
        {
        }
    }
}