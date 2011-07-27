using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CA.SharePoint;
using Microsoft.SharePoint;
using QuickFlow.Core;
using CodeArt.SharePoint.CamlQuery;
using System.Data;
using CA.WorkFlow.UI.Code;
using Microsoft.SharePoint.WebControls;

namespace CA.WorkFlow.UI._Layouts.CA.WorkFlows.StoreSampling
{
    public partial class ApproveForm : CAWorkFlowPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

        }


        void PushToReport()
        {
            //WorkflowContext curContext = WorkflowContext.Current;
            //WorkflowDataFields fields = curContext.DataFields;

            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList list = sps.GetList(CAWorkFlowConstants.WorkFlowListName.StoreSamplingReport);

            //foreach (DataRow row in DataForm1)
            //{
            //    SPListItem item = list.Items.Add();
            //    //item["WorkflowNumber"] = fields["WorkflowNumber"];
            //    //item["Store Number"] = fields["Store Number"];
            //    item["WorkflowNumber"] = DataForm1.WorkflowNumber;
            //    item["Store Number"] = DataForm1.StoreNumber;
            //    item["Actual Quantity"] = row["ActualQuantity"];
            //    item["Picked"] = row["Picked"];
            //    item.Web.AllowUnsafeUpdates = true;
            //    item.Update();
            //}
        }
    }
}
