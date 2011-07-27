using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.Office.Server;
using Microsoft.Office.Server.UserProfiles;
using CodeArt.SharePoint.CamlQuery;
using CA.SharePoint.Utilities.Common;
using Microsoft.Office.Server.Search.Query;
using System.Data;

using System.Configuration;
using System.Collections;





namespace CA.SharePoint
{
    public class UserProfileUtil
    {
        public static List<Employee> GetEmployeeFromSSPByDept(string dept)
        {
            List<Employee> lstEmployees = new List<Employee>();
            Employee employee = null;          
            try
            {
                //从SSP里面取得当前用户所在的部门名称
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        ServerContext context = ServerContext.GetContext(site);
                        UserProfileManager profileManager = new UserProfileManager(context);

                        foreach (UserProfile profile in profileManager)
                        {
                            if (profile[PropertyConstants.Department].Value == null)
                                continue;

                            string[] depts = profile[PropertyConstants.Department].Value.ToString().ToLower().Split(new Char[] {';', '/'});

                            employee = InstanceEmployee(profile, site);

                            if (depts.Contains(dept.ToLower()))
                            {                              
                                
                                switch (dept.ToLower())
                                { 
                                    case "admin" :
                                        if (employee.Title != "Receptionist")
                                        { 
                                            lstEmployees.Add(employee); 
                                        }
                                        break;
                                    default :
                                        lstEmployees.Add(employee);
                                        break;
                                }
                                //lstEmployees.Add(employee);
                            }
                            else if (string.IsNullOrEmpty(dept))
                            {
                                lstEmployees.Add(employee);
                            }

                            if (dept.ToLower() == "hr" && employee.Title == "Receptionist")
                            {
                                lstEmployees.Add(employee); 
                            }

                        }
                        
                    }
                }
                );

            }
            catch (Exception e)
            {
                throw new Exception("获取部门用户信息出错：" + e.Message);
            }
            return lstEmployees;
        }

        public static Employee GetEmployeeByDisplayName(string displayName)
        {
            Employee employee = null;
            try
            {
                //从SSP里面取得当前用户所在的部门名称
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        ServerContext context = ServerContext.GetContext(site);
                        UserProfileManager profileManager = new UserProfileManager(context);

                        foreach (UserProfile profile in profileManager)
                        {
                            if ((profile[PropertyConstants.UserName].Value.ToString().ToLower() == displayName.ToLower())
                                ||(profile[PropertyConstants.PreferredName].Value.ToString().ToLower()==displayName.ToLower()))
                            {
                                employee = InstanceEmployee(profile, site);
                                break;
                            }
                        }
                    }
                }
                );
            }
            catch (Exception e)
            {
                throw new Exception("获取当前的用户信息出错：" + e.Message);
            }

            return employee;
        }

        //根据用户名查找用户信息，如果查找用户名为系统管理员，则过滤此次请求
        public static Employee GetEmployeeEx(string userAccount)
        {
            if (userAccount.Equals("SHAREPOINT\\System", StringComparison.CurrentCultureIgnoreCase))
            {
                return null;
            }
            return GetEmployee(userAccount);
        }

          /// <summary>
        /// 根据登录名从SSP得到当前用户部门
        /// </summary>
        /// <param name="userAccount">用户的登录名</param>
        /// <returns></returns>
        public static Employee GetEmployee(string userAccount)
        {
            Employee employee = null;
            try
            {
                //从SSP里面取得当前用户所在的部门名称
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        ServerContext context = ServerContext.GetContext(site);
                        UserProfileManager profileManager = new UserProfileManager(context);

                        if (profileManager.UserExists(userAccount) || SPContext.Current.Web.CurrentUser.IsSiteAdmin)
                        {
                            if (SPContext.Current.Web.CurrentUser.IsSiteAdmin && userAccount == SPContext.Current.Web.CurrentUser.LoginName)
                            {
                                userAccount = System.Web.HttpContext.Current.User.Identity.Name;
                            }
                            UserProfile userProfile = profileManager.GetUserProfile(userAccount);
                            employee = InstanceEmployee(userProfile, site);
                        }
                        else
                        {
                            //throw new Exception("共享服务(SSP)中没有该用户的信息，可能是共享服务(SSP)没有和AD同步所致，请联系IT管理员联系！");
                        }
                    }
                }
                );   
            }
            catch (Exception e)
            {
                throw new Exception("获取当前的用户信息出错：" + e.Message);
            }

            return employee;
        }

        /// <summary>
        /// 根据登录名从SSP得到当前用户
        /// </summary>
        /// <param name="id">用户的ID</param>
        /// <returns></returns>
        public static Employee GetEmployee(Guid id)
        {
            Employee employee = null;
            try
            {
                //从SSP里面取得当前用户所在的部门名称
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                    {
                        ServerContext context = ServerContext.GetContext(site);
                        UserProfileManager profileManager = new UserProfileManager(context);

                        UserProfile userProfile = profileManager.GetUserProfile(id);
                        if (userProfile != null)
                            employee = InstanceEmployee(userProfile, site);
                    }
                }
                );
            }
            catch (Exception e)
            {
                //throw new Exception("获取当前的用户信息出错：" + e.Message);
            }

            return employee;
        }

        public static string GetDepartmentManager(string Dept)
        {
            SPList list = SharePointUtil.GetList(SPContext.Current.Site.RootWeb, CAConstants.ListName.Department);

            foreach (SPListItem item in list.Items)
            {
                if (item[CAConstants.FieldName.Name].ToString().Equals(Dept, StringComparison.CurrentCultureIgnoreCase))
                {
                    return (new SPFieldLookupValue(item[CAConstants.FieldName.ManagerAccount] + "")).LookupValue;
                }
            }

            return string.Empty;

        }

        public static string GetDepartmentDisplayName(string Dept)
        {
            SPList list = SharePointUtil.GetList(SPContext.Current.Site.RootWeb, CAConstants.ListName.Department);

            foreach (SPListItem item in list.Items)
            {
                if (item[CAConstants.FieldName.Name].ToString().Equals(Dept, StringComparison.CurrentCultureIgnoreCase))
                {
                    string name = item[CAConstants.FieldName.DisplayName] + "";
                    if (string.IsNullOrEmpty(name))
                        return Dept;
                    else
                        return name.ToLower();
                }
            }

            return Dept;

        }
        public static Employee InstanceEmployee(UserProfile profile, SPSite site)
        {
            Employee employee = new Employee();
            employee.UserAccount = profile[PropertyConstants.AccountName].Value + "";
            employee.DisplayName = profile[PropertyConstants.FirstName].Value + " " + profile[PropertyConstants.LastName].Value;
           
            employee.PreferredName=profile[PropertyConstants.PreferredName].Value + "";
            string strDepartment = profile[PropertyConstants.Department].Value + "";
            if (strDepartment.Contains(';'))
            {
                employee.Department = strDepartment.Substring(0, strDepartment.IndexOf(';') );
                employee.AllDepartment = strDepartment;
            }
            else
            {
                employee.Department = strDepartment;
                employee.AllDepartment = strDepartment;
            }
            employee.Manager = profile[PropertyConstants.Manager].Value + "";
            //add by wujun 20100709
            //begin
            employee.Phone = profile[PropertyConstants.WorkPhone].Value + "";
            employee.Mobile = profile[PropertyConstants.CellPhone].Value + "";
            employee.WorkEmail = profile[PropertyConstants.WorkEmail].Value + "";
            //http://wsc2337:91/personal/wsq/Shared%20Pictures/配置文件图片/懂得.gif
            employee.PhotoUrl = profile[PropertyConstants.PictureUrl].Value + "";
            //end
            //add by wujun 20100714
            employee.Title = profile[PropertyConstants.Title].Value + "";
            employee.More = profile[PropertyConstants.Office].Value + "";
            employee.Fax = profile[PropertyConstants.Fax].Value + "";
            employee.PopulateName = "";
            if (string.IsNullOrEmpty(employee.PhotoUrl))
            {
                string fileName = profile[PropertyConstants.UserName].Value + "";
                employee.PhotoUrl = ConfigurationManager.AppSettings["userPhotoLocation"] + fileName.Replace('.', ' ') + ConfigurationManager.AppSettings["userPhotoType"];
                if (!site.RootWeb.GetFile(employee.PhotoUrl).Exists)
                {
                    employee.PhotoUrl = ConfigurationManager.AppSettings["userPhotoLocation"] + "default.jpg";
                }
            }
            employee.ApproveRight = !string.IsNullOrEmpty((profile["ApproveRight"].Value + "").Trim());
            employee.EmployeeID = (profile["EmployeeId"].Value + "").Trim();

            //added by wsq 20101118
            List<string> listreports = new List<string>();
            foreach(UserProfile report in profile.GetDirectReports())
            {
                listreports.Add(report["UserName"].Value + "");
                
            }
            employee.DirectReports = listreports.ToArray();
            return employee;
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
                        group = web.Groups[strGroupName];

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
    }

    
}
