using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CA.SharePoint;
using CodeArt.SharePoint.CamlQuery;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace CA.WorkFlow.UI.TimeOff
{
    using System.Linq;
    using SharePoint.Utilities.Common;

    public partial class MultiTable : QFUserControl,IValidator
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnReload.Attributes["style"] = "display: none";

            if (IsPostBack)
                return;
            switch (this.ControlMode)
            {
                case SPControlMode.New:
                    this.DataTableRecord.Rows.Add();
                    BindDTRecord(this.RepeaterEdit);
                    break;
                case SPControlMode.Edit:
                    FillRecord();
                    BindDTRecord(this.RepeaterEdit);
                    break;
                case SPControlMode.Display:
                    FillRecord();
                    BindDTRecord(this.RepeaterDisplay);
                    break;
            }
        }

        private bool _SureDataTable = false;

        public bool SureDataTable
        {
            get { return _SureDataTable; }
            set { _SureDataTable = value; }
        }

        public DataTable DataTableRecord
        {
            get
            {
                this.EnSureDataTable();
                return (DataTable)ViewState["dtRecord"];
            }
            set
            {
                ViewState["dtRecord"] = value;
            }
        }

        public override void SetControlMode()
        {
            base.SetControlMode();

            this.imgbtnAdd.Visible =
                this.RepeaterEdit.Visible = this.ControlMode != SPControlMode.Display;
            this.RepeaterDisplay.Visible = this.ControlMode == SPControlMode.Display;
        }

        protected void imgbtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            this.DataTableRecord.Rows.Add();
            this.BindDTRecord(this.RepeaterEdit);
        }

        private void EnSureDataTable()
        {
            if (ViewState["dtRecord"] == null)
            {
                DataTableRecord = new DataTable();

                DataTableRecord.Columns.Add("LeaveType");
                DataTableRecord.Columns.Add("DateFrom");
                DataTableRecord.Columns.Add("DateTo");
                DataTableRecord.Columns.Add("LeaveDays");
                DataTableRecord.Columns.Add("DateFromTime");
                DataTableRecord.Columns.Add("DateToTime");
                DataTableRecord.Columns.Add("Balance");

                DataTableRecord.Columns.Add("Status");
            }
            if (this.SureDataTable == true)
                return;

            this.SureDataTable = true;
            if (this.RepeaterEdit.Items.Count > 0)
            {
                this.DataTableRecord.Rows.Clear();

                foreach (RepeaterItem item in this.RepeaterEdit.Items)
                {
                    this.DataTableRecord.Rows.Add(ConvertToDataRow(item));
                }
            }
        }

        private DataRow ConvertToDataRow(RepeaterItem item)
        {
            CADateTimeControl caStartDate = (CADateTimeControl)item.FindControl("datetimeForm");
            CADateTimeControl caEndDate = (CADateTimeControl)item.FindControl("datetimeTo");

            TextBox txtDays = (TextBox)item.FindControl("txtDays");
            DDLLeaveType ddlType = (DDLLeaveType)item.FindControl("ddlType");
            DropDownList ddlFromTime = (DropDownList)item.FindControl("ddlFromTime");
            DropDownList ddlFromTo = (DropDownList)item.FindControl("ddlFromTo");

            DataRow row = this.DataTableRecord.NewRow();
            row["DateFrom"] = caStartDate.IsDateEmpty ? null : caStartDate.SelectedDate.ToShortDateString();
            row["DateFromTime"] = ddlFromTime.SelectedValue;
            row["DateTo"] = caEndDate.IsDateEmpty ? null : caEndDate.SelectedDate.ToShortDateString();
            row["DateToTime"] = ddlFromTo.SelectedValue;
            row["LeaveType"] = ddlType.SelectedValue;
            row["LeaveDays"] = txtDays.Text;

            return row;
        }

        protected void RepeaterEdit_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                if (this.RepeaterEdit.Items.Count == 1)
                {
                    string script = "alert('The record can not be empty!');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "message", script, true);
                    return;
                }
                this.DataTableRecord.Rows.Remove(DataTableRecord.Rows[e.Item.ItemIndex]);
                BindDTRecord(this.RepeaterEdit);
            }
        }

        protected void RepeaterEdit_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView row = e.Item.DataItem as DataRowView;
                if (row == null)
                    return;
                ((DDLLeaveType)e.Item.FindControl("ddlType")).SelectedValue = row["LeaveType"] + "";
                CADateTimeControl formTime = ((CADateTimeControl)e.Item.FindControl("datetimeForm"));
                formTime.SelectedDate =
                     string.IsNullOrEmpty(row["DateFrom"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(row["DateFrom"].ToString());
                formTime.OnValueChangeClientScript = string.Format("SumDate('{0}','2')", formTime.ClientID);
                CADateTimeControl toTime =
                ((CADateTimeControl)e.Item.FindControl("datetimeTo"));
                toTime.OnValueChangeClientScript = string.Format("SumDate('{0}','2')", toTime.ClientID);
                toTime.SelectedDate =
                   string.IsNullOrEmpty(row["DateTo"].ToString()) ? DateTime.MinValue : Convert.ToDateTime(row["DateTo"].ToString());
                ((DropDownList)e.Item.FindControl("ddlFromTime")).SelectedValue = row["DateFromTime"] + "";
                ((DropDownList)e.Item.FindControl("ddlFromTo")).SelectedValue = row["DateToTime"] + "";
                ((TextBox)e.Item.FindControl("txtDays")).Text = row["LeaveDays"] + "";
            }
        }

        private void BindDTRecord(Repeater rpt)
        {
            rpt.DataSource = this.DataTableRecord;
            rpt.DataBind();
        }

        protected void btnSure_Click(object sender, EventArgs e)
        {
            foreach (DataRow row in this.DataTableRecord.Rows)
            {
            }
            this.RepeaterDisplay.Visible = true;
            BindDTRecord(this.RepeaterDisplay);
        }

        private void FillRecord()
        {
            ISharePointService sps = ServiceFactory.GetSharePointService(true);
            SPList listRecord = sps.GetList(CAWorkFlowConstants.ListName.LeaveRecord.ToString());

            //根据field来查询
            QueryField field = new QueryField("WorkflowID", false);
            string strTimeOffNumber = SPContext.Current.ListItem["WorkFlowNumber"] + "";
            SPListItemCollection items = sps.Query(listRecord, field.Equal(strTimeOffNumber), 100);

            DataTable dt = items.GetDataTable();
            if (dt == null || dt.Rows == null)
                return;

            DataRow rowAdd = null;

            foreach (DataRow row in dt.Rows)
            {
                rowAdd = this.DataTableRecord.Rows.Add();
                string strType = row["LeaveType"] + "";

                rowAdd["LeaveType"] = strType;
                if (!string.IsNullOrEmpty(row["DateFrom"] + ""))
                {
                    rowAdd["DateFrom"] = Convert.ToDateTime(row["DateFrom"]).ToShortDateString() + "";
                }
                if (!string.IsNullOrEmpty(row["DateTo"] + ""))
                {
                    rowAdd["DateTo"] = Convert.ToDateTime(row["DateTo"]).ToShortDateString() + "";
                }
                rowAdd["LeaveDays"] = row["LeaveDays"] + "";
                rowAdd["DateFromTime"] = row["DateFromTime"] + "";
                rowAdd["DateToTime"] = row["DateToTime"] + "";

                rowAdd["Status"] = row["Status"] + "";
                rowAdd = null;
            }
        }

        #region IValidator Members

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Page.Validators.Add(this);
        }

        private string _ErrorMessage = "Please supply valid  details.";

        public string ErrorMessage
        {
            get
            {
                return _ErrorMessage;
            }
            set
            {
                _ErrorMessage = value;
            }
        }

        private bool _IsValid=true;

        public bool IsValid
        {
            get
            {
                return _IsValid;
            }
            set
            {
                this._IsValid = value;
            }
        }

        public void Validate()
        {
            if (this.ControlMode == SPControlMode.Display)
            { this.IsValid = true; return; }

            for (int i = 0; i < this.RepeaterEdit.Items.Count; i++)
            {
                RepeaterItem item = RepeaterEdit.Items[i];

                CADateTimeControl caStartDate = (CADateTimeControl)item.FindControl("datetimeForm");
                CADateTimeControl caEndDate = (CADateTimeControl)item.FindControl("datetimeTo");
                TextBox txtDays = (TextBox)item.FindControl("txtDays");
                double dbValue=0;
                if (caStartDate.IsDateEmpty || caEndDate.IsDateEmpty || !double.TryParse(txtDays.Text,out dbValue))
                {
                    this.IsValid = false;
                    return;
                }
            }
            this.IsValid = true;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            if (!IsValid)
            {
                string script = "alert('" + ErrorMessage + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "message", script, true);
                //writer.Write(this.ErrorMessage);
            }
        }

        #endregion

        protected void btnReload_Click(object sender, EventArgs e)
        {
            string mixedValues = this.hidMixedDates.Value;

            if (mixedValues.IsNotNullOrWhitespace())
            {
                var values = mixedValues.Split('|');

                if (values.Length == 5)
                {
                    DateTime startDate;
                    DateTime endDate;

                    if (DateTime.TryParse(values[1], out startDate) && DateTime.TryParse(values[2], out endDate) && endDate >= startDate)
                    {
                        var tb = this.GetTextBoxByClientId(values[0]);
                        tb.Text = WorkFlowUtil.GetMixedDays(startDate, endDate, values[3], values[4]).ToString();
                    }
                }
            }
        }

        private TextBox GetTextBoxByClientId(string clientId)
        {
            return (from RepeaterItem row in this.RepeaterEdit.Items select row.FindControl("txtDays") as TextBox).FirstOrDefault(tb => tb != null && tb.ClientID.Equals(clientId));
        }
    }
}