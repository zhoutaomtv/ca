using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.ComponentModel;

namespace CA.Web
{
    public static class ControlUtil
    {
        public static Control FindControl(Control control, string controlID)
        {
            Control control1 = control;
            Control control2 = null;
            if (control == control.Page)
            {
                return control.FindControl(controlID);
            }
            while ((control2 == null) && (control1 != control.Page))
            {
                control1 = control1.NamingContainer;
                if (control1 == null)
                {
                    return null;
                    //throw new ObjectMapException("²»´æÔÚ¿Ø¼þ[" + controlID + "]", this);
                    //throw new HttpException(SR.GetString("DataBoundControlHelper_NoNamingContainer", new object[] { control.GetType().Name, control.ID }));
                }
                control2 = control1.FindControl(controlID);
            }           

            return control2;
        }

        public static Control FindChildControl(Control ctl, string id)
        {
            Control target = ctl.FindControl(id);

            if (target != null) return target;
            foreach (Control c in ctl.Controls)
            {
                target = FindChildControl(c, id);
                if (target != null) return target;
            }
            return target;
        }
    }
}
