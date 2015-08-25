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
using MahApps.Metro.Controls;
using Microsoft.Practices.ServiceLocation;

namespace Toggl2Jira
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.pwdBox.Password = ServiceLocator.Current.GetInstance<Toggl2Jira.ViewModel.MainViewModel>().Settings.JiraPassword;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ServiceLocator.Current.GetInstance<Toggl2Jira.ViewModel.MainViewModel>().Settings.JiraPassword = this.pwdBox.Password;
        }
    }
}
