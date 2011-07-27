using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Reflection;
using System.ComponentModel;
//using CA.Web.Controls.ComponentDesign;

namespace CA.Web.ComponentDesign
{
    /// <summary>
    ///  具有布局能力的 UI创建
    /// </summary>
    /// 
    //[TypeConverter(typeof(LayoutableConverter))]
    public class LayoutableUIBuilder : IDesignerUIBuilder
    {

        //private Unit _NameColumnWidth;

        //public Unit NameColumnWidth
        //{
        //    get { return _NameColumnWidth; }
        //    set { _NameColumnWidth = value; }
        //}

        //private Unit _ValueColumnWidth;

        //public Unit ValueColumnWidth
        //{
        //    get { return _ValueColumnWidth; }
        //    set { _ValueColumnWidth = value; }
        //}

        private int _RepeatColumns;
        /// <summary>
        /// 行能显示几列
        /// </summary>
        public int RepeatColumns
        {
            get
            {
                return _RepeatColumns;
            }
            set
            {
                _RepeatColumns = value;
            }
        }
        //记录所有编辑控件
        private IDictionary<string, IFiledEditor> FieldSetEditors = new Dictionary<string, IFiledEditor>();

        #region IDesignerUIBuilder 成员
        /// <summary>
        /// 动态创建编辑界面
        /// </summary>
        /// <param name="designer"></param>
        /// <param name="set"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public IDictionary<string, IFiledEditor> BuildEditUI( ComponentDesignControl designer , ComponentSet set, object obj)
        {
            if (_RepeatColumns % 2 != 0)
                _RepeatColumns += 1;
            int FieldNums = set.Fields.Count;
            int surplus = FieldNums % (_RepeatColumns / 2);
            int rowCount = GetRepeatRows(FieldNums, _RepeatColumns / 2);        

            for (int i = 0; i < rowCount; i++)
            {
                TableRow row = new TableRow();
                designer.Rows.Add(row);
                int tmp = GetRepeatCols(i, FieldNums, _RepeatColumns/2, rowCount);

                for (int j = 0; j < tmp; j++)
                {
                    FieldSet p = set.Fields[i * _RepeatColumns / 2 + j];
                    TableCell cell = new TableCell();
                    cell.CssClass = designer.NameCellCssClass;          
                        cell.Text = p.DisplayName;                   
                    row.Cells.Add(cell);                
                    
                    cell = new TableCell();                    
                    row.Cells.Add(cell);
                    System.Web.UI.WebControls.WebControl ctl = FieldEditorFactory.GetFieldEditor(p);
                    ctl.CssClass = designer.ValueCellCssClass;
                    ctl.ID = p.UniqueName;

                    IFiledEditor fe = (IFiledEditor)ctl;
                    fe.FieldValue = p.GetValue(obj);

                    this.FieldSetEditors.Add(p.UniqueName, fe);

                    cell.Controls.Add(ctl);

                    designer.RaiseControlCreatedEvent(ctl, p);
                }
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
        /// <summary>
        /// 动态创建回传界面
        /// </summary>
        /// <param name="designer"></param>
        /// <param name="set"></param>
        /// <returns></returns>
        public IDictionary<string, IFiledEditor> BuildRetrieveUI(ComponentDesignControl designer, ComponentSet set)
        {
            this.FieldSetEditors.Clear();
            if (_RepeatColumns % 2 != 0)
                _RepeatColumns += 1;
            int FieldNums = set.Fields.Count;
            int surplus = FieldNums % (_RepeatColumns / 2);
            int rows = FieldNums / (_RepeatColumns / 2);

            int rowCount = 0;
            if (surplus > 0)
            {
                rowCount = rows + 1;
            }
            else
            {
                rowCount = rows;
            }

            for (int i = 0; i < rowCount; i++)
            {
                TableRow row = new TableRow();
                designer.Rows.Add(row);
                int tmp = 0;
                if (i == rowCount - 1 && surplus == 0)
                {
                    tmp = _RepeatColumns/2;
                }
                else if (i == rowCount - 1 && surplus > 0)
                {
                    tmp = surplus;
                }
                else
                    tmp = _RepeatColumns/2;

                for (int j = 0; j < tmp; j++)
                {
                    FieldSet p = set.Fields[i * (_RepeatColumns / 2) + j];
                    TableCell cell = new TableCell();
                    row.Cells.Add(cell);
                    cell.Text = p.DisplayName;
                    cell.CssClass = designer.NameCellCssClass;


                    cell = new TableCell();
                    System.Web.UI.WebControls.WebControl ctl = FieldEditorFactory.GetFieldEditor(p);
                    ctl.CssClass = designer.ValueCellCssClass;
                    ctl.ID = p.UniqueName;

                    IFiledEditor fe = (IFiledEditor)ctl;
                    
                        this.FieldSetEditors.Add(p.UniqueName, fe);
                   

                    cell.Controls.Add(ctl);
                    row.Cells.Add(cell);

                   

                    designer.RaiseControlCreatedEvent(ctl, p);
                }
            }

            return FieldSetEditors;
        }
        /// <summary>
        /// 动态建立视图界面
        /// </summary>
        /// <param name="designer"></param>
        /// <param name="set"></param>
        /// <param name="obj"></param>
        public void BuildViewUI(ComponentDesignControl designer ,ComponentSet set, object obj)
        {
            if (_RepeatColumns % 2 != 0)
                _RepeatColumns += 1;
            int FieldNums = set.Fields.Count;
            int surplus = FieldNums % (_RepeatColumns/2);
            int rows = FieldNums / (_RepeatColumns/2);
            int rowCount = 0;

            //IList<FieldSet> sortedFields = FieldsSort(set.Fields);
            if (surplus > 0)
            {
                rowCount = rows + 1;
            }
            else
            {
                rowCount = rows;
            }

            for (int i = 0; i < rowCount; i++)
            {
                TableRow row = new TableRow();
                designer.Rows.Add(row);
                int tmp = 0;
                if (i == rowCount - 1 && surplus == 0)
                {
                    tmp = _RepeatColumns/2;
                }
                else if (i == rowCount - 1 && surplus > 0)
                {
                    tmp = surplus;
                }
                else
                    tmp = _RepeatColumns/2;

                for (int j = 0; j < tmp; j++)
                {
                    FieldSet p = set.Fields[i * _RepeatColumns / 2 + j];
                    
                    TableCell cell = new TableCell();

                    cell.Width = new Unit("131px");
                    row.Cells.Add(cell);                   
                    cell.Text = p.DisplayName;              
                    cell = new TableCell();
                    row.Cells.Add(cell);
                    cell.Text = "" + p.GetValue(obj);
                }
            }

        }

        #endregion

        #region 布局的行和列的算法
        private int GetRepeatRows(int TotalNums,int RepeatCols)
        {
            int surplus = TotalNums % RepeatCols;
            int rows = TotalNums / RepeatCols;
            int rowCount = 0;
            if (surplus > 0)
            {
                rowCount = rows + 1;
            }
            else
            {
                rowCount = rows;
            }
            return rowCount;
        }
        private int GetRepeatCols(int col, int TotalNums, int RepeatCols,int rowCount)
        {
            int tmp=0;
            int surplus = TotalNums % RepeatCols;
            if (col == rowCount - 1 && surplus == 0)
            {
                tmp = RepeatCols;
            }
            else if (col == rowCount - 1 && surplus > 0)
            {
                tmp = surplus;
            }
            else
                tmp = RepeatCols;
            return tmp;
        }
        #endregion

    }
}
