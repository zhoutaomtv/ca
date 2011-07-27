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

namespace CA.WorkFlow.UI._Layouts.CA.WorkFlows.TimeOff
{
    public partial class Test : System.Web.UI.Page
    {
        protected global::System.Web.UI.WebControls.ImageButton imgbtnAdd;

        protected global::System.Web.UI.HtmlControls.HtmlTableRow trTemplate;

        public void Val2(object sender, ServerValidateEventArgs e)
        {


            e.IsValid = false;
        }

      

    
        public DataTable dt
        {
            get
            {
               
                    return (DataTable)ViewState["dt"];
               
            }
            set
            {
                ViewState["dt"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                txt_hidden.Text = "init hidden textbox";

            }
          //   string num = "TimeOff_" + WorkFlowUtil.CreateWorkFlowNumber("TimeOff").ToString("000");
        

            this.imgbtnAdd.Click += new ImageClickEventHandler(imgbtnAdd_Click);
            this.Button1.Click+=new EventHandler(Button1_Click);
            //this.imgbtnSubmit.Click += new ImageClickEventHandler(imgbtnSubmit_Click);

            if (IsPostBack)
                return;
            dt = new DataTable();
            dt.Columns.Add("Name");
            dt.Columns.Add("ID");

            DataRow row = null;
            row = dt.Rows.Add();
            row["Name"] = "caixiang";
            row["ID"] = "1";

            row = dt.Rows.Add();
            row["ID"] = "2";
            row["Name"] = "caixiang2";


            this.Repeater1.DataSource = dt;
            this.Repeater1.DataBind();

            //this.gvTable.DataSource = dt;
            //this.gvTable.DataBind();

          

        }

        void imgbtnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            //this.trTemplate.Visible = false;
            //this.imgbtnAdd.Enabled = true;
            DataRow row = this.dt.Rows.Add();
            row["Name"] = this.txtName.Text;
            this.Repeater1.DataSource = dt;
            this.Repeater1.DataBind();
            //row["ID"]=
        }

        void imgbtnAdd_Click(object sender, ImageClickEventArgs e)
        {
            this.trTemplate.Visible = true;
            this.imgbtnAdd.Enabled = false;
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            RepeaterItemCollection items= this.Repeater1.Items;
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "delete")
            {
                dt.Rows.Remove(dt.Rows[0]);
                this.Repeater1.DataSource = dt;
                this.Repeater1.DataBind();
            }
        }

        protected void imgbtnSubmit_Click1(object sender, ImageClickEventArgs e)
        {

        }
    }
}
