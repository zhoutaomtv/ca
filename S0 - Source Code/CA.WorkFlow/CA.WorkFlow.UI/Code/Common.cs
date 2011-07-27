using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SharePoint;
using System.Web.UI.HtmlControls;
using QuickFlow;

namespace CA.WorkFlow.UI.Code
{
    public class Common
    {
        public static void IsShowComments(HtmlTableRow trComments, HtmlTable tblCommentsList)
        {
            SPListItem item = SPContext.Current.ListItem;
            string strFlag = item["Flag"] + "";
            if (strFlag == "Save")
            {
                trComments.Style.Add("display", "none");
                tblCommentsList.Style.Add("display", "none");
            }
        }

        public static bool IsSameUser(NameCollection user1,NameCollection user2,NameCollection user3)
        {
            bool flag = false;
            if (user1.Count == 1 && user2.Count == 1 && user3.Count == 1)
            {
                if (user1[0].ToString() == user2[0].ToString() && user2[0].ToString() == user3[0].ToString())
                {
                    flag = true;
                }
            }
            return flag;
        }
    }
}
