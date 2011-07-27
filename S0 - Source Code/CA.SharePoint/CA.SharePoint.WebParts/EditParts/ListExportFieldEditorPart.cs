using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using System.Collections.Specialized; 
using System.Xml;
using CA.SharePoint.WebPartSkin;

namespace CA.SharePoint.EditParts 
{
    /// <summary>
    ///  查询字段编辑控件
    /// </summary>
    public class ListExportFieldEditorPart : EditorPart
    {
        public ListExportFieldEditorPart()
        {

        }
 

        private FieldSelector _FieldSelector = new FieldSelector();

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            this.Title = WPResource.GetString("ListFieldEditorPart_ExportFieldEdit","Export Field");
           
            //ListQueryConditionWebPart wp = (ListQueryConditionWebPart)this.WebPartToEdit;

            //_FieldSelector.SetSelectedFields(wp.QueryFields, wp.List.Fields);
            this.Controls.Add(_FieldSelector);

            this.ChildControlsCreated = true;
        }
              

        public override bool ApplyChanges()
        {
            this.EnsureChildControls();              

            ListQueryConditionWebPart wp = (ListQueryConditionWebPart)WebPartToEdit;

            wp.ExportFields = _FieldSelector.GetSelectedFields();

            //wp.ReBuildQueryPanel();

            return true;
        }

        /// <summary>  
        /// 第一次加载控件时，或者提交修改时都会触发改方法。
        /// </summary>
        public override void SyncChanges()
        {
            this.EnsureChildControls();    

            ListQueryConditionWebPart wp = ((ListQueryConditionWebPart)WebPartToEdit) ;

            if( wp.List != null )
                _FieldSelector.SetSelectedFields(wp.ExportFields, wp.List.Fields);

            //this.Controls.Clear();
            //BuildBySelectedFields(fields);
        }

       
         

    }
}
