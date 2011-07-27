using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.Data;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Administration;
//using Microsoft.SharePoint.WebPartPages;
using Microsoft.Office.Server;
using Microsoft.Office.Server.Search.Query;

using CA.Web; 
using CA.SharePoint.CamlQuery;

namespace CA.SharePoint
{    

    public class ListQueryWebPart : BaseSPListWebPart , IPropertyPersistenceService
    {
        private Button _QueryBtn = new Button ();
        private Panel _QueryResultPanel = new Panel ();
        private RadioButton _AndBtn = new RadioButton();
        private RadioButton _OrBtn = new RadioButton();

        Microsoft.SharePoint.WebPartPages.ListViewWebPart _RenderWp = new Microsoft.SharePoint.WebPartPages.ListViewWebPart();

        private IList<IQueryControl> _KeyWorkControls = new List<IQueryControl>();

       

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.EnableViewState = true;
            this.EnsureChildControls();
        }        

        void AddHtml(string html)
        {
            this.Controls.Add( new LiteralControl( html ) );
        }
         
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            this.Controls.Clear();
            _KeyWorkControls.Clear();

            AddHtml("<table border='0' width='100%'>");

            AddHtml("<tr><td width='18%' valign='top'>");

            buildQueryPanel();

            AddHtml("</td><td valign='top'>");

            this.Controls.Add(_QueryResultPanel);

            AddHtml("</td></tr>");            

            AddHtml("</table>");

            //vp1.WebId = base.GetCurrentSPWeb().ID;
            _RenderWp.ID = "RenderWp";
            //_RenderWp.Title = this.List.Title;
            _RenderWp.ListName = "{" + List.ID.ToString().ToUpper() + "}"; // "{F199F086-F753-4A27-94A7-690F7C163AC5}"; //   //
 
            //vp1.ViewGuid = list.DefaultView.ID.ToString();

            string viewXml = this.PreListViewXml;

            if (viewXml != "")
                _RenderWp.ListViewXml = viewXml;
            else
                _RenderWp.ListViewXml = base.CurrentView.HtmlSchemaXml;             

            //_RenderWp.FrameState = Microsoft.SharePoint.WebPartPages.FrameState.Normal;
            //_RenderWp.FrameType = Microsoft.SharePoint.WebPartPages.FrameType.None ;//.None;
            _RenderWp.EnableViewState = true;

            _RenderWp.ChromeType = PartChromeType.None; //不显示标题

            //vp1.DetailLink = "/Lists/List/AllItems.aspx";
            //vp1.ExportControlledProperties = false;
            //vp1.IsIncluded = true;

            _QueryResultPanel.Controls.Add(_RenderWp);

            this.ChildControlsCreated = true;
        }

        protected string PreListViewXml
        {
            get
            {
                return ViewState["PreListViewXml"] != null ? (String)ViewState["PreListViewXml"] : "" ;
            }
            set
            {
                ViewState["PreListViewXml"] = value ;
            }
        }

        //生成查询输入控件
        void buildQueryPanel()
        {
            AddHtml("<table>");

            foreach (string fName in this.CurrentView.ViewFields)
            {
                SPField f = List.Fields.GetField(fName);

                IQueryControl qCtl = QueryControlFactory.GetQueryControl(f,this);
                qCtl.FieldName = fName;
                _KeyWorkControls.Add(qCtl);

                Control ctl = (Control)qCtl;

                Label lb = new Label();
                lb.Text = f.Title;

                AddHtml("<tr><td nowrap>");
                this.Controls.Add(lb);
                AddHtml("</td><td>");
                this.Controls.Add(ctl);
                AddHtml("</td></tr>");
            }

            _AndBtn.Text = "And";
            _AndBtn.Checked = true;
            _AndBtn.GroupName = this.ClientID;
            _OrBtn.Text = "Or";
            _OrBtn.GroupName = this.ClientID;

            AddHtml("<tr><td>");
             
            AddHtml("</td><td>");

            this.Controls.Add(_AndBtn);
            this.Controls.Add(_OrBtn);

            AddHtml("</td></tr>");

            AddHtml("</table>");

            _QueryBtn.Text = "Search";
            _QueryBtn.CssClass = "ms-ButtonHeightWidth";
            _QueryBtn.Click += new EventHandler(_QueryBtn_Click);
            this.Controls.Add(_QueryBtn);
        }

         
        void buildCaml()
        {
            CAMLExpression<object> expr = null;

            for (int i = 0; i < _KeyWorkControls.Count; i++)
            {
                IQueryControl wd = _KeyWorkControls[i];

                CAMLExpression<object> tempExpr = wd.QueryExpression;

                if (tempExpr == null)
                    continue;

                if (expr == null)
                    expr = tempExpr;
                else
                {
                    if (_AndBtn.Checked)
                        expr = expr & tempExpr;
                    else
                        expr = expr | tempExpr;
                }
            }

            try
            {
                string html = "";
                if (expr == null)
                {
                    // html = this.RenderView.RenderAsHtml();
                    _RenderWp.ListViewXml = this.CurrentView.HtmlSchemaXml;
                    return;
                }
                else
                {
                    //SPQuery q = new SPQuery( this.RenderView ) ;

                    //q.Query = CAMLBuilder.Where(expr);

                    //this.RenderView.Toolbar = "Standard";

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(this.CurrentView.HtmlSchemaXml);

                    XmlNode queryNode = doc.DocumentElement.SelectSingleNode("Query");
                    queryNode.InnerXml = CAMLBuilder.Where(expr);
                    
                    _RenderWp.ListViewXml = doc.InnerXml;

                    PreListViewXml = _RenderWp.ListViewXml;

                    // html = List.RenderAsHtml(q);

                    foreach (WebPart wp in this.Zone.WebParts)
                    {
                        if (wp is Microsoft.SharePoint.WebPartPages.ListViewWebPart)
                        {
                            Microsoft.SharePoint.WebPartPages.ListViewWebPart listWp = (Microsoft.SharePoint.WebPartPages.ListViewWebPart)wp;
                            listWp.ListViewXml = this.PreListViewXml;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                base.RegisterError(ex);
            }
        }
                
        void _QueryBtn_Click(object sender, EventArgs e)
        {

            buildCaml();

        }





        #region IPropertyPersistenceService 成员

        public string GetPropertyValue(Control ctl, string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPropertyValue(Control ctl, string name, string value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
