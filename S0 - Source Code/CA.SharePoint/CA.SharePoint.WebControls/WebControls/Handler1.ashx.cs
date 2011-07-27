using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Text;
using CA.SharePoint.Utilities.Common;
using Microsoft.SharePoint;
using System.Collections.Generic;

namespace CA.SharePoint.WebControls.WebControls
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Handler1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            context.Response.AddHeader("pragma", "no-cache");
            context.Response.AddHeader("cache-control", "");
            context.Response.CacheControl = "no-cache";
            context.Response.ContentType = "text/plain";
            string strSPDept = context.Request["dept"].ToString();
            string strEmp = context.Request["user"].ToString();
            StringBuilder str = new StringBuilder();
            List<Employee> employees = new List<Employee>();
            Employee employee = new Employee();
            SPList list = SharePointUtil.GetList(SPContext.Current.Site.RootWeb, CAConstants.ListName.Department);
            foreach (SPListItem item in list.Items)
            {
                if (item["DisplayName"] == null)
                    continue;

                string strTempSPDept = item["DisplayName"].ToString().ToLower();
                if (strTempSPDept == strSPDept.ToLower())
                {
                    employees.AddRange(UserProfileUtil.GetEmployeeFromSSPByDept(item["Name"].ToString()));
                }
            }
            employee = employees.Find(new Predicate<Employee>(delegate(Employee emp)
            {
                return emp.DisplayName.Trim().ToLower() == strEmp.Trim().ToLower();
            }));

            str.Append("<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" runat=\"server\">");
            str.Append("<tr><th width=\"108\" valign=\"top\"><div id=\"projectthumnail\">");
            str.AppendFormat("<img width=\"100\" src=\"{0}\" style=\"vertical-align:top\" /></div></th>", employee.PhotoUrl);
            str.Append("<th  align=\"left\" valign=\"top\">");
            str.Append("<table width=\"96%\" border=\"0\" align=\"left\" cellpadding=\"0\" cellspacing=\"0\">");
            str.AppendFormat("<tr><th width=\"15%\">Name:</th><th width=\"35%\" align=\"left\">{0}&nbsp;", employee.DisplayName);
            str.AppendFormat("</th><th width=\"15%\">Dept:</th><th width=\"35%\">{0}&nbsp;</th></tr>", ReplaceMTM(employee.AllDepartment));
            str.AppendFormat("<tr><th>Cell:</th><th>{0}&nbsp;</th>", employee.Mobile);
            str.AppendFormat("<th>Phone:</th><th>{0}&nbsp;</th></tr>", employee.Phone);
            str.AppendFormat("<tr><th width=\"10%\">Email:</th><th colspan=\"3\"><a href=\"mailto:{0}\">{0}&nbsp;</a></th></tr>", employee.WorkEmail);
            str.AppendFormat("<tr><th>Title:</th><th colspan=\"3\">{0}&nbsp;</th></tr>", employee.Title);
            str.AppendFormat("</table></th></tr></table>", employee.More);

            context.Response.Write(str.ToString());  
        }

        private string ReplaceMTM(string strInput)
        {
            string strOutput = string.Empty;
            if (strInput.Contains("MTM"))
            {
                int len = strInput.IndexOf("MTM");
                strOutput = strInput.Substring(len, 3);
            }
            else
            {
                strOutput = strInput;
            }
            return strOutput;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
