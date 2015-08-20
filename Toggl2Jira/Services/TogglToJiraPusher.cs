using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toggl2Jira.Services
{
    public interface ITimePusher
    {
        void PushTime(DateTime startDate, DateTime endDate);
    }

    public class TogglToJiraPusher : ITimePusher
    {
        public void PushTime(DateTime startDate, DateTime endDate)
        {
            //Atlassian.Jira.Jira aa = new Atlassian.Jira.Jira("urlhere", "loginhere", "passwordhere");
            //var issue = aa.GetIssue("FLW6-2247");
            //issue.AddWorklog(new Atlassian.Jira.Worklog)

            var apiKey = "apikeyhere";
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
