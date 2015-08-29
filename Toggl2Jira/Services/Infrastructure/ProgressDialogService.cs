using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using MahApps.Metro.Controls.Dialogs;

namespace Toggl2Jira.Services
{
    public interface IProgressDialogService
    {
        Task<ProgressDialogController> ShowProgressUnknown(object context, string message, string title);
    }

    public class MahProgressDialogService : IProgressDialogService
    {
        public Task<ProgressDialogController> ShowProgressUnknown(object context, string title, string message)
        {
            return DialogCoordinator.Instance.ShowProgressAsync(context, title, message);
        }
    }
}
