# Toggl2Jira
Pushes time logged with Toggl into Jira, automatically binds logged time to Jira issue.

This project allows to easily post time logged by you in Toggl application to Atlassian JIRA. The solution will look at the description of times logged in Toggl. It will always consider the first word (till whitespace) as a Jira issue ID, the rest of description will be considered as description itself.
Toggl2Jira will find all the issues in JIRA by their IDs fetched from Toggl time records and post work log for those issues (taken from Toggl time records).
