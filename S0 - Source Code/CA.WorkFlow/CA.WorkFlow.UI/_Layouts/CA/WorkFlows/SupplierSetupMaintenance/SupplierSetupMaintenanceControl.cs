namespace CA.WorkFlow.UI.SupplierSetupMaintenance
{
    using System.Data;
    using CodeArt.SharePoint.CamlQuery;
    using Microsoft.SharePoint;
    using SharePoint.Utilities.Common;

    public class SupplierSetupMaintenanceControl : BaseWorkflowUserControl
    {
        public DataTable GetDataTable(string workflowNumber, string enName, string cnName, bool isCompleted, string applicantAccount, string department)
        {
            var status = isCompleted ? "Completed" : null;
            SPListItemCollection lc = this.FilterVendor(workflowNumber, null, enName, cnName, status, applicantAccount, department);
            return lc.GetDataTable();
        }

        protected SPListItemCollection FilterVendor(string workflowNumber, string enName, string cnName, bool isCompleted, string applicantAccount, string department)
        {
            var status = isCompleted ? "Completed" : null;
            SPListItemCollection lc = this.FilterVendor(workflowNumber, null, enName, cnName, status, applicantAccount, department);
            return lc;
        }

        protected SPListItemCollection FilterVendor(string vendId, bool isCompleted, string applicantAccount, string department)
        {
            var status = isCompleted ? "Completed" : null;
            return FilterVendor(null, vendId, null, null, status, applicantAccount, department);
        }

        protected SPListItemCollection FilterVendor(string workflowNumber, string vendId, string enName, string cnName, string status, string applicantAccount, string department)
        {
            var qWorkflowNumber = new QueryField("Title", false);
            var qENName = new QueryField("EN_x0020_Name_x0020_of_x0020_Ven", false);
            var qCNName = new QueryField("CN_x0020_Name_x0020_of_x0020_Ven", false);
            var qRecordType = new QueryField("Record_x0020_Type", false);
            var qIsCompleted = new QueryField("Status", false);
            var qVendId = new QueryField("Vendor_x0020_ID", false);
            var qApplicantAccount = new QueryField("Applicant", false);
            var qDepartmentVal = new QueryField("DepartmentVal", false);

            CamlExpression exp = null;

            exp = WorkFlowUtil.LinkAnd(exp, qRecordType.Equal("New"));

            if (!string.IsNullOrEmpty(status))
            {
                exp = WorkFlowUtil.LinkAnd(exp, qIsCompleted.Equal(status));
            }

            if (!string.IsNullOrEmpty(workflowNumber))
            {
                exp = WorkFlowUtil.LinkAnd(exp, qWorkflowNumber.Equal(workflowNumber));
            }

            if (!string.IsNullOrEmpty(vendId))
            {
                exp = WorkFlowUtil.LinkAnd(exp, qVendId.Equal(vendId));
            }

            if (!string.IsNullOrEmpty(enName))
            {
                exp = WorkFlowUtil.LinkAnd(exp, qENName.Contains(enName));
            }

            if (!string.IsNullOrEmpty(cnName))
            {
                exp = WorkFlowUtil.LinkAnd(exp, qCNName.Contains(cnName));
            }
            if (!string.IsNullOrEmpty(applicantAccount))
            {
                //Applicant: Test1(CA\\test1)
                //applicantAccount: CA\\test1
                exp = WorkFlowUtil.LinkAnd(exp, qApplicantAccount.Contains(applicantAccount));
            }
            if (!string.IsNullOrEmpty(department))
            {
                exp = WorkFlowUtil.LinkAnd(exp, qDepartmentVal.Equal(department));
            }

            var result = ListQuery.Select().From(WorkFlowUtil.GetWorkflowList("Supplier Setup Maintenance Workflow"))
                .Where(exp)
                .OrderBy(new QueryField("Title", false), true)
                .GetItems();

            return result;
        }
    }
}