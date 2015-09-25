using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MahApps.Metro.Controls.Dialogs;
using Toggl2Jira.Model;
using Toggl2Jira.Services;

namespace Toggl2Jira.ViewModel
{
    /// <summary>
    /// See KindOfMagic about these attributes.
    /// </summary>
    class MagicAttribute : Attribute { }
    class NoMagicAttribute : Attribute { }

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
        private IProgressDialogService progressDialogService;
        private IMessageDlgService messageDlgService;
        private IAppSettingsService stgsService;
        private ITimePusher timePusherService;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IMessageDlgService messageDlgService, 
            IProgressDialogService progressDialogService, IAppSettingsService stgsService, ITimePusher timePusherService)
        {
            this.progressDialogService = progressDialogService;
            this.messageDlgService = messageDlgService;
            this.stgsService = stgsService;
            this.timePusherService = timePusherService;
            this.Settings = this.stgsService.ReadSettings();

            this.PostTimeEntries = new RelayCommand(async () =>
            {
                await this.DoOp(() => this.timePusherService.PushTime(this.Settings, DateTime.Now.AddDays(-7).Date, DateTime.Now.AddDays(1).Date), "posting logged time for today", "Time entries for today have been successfully posted.");
            });

            this.ClosingCommand = new RelayCommand(() =>
            {
                this.stgsService.SaveSettings(this.Settings);
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

        [Magic]
        public bool IsProcessing { get; set; }

        [Magic]
        public AppSetting Settings { get; set; }

        public RelayCommand ClosingCommand { get; set; }

        public RelayCommand PostTimeEntries { get; set; }

        private async Task DoOp(Func<Task> func, string operationDescription = null, string successMessage = null)
        {
            ExceptionDispatchInfo error = null; // this allows to rethrow exception so that stack trace was saved
            var progress = await this.progressDialogService.ShowProgressUnknown(this, "Please wait...", string.IsNullOrEmpty(operationDescription) ? "working" : operationDescription);
            try
            {
                await func();
            }
            catch (Exception e)
            {
                error = ExceptionDispatchInfo.Capture(e);
            }

            await progress.CloseAsync();
            if (error != null)
                await this.messageDlgService.ShowError(this, "Oops!", error.SourceException);
            else if (!string.IsNullOrEmpty(successMessage))
                await this.messageDlgService.ShowMessage(this, "Finished", successMessage);
        }
    }
}