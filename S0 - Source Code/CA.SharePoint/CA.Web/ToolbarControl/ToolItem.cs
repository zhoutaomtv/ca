using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CA.Web.ToolbarControl
{
    /// <summary>
    /// ¹¤¾ßÀ¹°´Å¥
    /// </summary>
    public class ToolItem
    {
        private string _Text;
        public string Text
        {
            set
            {
                _Text = value; 
            }
            get
            {
                return _Text;
            }
        }

        private string _Value = "";
        public string Value
        {
            set
            {
                _Value = value;
            }
            get
            {
                return _Value;
            }
        }


        private string _ImageUrl;
        public string ImageUrl
        {
            set
            {
                _ImageUrl = value;
            }
            get
            {
                return _ImageUrl ;
            }
        }

        private string _OnClientClick = "" ;
        public string OnClientClick
        {
            set
            {
                _OnClientClick = value;    
            }
            get
            {
                return _OnClientClick;
            }
        }

        private string _ConfirmMessage = "";
        public string ConfirmMessage
        {
            set
            {
                _ConfirmMessage = value;
                _OnClientClick = "confirm('" + value + "')";
            }
            get
            {
                return _ConfirmMessage;
            }
        }

        private string _ToolTip = "";
        public string ToolTip
        {
            set
            {
                _ToolTip = value;
            }
            get
            {
                return _ToolTip;
            }
        }

        private string _NavigateUrl = "";
        public string NavigateUrl
        {
            set
            {
                _NavigateUrl = value;
            }
            get
            {
                return _NavigateUrl;
            }
        }


        private bool _Visible = true;
        public bool Visible
        {
            set
            {
                _Visible = value;
            }
            get
            {
                return _Visible;
            }
        }



        //public event EventHandler OnClick ;




    }
}
