
using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Microsoft.SharePoint;
 
using CA.SharePoint.WebPartSkin;
using CA.SharePoint.EditParts;
using System.Xml;

namespace CA.SharePoint
{

    /// <summary>
    /// - <Where>
//- <Or>
//- <Eq>
//  <FieldRef Name="Created" /> 
//- <Value Type="DateTime">
//  <Today /> 
//  </Value>
//  </Eq>
//- <Eq>
//  <FieldRef Name="Author" /> 
//- <Value Type="Integer">
//  <UserID Type="Integer" /> 
//  </Value>
//  </Eq>
//  </Or>
//  </Where>
//  </Query>
    /// </summary>
    public class CAMLQueryProviderPart : BaseSPWebPart
    {

        private bool _AutoConnect = true;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [ResWebDisplayName("AutoConnect", "Auto Connect List")]
        public bool AutoConnect
        {
            get { return _AutoConnect; }
            set { _AutoConnect = value; }
        }

        private string _Query = "" ;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable(false)]
        //[ResWebDisplayName("CAMLQueryProviderPart_Query")]
        public string Query
        {
            get { return _Query; }
            set 
            {
                if (value != "")
                {
                    XmlDocument doc = new XmlDocument();
                    try
                    {

                    }
                    catch (Exception ex)
                    {
                        throw new SPException( "Query format error." , ex );
                    }
                }

                _Query = value; 
            }
        }

        /// <summary>
        /// 编辑模式下允许接受 查询体件，进行存贮。
        /// </summary>
        /// <param name="provider"></param>
        [ResConnectionConsumer("QueryCondition", "QueryConditionConsumer",
          AllowsMultipleConnections = true)]
        public void SetQueryCondition(IQueryConditionProvider provider)
        {
            //_QueryConditionProvider = provider;

            if (this.WebPartManager.DisplayMode.AllowPageDesign && !String.IsNullOrEmpty(  provider.QueryCondition.Query ) )
            {                
                Query = provider.QueryCondition.Query; 

                this._EditBox.Text = Query ;

                this.SetPersonalizationDirty();
            }
        }


        [ResConnectionProvider("QueryCondition", "QueryConditionProvider")]
        public IQueryConditionProvider ProvideQueryCondition()
        {
            if (String.IsNullOrEmpty(_Query))
                return null;

            return new QueryConditionProvider( this._Query , null );
        }

        string ProcessQueryTag(string q)
        {
            StringBuilder sb = new StringBuilder(q);

            sb.Replace("[@Date]", DateTime.Now.ToShortDateString());

            sb.Replace("[@DateTime]", DateTime.Now.ToString());

            return sb.ToString();
        }

        protected TextBox _EditBox = new TextBox();
        protected Button _SetBtn = new Button();
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            if (this.WebPartManager.DisplayMode.AllowPageDesign)
            {
                _EditBox.TextMode = TextBoxMode.MultiLine;
                _EditBox.Text = this.Query;
                _EditBox.Width = new Unit( "100%" );
                _EditBox.Height = new Unit( "100px");
                
                this.Controls.Add(_EditBox);
                _SetBtn.Text = "Submit";
                _SetBtn.Click += new EventHandler(_SetBtn_Click);

                this.Controls.Add(_SetBtn);
            }

        }

        void _SetBtn_Click(object sender, EventArgs e)
        {
            this.Query = _EditBox.Text.Trim();

            this.SetPersonalizationDirty(); 
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (_AutoConnect)
                SetCurrentListQuery(this.Query);
        }

        private void SetCurrentListQuery(string qxml)
        {
            if (String.IsNullOrEmpty(qxml)) return;

            foreach (WebPart wp in this.Zone.WebParts)
            {
                if (wp is Microsoft.SharePoint.WebPartPages.ListViewWebPart)
                {
                    Microsoft.SharePoint.WebPartPages.ListViewWebPart listWp = (Microsoft.SharePoint.WebPartPages.ListViewWebPart)wp;

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(listWp.ListViewXml);

                    SharePointUtil.ChangeSchemaXmlQuery(doc, qxml);

                    listWp.ListViewXml = doc.InnerXml;

                    break;
                }
            }
        }
    }
}
