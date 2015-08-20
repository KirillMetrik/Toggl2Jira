using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toggl2Jira.Model;
using Dapper;

namespace Toggl2Jira.Services
{
    public interface IAppSettingsService
    {
        AppSetting ReadSettings();

        void SaveSettings(AppSetting settings);
    }

    public class AppSettingsService : IAppSettingsService
    {
        public AppSetting ReadSettings()
        {
            var sqlite = new SQLiteActionProvider();
            AppSetting result = null;

            sqlite.DoDb(conn =>
            {
                result = conn.Query<AppSetting>("select * from AppSetting").FirstOrDefault();
            });
            return result ?? new AppSetting();
        }

        public void SaveSettings(AppSetting settings)
        {
            if (settings == null)
                return;

            var sqlite = new SQLiteActionProvider();
            sqlite.DoDb(conn =>
                {
                    conn.Execute("insert or replace into AppSetting (Id, JiraLogin, JiraPassword, JiraUrl, TogglApiKey) values(@Id, @jlogin, @jpwd, @jurl, @toggl)",
                        new { Id = 1, jlogin = settings.JiraLogin, jpwd = settings.JiraPassword, jurl = settings.JiraUrl, toggl = settings.TogglApiKey });
                });
        }
    }
}
