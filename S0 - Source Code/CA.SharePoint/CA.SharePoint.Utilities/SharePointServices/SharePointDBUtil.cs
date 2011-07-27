using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace CA.SharePoint
{
    public class SharePointDBUtil
    {
        public SharePointDBUtil(string connString)
        {
            this._DBConnectionString = connString;
        }

        public SharePointDBUtil() 
        { }

        string _DBConnectionString = "";

        public string DBConnectionString
        {
            get
            {
                if (_DBConnectionString != "")
                    return _DBConnectionString;

                if (ConfigurationManager.ConnectionStrings["CA_MossDB"] == null)
                    throw new Exception("配置文件缺少 CA_MossDB 库的连接字符串");

                _DBConnectionString = ConfigurationManager.ConnectionStrings["CA_MossDB"].ConnectionString;
                return _DBConnectionString;
            }
        }    
      
        public DataTable GetSiteSizeTable(Guid siteID)
        {
            using (SqlConnection DBConnection = new SqlConnection(DBConnectionString))
            {
                DBConnection.Open();
                SqlCommand myCommand = new SqlCommand("proc_GetDocLibrarySizes ", DBConnection);
                myCommand.CommandType=CommandType.StoredProcedure;
                myCommand.Parameters.Add(new SqlParameter("@SiteId", siteID.ToString()));

                SqlDataAdapter da = new SqlDataAdapter(myCommand);
                DataTable dt=new DataTable();
                da.Fill(dt);
                return dt;   
              
            }
        }

    
    }
}
