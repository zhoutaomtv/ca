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
using QuickFlow.UI.Controls;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint;
using CA.SharePoint.Utilities.Common;
using CA.SharePoint;

namespace CA.WorkFlow.UI
{
    public class QFUserControl:UserControl
    {
        private SPControlMode _ControlMode = SPControlMode.Invalid;
        public virtual SPControlMode ControlMode
        {
            get
            {
                return this._ControlMode;
            }
            set
            {
                this._ControlMode = value;
            }
        }

        private Employee _CurrentEmployee = null;
        public Employee CurrentEmployee
        {
            get
            {              
                if (_CurrentEmployee == null)
                {
                    this.ViewState["CA_CurrentEmployee"] = UserProfileUtil.GetEmployee(SPContext.Current.Web.CurrentUser.LoginName);
                    _CurrentEmployee = (Employee)this.ViewState["CA_CurrentEmployee"];
                }
                return _CurrentEmployee;
            }
        }

       protected override void OnLoad(EventArgs e)
        {
            SetControlMode();
            base.OnLoad(e);
        }

        public virtual void SetControlMode()
        {
            SetMode(this);            
        }

        protected virtual void SetMode(Control ctl)
        {
            if (ctl.Controls.Count > 0)
            {
                foreach (Control tmp in ctl.Controls)
                {
                    if (tmp is BaseFieldControl )
                    {
                        ((BaseFieldControl)tmp).ControlMode = this.ControlMode;
                        continue;
                    }
                    else if (tmp is FormComponent)
                    {
                        ((FormComponent)tmp).ControlMode = this.ControlMode;
                    }
                    else if (tmp is QFUserControl)
                    {
                        ((QFUserControl)tmp).ControlMode = this.ControlMode;
                    }
                }
            }         
        }

        protected void registerStartUpJS(Page page, Type type, string key, string script, bool addScriptTags)
        {
            ScriptManager.RegisterStartupScript(page, type, key, script, addScriptTags);
        }
    }
    
}
