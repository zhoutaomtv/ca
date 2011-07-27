using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.ComponentModel;

namespace CA.SharePoint
{
    public class ClockWebPart : FlashWebPart 
    {
        //public ClockWebPart()
        //{
        //}

        private ClockStyle _ClockStype = ClockStyle.Style1;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [WebDisplayName("Clock Style")]
        public ClockStyle ClockStype
        {
            get { return _ClockStype; }
            set 
            { 
                _ClockStype = value;
                
            }
        }

        private string GetFilePath(ClockStyle s)
        {
            string fileFolder = Constant.CA_PAGE_PATH + "Clock/";

            switch (s)
            {
                case ClockStyle.Style1 :
                    return fileFolder + "001.swf";
                case ClockStyle.Style2:
                    return fileFolder + "002.swf";
                case ClockStyle.Style3:
                    return fileFolder + "003.swf";
                case ClockStyle.Style4:
                    return fileFolder + "004.swf";
                case ClockStyle.Style5:
                    return fileFolder + "005.swf";

                default:
                    return fileFolder + "001.swf";

            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            this.FilePath = this.GetFilePath(ClockStype);

            

            base.OnPreRender(e);
        }


    }


    public enum ClockStyle
    {
        //Default ,
        Style1,
        Style2,
        Style3,
        Style4,
        Style5,
        Style6,
        
    }
}
