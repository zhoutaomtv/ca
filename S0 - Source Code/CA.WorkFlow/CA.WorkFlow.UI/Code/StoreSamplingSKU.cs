using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CA.SharePoint;
using Microsoft.SharePoint;
using System.Data;

namespace CA.WorkFlow.UI.Code
{
    public class StoreSamplingSKU
    {
        public string ListName
        {
            get {
                return "Store Sampling SKU";
            }
        }

        private SPList List;
        

        public StoreSamplingSKU()
        {          
            
            //error
            //ISharePointService sps = ServiceFactory.GetSharePointService(true);
            //List = sps.GetList(ListName);
        }

        public void Save(DataTable dt, string workflowNumber)
        {
            foreach (DataRow row in dt.Rows)
            {
                SPListItem item = List.Items.Add();
                item["WorkflowNumber"] = workflowNumber;
                item["SKU number"] = row["SKUnumber"];
                item["Class"] = row["Class"];
                item["Actual Quantity"] = row["ActualQuantity"];
                item["Picked"] = row["Picked"];
                item.Web.AllowUnsafeUpdates = true;
                item.Update();
            }
        }

        public void Save(DataTable dt, string workflowNumber, bool doCleaningBeforeAction)
        {
            if (doCleaningBeforeAction)
            {
                WorkFlowUtil.RemoveExistingRecord(List, "WorkflowNumber", workflowNumber);
            }
            Save(dt, workflowNumber);
        }
    }
}
