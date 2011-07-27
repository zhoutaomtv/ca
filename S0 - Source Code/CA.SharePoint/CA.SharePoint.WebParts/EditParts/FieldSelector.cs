
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;
using System.Collections.Specialized; 
using System.Xml;
using CA.SharePoint.WebPartSkin;

namespace CA.SharePoint.EditParts 
{
    /// <summary>
    ///  查询字段编辑控件
    /// </summary>
    public class FieldSelector : Table
    {
        public FieldSelector()
        {

        }

        private Dictionary<string, CheckBox> _CheckboxList = new Dictionary<string, CheckBox>();

        private Dictionary<string, DropDownList> _IndexList = new Dictionary<string, DropDownList>();

        /// <summary>
        /// 生成排序下拉列表
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        DropDownList GenList(int count)
        {
            DropDownList list = new DropDownList();
            list.CssClass = "ms-RadioText";

            for (int i = 1; i <= count; i++)
            {
                list.Items.Add(i.ToString());
            }

            return list;
        }

        /// <summary>
        /// 将控件添加到行
        /// </summary>
        /// <param name="ctls"></param>
        void AddRow(params Control[] ctls)
        {
            TableRow row = new TableRow();
            this.Rows.Add(row);

            foreach (Control c in ctls)
            {
                TableCell cell = new TableCell();
                row.Cells.Add(cell);
                cell.Controls.Add(c);
            }
        }

        private Dictionary<String, String> _AllFields;
        /// <summary>
        /// 保存所有字段的内部名和Title
        /// </summary>
        private Dictionary<String,String> AllFields
        {
            get
            {
                if (_AllFields == null)
                    _AllFields = ViewState["_AllFields"] as Dictionary<String,String> ;

                return _AllFields ;
            }
            set
            {
                _AllFields = value ;
                ViewState["_AllFields"] = _AllFields;
            }
        }

        /// <summary>
        /// 获取选择的字段，已经排序
        /// </summary>
        /// <returns></returns>
        public List<String> GetSelectedFields()
        {
            List<String> selectedFields = new List<string>();
            Dictionary<string, int> sequenceList = new Dictionary<string, int>();

            this.EnsureChildControls();

            if (this.AllFields == null)
                return null;

            foreach (string fieldName in this.AllFields.Keys)
            {
                //if (fieldName.Hidden) continue;

                CheckBox ck = _CheckboxList[fieldName];

                if (ck.Checked)
                {
                    DropDownList numlist = _IndexList[fieldName];
                    selectedFields.Add(fieldName);
                    sequenceList.Add(fieldName, numlist.SelectedIndex);
                }
            }

            selectedFields.Sort( new FieldNameCompare( sequenceList ) );            

            return selectedFields ;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            //还原控件
            this.BuildUI(new List<String>(), this.AllFields);

            this.ChildControlsCreated = true;
        }
 

        /// <summary>
        /// 将字段名列表按照 排序号排序
        /// </summary>
        class FieldNameCompare : IComparer<String>
        {
            Dictionary<string, int> _sequenceList;
            public FieldNameCompare(Dictionary<string, int> sequenceList)
            {
                _sequenceList = sequenceList;
            }

            #region IComparer<string> 成员

            public int Compare(string x, string y)
            {
                int index1 = _sequenceList[x];
                int index2 = _sequenceList[y];

                return index1.CompareTo(index2);                 
            }

            #endregion
        }
        /// <summary>
        /// 设置选中的字段
        /// </summary>
        /// <param name="selectedFields"></param>
        /// <param name="allFields"></param>
        public void SetSelectedFields(List<String> selectedFields, SPFieldCollection allFields)
        {           

            List<SPField> fieldsToDisplay = new List<SPField>();

            foreach ( SPField f in allFields )
            {
                if (!f.Hidden)
                {
                    fieldsToDisplay.Add(f);
                }
            }

            //排序字段
            fieldsToDisplay.Sort( new FieldCompare( selectedFields ) ) ;

            Dictionary<String, string> allFieldNames = new Dictionary<String, string>();

            foreach (SPField f in fieldsToDisplay)
            {
                 allFieldNames.Add(f.InternalName, f.Title+f.AuthoringInfo);
            }

            this.AllFields = allFieldNames;

            BuildUI(selectedFields, allFieldNames);           
        }

        public void BuildUI( List<String> selectedFields, Dictionary<String,String> allFields )
        {
            this.Rows.Clear();

            TableRow row = null;
            TableCell cell = null;

            row = new TableRow();
            Rows.Add(row);
            cell = new TableCell();
            cell.Text = "Display";
            row.Cells.Add(cell);
            cell = new TableCell();
            cell.Text = "Column";
            row.Cells.Add(cell);
            cell = new TableCell();
            cell.Text = "Position";
            row.Cells.Add(cell);

            if (allFields == null || allFields.Count == 0) return;

            _CheckboxList = new Dictionary<string, CheckBox>();
            _IndexList = new Dictionary<string, DropDownList>();

            int i = 0;
            foreach ( string s in allFields.Keys )
            { 
                CheckBox ck = new CheckBox();
                ck.ID = "_" + i + "_selected";
                ck.Checked = selectedFields.Contains(s);

                LiteralControl title = new LiteralControl( allFields[s] );

                DropDownList numlist = GenList(allFields.Count);
                numlist.ID = "_" + i  +"_index";
                numlist.SelectedIndex = i;
                this.AddRow(ck, title, numlist);

                _CheckboxList.Add(s, ck);
                _IndexList.Add(s, numlist);

                i++;
            }

            this.ChildControlsCreated = true;
        }

        /// <summary>
        /// 将字段按照 选择顺序排序
        /// </summary>
        class FieldCompare : IComparer<SPField>
        {
            List<String> _SelectedFields;
            public FieldCompare(List<String> selectedFields)
            {
                _SelectedFields = selectedFields;
            }

            #region IComparer<SPField> 成员

            public int Compare(SPField f1, SPField f2) // 值 条件 小于零x 小于 y。零x 等于 y。大于零x 大于 y。
            {
                int index1 = -1;
                int index2 = -1;

                if (_SelectedFields.Contains(f1.InternalName))
                    index1 = +_SelectedFields.IndexOf(f1.InternalName);

                if (_SelectedFields.Contains(f2.InternalName))
                    index2 = +_SelectedFields.IndexOf(f2.InternalName);

                if (index1 == index2)
                    return 0;
                else if (index1 == -1)
                    return 1;
                else if (index2 == -1)
                    return -1;
                else
                    return index1.CompareTo(index2);

                //return index2.CompareTo( index1 ) ;
            }

            #endregion
        }

         

    }
}
