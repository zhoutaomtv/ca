using Microsoft.SharePoint;
using CodeArt.SharePoint.CamlQuery;
namespace CA.WorkFlow.UI.CreationOrder
{
    public class CreateOrderUserControl : BaseWorkflowUserControl
    {
        protected bool isExistOrder(string orderNumber, string department)
        {
            var qOrderNumber = new QueryField("Order_x0020_Number", false);
            var qDepartment = new QueryField("Department", false);
            CamlExpression exp = null;
            exp = WorkFlowUtil.LinkAnd(exp, qOrderNumber.Equal(orderNumber));
            exp = WorkFlowUtil.LinkAnd(exp, qDepartment.Equal(department));

            SPListItemCollection lc = ListQuery.Select()
                .From(WorkFlowUtil.GetWorkflowList("CreationOrder"))
                .Where(exp)
                .GetItems();
            
            return lc.Count > 0;
        }
    }
}