using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CA.SharePoint;
using Microsoft.SharePoint;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI;
using FredCK.FCKeditorV2;
using Microsoft.SharePoint.WebPartPages;


namespace CA.SharePoint
{
    public class FckMossEditor : BaseSPWebPart, IWebEditable
    {
        public FckMossEditor()
        {
            this.ExportMode = WebPartExportMode.All;
        }

        private string displayText = "Hello World!";
        [WebBrowsable(true), Personalizable(true)]
        public string DisplayText
        {
            get { return displayText; }
            set { displayText = value; }
        }
        protected override void RenderContents(System.Web.UI.HtmlTextWriter writer)
        {
            writer.Write(displayText);
            base.RenderContents(writer);
        }

        #region IWebEditable Members
        EditorPartCollection IWebEditable.CreateEditorParts()
        {
            List<HtmlEditor> editors = new List<HtmlEditor>();
            editors.Add(new HtmlEditor());
            return new EditorPartCollection(editors);
            
        }
        object IWebEditable.WebBrowsableObject
        {
            get { return this; }
        }
        #endregion

    }

    class HtmlEditor : EditorPart
    {
        private FCKeditor htmlContentTxt;
        public HtmlEditor()
        {
            this.ID = "HtmlEditor";
        }
        protected override void CreateChildControls()
        {
            htmlContentTxt = new FCKeditor();
            htmlContentTxt.ImageBrowserURL = "/fckeditor/fileUpload.aspx";
            htmlContentTxt.ToolbarSet = "MyToolbar";
            htmlContentTxt.Width = 525;
            htmlContentTxt.Height = 500;
            Controls.Add(htmlContentTxt);
        }
        public override bool ApplyChanges()
        {
            EnsureChildControls();
            FckMossEditor part = WebPartToEdit as FckMossEditor;
            if (part != null)
            {
                part.DisplayText = htmlContentTxt.Value;
            }
            else
            {
                return false;
            }
            return true;

        }
        public override void SyncChanges()
        {
            EnsureChildControls();
            FckMossEditor part = WebPartToEdit as FckMossEditor;
            if (part != null)
            {
                htmlContentTxt.Value = part.DisplayText;
            }

        }

    }

}
