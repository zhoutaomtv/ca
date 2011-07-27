
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Reflection;
using System.Xml;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;
//using Microsoft.SharePoint.Publishing;

using CA.SharePoint.WebPartSkin;
using CA.SharePoint.EditParts;

namespace CA.SharePoint
{
    /// <summary>
    /// 列表表单 webpart
    /// </summary>
    public class ListFormWebPart : BaseSPListWebPart 
    {
        private string _redirectUrl = "";
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [WebDisplayName("Redirect Url After Submit（Default Current Page）")]
        public virtual string RedirectUrl
        {
            get { return _redirectUrl; }
            set { _redirectUrl = value; }
        }

        private string _AfterSubmitMessage = "";
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [WebDisplayName("Display Message After Submit")]
        public virtual string AfterSubmitMessage
        {
            get { return _AfterSubmitMessage; }
            set { _AfterSubmitMessage = value; }
        }

        private string _TemplateName = "ListForm";
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [WebDisplayName("Display Template ID")]
        public virtual string RenderingTemplateName
        {
            get { return _TemplateName; }
            set { _TemplateName = value; }			    
        }

        // ~/_CONTROLTEMPLATES/MCS_SurveyForm.ascx
        private string _ControlPath;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [WebDisplayName("Control Template Path")]
        public virtual string ControlTemplatePath
        {
            get { return _ControlPath; }
            set { _ControlPath = value; } 
        }

        private SPControlMode _FormMode = SPControlMode.Invalid;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [WebDisplayName("Control Mode")]
        public virtual SPControlMode FormMode
        {
            get { return _FormMode; }
            set { _FormMode = value; }
        }

        private int _ItemId = 0 ;
        //[Personalizable(PersonalizationScope.Shared)]
        //[WebBrowsable]
        //[WebDisplayName("List Item ID")] 
        private int CurListItemId
        {
            get { return _ItemId; }
            set { _ItemId = value; }
        }
        

        public event ItemUpdatingEventHandler ItemUpdating;

        private SPContext m_itemContext;
        [WebPartStorage(Storage.None), Browsable(false)]
        public SPContext FormContext
        {
            get
            {
                EnsureFormContext();
                return this.m_itemContext;
            }
            set
            {
                this.m_itemContext = value;
            }
        }

        void EnsureFormContext()
        {
            if ((this.m_itemContext == null) && (this.Context != null))
            {
                if (base.List == null)
                {
                    throw new SPException(SPResource.GetString("ListWithTitleDoesNotExist", new object[] { base.ListName }));
                }

                if (Page.Request.QueryString["ID"] != null)
                {
                    //if (this.CurListItemId <= 0)
                    this.CurListItemId = Convert.ToInt32(Page.Request.QueryString["ID"]);

                    if (this.FormMode == SPControlMode.Invalid)  
                        this.FormMode = SPControlMode.Display ;
                }
                else
                {
                    if (this.FormMode == SPControlMode.Invalid)
                        this.FormMode = SPControlMode.New ;
                }

                this.m_itemContext = SPContext.GetContext(this.Context, this.CurListItemId, this.List.ID, base.Web);

                if (this._FormMode == SPControlMode.Invalid)
                {
                    this._FormMode = this.m_itemContext.FormContext.FormMode;
                }
                else
                {
                    this.m_itemContext.FormContext.FormMode = this.FormMode;
                    //this.m_itemContext.FormContext.DisableInitialFocus = this.DisableInitialFocus;
                    this.m_itemContext.FormContext.SetFormMode(this.FormMode, true);
                }
            }    
        }


        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            if (List == null)
            {
                base.RegisterShowToolPanelControl("Please", "open Tools panel", "，configure “List Name”。");
                return;
            }  

			EnsureFormContext();

			if (this.List.BaseType == SPBaseType.DocumentLibrary && this.FormMode == SPControlMode.New)
			{
				this.Controls.Add(new LiteralControl("ListFormWebPart can not be on New page in document library"));
				return;
			}

            //this.Controls.Clear();

            if (!String.IsNullOrEmpty(this.ControlTemplatePath))
                this.CreateChildByControlTemplate();
            else if (!String.IsNullOrEmpty(this.RenderingTemplateName))
                this.CreateChildByRenderingTemplate();
            else
            {
                base.RegisterShowToolPanelControl("Please", "open Tools panel", "，configure “Display Template” or “Control Template”。");
            }
        }

        void CreateChildByControlTemplate()
        {
             try
            {
                Control templateCtl = Page.LoadControl( this.ControlTemplatePath ) ;

                //通过控制父FormComponent的属性控制子FormField的属性
                 //子FormField控件会自动获取父的相应属性
                UserControlTemplateContainer container = new UserControlTemplateContainer();
                container.RenderContext = this.FormContext;
                container.ControlMode = this.FormMode;
                container.ItemId = this.CurListItemId;

                container.Controls.Add(templateCtl);
                this.Controls.Add( container );

                return;

                //foreach (Control c in templateCtl.Controls)
                //{
                //    if (c is FormComponent)
                //    {
                //        FormComponent field = (FormComponent)c;
                //        field.ControlMode = this.FormMode;
                //        field.ItemId = this.ListItemId;
                //        field.RenderContext = this.ItemContext;
                //    }                   
                //    else if (c is TemplateBasedControl)
                //    {
                //        TemplateBasedControl template = (TemplateBasedControl)c;

                //        template.RenderContext = this.ItemContext;
                //    }
                      
                //}

                //this.Controls.Add(templateCtl);
            }
            catch (Exception ex)
            {
                base.RegisterError(ex);
            }
        }

        protected override void EnsureChildControls()
        {
            try
            {
                base.EnsureChildControls();
            }
            catch (Exception ex)
            {
                base.RegisterError(ex);
            }
        }

        void CreateChildByRenderingTemplate()
        {
            try
            {
                Microsoft.SharePoint.WebPartPages.ListFormWebPart form = new Microsoft.SharePoint.WebPartPages.ListFormWebPart();

                form.TemplateName = this.RenderingTemplateName;

                form.ID = this.ID + "_Form";
                form.Title = this.Title;
                form.ListName = List.ID.ToString("B").ToUpper();

                form.ControlMode = this.FormMode;

                form.ChromeType = PartChromeType.None; //不显示标题

                form.ListItemId = this.CurListItemId;

                form.DisableInitialFocus = true;

                //form.TemplateControl.FindControl("");

                this.Controls.Add(form);

            }
            catch (Exception ex)
            {
                base.RegisterError(ex);
            }
        }       

        protected override void OnLoad(EventArgs e)
        {
             try
            {
                base.OnLoad(e);
            }
            catch (Exception ex)
            {
                base.RegisterError(ex);
            }
        }

        const string IsFormSaved_Context_Key = "ListFormWebPart_IsFormSaved";
        static public bool IsFormSaved
        {
            set
            {
                if (HttpContext.Current == null)
                    throw new NotSupportedException("HttpContext is null");
                HttpContext.Current.Items[IsFormSaved_Context_Key] = value ;
            }
            get
            {               
                if (HttpContext.Current == null)
                    throw new NotSupportedException("HttpContext is null");

                if (HttpContext.Current.Items.Contains(IsFormSaved_Context_Key))
                    return Convert.ToBoolean(HttpContext.Current.Items[IsFormSaved_Context_Key]);
                else
                    return false;                 
            }            
        }

        /// <summary>
        /// 处理提交成功事件 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            if (!(args is CommandEventArgs)) return false;

            CommandEventArgs carg = args as CommandEventArgs ;

            if (carg.CommandName.ToLower() != "saveitem") return false;

            if (!ListFormWebPart.IsFormSaved)
            {
                this.Page.Validate();

                if (!this.Page.IsValid)
                {
                    return true;
                }

                try
                {
                    SaveData();
                }
                catch (Exception ex)
                {
                    CA.Web.Script.Alert(this, ex.Message);
                    return true;
                }
            }

            string url = this.RedirectUrl;

            if( String.IsNullOrEmpty(url) )
                url = Page.Request.RawUrl ;

            if (!String.IsNullOrEmpty(this.AfterSubmitMessage))
            {
                CA.Web.Script.Alert(this, this.AfterSubmitMessage);

                CA.Web.Script.Redirect(this, url);
            }
            else
            {
                Page.Response.Redirect(url);
            }

            return true;
        }

        protected virtual void  SaveData()
        {
            SPListItem listItem;

            //if (this.FormMode == SPControlMode.New)
            //    listItem = this.List.Items.Add();
            //else
                listItem = this.FormContext.ListItem;

            foreach (BaseFieldControl f in this.FormContext.FormContext.FieldControlCollection)
            {
                try
                {
                   if (!f.Field.ReadOnlyField)
                       f.UpdateFieldValueInItem();
                 
                    //    listItem[f.FieldName] = f.Value;
                }
                catch  { }
            }

            if( this.ItemUpdating != null )
                this.ItemUpdating( this , new ItemUpdatingEventArgs( listItem ) );

            listItem.Update();
        }

    }

    public delegate void ItemUpdatingEventHandler(object sender, ItemUpdatingEventArgs e );

    public class ItemUpdatingEventArgs : EventArgs
    {
        public ItemUpdatingEventArgs( SPListItem item )
        {
            ListItem = item;
        }

        public readonly SPListItem ListItem;
       // public readonly SPControlMode FormMode;
    }

    public class UserControlTemplateContainer : FormComponent 
    {

    }
}
