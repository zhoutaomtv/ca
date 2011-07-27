//----------------------------------------------------------------
// Copyright (C) 2005 上海互联网软件有限公司
// 版权所有。 
// All rights reserved.

// 文件名：HierarchicalUIBuilder.cs
// 文件功能描述：层次对象UI创建
// 
// 
// 创建标识： 张建义 2007-7-3
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using CA.Web;
using System.Web.UI.WebControls;
using System.Reflection;

namespace CA.Web.ComponentDesign
{
    /// <summary>
    /// 层次对象UI创建
    /// </summary>
    public class HierarchicalUIBuilder : IDesignerUIBuilder
    {
        //private string _NameColumnCssClass;

        //public string NameColumnCssClass
        //{
        //    get { return _NameColumnCssClass; }
        //    set { _NameColumnCssClass = value; }
        //}

        //private string _ValueColumnCssClass;

        //public string ValueColumnCssClass
        //{
        //    get { return _ValueColumnCssClass; }
        //    set { _ValueColumnCssClass = value; }
        //}

        private Unit _NameColumnWidth;

        public Unit NameColumnWidth
        {
            get { return _NameColumnWidth; }
            set { _NameColumnWidth = value; }
        }

        private Unit _ValueColumnWidth;

        public Unit ValueColumnWidth
        {
            get { return _ValueColumnWidth; }
            set { _ValueColumnWidth = value; }
        }        
        
        //记录所有编辑控件
        private IDictionary<string, IFiledEditor> FieldSetEditors = new Dictionary<string, IFiledEditor>();

        #region IDesignerUIBuilder 成员

        public IDictionary<string, IFiledEditor> BuildEditUI( ComponentDesignControl designer , ComponentSet set, object obj)
        {
            foreach (FieldSet p in set.Fields)
            {
                TableRow row = new TableRow();
                designer.Rows.Add(row);

                TableCell cell = new TableCell();
                cell.CssClass = designer.NameCellCssClass;
                cell.Width = NameColumnWidth;
                row.Cells.Add(cell);

                if (set.Depth == 0)
                    cell.Text = FormatDisplayName(p.DisplayName, designer);
                else
                    cell.Text = this.GenSpace(set.Depth + 2) + FormatDisplayName(p.DisplayName, designer);

                cell = new TableCell();
                cell.CssClass = designer.ValueCellCssClass;
                cell.Width = ValueColumnWidth;
                row.Cells.Add(cell);

                System.Web.UI.WebControls.WebControl ctl = FieldEditorFactory.GetFieldEditor(p);

                ctl.ID = p.UniqueName;

                IFiledEditor fe = (IFiledEditor)ctl;
                fe.FieldValue = p.GetValue(obj);

                this.FieldSetEditors.Add(p.UniqueName, fe);

                cell.Controls.Add(ctl);

                designer.RaiseControlCreatedEvent(ctl, p);
            }

            foreach (ComponentSet subSet in set.SubSet)
            {
                TableRow row = new TableRow();
                designer.Rows.Add(row);

                TableCell cell = new TableCell();
                cell.ColumnSpan = 2;
                row.Cells.Add(cell);
                cell.Text = this.GenSpace(subSet.ParentSet.Depth) + FormatDisplayName(subSet.DisplayName,designer);

                BuildEditUI( designer , subSet, subSet.GetValue(obj));
            }

            return FieldSetEditors;
        }

        private string GenSpace(int depth)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < depth; i++)
            {
                sb.Append("&nbsp;&nbsp;");
            }
            return sb.ToString();
        }

        public IDictionary<string, IFiledEditor> BuildRetrieveUI(ComponentDesignControl designer, ComponentSet set)
        {
            foreach (FieldSet p in set.Fields)
            {
                TableRow row = new TableRow();
                designer.Rows.Add(row);

                TableCell cell = new TableCell();
                cell.CssClass = designer.NameCellCssClass;
                cell.Width = NameColumnWidth;
                row.Cells.Add(cell);

                if (set.Depth == 0)
                    cell.Text = FormatDisplayName(p.DisplayName, designer);
                else
                    cell.Text = this.GenSpace(set.Depth + 2) + FormatDisplayName(p.DisplayName, designer);

                cell = new TableCell();
                cell.CssClass = designer.ValueCellCssClass;
                cell.Width = ValueColumnWidth;
                row.Cells.Add(cell);

                System.Web.UI.WebControls.WebControl ctl = FieldEditorFactory.GetFieldEditor(p);
                ctl.ID = p.UniqueName;

                IFiledEditor fe = (IFiledEditor)ctl;
                //fe.FieldValue = p.GetValue(obj);

                this.FieldSetEditors.Add(p.UniqueName, fe);

                cell.Controls.Add(ctl);

                designer.RaiseControlCreatedEvent(ctl, p);
            }

            foreach (ComponentSet subSet in set.SubSet)
            {
                TableRow row = new TableRow();
                designer.Rows.Add(row);

                TableCell cell = new TableCell();
                cell.ColumnSpan = 2;
                row.Cells.Add(cell);
                cell.Text = this.GenSpace(subSet.ParentSet.Depth) + subSet.DisplayName;

                BuildRetrieveUI(designer, subSet);
            }

            return FieldSetEditors;
        }

        public void BuildViewUI(ComponentDesignControl designer ,ComponentSet set, object obj)
        {
            foreach (FieldSet p in set.Fields)
            {
                TableRow row = new TableRow();
                designer.Rows.Add(row);

                TableCell cell = new TableCell();
                cell.Width = NameColumnWidth;
                cell.CssClass = designer.NameCellCssClass;
                row.Cells.Add(cell);

                if (set.Depth == 0)
                    cell.Text = FormatDisplayName(p.DisplayName, designer);
                else
                    cell.Text = this.GenSpace(set.Depth + 2) + FormatDisplayName(p.DisplayName, designer);

                //
                cell = new TableCell();
                cell.CssClass = designer.ValueCellCssClass;
                cell.Width = ValueColumnWidth;
                row.Cells.Add(cell);
                cell.Text = "" + p.GetValue(obj);
            }

            foreach (ComponentSet subSet in set.SubSet)
            {
                TableRow row = new TableRow();
                designer.Rows.Add(row);

                TableCell cell = new TableCell();
                cell.ColumnSpan = 2;
                row.Cells.Add(cell);
                cell.Text = this.GenSpace(subSet.ParentSet.Depth) + FormatDisplayName( subSet.DisplayName,designer);

                BuildViewUI(designer,subSet, subSet.GetValue(obj));
            }
        }

        private string FormatDisplayName(string name, ComponentDesignControl designer)
        {
            if (String.IsNullOrEmpty(designer.FieldNameFormatString))
                return name;
            else
                return String.Format(designer.FieldNameFormatString, name);
        }

        #endregion

        

    }
}
