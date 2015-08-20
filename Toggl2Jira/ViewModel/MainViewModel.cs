using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Toggl2Jira.Model;
using Toggl2Jira.Services;

namespace Toggl2Jira.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private IAppSettingsService stgsService;
        private AppSetting settings;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IAppSettingsService stgsService)
        {
            this.stgsService = stgsService;
            this.ReadSettings();

            this.ClosingCommand = new RelayCommand(() =>
            {
                this.stgsService.SaveSettings(this.settings);
            });

            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        public AppSetting Settings
        {
            get { return this.settings; }
            set
            {
                this.settings = value;
                this.RaisePropertyChanged("Settings");
            }
        }

        public RelayCommand ClosingCommand { get; set; }

        private void ReadSettings()
        {
            this.Settings = this.stgsService.ReadSettings();
        }
    }
}