using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.SharePoint;

namespace CA.SharePoint.Utilities.Common
{
    [DataObject]
    [Serializable]
    public class Employee 
    {
        #region Properties

        private String _UserAccount;
        /// <summary>
        /// 帐号
        /// </summary>
        public String UserAccount
        {
            get { return _UserAccount; }
            set { _UserAccount = value; }
        }

        private string _DisplayName;

        public string DisplayName
        {
            get { return _DisplayName; }
            set { _DisplayName = value; }
        }

        private string _PreferredName;
        public string PreferredName
        {
            get { return _PreferredName; }
            set { _PreferredName = value; }
        }
       
        private string _WorkEmail;

        public string WorkEmail
        {
            get { return _WorkEmail; }
            set { _WorkEmail = value; }
        }

        //add wujun 20100709
        //begin
        private string _Phone;
        /// <summary>
        /// 固定电话
        /// </summary>
        public string Phone
        {
            get { return _Phone; }
            set { _Phone = value; }
        }
        
        private string _Mobile;
        /// <summary>
        /// 移动电话
        /// </summary>
        public string Mobile
        {
            get { return _Mobile; }
            set { _Mobile = value; }
        }
        //end
        //add by wujun 20100714
        private string _Title;
        /// <summary>
        /// 职位
        /// </summary>
        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        private string _More;
        /// <summary>
        /// 职务
        /// </summary>
        public string More
        {
            get { return _More; }
            set { _More = value; }
        }
        private string _fax;
        /// <summary>
        /// 传真
        /// </summary>
        public string Fax
        {
            get { return _fax; }
            set { _fax = value; }
        }
        private string _populateName;
        /// <summary>
        /// 居住名
        /// </summary>
        public string PopulateName
        {
            get { return _populateName; }
            set { _populateName = value; }
        }
        private String _SelfIntroduction;
        /// <summary>
        /// 自我介绍
        /// </summary>
        public String SelfIntroduction
        {
            get { return _SelfIntroduction; }
            set { _SelfIntroduction = value; }
        }

        private String _Interest;
        /// <summary>
        /// 兴趣爱好
        /// </summary>
        public String Interest
        {
            get { return _Interest; }
            set { _Interest = value; }
        }

        private String _UnitName;
        /// <summary>
        /// 部门名称
        /// </summary>
        public String UnitName
        {
            get { return _UnitName; }
            set { _UnitName = value; }
        }

        private String _PhotoUrl;
        /// <summary>
        /// 照片
        /// </summary>
        public String PhotoUrl
        {
            get { return _PhotoUrl; }
            set { _PhotoUrl = value; }
        }

        private string _Manager;

        public string Manager
        {
            get { return _Manager; }
            set { _Manager = value; }
        }

        private string _EmployeeID;
        /// <summary>
        /// 员工ID
        /// </summary>
        public string EmployeeID
        {
            get { return _EmployeeID; }
            set { _EmployeeID = value; }
        }

        private String _Department;
        /// <summary>
        /// 管辖部门ID
        /// </summary>
        public String Department
        {
            get { return _Department; }
            set { _Department = value; }
        }

        private String _AllDepartment;
        /// <summary>
        /// 管辖部门ID
        /// </summary>
        public String AllDepartment
        {
            get { return _AllDepartment; }
            set { _AllDepartment = value; }
        }

        private bool _approveRight;

        public bool ApproveRight
        {
            get { return _approveRight; }
            set { _approveRight = value; }
        }

        //added by wsq 20101118
        private string[] _directReports;

        public string[] DirectReports
        {
            get { return _directReports; }
            set { _directReports = value; }
        }
     
        #endregion

        #region Construct function

        public Employee()
        {

        }

        /// <summary>
        /// 索引器，方便动态查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Object this[String name]
        {
            get
            {
                return this.GetType().GetProperty(name).GetValue(this, null);
            }
            set
            {
                this.GetType().GetProperty(name).SetValue(this, value, null);
            }
        }

        #endregion

        #region Find

        /// <summary>
        /// 查询员工列表
        /// </summary>
        /// <param name="code">部门编码</param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IList<Employee> FindAllByUnitCode(String deptCode)
        {
            if (String.IsNullOrEmpty(deptCode)) return null;

            return null;

        }

     

        /// <summary>
        /// 查询部门经理帐号
        /// </summary>
        /// <param name="web"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        private static String GetUnitManagerAccount(string accountNum)
        {
            return string.Empty;
        }

      

        /// <summary>
        /// 根据帐号查询部门ID
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        private static Int32? GetUnitIDByAccount(String account)
        {
           
            return null;
        }    

        #endregion

        #region Update

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="photo">照片</param>
        /// <param name="photoName">照片文件名称</param>
        /// <param name="url">当前网站Url</param>
        /// <param name="id">员工ID</param>
        /// <param name="interest">兴趣爱好</param>
        /// <param name="selfIntroduction">自我介绍</param>
        /// <param name="unitId">管辖部门ID</param>
        public static void Update(Byte[] photo, String photoName, String url, Int32 id, String interest, String selfIntroduction, String unitId, String maxim)
        {
            try
            {
                using (SPSite site = new SPSite(url))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
//                        SPList list = web.Lists[Const.EmployeeExtendPropListName];

//                        SPQuery query = new SPQuery();
//                        String strQuery = @"<Where>
//                                        <Eq>
//                                            <FieldRef Name='{0}' />
//                                            <Value Type='Text'>{1}</Value>
//                                        </Eq>
//                                    </Where>";
//                        query.Query = String.Format(strQuery, list.Fields["编号"].InternalName, id.ToString());

//                        SPListItemCollection items = list.GetItems(query);
//                        SPListItem item = null;

//                        if (items != null && items.Count > 0)
//                        {
//                            item = items[0];
//                            if (item.Attachments != null && item.Attachments.Count > 0 && photo.Length > 0)
//                            {
//                                for (Int32 i = 0; i < item.Attachments.Count; i++)
//                                {
//                                    item.Attachments.Delete(item.Attachments[i].ToString());
//                                }
//                            }
//                        }
//                        else
//                        {
//                            item = list.Items.Add();
//                            item["编号"] = id.ToString();
//                            item.Update();
//                        }

//                        if (photo.Length > 0)
//                        {
//                            String photoUrl = item.Attachments.UrlPrefix + photoName;
//                            item.Attachments.Add(photoUrl, photo);
//                            item["照片"] = photoUrl;
//                        }
//                        item["兴趣爱好"] = interest;
//                        item["个人介绍"] = selfIntroduction;
//                        item["UnitID"] = unitId;
//                        item["座右铭"] = maxim;
//                        item.Update();
                    }
                }
            }
            catch (Exception e)
            {
               
            }
        }

        #endregion
    }
}
