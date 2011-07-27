namespace CA.WorkFlow.UI.InternalOrderMaintenance
{
    using System.Data;
    using CodeArt.SharePoint.CamlQuery;
    using Microsoft.SharePoint;
    using SharePoint.Utilities.Common;
    using System.Text;
    using System;

    public class InternalOrderUserControl : BaseWorkflowUserControl
    {
        protected SPListItem GetOrderInfo(string orderNumber, string department)
        {
            SPListItem item = null;
            var qOrderNumber = new QueryField("Order_x0020_Number", false);
            var qDepartment = new QueryField("Department", false);
            var qStatus = new QueryField("Status", false);
            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, qOrderNumber.Equal(orderNumber));
            exp = WorkFlowUtil.LinkAnd(exp, qDepartment.Equal(department));

            var status = CAWorkflowStatus.Completed;
            exp = WorkFlowUtil.LinkAnd(exp, qStatus.Equal(status));

            SPListItemCollection lc = ListQuery.Select()
                .From(WorkFlowUtil.GetWorkflowList("CreationOrder"))
                .Where(exp)
                .GetItems();
            if (lc.Count > 0)
            {
                item = lc[0];
            }
            return item;
        }

        public DataTable GetChangeHistory(string orderNumber, string department)
        {
            var changHistoryTable = new DataTable();
            SPListItemCollection lc = FilterChangeHistory(orderNumber, department, true);
            changHistoryTable = lc.GetDataTable();
            return changHistoryTable;
        }

        protected SPListItem GetLatestHistory(string orderNumber, string department)
        {
            SPListItemCollection lc = FilterChangeHistory(orderNumber, department, true);
            if (lc.Count > 0)
            {
                return lc[0];
            }
            return null;
            
        }

        protected bool IsExistRunningMaintenance(string orderNumber, string department)
        {
            SPListItemCollection lc = FilterChangeHistory(orderNumber, department, false);
            if (lc.Count > 0)
            {
                return true;
            }
            return false;
        }

        protected bool IsExistRunningMaintenance(string orderNumber)
        {
            return IsExistRunningMaintenance(orderNumber, null);
        }

        private SPListItemCollection FilterChangeHistory(string orderNumber, bool isCompleted)
        {
            return FilterChangeHistory(orderNumber, null, isCompleted);
        }

        private SPListItemCollection FilterChangeHistory(string orderNumber, string department, bool isCompleted)
        {
            string status = null;
            var qOrderNumber = new QueryField("Order_x0020_Number", false);
            var qStatus = new QueryField("Status", false);
            var qDepartment = new QueryField("Department", false);
            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, qOrderNumber.Equal(orderNumber));
            if (department.IsNotNullOrWhitespace())
            {
                exp = WorkFlowUtil.LinkAnd(exp, qDepartment.Equal(department));
            }            
            if (isCompleted)
            {
                status = CAWorkflowStatus.Completed;
                exp = WorkFlowUtil.LinkAnd(exp, qStatus.Equal(status));
            }
            else
            {
                //the status should not be "pending", "notstart", "financemanagerreject", "cforeject".
                status = CAWorkflowStatus.Pending;
                exp = WorkFlowUtil.LinkAnd(exp, qStatus.NotEqual(status));
                status = CAWorkflowStatus.Completed;
                exp = WorkFlowUtil.LinkAnd(exp, qStatus.NotEqual(status));
                status = CAWorkflowStatus.IODepartmentManagerReject;
                exp = WorkFlowUtil.LinkAnd(exp, qStatus.NotEqual(status));
                status = CAWorkflowStatus.IOCFOReject;
                exp = WorkFlowUtil.LinkAnd(exp, qStatus.NotEqual(status));
                status = CAWorkflowStatus.IOFinanceReject;
                exp = WorkFlowUtil.LinkAnd(exp, qStatus.NotEqual(status));
            }

            SPListItemCollection lc = ListQuery.Select()
                .From(WorkFlowUtil.GetWorkflowList("Internal Order Maintenance Workflow"))
                .Where(exp)
                .OrderBy(new QueryField("Title", false), false)
                .GetItems();
            return lc;
        }

        //Create the link for the attachments
        protected string GetAttachTable(SPListItem listItem)
        {
            StringBuilder attachmentTable = new StringBuilder();
            SPAttachmentCollection ac = listItem.Attachments;
            for (int i = 0; i < ac.Count; i++)
            {
                attachmentTable.Append("<a href=\"");
                attachmentTable.Append(ac.UrlPrefix + ac[i]);
                attachmentTable.Append("\">");
                attachmentTable.Append(ac[i]);
                attachmentTable.Append("</a></br>");
            }
            return attachmentTable.ToString();
        }

    }
}