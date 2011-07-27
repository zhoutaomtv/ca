using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;

namespace CA.SharePoint.CamlQuery
{
    public class TestCase
    {

        void Test1()
        {
            using (SPWeb webSite = SPContext.Current.Site.AllWebs["Site_Name"])
            {
                SPList list = webSite.Lists["List_Name"];

                SPQuery query = new SPQuery();

                query.ViewXml = "<View><Query><OrderBy><FieldRef Name='ID'/>" +
                    "</OrderBy><Where><Or><Geq><FieldRef Name='Field1'/>" +
                    "<Value Type='Number'>1500</Value></Geq><Leq>" +
                    "<FieldRef Name='Field2'/><Value Type='Number'>500</Value>" +
                    "</Leq></Or></Where></Query><ViewFields>" +
                    "<FieldRef Name='Title'/>" +
                    "<FieldRef Name='Field1'/><FieldRef Name='Field2'/>" +
                    "<FieldRef Name='Field3'/><FieldRef Name='Field4'/>" +
                    "</ViewFields><RowLimit>100</RowLimit></View>";
                SPListItemCollection items = list.GetItems(query);

                foreach (SPListItem item in items)
                {
                   // Response.Write(SPEncode.HtmlEncode(item.Xml) + "<BR>");
                }
            }

            //SPFieldType t = null;

        }


        void Test2()
        {
            SPList list = null;

            CAMLExpression<Document> q = Document.Name.Equal("111") && Document.Subject == "ee";

            ListQuery.Select(Document.Name, Document.Subject)
                    .From(list)
                    .Where( Document.Name.IsNotNull && Document.FieldRef("subject").IsNull )
                    ;

        }


    }

    public class Document
    {
        public static FieldRef<Document> Name = new FieldRef<Document>("Name");

        public static FieldRef<Document> Subject = new FieldRef<Document>("Subject");

        public static FieldRef<Document> FieldRef(string name)
        {
            return new FieldRef<Document>(name);
        }

        public static TypeFieldRef<Document,int> Size = new TypeFieldRef<Document,int>("Size");
    }

    

}
