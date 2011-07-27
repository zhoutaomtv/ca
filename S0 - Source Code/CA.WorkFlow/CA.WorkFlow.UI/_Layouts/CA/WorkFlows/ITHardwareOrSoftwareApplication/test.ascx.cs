using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;

namespace CA.WorkFlow.UI._Layouts.CA.WorkFlows.ITHardwareOrSoftwareApplication
{
    public partial class test : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public DataTable DataTableRecord
        {
            get
            {
                if (ViewState["dtRecord"] == null)
                {
                    DataTableRecord = new DataTable();

                    DataTableRecord.Columns.Add("HardwareOrSoftwareName");
                    DataTableRecord.Columns.Add("Cost");
                }
                return (DataTable)ViewState["dtRecord"];
            }
            set
            {
                ViewState["dtRecord"] = value;
            }
        }

        private void BindDTRecord(Repeater rpt)
        {
            rpt.DataSource = this.DataTableRecord;
            rpt.DataBind();
        }

        protected void btnSure_Click(object sender, EventArgs e)
        {
            //foreach (DataRow row in this.DataTableRecord.Rows)
            //{

            //}
            //this.RepeaterDisplay.Visible = true;
            //BindDTRecord(this.RepeaterDisplay);
        }

        protected void imgbtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            if (this.rptItRequest.Items.Count > 0)
            {
                this.DataTableRecord.Rows.Clear();

                foreach (RepeaterItem item in this.rptItRequest.Items)
                {

                    HtmlInputText txtHardName = (HtmlInputText)item.FindControl("txtHardName");
                    HtmlInputText txtCost = (HtmlInputText)item.FindControl("txtCost");
                    DataRow row = this.DataTableRecord.NewRow();
                    row["HardwareOrSoftwareName"] = txtHardName.Value;
                    row["Cost"] = txtCost.Value;

                    this.DataTableRecord.Rows.Add(row);
                }
            }
            this.DataTableRecord.Rows.Add();
            BindDTRecord(this.rptItRequest);
        }

        protected void rptItRequest_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                this.DataTableRecord.Rows.Remove(DataTableRecord.Rows[e.Item.ItemIndex]);
                BindDTRecord(this.rptItRequest);
            }
        }

        protected void rptItRequest_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView row = e.Item.DataItem as DataRowView;
                if (row == null)
                    return;
                ((HtmlInputText)e.Item.FindControl("txtHardName")).Value = row["HardwareOrSoftwareName"] + "";
                ((HtmlInputText)e.Item.FindControl("txtCost")).Value = row["Cost"] + "";
            }
        }
    }
}