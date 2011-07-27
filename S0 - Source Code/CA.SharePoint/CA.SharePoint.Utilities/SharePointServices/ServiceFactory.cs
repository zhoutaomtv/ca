using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;

namespace CA.SharePoint
{
    public static class ServiceFactory
    {

        static public ISharePointService GetSharePointService( bool userAdminPermission )
        {
            if (userAdminPermission)
            {
                return new SharePointServiceWithAdminPermission();
            }
            else
            {
                return new SharePointService();
            }
        }

        static public ISharePointService GetSharePointService(bool userAdminPermission,SPWeb web)
        {
            if (userAdminPermission)
            {
                return new SharePointServiceWithAdminPermission(web);
            }
            else
            {
                return new SharePointService(web);
            }
        }


    }
}
