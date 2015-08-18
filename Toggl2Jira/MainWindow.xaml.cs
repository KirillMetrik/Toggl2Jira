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

namespace Toggl2Jira
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Atlassian.Jira.Jira aa = new Atlassian.Jira.Jira("https://jira.radium.cz", "metrik", "DHpon277");
            //var issue = aa.GetIssue("FLW6-2247");
            //issue.AddWorklog(new Atlassian.Jira.Worklog)

            var apiKey = "5259a998358460feb2e6e78296f8f61a";
            var t = new Toggl.Toggl(apiKey);
            var c = t.User.GetCurrent();

            var timeSrv = new Toggl.Services.TimeEntryService(apiKey);
            var prams = new Toggl.QueryObjects.TimeEntryParams();

            // there is an issue with the date ranges have
            // to step out of the range on the end.
            // To capture the end of the billing range day + 1
            prams.StartDate = DateTime.Now.AddMonths(-1);
            prams.EndDate = DateTime.Now.AddMonths(1);

            var hours = timeSrv.List(prams)
                                    .Where(w => !string.IsNullOrEmpty(w.Description)).ToList();

        }
    }
}
