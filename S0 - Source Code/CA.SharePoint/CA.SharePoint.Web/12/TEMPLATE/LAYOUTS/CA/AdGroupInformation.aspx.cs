using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text;

namespace CA.SharePoint.Web
{
    public partial class AdGroupInformation : System.Web.UI.Page
    {
        public SortedList<string, SearchResult> GetSortedGroups()
        {
            SortedList<string, SearchResult> result = new SortedList<string, SearchResult>();

            try
            {
                DirectoryEntry objADAM = new DirectoryEntry("LDAP://OU=dlgroup,dc=cnaidc,dc=cn", "cnaidc\\spsadmin", "ciicit#4%6", AuthenticationTypes.Secure);
                //objADAM.RefreshCache();

                DirectorySearcher objSearchADAM = new DirectorySearcher(objADAM);
                objSearchADAM.Filter = "(&(objectClass=group))";
                objSearchADAM.SearchScope = System.DirectoryServices.SearchScope.Subtree;
                SearchResultCollection objSearchResults = objSearchADAM.FindAll();
                objADAM.Close();

                if (objSearchResults.Count != 0)
                {
                    foreach (SearchResult objResult in objSearchResults)
                    {
                        DirectoryEntry objGroupEntry = objResult.GetDirectoryEntry();
                        if (objGroupEntry.Properties["member"].Count > 0)
                        {
                            result.Add(objGroupEntry.Name, objResult);
                        }
                        objGroupEntry.Close();
                    }
                }
                else
                {
                    throw new Exception("No groups found");
                }
                
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        public SortedList<string, PropertyCollection> GetSortedUsers(SearchResult group)
        {
            SortedList<string, PropertyCollection> result = new SortedList<string, PropertyCollection>();

            try
            {
                foreach (Object memberColl in group.Properties["member"])
                {
                    string strmemberColl = memberColl.ToString().ToLower();
                    if (strmemberColl.Contains("ou=sh") || strmemberColl.Contains("ou=store"))
                    {
                        DirectoryEntry objUserEntry = new DirectoryEntry("LDAP://" + memberColl, "cnadic\\spsadmin", "ciicit#4%6", AuthenticationTypes.Secure);
                        //objUserEntry.RefreshCache();

                        PropertyCollection userProps = objUserEntry.Properties;
                        objUserEntry.Close();

                        if (!string.IsNullOrEmpty(userProps["displayName"].Value + ""))
                        {
                            result.Add(userProps["displayName"].Value.ToString(), userProps);
                        }
                        
                    }

                    //object obVal = userProps["displayName"].Value;
                    //object obAcc = userProps["sAMAccountName"].Value;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return result;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            StringBuilder op = new StringBuilder();

            op.Append("<table>");
            string dot = ",<span class=\"s1\"></span>";

            foreach (SearchResult group in GetSortedGroups().Values)
            {
                SortedList<string, PropertyCollection> users = GetSortedUsers(group);
                if (users.Count > 0)
                {
                    DirectoryEntry objGroupEntry = group.GetDirectoryEntry();
                    op.Append("<tr class=\"tr1\"><td>" + objGroupEntry.Name.Substring(3) + "</td></tr>");
                    objGroupEntry.Close();
                    op.Append("<tr class=\"tr2\"><td class=\"td2\">");
                    foreach (PropertyCollection pc in users.Values)
                    {
                        op.Append(BoldFirst(pc["displayName"].Value.ToString()) + dot);
                    }
                    op.Remove(op.Length - dot.Length, dot.Length);
                    op.Append("</td></tr>");
                }
            }
            op.Append("</table>");
            lblop.Text = op.ToString();
        }

        string BoldFirst(string str)
        {
            return "<span class=\"bf\">" + str.Substring(0, 1) + "</span>" + str.Substring(1);
        }
    }
}
