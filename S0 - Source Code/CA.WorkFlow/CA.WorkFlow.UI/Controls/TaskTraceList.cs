using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using QuickFlow.Core;
using SmartForm;
using System.Data;
using CodeArt.SharePoint.CamlQuery;
using Microsoft.SharePoint;
using System.Web;


namespace CA.WorkFlow.UI
{
    public class TaskTraceList : SPGridView
    {
        private string _TaskFilters = "状态:已完成";
        /// <summary>
        /// 显示过滤器 状态:以完成&Title=dfsdf
        /// </summary>
        public string TaskFilters
        {
            get { return _TaskFilters; }
            set { _TaskFilters = value; }
        }       

        public TaskTraceList()
        {
            this.AutoGenerateColumns = false;
        }

        private void CreateColumns()
        {
            BoundField f1 = new BoundField();
            //f1.HeaderText = "任务/Task";
            //f1.DataField = "Title";
            //this.Columns.Add(f1);

            f1 = new BoundField();
            f1.HeaderText = "操作人/User";
            f1.DataField = "AssignedTo";
            this.Columns.Add(f1);

            f1 = new BoundField();
            f1.HeaderText = "状态/Status";
            f1.DataField = "Status";
            this.Columns.Add(f1);

            f1 = new BoundField();
            f1.HtmlEncode = false;
            f1.HeaderText = "意见/Comments";
            f1.DataField = "Body";
            f1.HtmlEncode = false;
            this.Columns.Add(f1);

            f1 = new BoundField();
            f1.HeaderText = "结果/Outcome";
            f1.DataField = "WorkflowOutcome";
            this.Columns.Add(f1);

            f1 = new BoundField();
            f1.HeaderText = "开始时间/StartDate";
            f1.DataField = "StartDate";
            this.Columns.Add(f1);

            f1 = new BoundField();
            f1.HeaderText = "结束时间/CompleteDate";
            f1.DataField = "CompleteDate";
            this.Columns.Add(f1);
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (WorkflowContext.Current.Mode != ContextMode.Start)
            {
                if (this.Columns.Count == 0)
                {
                    CreateColumns();
                }

                DataTable tasks = GetTasks();

                if (tasks != null)
                {
                    tasks.Columns.Add("CompleteDate");

                    foreach (DataRow r in tasks.Rows)
                    {
                        if (r["Status"].ToString() == "Completed")
                            r["CompleteDate"] = r["Modified"];
                    }
                }

                if (tasks != null && tasks.Rows.Count > 0)
                {
                    this.DataSource = tasks;
                    this.DataBind();
                }
                else
                {
                    this.Visible = false;
                }
            }
            else
            {
                this.Visible = false;
            }
        }

        DataTable GetTasks()
        {
            string itemId = HttpContext.Current.Request.QueryString["id"];

            TypedQueryField<Int32> WorkflowItemIdField = new TypedQueryField<Int32>("WorkflowItemId", false);

          
            QueryField field2 = new QueryField("WorkflowListId", false);
            CamlExpression queryExpr = (WorkflowItemIdField == Convert.ToInt32(itemId))&&(field2==SPContext.Current.ListId);
            SPQuery q = new SPQuery();
            SPListItemCollection items = null;
               SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                    {
                        SPList list = web.Lists["Tasks"];
                        SPQuery query = new SPQuery
                        {
                            Query = CamlBuilder.Where(list, queryExpr)
                        };
                        items = list.GetItems(query);                      
                    }
                }
            });

            DataTable t = items.GetDataTable();

            if (t != null)
            {

                t.Columns.Add(WorkflowConstants.Field_TaskStep);
                t.Columns.Add(WorkflowConstants.TaskFormURLPropertyName);
                t.Columns.Add(WorkflowConstants.Field_StatusName);

                for (int i = 0; i < t.Rows.Count; i++)
                {
                    DataRow r = t.Rows[i];
                    WorkflowTask task = WorkflowTask.FromListItem(items[i]);
                    r[WorkflowConstants.Field_TaskStep] = task.Step;
                    r[WorkflowConstants.TaskFormURLPropertyName] = task.FormURL;
                    r[WorkflowConstants.Field_StatusName] = task.StatusName;
                    r["Status"] = task.Status.ToString();
                }
            }

            return t;
        }

        //public DataTable GetTasks(string filters)
        //{
        //    // This item is obfuscated and can not be translated.
        //    TypedQueryField<int> field = new TypedQueryField<int>("WorkflowItemId", false);
        //    QueryField field2 = new QueryField("WorkflowListId", false);
        //    bool flag1 = field == SPContext.Current.ListItem.ID;
        //    if (CamlExpression.op_False((CamlExpression)flag1))
        //    {
        //        goto Label_0054;
        //    }
        //    CamlExpression queryExpr = (CamlExpression)(flag1 & (field2 == this.listId));
        //    if (!string.IsNullOrEmpty(filters))
        //    {
        //        string[] strArray = filters.Split(new char[] { '&', ';' });
        //        foreach (string str in strArray)
        //        {
        //            string[] strArray2 = str.Split(new char[] { ':' });
        //            if (strArray2.Length != 2)
        //            {
        //                throw new Exception("filter error:" + filters);
        //            }
        //            QueryField field3 = new QueryField(strArray2[0], false);
        //            if (CamlExpression.op_False(queryExpr))
        //            {
        //                goto Label_00F9;
        //            }
        //            queryExpr &= field3 == strArray2[1];
        //        }
        //    }
        //    DataTable t = null;
        //    SPListItemCollection items = null;
        //    SPSecurity.RunWithElevatedPrivileges(delegate
        //    {
        //        using (SPSite site = new SPSite(SPContext.Current.Site.ID))
        //        {
        //            using (SPWeb web = site.OpenWeb(SPContext.Current.Site.RootWeb.ID))
        //            {
        //                SPList list = web.Lists["Tasks"];
        //                SPQuery query = new SPQuery
        //                {
        //                    Query = CamlBuilder.Where(list, queryExpr)
        //                };
        //                items = list.GetItems(query);
        //                t = items.GetDataTable();
        //            }
        //        }
        //    });
        //    if (t != null)
        //    {
        //        t.Columns.Add(WorkflowConstants.Field_TaskStep);
        //        t.Columns.Add("TaskFormURL");
        //        t.Columns.Add(WorkflowConstants.Field_StatusName);
        //        for (int i = 0; i < t.Rows.Count; i++)
        //        {
        //            DataRow row = t.Rows[i];
        //            WorkflowTask task = WorkflowTask.FromListItem(items[i]);
        //            row[WorkflowConstants.Field_TaskStep] = task.Step;
        //            row["TaskFormURL"] = task.FormURL;
        //            row[WorkflowConstants.Field_StatusName] = task.StatusName;
        //            row["Status"] = task.Status.ToString();
        //        }
        //    }
        //    return t;
        //}


    }
}
