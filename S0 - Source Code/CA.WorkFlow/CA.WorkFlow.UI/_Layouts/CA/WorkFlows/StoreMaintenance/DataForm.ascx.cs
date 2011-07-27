using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;
using CA.SharePoint.Utilities.Common;
using CA.SharePoint;
using QuickFlow.Core;
using System.Data;
using CodeArt.SharePoint.CamlQuery;
using System.Text;
using System.IO;

namespace CA.WorkFlow.UI.StoreMaintenance
{
    public partial class DataForm : QFUserControl
    {
        public string WorkflowNumber
        {
            get
            {
                return lblWorkflowNumber.Text;
            }
            set
            {
                lblWorkflowNumber.Text = value;
            }
        }

        /// <summary>
        /// Store Maintenance Items1
        /// </summary>
        public DataTable Asso1
        {
            get
            {
                if (ViewState["Asso1"] == null)
                    InitAsso1();
                return (DataTable)ViewState["Asso1"];
            }
            set
            {
                ViewState["Asso1"] = value;
            }
        }

        /// <summary>
        /// Store Maintenance Items2
        /// </summary>
        public DataTable Asso2
        {
            get
            {
                if (ViewState["Asso2"] == null)
                    InitAsso2();
                return (DataTable)ViewState["Asso2"];
            }
            set
            {
                ViewState["Asso2"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblLoginName.Text = SPContext.Current.Site.RootWeb.CurrentUser.LoginName;

                ISharePointService sps = ServiceFactory.GetSharePointService(true, SPContext.Current.Site.RootWeb);
                SPListItemCollection stores = sps.GetList("Stores").GetItems(new SPQuery());
                for (int i = 0; i < stores.Count; i++)
                {
                    ddlCostCenter.Items.Add(new ListItem(stores[i]["Cost Center"] + " " + stores[i]["DisplayName"], stores[i]["Cost Center"] + ""));
                }

                
                LoadData();
                
            }
            
            //if (this.ControlMode == SPControlMode.New)
            //{
            //    this.btnAdd1.Visible = true;
            //    this.rpt1.Visible = true;
            //}
        }


        private void InitAsso1()
        {
            Asso1 = new DataTable();
            Asso1.Columns.Add("WorkflowNumber");
            Asso1.Columns.Add("Reason");
            Asso1.Columns.Add("Description");
            Asso1.Columns.Add("Remark");
            Asso1.Columns.Add("Seq");
        }

        private void InitAsso2()
        {
            Asso2 = new DataTable();
            Asso2.Columns.Add("WorkflowNumber");
            Asso2.Columns.Add("Reason");
            Asso2.Columns.Add("Price");
            Asso2.Columns.Add("Quantity");
            Asso2.Columns.Add("Seq");
            //Asso2.Columns.Add("Subtotal");
            Asso2.Columns.Add("Total");
        }

        private void LoadData()
        {
            if (this.ControlMode != SPControlMode.New)
            {
                //load main
                SPListItem curItem = SPContext.Current.ListItem;

                lblWorkflowNumber.Text = curItem["WorkflowNumber"] + "";
                ddlType1.SelectedValue = curItem["Type1"] + "";
                //txtCostCenter.Text = curItem["CostCenter"] + "";
                ddlCostCenter.SelectedValue = curItem["CostCenter"] + "";
                txtBudgetValue.Text = curItem["BudgetValue"] + "";
                //txtPaymentTerm.Text = curItem["PaymentTerm"] + "";

                LoadAsso1();
                LoadAsso2();

                if (this.ControlMode == SPControlMode.Edit)
                {
                    switch (WorkflowContext.Current.Task.Step)
                    {
                        case "CMManagerGroupReview":
                            FreezeMain();
                            BindRpt1Display();
                            break;
                        case "StoreManagerApprove":
                            FreezeForm();
                            break;
                        case "AreaManagerApprove":
                            FreezeForm();
                            break;
                        case "ConstructionHeadApprove":
                            FreezeForm();
                            break;
                        case "SOTeamGroupApprove":
                            FreezeForm();
                            break;
                        case "CMManagerGroupOrders":
                            FreezeForm();
                            break;
                        case "StoreManagerEvaluates":
                            FreezeForm();
                            ShowEx();
                            break;
                        case "RequestSubmit":
                            BindRpt2Display();
                            break;
                        default:
                            break;
                    }
                }
                else if (this.ControlMode == SPControlMode.Display)
                {
                    FreezeForm();
                    FreezeAsso();
                    ShowEx();
                }
            }
            
        }

        private void ShowEx()
        {
            string fileurl = "/tmpfiles/excel/" + WorkflowNumber + ".xls";
            if (File.Exists(Server.MapPath(fileurl)))
            {
                lblex.Text = "<a href='" + fileurl + "' target='_blank'>Export Order</a>";
                lblex.Visible = true;
            }
        }

        private void LoadAsso1()
        {
            InitAsso1();

            ISharePointService sps = ServiceFactory.GetSharePointService(true);

            //获取列表
            SPList list = sps.GetList(CAWorkFlowConstants.WorkFlowListName.StoreMaintenanceItems1 + "");

            QueryField field = new QueryField("WorkflowNumber", false);

            SPListItemCollection items = sps.Query(list, field.Equal(WorkflowNumber), 0);

            foreach (SPListItem item in items)
            {
                DataRow row = Asso1.Rows.Add();
                row["WorkflowNumber"] = item["WorkflowNumber"];
                row["Reason"] = item["Reason"];
                row["Description"] = item["Description"];
                row["Remark"] = item["Remark"];
                row["Seq"] = item["Seq"];
            }

            BindRpt1();
        }

        private void LoadAsso2()
        {
            InitAsso2();

            ISharePointService sps = ServiceFactory.GetSharePointService(true);

            //获取列表
            SPList list = sps.GetList(CAWorkFlowConstants.WorkFlowListName.StoreMaintenanceItems2 + "");

            QueryField field = new QueryField("WorkflowNumber", false);

            SPListItemCollection items = sps.Query(list, field.Equal(WorkflowNumber), 0);

            decimal sum = 0;
            foreach (SPListItem item in items)
            {
                DataRow row = Asso2.Rows.Add();
                row["WorkflowNumber"] = item["WorkflowNumber"];
                row["Reason"] = item["Reason"];
                row["Quantity"] = item["Quantity"];
                row["Seq"] = item["Seq"];
                decimal price = 0;
                //int quantity = 0;
                decimal.TryParse(item["Price"] + "", out price);
                //int.TryParse(item["Quantity"] + "", out quantity);
                //decimal subtotal = price * quantity;
                //row["Price"] = price.ToString("N2");
                //row["Subtotal"] = subtotal.ToString("N2");
                //sum += subtotal;
                row["Price"] = price.ToString("N2");
                decimal total = 0;
                decimal.TryParse(item["Total"] + "", out total);
                row["Total"] = total.ToString("N2");
                sum += total;
                
            }

            //lblSum.Text = sum.ToString("N2");

            BindRpt2();
        }

        public void FreezeForm()
        {
            //Panel1.Enabled = false;
            ddlBudgetApproved.Enabled = false;
            ddlCostCenter.Enabled = false;
            ddlType1.Enabled = false;
            txtBudgetValue.Enabled = false;

            BindRpt1Display();
            BindRpt2Display();
        }

        public void FreezeMain()
        {
            Panel1.Enabled = false;
        }

        public void FreezeAsso()
        {
            BindRpt1Display();
            BindRpt2Display();
        }

        public void BindRpt1()
        {
            this.rpt1.DataSource = Asso1;
            this.rpt1.DataBind();
            this.rpt1.Visible = true;
            this.rpt1Display.Visible = false;
        }

        public void BindRpt2()
        {
            this.rpt2.DataSource = Asso2;
            this.rpt2.DataBind();
            this.rpt2.Visible = true;
            this.rpt2Display.Visible = false;
        }

        public void BindRpt1Display()
        {
            this.rpt1Display.DataSource = Asso1;
            this.rpt1Display.DataBind();
            this.rpt1Display.Visible = true;
            this.btnAdd1.Visible = false;
            this.rpt1.Visible = false;
        }

        public void BindRpt2Display()
        {
            this.rpt2Display.DataSource = Asso2;
            this.rpt2Display.DataBind();
            this.rpt2Display.Visible = true;
            this.btnAdd2.Visible = false;
            this.rpt2.Visible = false;
        }




        protected void rpt1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
            {
                Asso1.Rows.Remove(Asso1.Rows[e.Item.ItemIndex]);
                this.rpt1.DataSource = Asso1;
                this.rpt1.DataBind();
            }
        }

        protected void rpt2_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("delete", StringComparison.CurrentCultureIgnoreCase))
            {
                Asso2.Rows.Remove(Asso2.Rows[e.Item.ItemIndex]);
                this.rpt2.DataSource = Asso2;
                this.rpt2.DataBind();
            }
        }

        protected void rpt1_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DataRowView row = e.Item.DataItem as DataRowView;
                if (row == null)
                    return;

                Label lbl11 = (Label)e.Item.FindControl("lbl11");
                TextBox txt12 = (TextBox)e.Item.FindControl("txt12");
                TextBox txt13 = (TextBox)e.Item.FindControl("txt13");
                TextBox txt14 = (TextBox)e.Item.FindControl("txt14");

                lbl11.Text = row["Seq"].ToString();
                txt12.Text = row["Reason"].ToString();
                txt13.Text = row["Description"].ToString();
                txt14.Text = row["Remark"].ToString();
            }
        }

        protected void rpt2_ItemDataBound(object source, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                DataRowView row = e.Item.DataItem as DataRowView;
                if (row == null)
                    return;

                Label lbl21 = (Label)e.Item.FindControl("lbl21");
                TextBox txt22 = (TextBox)e.Item.FindControl("txt22");
                TextBox txt23 = (TextBox)e.Item.FindControl("txt23");
                TextBox txt24 = (TextBox)e.Item.FindControl("txt24");
                //Label lbl25 = (Label)e.Item.FindControl("lbl25");
                TextBox txt25 = (TextBox)e.Item.FindControl("txt25");

                lbl21.Text = row["Seq"].ToString();
                txt22.Text = row["Reason"].ToString();
                if (!string.IsNullOrEmpty(row["Price"].ToString()))
                {
                    txt23.Text = decimal.Parse(row["Price"].ToString()).ToString("N2");
                }
                txt24.Text = row["Quantity"].ToString();
                if (!string.IsNullOrEmpty(row["Total"].ToString()))
                {
                    txt25.Text = decimal.Parse(row["Total"].ToString()).ToString("N2");
                }

                //decimal price = 0;
                //int quantity = 0;
                //decimal.TryParse(txt23.Text, out price);
                //int.TryParse(txt24.Text, out quantity);
                //decimal subtotal = price * quantity;
                //decimal total = 0;
                //decimal.TryParse(txt25.Text, out total);

                //txt23.Text = price.ToString("N2");
                //lbl25.Text = subtotal.ToString("N2");
                //txt25.Text = total.ToString("N2");
            }
        }

        protected void btnAdd1_Click(object sender, ImageClickEventArgs e)
        {
            if (rpt1.Items.Count > 0)
            {
                Rpt1ToDt1();
            }

            Asso1.Rows.Add();
            this.rpt1.DataSource = Asso1;
            this.rpt1.DataBind();
        }

        protected void btnAdd2_Click(object sender, ImageClickEventArgs e)
        {
            if (rpt2.Items.Count > 0)
            {
                Rpt2ToDt2();
            }

            Asso2.Rows.Add();
            this.rpt2.DataSource = Asso2;
            this.rpt2.DataBind();
        }

        public string Validate()
        {
            return ValidateMain() + ValidateAsso();
        }

        private string ValidateMain()
        {
            StringBuilder output = new StringBuilder();

            if (string.IsNullOrEmpty(ddlType1.SelectedValue))
                output.Append("Type can not be blank.\\n");
            //if (string.IsNullOrEmpty(txtCostCenter.Text))
            //    output.Append("Cost Center can not be blank.\\n");
            if (string.IsNullOrEmpty(ddlBudgetApproved.SelectedValue))
                output.Append("Budget Approved can not be blank.\\n");
            //if (string.IsNullOrEmpty(txtBudgetValue.Text))
            //    output.Append("Budget Value can not be blank.<br/>");

            return output.ToString();
        }

        private string ValidateAsso()
        {
            string status = string.Empty;

            Rpt1ToDt1();
            Rpt2ToDt2();
            if (Asso1.Rows.Count == 0 && Asso2.Rows.Count == 0)
            {
                status = "Reason of request can not be blank.";
            }

            return status;
        }



        public void Rpt1ToDt1()
        {
            Asso1.Rows.Clear();
            int seq = 1;
            for (int i = 0; i < rpt1.Items.Count; i++)
            {
                RepeaterItem item = rpt1.Items[i];
                string reason = ((TextBox)item.FindControl("txt12")).Text;
                string description = ((TextBox)item.FindControl("txt13")).Text;
                string remark = ((TextBox)item.FindControl("txt14")).Text;
                if (!string.IsNullOrEmpty(reason))
                {
                    DataRow row = Asso1.Rows.Add();
                    row["Seq"] = seq.ToString();
                    row["Reason"] = reason;
                    row["Description"] = description;
                    row["Remark"] = remark;
                }
                seq += 1;
            }
        }

        public void Rpt2ToDt2()
        {
            Asso2.Rows.Clear();
            int seq = 1;
            for (int i = 0; i < rpt2.Items.Count; i++)
            {
                RepeaterItem item = rpt2.Items[i];
                string reason = ((TextBox)item.FindControl("txt22")).Text;
                string price = ((TextBox)item.FindControl("txt23")).Text;
                string quantity = ((TextBox)item.FindControl("txt24")).Text;
                string total = ((TextBox)item.FindControl("txt25")).Text;
                if (!string.IsNullOrEmpty(reason))
                {
                    DataRow row = Asso2.Rows.Add();
                    row["Seq"] = seq.ToString();
                    row["Reason"] = reason;
                    row["Price"] = price;
                    row["Quantity"] = quantity;
                    row["Total"] = total;
                }
                seq += 1;
            }
        }
    }
}