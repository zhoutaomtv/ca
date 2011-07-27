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
using CA.SharePoint.EventReveivers;
using CA.SharePoint.Utilities;
using CA.Web;

namespace CA.SharePoint.Web
{
    public partial class DocumentLimit : LayoutsPageBase
    {
        public SPList CurrentList
        {
            get
            {
                return SPContext.Current.List;
            }
        }

        public string ListName
        {
            get
            {
                return SPContext.Current.List.Title;
            }
        }

        public string RedirectUrl
        {
            get
            {
                return SPContext.Current.Web.Url + "/_layouts/listedit.aspx?List=" + CurrentList.ID;
            }
        }      

        protected override bool RequireSiteAdministrator
        {
            get
            {
                return true;
            }
        }

        private Script _Script;

        public Script Script
        {
            get
            {
                if (_Script == null)
                {
                    _Script = new Script(this.Page);
                }
                return _Script;
            }
            set { _Script = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SPEventReceiverDefinition def = EventReceiverManager.GetEventDefinition(CurrentList, typeof(EHDocumentLimit), SPEventReceiverType.ItemAdding);
                if (def != null)
                {
                    this.txtFileLimit.Text = def.Data;
                }
            }
            this.btnSure.Click += new EventHandler(btnSure_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
        } 

        void btnSure_Click(object sender, EventArgs e)
        {          
            Type receiverType = typeof(EHDocumentLimit);
          
            string strSize = this.txtFileLimit.Text.Trim();
            double dbSize;

            if (!Double.TryParse(strSize, out dbSize))
            {
                this.Script.Alert("Please enter a double value!");
                EventReceiverManager.RemoveEventReceivers(CurrentList, receiverType);
            }
            else
            {
                EventReceiverManager.SetEventReceivers(CurrentList, receiverType, strSize, SPEventReceiverType.ItemAdding);
            }

            Response.Redirect(this.RedirectUrl);
            
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(this.RedirectUrl);
        } 

      
    }
}
