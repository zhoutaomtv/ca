using System;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CA.Web
{

    /// <summary>
    /// UIHelper 的摘要说明。
    /// </summary>
    public class UIHelper
    {
        

        //		public static sring RenderSequenceBox( DataRowView row  , string keyField )
        //		{
        //			return "<input type=text maxlength=3 size=3 name='__ListSequence-"+row[keyField]+" value='"+row["Sequence"]+"'>";
        //		}

        public static string RenderCheckBox(Control dg, string key, object value, bool check)
        {
            if (check)
                return "<input type='checkbox'  name='__" + dg.ID + "__CheckBox__" + key + "' value='" + value + "' checked='" + check + "'>";
            else
                return "<input type='checkbox'  name='__" + dg.ID + "__CheckBox__" + key + "' value='" + value + "'>";

        }

        public static string[] GetCheckedValues(Control dg, string key)
        {
            System.Web.HttpRequest r = System.Web.HttpContext.Current.Request;

            string ids = r.Form["__" + dg.ID + "__CheckBox__" + key];

            if (ids == null || ids == "")
                return null;

            return ids.Split(',');
        }


        #region 排序修改函数

        /// <summary>
        /// 生成排序UI
        /// </summary>
        /// <param name="dg"></param>
        /// <param name="row"></param>
        /// <param name="keyField"></param>
        /// <param name="sequenceField"></param>
        /// <returns></returns>
        //public static string RenderSequenceBox(Control dg, DataRowView row, string keyField, string sequenceField)
        //{
        //    return "<input type=text maxlength=3 size=3 name='__" + dg.ID + "__Sequence-" + row[keyField] + "' value='" + row[sequenceField] + "'>";
        //}

        /// <summary>
        /// 生成排序UI
        /// </summary>
        /// <param name="dg"></param>
        /// <param name="keyValue"></param>
        /// <param name="sequenceValue"></param>
        /// <returns></returns>
        public static string RenderSequenceBox(Control dg, object keyValue, int sequenceValue)
        {
            return "<input type=text maxlength=4 size=3 name='__" + dg.ID + "__Sequence-" + keyValue + "' value='" + sequenceValue + "'>";
        }

        /// <summary>
        ///  获取排序结果
        /// </summary>
        /// <param name="dg"></param>
        /// <returns></returns>
        public static IDictionary GetGridSequenceSet(Control dg)
        {
            System.Web.HttpRequest r = System.Web.HttpContext.Current.Request;

            Hashtable list = new Hashtable();

            foreach (string key in r.Form.Keys)
            {
                if (key.StartsWith("__" + dg.ID + "__Sequence-"))
                {
                    string v = r.Form[key].Trim();

                    list.Add(key.Split('-')[1], v );
                }
            }

            return list;
        }

        #endregion

        #region UI批量邦定


        /// <summary>
        /// 获取以N-前缀命名的所有控件值
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        //public static IDictionary GetNamingControlsValue(Page p)
        //{
        //    HtmlForm form = (HtmlForm)p.Controls[1];

        //    return GetNamingControlsValue(form);
        //}

        //public static IDictionary GetNamingControlsValue(Control cc)
        //{
        //    Hashtable list = new Hashtable();

        //    foreach (Control c in cc.Controls)
        //    {
        //        if (c.ID.StartsWith("N_") == false) continue;

        //        string key = c.ID.Split('_')[1];

        //        if (c is TextBox)
        //        {
        //            list.Add(key, (c as TextBox).Text);
        //        }
        //        else if (c is DropDownList)
        //        {
        //            list.Add(key, (c as DropDownList).SelectedValue);
        //        }
        //        else if (c is RadioButton)
        //        {
        //            list.Add(key, (c as RadioButton).Checked);
        //        }
        //        else if (c is CheckBox)
        //        {

        //        }
        //    }

        //    return list;
        //}


        //public static void SetNamingControlsValue(Page p, IDictionary list)
        //{
        //    HtmlForm form = (HtmlForm)p.Controls[1];

        //    SetNamingControlsValue(form, list);
        //}

        //public static void SetNamingControlsValue(Control cc, IDictionary list)
        //{
        //    foreach (Control c in cc.Controls)
        //    {
        //        if (c.ID.StartsWith("N_") == false) continue;

        //        string key = c.ID.Split('_')[1];

        //        if (!list.Contains(key)) continue;

        //        object v = list[key];

        //        if (v == null) continue;

        //        if (c is TextBox)
        //        {
        //            (c as TextBox).Text = v.ToString();
        //        }
        //        else if (c is DropDownList)
        //        {
        //            (c as DropDownList).SelectedValue = v.ToString();
        //        }
        //        else if (c is RadioButton)
        //        {
        //            RadioButton r = c as RadioButton;

        //            if (v is bool)
        //                r.Checked = Convert.ToBoolean(v);
        //            else
        //            {
        //                if (r.Text == v.ToString())
        //                    r.Checked = true;
        //                else
        //                    r.Checked = false;
        //            }

        //        }
        //        else if (c is CheckBox)
        //        {
        //            (c as CheckBox).Checked = Convert.ToBoolean(v);
        //        }
        //    }
        //}

        //public static void SetNamingControlsValue(Control cc, object o)
        //{
        //    Sor.SorInfo.ISorInfo sf = Sor.SorInfo.SorInfoManager.GetSorInfo(o.GetType(), Sor.Attributes.SorMappingType.PublicProperty);

        //    if (o == null)
        //        throw new Exception("不支持");

        //    Hashtable list = new Hashtable();

        //    foreach (Sor.SorInfo.SorMember sm in sf.SorMembers)
        //    {
        //        list.Add(sm.Name, sm.GetValue(o));
        //    }

        //    SetNamingControlsValue(cc, list);
        //}

        //public static void SetValueByNamingControls(Control cc, object o)
        //{
        //    Sor.SorInfo.ISorInfo sf = Sor.SorInfo.SorInfoManager.GetSorInfo(o.GetType(), Sor.Attributes.SorMappingType.PublicProperty);

        //    if (o == null)
        //        throw new Exception("不支持");

        //    foreach (Control c in cc.Controls)
        //    {
        //        if (c.ID.StartsWith("N_") == false) continue;

        //        string key = c.ID.Split('_')[1];

        //        Sor.SorInfo.SorMember sm = sf.SorMembers.GetSorMemberByName(key);

        //        if (sm == null) return;

        //        if (c is TextBox)
        //        {
        //            sm.SetValue(o, (c as TextBox).Text);
        //        }
        //        else if (c is DropDownList)
        //        {
        //            sm.SetValue(o, (c as DropDownList).SelectedValue);
        //        }
        //        else if (c is RadioButton)
        //        {
        //            sm.SetValue(o, (c as RadioButton).Checked);
        //        }
        //        else if (c is CheckBox)
        //        {

        //        }
        //    }
        //}

        //public static void SetNamingControlsValue(Control cc, DataTable t)
        //{
        //    if (t == null || t.Rows.Count == 0) return;

        //    Hashtable list = new Hashtable();

        //    foreach (DataColumn col in t.Columns)
        //    {
        //        list.Add(col.ColumnName, t.Rows[0][col.ColumnName]);
        //    }

        //    SetNamingControlsValue(cc, list);
        //}


        #endregion




    }
}

