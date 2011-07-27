using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Administration;

namespace CA.SharePoint.WebControls
{
    public class ProfileRedirectControl : UserControl, IFormDelegateControlSource
    {        
        public void OnFormInit(object objOfInterest)
        {
            SPWeb web = SPContext.Current.Web;

            if (CustomMySettingsPageEnabled(web.Site))
            {
                string url = web.Url + "/_layouts/ca/userdetail.aspx?" + Request.ServerVariables["QUERY_STRING"];
                Response.Redirect(url, true);
            }           
        }

        public void OnFormSave(object objOfInterest) { }
        
        private bool CustomMySettingsPageEnabled(SPSite site)
        {
            foreach (SPFeature feature in site.Features)
            {
                SPFeatureDefinition definition = feature.Definition;
                if (definition != null && definition.DisplayName == "ProfileCustomPage")
                    return true;                
            }
            return false;
        }
    }
}