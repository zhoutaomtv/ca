using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.SharePoint;

namespace CA.SharePoint.Utilities.Common
{
    public class CAConstants
    {
        public struct ListName
        {
            public const string CompanyLink = "CompanyLink";
            public const string DepartmentLinks = "Department Links";
            public const string UserInfoList = "User Information List";
            public const string Department = "Department";
            public const string MarketingCommunicationNotes = "MarketingCommunicationNotes";
        }

        public struct FieldName
        {
            public const string Name = "Name";
            public const string DisplayName = "DisplayName";
            public const string Description = "Description";
            public const string Manager = "Manager";
            public const string ManagerAccount = "ManagerAccount";
        }

        public struct GroupName
        {
            public const string CAOwners = "C&A Owners";
            public const string DepartmentPageMangager = "DepartmentPage Managers";
        }
    }

    [DataObject]
    public class CALink 
    {
        public string LinkName { get; set; }
        public string LinkUrl { get; set; }

        public CALink()
        {

        }

        public CALink(string lookupString)
        {
            SPFieldUrlValue value = new SPFieldUrlValue(lookupString);

            this.LinkName = value.Description;
            this.LinkUrl = value.Url;

        }
    }
}
  
