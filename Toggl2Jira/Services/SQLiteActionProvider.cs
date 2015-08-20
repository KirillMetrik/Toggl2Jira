using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Toggl2Jira.Services
{
    public class SQLiteActionProvider
    {
        private static string connString;

        static SQLiteActionProvider()
        {
            connString = string.Format(@"data source={0};useutf16encoding=True", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.sqlite"));
            EnsureDbReady();
        }

        public void DoDb(Action<IDbConnection> action)
        {
            using (var connection = new SQLiteConnection(connString))
            {
                connection.Open();
                action(connection);
            }
        }

        private static void EnsureDbReady()
        {
            SQLiteActionProvider sqlite = new SQLiteActionProvider();
            sqlite.DoDb(conn =>
                {
                    conn.Execute(@"CREATE TABLE IF NOT EXISTS AppSetting (Id INTEGER, JiraLogin TEXT, JiraPassword TEXT, JiraUrl TEXT, TogglApiKey TEXT, PRIMARY KEY (Id))");
                });
        }
    }
}
