using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toggl2Jira.Model
{
    public class AppSetting
    {
        public string TogglApiKey { get; set; }

        public string JiraUrl { get; set; }

        public string JiraLogin { get; set; }

        public string JiraPassword { get; set; }
    }
}
