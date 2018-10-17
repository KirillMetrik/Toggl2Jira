using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Atlassian.Jira;
using Toggl.QueryObjects;
using Toggl.Services;
using Toggl2Jira.Model;

namespace Toggl2Jira.Services
{
    public interface ITimePusher
    {
        Task PushTime(AppSetting settings, DateTime startDate, DateTime endDate);
    }

    public class TogglToJiraPusher : ITimePusher
    {
        private const string POSTED_TAG = "t2j:posted";

        public async Task PushTime(AppSetting settings, DateTime startDate, DateTime endDate)
        {
            //tra-74

            if (settings == null)
                throw new Exception("No Jira or Toggl credentials provided.");
            this.VerifySetting(settings.JiraLogin, "JIRA login");
            this.VerifySetting(settings.JiraPassword, "JIRA password");
            this.VerifySetting(settings.JiraUrl, "JIRA URL");
            this.VerifySetting(settings.TogglApiKey, "Toggl API Key");

            var jira = Jira.CreateRestClient(settings.JiraUrl, settings.JiraLogin, settings.JiraPassword);
            var toggl = new Toggl.Toggl(settings.TogglApiKey);

            await Task.Run(async () =>
                {
                    var timeService = new TimeEntryService(settings.TogglApiKey);
                    var timeParams = new TimeEntryParams();

                    timeParams.StartDate = startDate.Date;
                    timeParams.EndDate = endDate.Date;

                    foreach (var te in timeService.List(timeParams).Where(w => (w.TagNames == null || !w.TagNames.Contains(POSTED_TAG))
                                                                                && !string.IsNullOrEmpty(w.Description)))
                    {
                        //Debug.WriteLine(te.Start);
                        //Debug.WriteLine(DateTime.Parse(te.Start));
                        KeyValuePair<string, string> description = this.ParseDescription(te.Description);
                        if (string.IsNullOrEmpty(description.Key))
                            continue;

                        var issue = await jira.Issues.GetIssueAsync(description.Key);
                        await issue.AddWorklogAsync(new Worklog(this.GetMinutes(te.Duration.GetValueOrDefault()), DateTime.Parse(te.Start), description.Value));

                        if (te.TagNames == null)
                            te.TagNames = new List<string>();
                        te.TagNames.Add(POSTED_TAG);
                        timeService.Edit(te);
                    }
                });

            //return Task.Run(() =>
            //    {
            //        //Atlassian.Jira.Jira aa = new Atlassian.Jira.Jira("urlhere", "loginhere", "passwordhere");
            //        //var issue = aa.GetIssue("FLW6-2247");
            //        //issue.AddWorklog(new Atlassian.Jira.Worklog)

            //        //var apiKey = "apikeyhere";
            //        //var t = new Toggl.Toggl(apiKey);
            //        //var c = t.User.GetCurrent();

            //        //var timeSrv = new Toggl.Services.TimeEntryService(apiKey);
            //        //var prams = new Toggl.QueryObjects.TimeEntryParams();

            //        //// there is an issue with the date ranges have
            //        //// to step out of the range on the end.
            //        //// To capture the end of the billing range day + 1
            //        //prams.StartDate = DateTime.Now.AddMonths(-1);
            //        //prams.EndDate = DateTime.Now.AddMonths(1);

            //        //var hours = timeSrv.List(prams)
            //        //                        .Where(w => !string.IsNullOrEmpty(w.Description)).ToList();
            //    });
        }

        private void VerifySetting(string setting, string name)
        {
            if (string.IsNullOrEmpty(setting))
                throw new Exception(string.Format("The required credential '{0}' is empty.", name));
        }

        private KeyValuePair<string,string> ParseDescription(string description)
        {
            if (string.IsNullOrEmpty(description))
                return new KeyValuePair<string, string>();

            int index = description.IndexOfAny(new char[] { ' ', '\t' });
            if (index == -1)
                return new KeyValuePair<string, string>();

            string key = description.Substring(0, index).Trim();
            string value = index == description.Length ? "" : description.Substring(index + 1).Trim();
            return new KeyValuePair<string, string>(key, value);
        }

        private string GetMinutes(double seconds)
        {
            return Math.Round(seconds / 60f, 0).ToString() + "m";
        }
    }
}
