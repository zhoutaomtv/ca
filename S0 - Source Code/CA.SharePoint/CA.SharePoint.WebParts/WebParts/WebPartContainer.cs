using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using System.ComponentModel;
using System.Web.UI;

namespace CA.SharePoint
{
    /// <summary>
    /// Use to load the UserControl(*.ascx)
    /// </summary>    
    public class WebPartContainer : BaseSPWebPart
    {
        public WebPartContainer()
        {
            //default folder path
            this._userControlPath = "/_layouts/CA/WebControls";
        }

        private UserControl _control;                   //load the specify ascx file to the object
        private string _userControlPath;                //default folder path
        private string _userControl = string.Empty;     //specify ascx file path

        [WebBrowsable(false), DefaultValue("/_layouts/CA/WebControls")]
        public string UserControlPath
        {
            get
            {
                return this._userControlPath;
            }
            set
            {
                this._userControlPath = value;
            }
        }

        /// <summary>
        /// Custom property for webpart
        /// User can spcify file path from the property
        /// </summary>
        [WebBrowsable(true), Personalizable(true)]
        public string UserControl
        {
            get
            {
                return this._userControl;
            }
            set
            {
                this._userControl = value;
            }
        }

        /// <summary>
        /// load the spcify ascx file to page
        /// </summary>
        protected internal void LoadUserControl()
        {
            //when user spcify file
            if (!string.IsNullOrEmpty(this._userControl))
            {
                try
                {
                    base.CreateChildControls();
                    this.Controls.Clear();
                    this._control = (UserControl)Page.LoadControl(this._userControlPath + "/" + this._userControl);
                    this.Controls.Add(this._control);
                }
                catch (System.Threading.ThreadAbortException tae)
                {
                    //When exporting the excel, this exception will come up, and never mind it won't affect the system.               
                }
                catch (Exception ex)
                {
                    this.Page.Response.Write(ex.ToString());
                    throw ex;
                }
            }
        }

        protected override void CreateChildControls()
        {
            LoadUserControl();
        }
    }
}
