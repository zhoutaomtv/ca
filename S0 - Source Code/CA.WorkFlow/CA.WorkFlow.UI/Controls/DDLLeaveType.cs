using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.SharePoint.WebControls;

namespace CA.WorkFlow.UI
{
    public class DDLLeaveType : DropDownList
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            string[] arrayType = new string[] { 
                "Annual Leave 年假",
                "Sick Leave 病假",
                "Maternity Leave 产假",
                "No Pay Leave 不带薪假",
                "Marriage Leave 婚假",
                "Compassionate Leave 丧假",
                "Leave in-lieu-of Overtime 加班调休",
                "Others 其他"
            };          
            foreach (string type in arrayType)
            {
                base.Items.Add(type);
            }
        }
    }
}
