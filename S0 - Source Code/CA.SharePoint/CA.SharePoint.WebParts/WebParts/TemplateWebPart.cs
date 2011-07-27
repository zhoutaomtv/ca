using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace CA.SharePoint.WebParts
{
    public class TemplateWebPart:BaseSPWebPart
    {
        ControlTemplateManager _TemplateManager;
        /// <summary>
        /// 模板管理类
        /// </summary>
        protected ControlTemplateManager TemplateManager
        {
            get
            {
                if (_TemplateManager == null)
                    _TemplateManager = ControlTemplateManager.GetIntance(this.Page);

                return _TemplateManager;
            }
        }

        protected Control GetTemplateControl(string controlId)
        {
            string template = this.TemplateName;

            if (String.IsNullOrEmpty(template))
            {
                throw new Exception("DefaultTemplateName no setting.");
            }

            return TemplateManager.GetTemplateControl(template);
        }

        /// <summary>
        /// 默认模板名--用户控件名
        /// </summary>
        protected virtual string DefaultTemplateName
        {
            get
            {
                return "";
            }
        }

        private string _TemplateName;
        public string TemplateName
        {
            get
            {
                if (String.IsNullOrEmpty(_TemplateName))
                    return this.DefaultTemplateName;
                else
                    return _TemplateName;
            }
            set { _TemplateName = value; }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            this.Controls.Clear();
            Control _control = GetTemplateControl( this.TemplateName);
            this.Controls.Add(_control);
        }
    }
}
