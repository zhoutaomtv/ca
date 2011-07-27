using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint;

namespace CA.SharePoint.Common
{
    [Serializable]
    public class MailAlertConfig
    {
        #region Fields
        //配置列表的名称
        private static readonly string _siteMailAlertConfig = "_eMailAlertConfig";


        private string _listNames = string.Empty;
        /// <summary>
        /// 列表的名称
        /// </summary>
        public string ListNames
        {
            get { return _listNames; }
            set { _listNames = value; }
        }

        private string _timeSpan = string.Empty;
        /// <summary>
        /// 查询条件
        /// </summary>
        public string TimeSpan
        {
            get { return _timeSpan; }
            set { _timeSpan = value; }
        }

        private uint _maxRows = 100;
        /// <summary>
        /// 结果显示的最大条目
        /// </summary>
        public uint MaxRows
        {
            get { return _maxRows; }
            set { _maxRows = value; }
        }

        private string _emailTitle = "List Update Notification";
        /// <summary>
        /// 邮件主题
        /// </summary>
        public string EmailTitle
        {
            get { return _emailTitle; }
            set { _emailTitle = value; }
        }

        private string _emailTo = string.Empty;
        /// <summary>
        /// 收件人
        /// </summary>
        public string EmailTo
        {
            get { return _emailTo; }
            set { _emailTo = value; }
        }

        private string _emailBody = string.Empty;
        /// <summary>
        /// 邮件内容
        /// </summary>
        public string EmailBody
        {
            get { return _emailBody; }
            set { _emailBody = value; }
        }

        private string _viewFields = string.Empty;
        /// <summary>
        /// 需要显示的类表字段
        /// </summary>
        public string ViewFields
        {
            get { return _viewFields; }
            set { _viewFields = value; }
        }

        private string _userServiceUrl = string.Empty;

        /// <summary>
        /// Web Service URL
        /// </summary>
        public string UserServiceUrl
        {
            get { return _userServiceUrl; }
            set { _userServiceUrl = value; }
        }

        private string _smtpServerAddress = string.Empty;
        /// <summary>
        /// SMTP服务器IP地址
        /// </summary>
        public string SmtpServerAddress
        {
            get { return _smtpServerAddress; }
            set { _smtpServerAddress = value; }
        }

        private string _senderMailAddress = string.Empty;
        /// <summary>
        /// 发送者电子邮件地址
        /// </summary>
        public string SenderMailAddress
        {
            get { return _senderMailAddress; }
            set { _senderMailAddress = value; }
        }


        #endregion

        #region Methods
        public void SaveSetting(string id)
        {
            ConfigManager cmg = ConfigManager.GetConfigManager(_siteMailAlertConfig);
            cmg.SetConfigData(id, this);
        }


        public static MailAlertConfig GetSetting(string id)
        {
            ConfigManager cmg = ConfigManager.GetConfigManager(_siteMailAlertConfig);

            MailAlertConfig setting = cmg.GetConfigData<MailAlertConfig>(id);

            if (null == setting)
            {
                setting = new MailAlertConfig();
                setting.SaveSetting(id);
            }

            return setting;
        }
        #endregion

    }
}
