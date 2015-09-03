using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Views;
using MahApps.Metro.Controls.Dialogs;

namespace Toggl2Jira.Services
{
    public interface IMessageDlgService
    {
        Task<MessageDialogResult> ShowError(object context, string title, Exception error);

        Task<MessageDialogResult> ShowError(object context, string title, string message);

        Task<MessageDialogResult> ShowMessage(object context, string title, string message);
    }

    public class MessageDialogService : IMessageDlgService
    {
        public Task<MessageDialogResult> ShowError(object context, string title, Exception error)
        {
            return DialogCoordinator.Instance.ShowMessageAsync(context, "ERROR: " + title, error.ToString(), MessageDialogStyle.Affirmative);
        }

        public Task<MessageDialogResult> ShowError(object context, string title, string message)
        {
            return DialogCoordinator.Instance.ShowMessageAsync(context, "ERROR: " + title, message, MessageDialogStyle.Affirmative);
        }

        public Task<MessageDialogResult> ShowMessage(object context, string title, string message)
        {
            return DialogCoordinator.Instance.ShowMessageAsync(context, title, message, MessageDialogStyle.Affirmative);
        }
    }
}
