using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;
using CA.SharePoint.Utilities.Common;
using CA.SharePoint;
using CodeArt.SharePoint.CamlQuery;
using QuickFlow.Core;


namespace CA.WorkFlow.UI.TimeOff
{
    public partial class DataFormNew : QFUserControl
    {
        public struct Constants
        {
            public const string WorkflowName = "Leave Application Workflow";
            public const string CancelWorkflowName = "Leave Application Cancel Workflow";
            public const string HeadApproveStep = "DepartmentHeaderApprove";
            public const string HREeviewStep = "HRReview";
            public const string HRHeadApproveStep = "HRHeadApprove";
        }

        private Employee _Applicant;
        public Employee Applicant
        {
            get { return _Applicant; }
            set { _Applicant = value; }
        }

        public string ApplicantName
        {
            get
            {
                if (this.ControlMode == SPControlMode.New)
                {
                   // return this.Applicant.DisplayName;
                    return this.Applicant.PreferredName;
                }
                else
                {
                    return SPContext.Current.ListItem["Applicant"] + "";
                }
            }
        }
        
        public double Balance
        {
            get
            {
                return Convert.ToDouble( ViewState["Balance"]);
            }
            set
            {
                ViewState["Balance"] = value;
            }
        }

        public double SickBalance
        {
            get
            {
                return  Convert.ToDouble(ViewState["SickBalance"]);
            }
            set
            {
                ViewState["SickBalance"] = value;
            }
        }

        private double _SickLeaveBalance;
        public double SickLeaveBalance
        {
            get
            {
                this.EnsureGetValue();
                return _SickLeaveBalance;
            }
            set { this._SickLeaveBalance = value; }
        }

        //
        private double _LeaveBalance;
        public double LeaveBalance
        {
            get
            {
                this.EnsureGetValue();
                return _LeaveBalance;
            }
            set { this._LeaveBalance = value; }
        }    

        private bool _IsSickLeave = false;
        public bool IsSickLeave
        {
            get
            {
                this.EnsureGetValue();
                return this._IsSickLeave;
            }
            set
            {
                this._IsSickLeave = value;
            }
        }

        public string EmployeeNo
        {
            get
            {
                return this.txtEmplyeeNum.Text.Trim();
            }
        }

        private bool _IsEnsure = false;
        public bool IsEnsure
        {
            get { return _IsEnsure; }
            set { _IsEnsure = value; }
        }

        private void EnsureGetValue()
        {
            if (IsEnsure)
                return;
            this._LeaveBalance = 0;
            this._SickLeaveBalance = 0;
            foreach (DataRow row in this.DataTableRecord.Rows)
            {
                string strType = row["LeaveType"] + "";
                string strDays = row["LeaveDays"]+"";
                if (!string.IsNullOrEmpty(strDays))
                {
                    double dbDays = Convert.ToDouble(strDays);
                    if (strType == "Annual Leave 年假")
                    {
                        _LeaveBalance += dbDays;
                    }
                    else
                    {
                        this.IsSickLeave = true;
                        if (strType == "Sick Leave 病假")
                        {
                            this._SickLeaveBalance += dbDays;                            
                        }
                    }
                }
            }
            this.IsEnsure = true;
        }

        public string CurrentYear
        {
            get
            {
                return DateTime.Now.Year.ToString();                    
            }
        }

        private bool _AttachmentVisible;
        public bool AttachmentVisible
        {
            set
            {
                this.trAttachment.Visible = false;//目前不需要附件功能，所以始终visable掉
                //this.trAttachment.Visible = value;
                this._AttachmentVisible = value;
            }          
        }     
        
        public override void SetControlMode()
        {
            base.SetControlMode();
            if (this.ControlMode == SPControlMode.New)
            {
                //用来控制新建的时候给 formfield赋值 省去代码操作列表赋值
                this.tableNew.Visible = true;
                this.tableDisplay.Style.Add("display", "none");
                this.formFieldNumber.Visible = false;
                this.naSpan.Visible = true;
            }
            else if(this.ControlMode==SPControlMode.Edit)
            {
                this.tableNew.Visible = true;
                this.tableDisplay.Style.Add("display", "none");
                this.formFieldNumber.Visible = false;
                this.naSpan.Visible = true;
                if (!this.Page.IsPostBack)
                {
                    this.txtEmplyeeNum.Text = SPContext.Current.ListItem["EmployeeID"] + "";
                }
            }
            else if (this.ControlMode == SPControlMode.Display)
            {
                this.tableNew.Visible = false;
      
                if (WorkflowContext.Current.Mode ==QuickFlow.Core.ContextMode.View  || WorkflowContext.Current.Task.Title != "HR Review")
                {                    
                    this.attacthment.ControlMode = SPControlMode.Display;                  
                }
                else if (WorkflowContext.Current.Task.Title == "HR Review")
                {
                    if(SPContext.Current.ListItem.Attachments.Count==0)
                    this.attacthment.ControlMode = SPControlMode.New;  
                    else
                        this.attacthment.ControlMode = SPControlMode.Edit;  
                }
            }
            this.formFieldNumber.ControlMode = SPControlMode.Display;
        }   

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.Applicant = this.CurrentEmployee;
                FillBalanceData();//this must on the top,beacause this fill the balance
                if (this.ControlMode != SPControlMode.Display)
                {
                    FillEmployeeData(this.Applicant);
                }                                     
            }
            else
            {
                this.CAPeopleFinder1.Load += new EventHandler(CAPeopleFinder1_Load);
            }
        }

        /// <summary>
        /// 用来处理选人事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CAPeopleFinder1_Load(object sender, EventArgs e)
        {
            string userAccount = SPContext.Current.Web.CurrentUser.LoginName;
            if (this.CAPeopleFinder1.Accounts.Count > 0)
            {
                userAccount = this.CAPeopleFinder1.Accounts[0]+"";
            }
            this.Applicant = UserProfileUtil.GetEmployee(userAccount);
            FillEmployeeData(this.Applicant);
            FillBalanceData();
        }

        /// <summary>
        /// 给页面员工基本信息赋值
        /// </summary>
        /// <param name="employee"></param>
        private void FillEmployeeData(Employee employee)
        {
            this.labPeople.Text = employee.DisplayName;
            this.labDepartment.Text = employee.Department;
            this.labPostion.Text = employee.Title;
            //this.labEmployeeNO.Text = employee.EmployeeID;

            this.formFieldApplicant.Value = employee.PreferredName; 
            this.formFieldDeptment.Value = employee.Department;
            this.formFieldJobTitle.Value = employee.Title;
            //this.formFieldEmployeeID.Value = employee.EmployeeID;               
        }

        /// <summary>
        /// 给页面赋值
        /// </summary>
        private void FillBalanceData()
        {
            //得到sharepointservice 用来进行sharepoint API操作
             ISharePointService sps = ServiceFactory.GetSharePointService(true);

            //获取列表
            SPList listBalance = sps.GetList(CAWorkFlowConstants.ListName.LeaveBalance.ToString());

            //根据field来查询
            QueryField field=new QueryField("Employee");
            QueryField field2 =new QueryField("Year");
            SPListItemCollection items = sps.Query(listBalance, field.Equal(this.ApplicantName) && field2.Equal(DateTime.Now.Year), 1);
                        
            //如果有值
            if (items.Count > 0)
            {
                this.labAnnulLeave.Text = items[0]["AnnualBalance"] + "";
                this.labSickLeave.Text = items[0]["SickBalance"] + "";
                this.labAnnualEntitlement.Text = items[0]["AnnualEntitlement"] + "";
                this.labSickEntitlement.Text = items[0]["SickEntitlement"] + "";

                this.Balance =  Convert.ToDouble(items[0]["AnnualBalance"]);
                this.SickBalance =  Convert.ToDouble(items[0]["SickBalance"]);
            }
            else
            {
                this.labAnnulLeave.Text = "0";
                this.labSickLeave.Text = "0";
                this.labAnnualEntitlement.Text = "0";
                this.labSickEntitlement.Text = "0";
            }
        }      

        /// <summary>
        /// 将动态表暂存在viewstate中，提交时候统一插入List
        /// </summary>
        public DataTable DataTableRecord
        {
            get
            {
                return this.MultiTable1.DataTableRecord;            
            }           
        }          
    }
}