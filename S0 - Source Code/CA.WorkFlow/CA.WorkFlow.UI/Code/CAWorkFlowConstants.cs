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

namespace CA.WorkFlow.UI
{
    public class CAWorkFlowConstants
    {
        public enum ListName
        {
            LeaveBalance,
            WorkFlowNumber,
            BusinessCard,
            LeaveRecord,
            TravelRequestWorkflow,
            TravelDetails,
            TravelHotelInfo,
            TravelVehicleInfo,
            PODetails,
            NewEmployeeEquipmentApplication,
            ITRequestItems,
            NewStoreBudgetApplication,
            Stores,
            TravelApplication,
            UserAssistant,
            StoreSamplingWorkflow,
        }

        public struct WorkFlowListName
        {
           

            public static string BusinessCard
            {
                get { return "Business Card Application"; }
            }
            public static string NewSupplierCreationWorkFlow
            {
                get { return "New Supplier Creation WorkFlow"; }
            }
            public static string NewEmployeeEquipmentApplication
            {
                get { return "New Employee Equipment Application"; }
            }
            public static string StoreSamplingWorkflow
            {
                get { return "Store Sampling Workflow"; }
            }
            public static string StoreSamplingSKU
            {
                get { return "Store Sampling SKU"; }
            }
            public static string StoreSamplingReport
            {
                get { return "Store Sampling Report"; }
            }
            public static string ItRequestWorkFlow
            {
                get { return "IT Request WorkFlow"; }
            }
            public static string ITRequestItems
            {
                get { return "IT Request Items"; }
            }
            public static string StoreMaintenanceWorkflow
            {
                get { return "Store Maintenance Workflow"; }
            }
            public static string StoreMaintenanceItems1
            {
                get { return "Store Maintenance Items1"; }
            }
            public static string StoreMaintenanceItems2
            {
                get { return "Store Maintenance Items2"; }
            }
            public static string ConstructionPurchasingWorkflow
            {
                get { return "Construction Purchasing Workflow"; }
            }
            public static string ConstructionItems
            {
                get { return "Construction Items"; }
            }
            public static string ChangeRequestWorkflow
            {
                get { return "Change Request Workflow"; }
            }
            public static string ChangeRequestReport
            {
                get { return "Change Request Report"; }
            }
        }
    }
}
