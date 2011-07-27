using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using CA.SharePoint;
using CA.SharePoint.Utilities.Common;
using CodeArt.SharePoint.CamlQuery;
using Microsoft.SharePoint;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Specialized;
using System.Threading;
using System.Diagnostics;
using QuickFlow;

namespace CA.WorkFlow.UI
{
    public static class WorkFlowUtil
    {
        public static SPFieldUserValueCollection GetApproversValue()
        {
            SPListItem item = SPContext.Current.ListItem;

            SPFieldUserValueCollection col = item["Approvers"] as SPFieldUserValueCollection;

            if (col != null)
            {
                SPUser user = SPContext.Current.Web.CurrentUser;
                SPFieldUserValue value = new SPFieldUserValue(SPContext.Current.Site.RootWeb, user.ID, user.Name);

                bool IsExist = false;
                foreach (SPFieldUserValue v in col)
                {
                    if (v.LookupId == value.LookupId)
                    {
                        IsExist = true;
                        break;
                    }
                }
                if (!IsExist)
                {
                    col.Add(value);
                }
            }
            else
            {
                SPUser user = SPContext.Current.Web.CurrentUser;
                SPFieldUserValue value = new SPFieldUserValue(SPContext.Current.Site.RootWeb, user.ID, user.Name);
                col = new SPFieldUserValueCollection();
                col.Add(value);
            }

            return col;
        }

        public static int CreateWorkFlowNumber(string workflowName)
        {
            int nNum = 1;
            CA.SharePoint.ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList list = sps.GetList(CAWorkFlowConstants.ListName.WorkFlowNumber.ToString());

            QueryField field = new QueryField("Title");

            SPListItemCollection items = sps.Query(list, field.Equal(workflowName), 1, null);
            if (items != null && items.Count > 0)
            {
                SPListItem item = list.GetItemById(items[0].ID);
                nNum = Convert.ToInt32(items[0]["Number"]) + 1;
                item["Number"] = Convert.ToDouble(nNum);
                item.Web.AllowUnsafeUpdates = true;
                item.Update();
            }
            else
            {
                SPListItem item = list.Items.Add();
                item["WorkFlowName"] = workflowName;
                item["Number"] = nNum;
                item.Web.AllowUnsafeUpdates = true;
                item.Update();
            }

            return nNum;
        }

        public static Employee GetEmployeeApprover(Employee emp)
        {
            Employee approver = null;
            try
            {
                emp = UserProfileUtil.GetEmployee(emp.Manager);
                do
                {
                    if (emp.ApproveRight)
                    {
                        approver = emp;
                    }
                    else
                    {
                        emp = UserProfileUtil.GetEmployee(emp.Manager);
                    }
                }
                while ((approver == null) && (emp != null));
            }
            catch (Exception ex)
            {
                approver = null; ;
            }

            return approver;
        }

        public static void RemoveExistingRecord(SPList list, string key, string val)
        {
            CA.SharePoint.ISharePointService sps = CA.SharePoint.ServiceFactory.GetSharePointService(true);
            QueryField field = new QueryField(key, false);
            SPListItemCollection items = sps.Query(list, field.Equal(val), 0);

            for (int nIndex = items.Count - 1; nIndex >= 0; nIndex--)
            {
                items[nIndex].Web.AllowUnsafeUpdates = true;
                items[nIndex].Delete();
            }
        }

        //查找组中第一个用户
        public static string GetUserInGroup(string strGroupName)
        {
            SPGroup group = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        group = web.Groups[strGroupName];
                    }
                }
            });

            if (group != null)
            {
                for (int i = 0; i < group.Users.Count; i++)
                {
                    if (!group.Users[i].LoginName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
                    {
                        return group.Users[i].LoginName;
                    }
                }
            }

            return null;
        }

        public static QuickFlow.NameCollection GetUsersInGroup(string strGroupName)
        {
            QuickFlow.NameCollection names = new QuickFlow.NameCollection();
            SPGroup group = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                    {
                        group = web.Groups[strGroupName];
                    }
                }
            });

            if (group != null)
            {
                foreach (SPUser user in group.Users)
                {
                    if (user.IsSiteAdmin || user.Name == "System Account")
                        continue;
                    names.Add(user.LoginName);
                }
            }

            return names;
        }

        public static List<SPUser> GetSPUsersInGroup(string strGroupName)
        {
            List<SPUser> users = new List<SPUser>();
            SPGroup group = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                    {
                        group = web.Groups[strGroupName];
                    }
                }
            });

            if (group != null)
            {
                foreach (SPUser user in group.Users)
                {
                    if (user.IsSiteAdmin)
                        continue;
                    users.Add(user);
                }
            }

            return users;
        }

        //取出组中用户
        public static List<string> UserListInGroup(string strGroupName)
        {
            SPGroup group = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
                    {
                        group = web.SiteGroups[strGroupName];
                    }
                }
            });
            List<string> lst = new List<string>();
            if (group != null)
            {
                int len = group.Users.Count;
                for (int i = 0; i < len; i++)
                {
                    if (!group.Users[i].LoginName.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
                    {
                        lst.Add(group.Users[i].LoginName);
                    }
                }
            }
            return lst;
        }

        //查找组名
        public static SPGroup GetUserGroup(string strGroupName)
        {
            SPGroup group = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        group = web.Groups[strGroupName];
                    }
                }
            });

            if (group != null)
            {
                return group;
            }
            else
                return null;
        }

        public static string GetAttachmentFolder(string workflowName, string trackingNumber)
        {
            if (string.IsNullOrEmpty(trackingNumber))
            {
                return ConfigurationManager.AppSettings["attachmentLibraryList"] + "/" + workflowName + "/temp/";
            }
            else
            {
                return ConfigurationManager.AppSettings["attachmentLibraryList"] + "/" + workflowName + "/" + trackingNumber + "/";
            }
        }

        public static QuickFlow.NameCollection GetNewsApproveUsers(string listTitle)
        {
            QuickFlow.NameCollection names = new QuickFlow.NameCollection();
            try
            {
                QueryField field = new QueryField("Title");
                CA.SharePoint.ISharePointService sps = CA.SharePoint.ServiceFactory.GetSharePointService(true);
                SPList list = sps.GetList("NewsApproveConfig");
                SPListItemCollection items = sps.Query(list, field.Equal(listTitle), 1);

                if (items != null && items.Count > 0)
                {
                    SPFieldUserValueCollection users = items[0]["Approvers"] as SPFieldUserValueCollection;
                    foreach (SPFieldUserValue user in users)
                    {
                        string userLoginName = user.LookupValue;
                        names.Add(userLoginName);
                    }
                }
            }
            catch { }
            return names;
        }

        //在同部门找上级审批人
        public static Employee GetEmployeeApproverInDept(Employee emp, bool isFindmanager, bool isFQ)
        {
            Employee approver = null;
            Employee old = emp;
            try
            {
                do
                {
                    if (!isFindmanager)
                    {
                        if (emp.ApproveRight)
                        {
                            approver = emp;
                        }
                        else
                        {
                            emp = UserProfileUtil.GetEmployee(emp.Manager);
                        }
                    }
                    else
                    {
                        emp = UserProfileUtil.GetEmployee(emp.Manager);
                        if (!IsSameDepartment(old.Department, emp.Department))
                        {
                            if (isFQ)
                            {
                                if (old.ApproveRight)
                                {
                                    approver = old;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            if (emp.ApproveRight)
                            {
                                approver = emp;
                                break;
                            }
                        }
                    }
                }
                while ((approver == null) && (emp != null) && (IsSameDepartment(old.Department, emp.Department)));
            }
            catch (Exception ex)
            {
                approver = null;
            }

            return approver;
        }

        public static bool IsSameDepartment(string Dept1, string Dept2)
        {
            string dept1, dept2;
            if (Dept1.Contains(';'))
                dept1 = Dept1.Substring(0, Dept1.IndexOf(';') + 1).ToLower();
            else
                dept1 = Dept1.ToLower();
            if (Dept2.Contains(';'))
                dept2 = Dept2.Substring(0, Dept2.IndexOf(';') + 1).ToLower();
            else
                dept2 = Dept2.ToLower();

            if (dept1.Equals(dept2))
            {
                return true;
            }

            if (UserProfileUtil.GetDepartmentDisplayName(dept1).Equals(UserProfileUtil.GetDepartmentDisplayName(dept2)))
                return true;

            return false;
        }

        public static decimal GetMixedDays(DateTime formBeginAt, DateTime formEndAt, string formBeginAt_am_or_pm, string formEndAt_am_or_pm, DateTime queryBeginAt, DateTime queryEndAt)
        {

            var days = decimal.Zero;

            //默认查询范围完全包含请假范围，取请假起始日期和截止日期
            DateTime beginAt = formBeginAt;
            DateTime endAt = formEndAt;

            //请假起始日期比查询起始日期早，取查询起始日期
            if (formBeginAt < queryBeginAt)
            {
                beginAt = queryBeginAt;
            }

            //请假截止日期比查询截止日期晚，取查询截止日期
            if (formEndAt > queryEndAt)
            {
                endAt = queryEndAt;
            }

            if (beginAt <= endAt)
            {
                //days = (endAt - beginAt).Days + 1;

                SPList festivalList = SPContext.Current.Web.Site.RootWeb.Lists["Festivals"];

                //遍历算天数，忽略双休，假期
                while (beginAt <= endAt)
                {
                    if (Convert.ToInt32(beginAt.DayOfWeek) != 0 && Convert.ToInt32(beginAt.DayOfWeek) != 6)
                    {
                        if (festivalList.Items.Count > 0)
                        {
                            if (!festivalList.Items.GetDataTable().AsEnumerable().Any(row =>
                                DateTime.Parse(row["OffDay"] + "").Date == beginAt.Date))
                            {
                                days += 1m;
                            }
                        }
                        else
                        {
                            days += 1m;
                        }
                    }

                    beginAt = beginAt.AddDays(1);
                }

                //如果这个表单起始日期在查询起始日期之后，且从下午开始请，那么少计半天
                if (Convert.ToInt32(formBeginAt.DayOfWeek) != 0 && Convert.ToInt32(formBeginAt.DayOfWeek) != 6
                    && formBeginAt >= queryBeginAt && formBeginAt_am_or_pm.ToLower() == "pm")
                {
                    days -= 0.5m;
                }

                //如果这个表单截止日期在查询截止日期之前，且请到上午，那么少计半天
                if (Convert.ToInt32(formEndAt.DayOfWeek) != 0 && Convert.ToInt32(formEndAt.DayOfWeek) != 6
                    && formEndAt <= queryEndAt && formEndAt_am_or_pm.ToLower() == "am")
                {
                    days -= 0.5m;
                }
            }

            return days;
        }

        public static decimal GetMixedDays(DateTime formBeginAt, DateTime formEndAt, string formBeginAt_am_or_pm, string formEndAt_am_or_pm)
        {

            var days = decimal.Zero;

            SPList festivalList = SPContext.Current.Web.Site.RootWeb.Lists["Festivals"];

            var tmpDate = formBeginAt;

            //遍历算天数，忽略双休，假期
            while (tmpDate <= formEndAt)
            {
                if (Convert.ToInt32(tmpDate.DayOfWeek) != 0 && Convert.ToInt32(tmpDate.DayOfWeek) != 6)
                {
                    if (festivalList.Items.Count > 0)
                    {
                        if (!festivalList.Items.GetDataTable().AsEnumerable().Any(row =>
                            DateTime.Parse(row["OffDay"] + "").Date == tmpDate.Date))
                        {
                            days += 1m;
                        }
                    }
                    else
                    {
                        days += 1m;
                    }
                }

                tmpDate = tmpDate.AddDays(1);
            }

            //如果这个表单从下午开始请，那么少计半天
            if (Convert.ToInt32(formBeginAt.DayOfWeek) != 0 && Convert.ToInt32(formBeginAt.DayOfWeek) != 6
                 && formBeginAt_am_or_pm.ToLower() == "pm")
            {
                days -= 0.5m;
            }

            //如果这个表单请到上午，那么少计半天
            if (Convert.ToInt32(formEndAt.DayOfWeek) != 0 && Convert.ToInt32(formEndAt.DayOfWeek) != 6
                 && formEndAt_am_or_pm.ToLower() == "am")
            {
                days -= 0.5m;
            }


            return days;
        }

        public static CamlExpression LinkAnd(CamlExpression expr1, CamlExpression expr2)
        {
            if (expr1 == null)
                return expr2;
            else
                return expr1 && expr2;
        }

        public static CamlExpression LinkOr(CamlExpression expr1, CamlExpression expr2)
        {
            if (expr1 == null)
                return expr2;
            else
                return expr1 || expr2;
        }

        //Example: 296;#CA\\test1;#297;#CA\test2
        //Return employee objects
        public static List<Employee> GetEmployees(string approversStr)
        {
            List<Employee> employees = new List<Employee>();
            List<string> accounts = GetEmployeeAccounts(approversStr);
            Employee emp = null;
            foreach (string a in accounts)
            {
                if (a.IsNullOrWhitespace())
                {
                    continue;
                }
                emp = UserProfileUtil.GetEmployeeEx(a);
                if (emp != null)
                {
                    employees.Add(emp);
                }
            }
            return employees;
        }

        //Example: 296;#CA\\test1;#297;#CA\test2
        //Return user accounts of employee
        public static List<string> GetEmployeeAccounts(string approversStr)
        {
            //List<string> accounts = new List<string>();
            //var accountPattern = @"\;\#(?<Account>[^;#]*)?\;\#";
            //var accountExpression = new Regex(accountPattern);
            //approversStr = approversStr + ";#";
            //var accountMatches = accountExpression.Matches(approversStr);
            //foreach (Match m in accountMatches)
            //{
            //    accounts.Add(m.Groups["Account"].Value);
            //}
            //return accounts;
            return approversStr.Split(';').ToList<string>();
        }

        //Example: Test(CA\\test1)
        //Return employee object
        public static Employee GetEmployee(string applicantStr)
        {
            string account = GetApplicantAccount(applicantStr);
            Employee emp = UserProfileUtil.GetEmployeeEx(account);
            return emp;

        }

        //Example: Test(CA\\test1)
        //Return user account of employee
        public static string GetApplicantAccount(string applicantStr)
        {
            var accountPattern = @"^.*?\((.*?)\)$";
            var accountExpression = new Regex(accountPattern);
            return accountExpression.Match(applicantStr).Groups[1].Value;
        }

        public static void SendMail(string subject, string bodyTemplate, List<string> paramters, List<Employee> mailEmpList)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                foreach (Employee emp in mailEmpList)
                {
                    if (emp.WorkEmail.IsNullOrWhitespace())
                    {
                        continue;
                    }
                    IMailService mailService = MailServiceFactory.GetMailService();
                    paramters[0] = emp.DisplayName; //Set the display name
                    string body = string.Format(bodyTemplate, paramters.ToArray());
                    StringCollection sc = new StringCollection();
                    sc.Add(emp.WorkEmail);
                    try
                    {
                        mailService.SendMail(subject, body, sc);
                    }
                    catch (Exception ex)
                    {
                        // Create the source, if it does not already exist.
                        if (!EventLog.SourceExists("C&A"))
                        {
                            EventLog.CreateEventSource("C&A", "Mail");
                        }

                        // Create an EventLog instance and assign its source.
                        EventLog myLog = new EventLog();
                        myLog.Source = "C&A";

                        // Write an informational entry to the event log.    
                        myLog.WriteEntry(ex.Message, EventLogEntryType.Error);
                        //TODO: display the error message
                    }
                }
            });
        }

        //Return the list instance
        public static SPList GetWorkflowList(string listName)
        {
            return SPContext.Current.Site.OpenWeb("workflowcenter").Lists[listName];
        }

        //Get the list item that contains email subject and body
        public static SPListItem GetEmailTemplateByTitle(string title)
        {
            var qTitle = new QueryField("Title", false);
            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, qTitle.Equal(title));
            SPListItemCollection lc = ListQuery.Select()
                .From(GetWorkflowList("EmailTemplate"))
                .Where(exp)
                .GetItems();

            return lc.Count == 0 ? null : lc[0];
        }

        //Return the display name of employee
        public static string GetEmployeeName(string userAccount)
        {
            string displayName = string.Empty;
            Employee u = UserProfileUtil.GetEmployeeEx(userAccount);
            return u != null ? u.DisplayName : displayName;
        }

        //Return the approvers names
        public static string GetApproversNames(List<Employee> employees)
        {
            if (employees.Count == 0)
            {
                return string.Empty;
            }
            //Get the display name of approvers and applicant from userprofile
            StringBuilder approverNames = new StringBuilder();
            foreach (Employee emp in employees)
            {
                approverNames.Append(emp.DisplayName + ", ");
            }
            string temp = approverNames.ToString();
            return temp.Substring(0, temp.Length - 2);
        }

        public static string GetDeleman(string loginName, string moduleId)
        {
            string deleman = null;
            var now = DateTime.Now;

            var qApprover = new QueryField("ApproverLoginName", false);
            var qBeginOn = new QueryField("BeginOn", false);
            var qEndOn = new QueryField("EndOn", false);
            var qModules = new QueryField("Modules", false);
            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, qApprover.Equal(loginName));
            exp = WorkFlowUtil.LinkAnd(exp, qBeginOn.LessEqual(now));
            exp = WorkFlowUtil.LinkAnd(exp, qEndOn.MoreEqual(now));
            exp = WorkFlowUtil.LinkAnd(exp, qModules.Contains(moduleId));
            SPListItemCollection coll = ListQuery.Select()
                .From(GetWorkflowList("Delegates"))
                .Where(exp)
                .GetItems();
            if (coll.Count > 0)
            {
                deleman = coll[0]["DelegateToLoginName"] + "";
            }
            return deleman;
        }

        /*
         * Get delegate users according to the given multi names.
         * @Return array that contains delegate users
         */
        public static List<string> GetDelemans(List<string> loginNames, string moduleId)
        {
            List<string> delemans = new List<string>();
            var now = DateTime.Now;

            var qApprover = new QueryField("ApproverLoginName", false);
            var qBeginOn = new QueryField("BeginOn", false);
            var qEndOn = new QueryField("EndOn", false);
            var qModules = new QueryField("Modules", false);
            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, qModules.Contains(moduleId));            
            exp = WorkFlowUtil.LinkAnd(exp, qBeginOn.LessEqual(now));
            exp = WorkFlowUtil.LinkAnd(exp, qEndOn.MoreEqual(now));

            CamlExpression exp2 = null;
            foreach (string name in loginNames)
            {
                exp2 = WorkFlowUtil.LinkOr(exp2, qApprover.Equal(name));
            }

            exp = WorkFlowUtil.LinkAnd(exp, exp2);            
            
            SPListItemCollection coll = ListQuery.Select()
                .From(GetWorkflowList("Delegates"))
                .Where(exp)
                .GetItems();

            foreach (SPListItem item in coll)
            {
                delemans.Add(item["DelegateToLoginName"].ToString());
            }
            
            return delemans;
        }

        //Return task users object according to special group
        public static NameCollection GetTaskUsers(string group, string moduleId)
        {
            var taskUsers = new NameCollection();
            List<string> delemans = null;
            List<string> groupUsers = null;

            groupUsers = WorkFlowUtil.UserListInGroup(group);
            taskUsers.AddRange(groupUsers.ToArray());

            delemans = WorkFlowUtil.GetDelemans(groupUsers, moduleId);
            if (delemans.Count > 0)
            {
                taskUsers.AddRange(delemans.ToArray());
            }

            return taskUsers;
        }
    }
}