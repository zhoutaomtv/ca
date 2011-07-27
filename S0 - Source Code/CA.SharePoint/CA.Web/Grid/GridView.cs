using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;

using System.Web.UI.WebControls;
using System.Web.UI;

[assembly: TagPrefix("CA.Web", "CA")]
namespace CA.Web
{
    public class GridView : System.Web.UI.WebControls.GridView
    {
        public GridView()
        {
            this.HeaderStyle.Wrap = false;
        }

        #region 属性
        /**/
        /// <summary>
        /// 是否启用或者禁止多列排序
        /// </summary>
        [
        Description("是否启用多列排序功能"),
        Category("排序"),
        DefaultValue("false"),
        ]
        public bool AllowMultiColumnSorting
        { 
            get
            {
                object o = ViewState["EnableMultiColumnSorting"];
                return (o != null ? (bool)o : false);
            }
            set
            {
                AllowSorting = true;
                ViewState["EnableMultiColumnSorting"] = value;
            }
        } 

        /// <summary>
        /// 设置或者获取是否升序，默认为升序
        /// </summary>
        [
        Description("获得或者设置是否已升序显示。"),
        Category("自定义类别"),
        Bindable(true),
        DefaultValue(true)
        ]
        public bool IsSortAscending
        {
            get
            {
                if (this.ViewState["IsSortAscending"] != null)
                {
                    return ((bool)this.ViewState["IsSortAscending"]);
                }
                return true;
            }
            set
            {
                this.ViewState["IsSortAscending"] = value;
            }
        }

        /// <summary>
        /// 设置或者获得排序字段
        /// </summary>
        [
        Description("获得或者设置排序字段。"),
        Category("自定义类别"),
        Bindable(true),
        DefaultValue("")
        ]
        public string SortField
        {
            get
            {
                return Convert.ToString(this.ViewState["SortKeyField"]);
            }
            set
            {
                this.ViewState["SortKeyField"] = value;
            }
        }
        #endregion


        protected override void OnSorting(GridViewSortEventArgs e)
        {
            this.SortField = e.SortExpression;

            if (ViewState["SortField"] != null)
            {
                if (this.SortField == ViewState["SortField"].ToString())
                {
                    this.IsSortAscending = !this.IsSortAscending;
                }
                else
                    this.IsSortAscending = true;
            }
            else
            {
                this.IsSortAscending = true;
            }

            ViewState["SortField"] = this.SortField;

            base.OnSorting(e);
        }


        #region 重写方法

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

           
        }

        #endregion

        #region 添加GridView样式、在排序时增加图片
        protected override void OnRowCreated(GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //当鼠标移至行时，改变行的颜色
                e.Row.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#e7e7e7';");
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='';");
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                //设置GridView标题的样式
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    e.Row.Cells[i].CssClass = "TdHeaderStyle1";
                }
                //添加排序的状态图片
                DisplaySortOrderImages(e.Row);//
                this.CreateRow(0, 0, DataControlRowType.EmptyDataRow, DataControlRowState.Normal);
            }
            base.OnRowCreated(e);
         
        }

        //添加排序标注
        protected void DisplaySortOrderImages(GridViewRow dgItem)
        {
            for (int i = 0; i < dgItem.Cells.Count; i++)
            {
                System.Web.UI.WebControls.Label label1 = new System.Web.UI.WebControls.Label();

                if (dgItem.Cells[i].Controls.Count > 0 && dgItem.Cells[i].Controls[0] is LinkButton)
                {
                    string column = ((LinkButton)dgItem.Cells[i].Controls[0]).CommandArgument;
                    if (this.SortField == column)
                    {
                        label1.CssClass = "linkPager";
                        label1.Font.Name = "webdings";

                        label1.Text = (this.IsSortAscending ? " 5" : " 6");

                        dgItem.Cells[i].Controls.Add(label1);

                        return;
                    }
                    else
                    {
                        label1.Text = "";
                        dgItem.Cells[i].Controls.Add(label1);
                    }
                }
            }
        }
        #endregion

        #region 方法
        /**/
        /// <summary>
        ///  获取排序字段
        /// </summary>
        protected string GetSortExpression(GridViewSortEventArgs e)
        {
            string[] sortColumns = null;

            string sortAttribute = SortExpression;

            if (sortAttribute != String.Empty)//有排序字段
            {
                sortColumns = sortAttribute.Split(",".ToCharArray());
            }
            //SortExpression = e.SortExpression;       
            if (sortAttribute.IndexOf(e.SortExpression) > 0 || sortAttribute.StartsWith(e.SortExpression))
            {
                sortAttribute = ModifySortExpression(sortColumns, e.SortExpression);
            }
            else
            {
                sortAttribute = String.Concat(",", e.SortExpression, " ASC ");
            }

            return sortAttribute.TrimStart(",".ToCharArray()).TrimEnd(",".ToCharArray());
            //return e.SortExpression;
        }

        /**/
        /// <summary>
        ///  修改排序顺序
        /// </summary>
        protected string ModifySortExpression(string[] sortColumns, string sortExpression)
        {
            string ascSortExpression = String.Concat(sortExpression, " ASC ");
            string descSortExpression = String.Concat(sortExpression, " DESC ");

            for (int i = 0; i < sortColumns.Length; i++)
            {
                //清除上次排序的信息
                Array.Clear(sortColumns, i, 1);

                if (ascSortExpression.Equals(sortColumns[i]))
                {
                    sortColumns[i] = descSortExpression;
                }
                else if (descSortExpression.Equals(sortColumns[i]))
                {
                    sortColumns[i] = ascSortExpression;
                }
            }
            return String.Join(",", sortColumns).Replace(",,", ",").TrimStart(",".ToCharArray());
        }

        /**/
        /// <summary>
        ///  获取当前的表达式对所选列进行排序
        /// </summary>
        protected void SearchSortExpression(string[] sortColumns, string sortColumn, out string sortOrder, out int sortOrderNo)
        {
            sortOrder = "";
            sortOrderNo = -1;
            for (int i = 0; i < sortColumns.Length; i++)
            {
                //if (sortColumns[i].StartsWith(sortColumn))/*排除两字段以相同的名称开头的情况*/
                if (sortColumns[i].Split(' ')[0].ToString() == sortColumn)
                {
                    sortOrderNo = i + 1;
                    if (AllowMultiColumnSorting)//多列排序
                    {
                        sortOrder = sortColumns[i].Substring(sortColumn.Length).Trim();
                    }
                    else
                    {
                        sortOrder = ((SortDirection == SortDirection.Ascending) ? "ASC" : "DESC");
                    }
                }
            }

        }
        #endregion

        private bool _enableEmptyContentRender = true ;
        /// <summary>
        /// 是否数据为空时显示标题行
        /// </summary>
        public bool EnableEmptyContentRender
        {
            set { _enableEmptyContentRender = value; }
            get { return _enableEmptyContentRender; }
        }

        private string _EmptyDataCellCssClass ;
        /// <summary>
        /// 为空时信息单元格样式类
        /// </summary>
        public string EmptyDataCellCssClass
        {
            set { _EmptyDataCellCssClass = value ; }
            get { return _EmptyDataCellCssClass ; }
        }

        /// <summary>
        /// 为空时输出内容
        /// </summary>
        /// <param name="writer"></param>
        protected virtual void RenderEmptyContent(HtmlTextWriter writer)
        {
            Table t = new Table();
            t.CssClass = this.CssClass;
            t.GridLines = this.GridLines;
            t.BorderStyle = this.BorderStyle;
            t.BorderWidth = this.BorderWidth;
            t.CellPadding = this.CellPadding;
            t.CellSpacing = this.CellSpacing;

            t.HorizontalAlign = this.HorizontalAlign;

            t.Width = this.Width;

            t.CopyBaseAttributes(this);

            TableRow row = new TableRow();
            t.Rows.Add(row);

            foreach (DataControlField f in this.Columns)
            {
                TableCell cell = new TableCell();

                cell.Text = f.HeaderText;

                cell.CssClass = "TdHeaderStyle1";

                row.Cells.Add(cell);
            }

            TableRow row2 = new TableRow();
            t.Rows.Add(row2);

            TableCell msgCell = new TableCell();
            msgCell.CssClass = this._EmptyDataCellCssClass;

            if (this.EmptyDataTemplate != null)
            {
                this.EmptyDataTemplate.InstantiateIn(msgCell);
            }
            else
            {
                msgCell.Text = this.EmptyDataText;
            }

            msgCell.HorizontalAlign = HorizontalAlign.Center;
            msgCell.ColumnSpan = this.Columns.Count;

            row2.Cells.Add(msgCell);

            t.RenderControl(writer);
       }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            foreach (DataControlField f in this.Columns) //英文断字
            {
                if (f.ItemStyle.Wrap && String.IsNullOrEmpty(f.ItemStyle.CssClass))
                {
                    f.ItemStyle.CssClass = "TdWordBreak";
                }
            }
        }

        protected override void  Render(HtmlTextWriter writer)
        {
            if ( _enableEmptyContentRender && ( this.Rows.Count == 0 || this.Rows[0].RowType == DataControlRowType.EmptyDataRow) )
            {
                RenderEmptyContent(writer);
            }
            else
            {
                base.Render(writer);
            }
        }


     

    }
}