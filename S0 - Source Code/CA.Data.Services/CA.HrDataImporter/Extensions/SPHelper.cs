namespace CA.HrDataImporter.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Microsoft.Office.Server.UserProfiles;
    using Microsoft.SharePoint;

    public enum UserProfileUpdateStatus 
    { 
        NewlyAdded,
        Updated
    }

    public static class SPHelper
    {
        public static IList<string> GetImportExceptionsFromSPList(SPWeb web, string listName)
        {
            web.AssertNotNull("web");
            listName.AssertNotNull("listName");

            try
            {
                var list = new List<string>();

                var exceptions = web.Lists[listName];

                list.AddRange(from SPListItem item in exceptions.Items select item["Title"].ToString().Trim());

                return list;
            }
            catch (Exception ex)
            {
                throw new SPException(String.Format("List [{1}] does not exist in the site [{0}] or user has no access to the list.", web.Url, listName), ex);
            }
        }

        public static UserProfileUpdateStatus UpdateUserProfileByAccount(UserProfileManager upm, string account, DataRow userInfo, DataColumnCollection columns)
        {
            upm.AssertNotNull("upm");
            userInfo.AssertNotNull("userInfo");
            columns.AssertNotNull("columns");

            bool exists = upm.UserExists(account);

            UserProfile profile = exists ? upm.GetUserProfile(account) : upm.CreateUserProfile(account);

            foreach (string colName in from DataColumn column in columns select column.ColumnName)
            {
                bool editable = profile[colName].Property.IsAdminEditable;

                if (editable)
                {
                    try
                    {
                        profile[colName].Value = userInfo[colName];
                    }
                    catch 
                    { 
                    }
                }

            }

            profile.Commit();

            return exists ? UserProfileUpdateStatus.Updated : UserProfileUpdateStatus.NewlyAdded;
        }
    }
}