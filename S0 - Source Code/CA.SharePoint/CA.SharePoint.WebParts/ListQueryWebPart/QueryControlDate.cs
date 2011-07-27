using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Drawing;
using System.Xml;
using System.Xml.Serialization;
using System.Data;

using Microsoft.SharePoint;
using CA.SharePoint.CamlQuery;

namespace CA.SharePoint
{


    /// <summary>
    /// 时间类型的查询控件
    /// </summary>
    class QueryControlDate : WebControl, IQueryControl
    {
        Microsoft.SharePoint.WebControls.DateTimeControl _beginDate = new Microsoft.SharePoint.WebControls.DateTimeControl();
        Microsoft.SharePoint.WebControls.DateTimeControl _endDate = new Microsoft.SharePoint.WebControls.DateTimeControl();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            _beginDate.DateOnly = true;
            _beginDate.EnableViewState = true;
            _endDate.DateOnly = true;
            _endDate.EnableViewState = true;

           //_beginDate.CssClassTextBox = "test";

            AddHtml("<table border='0' cellpadding='0' cellspacing='0'><tr><td>");

            this.Controls.Add(_beginDate);

             AddHtml("</td><td>-</td><td>");

            this.Controls.Add(_endDate);

            AddHtml( "</td></tr></table>" );

            ChangeWidth(_beginDate);
            ChangeWidth(_endDate);

            //if (_PropertyPersistenceService != null)
            //{
            //    if (!Page.IsPostBack)
            //    {
            //        string begin = _PropertyPersistenceService.GetPropertyValue(this, "beginTime");
            //        string end = _PropertyPersistenceService.GetPropertyValue(this, "endTime");

            //        if ( !String.IsNullOrEmpty( begin ))
            //            _beginDate.SelectedDate = Convert.ToDateTime( begin );

            //        if ( !String.IsNullOrEmpty(end ) )
            //            _endDate.SelectedDate = Convert.ToDateTime(end);
            //    }
            //}

            this.ChildControlsCreated = true;
        }

        void ChangeWidth(Control ctl)
        {
            if (ctl.Controls.Count == 0)
                return;

            TextBox ctl1 = ctl.Controls[0] as TextBox ;

            if (ctl1 != null)
                ctl1.Width = new Unit("60px");            
        }

        void AddHtml(string html)
        {
            this.Controls.Add( new LiteralControl(html) );
        }

        private string _FieldName;
        public string FieldName
        {
            get { return _FieldName; }
            set { _FieldName = value; }
        }

        public CAMLExpression<object> QueryExpression
        {
            get
            {
                this.EnsureChildControls();

                if (_PropertyPersistenceService != null)
                {
                    if (Page.IsPostBack)
                    {
                        _PropertyPersistenceService.SetPropertyValue(this, "beginTime",
                            _beginDate.IsDateEmpty ? "" : _beginDate.SelectedDate.ToString());

                        _PropertyPersistenceService.SetPropertyValue(this, "endTime",
                            _endDate.IsDateEmpty ? "" : _endDate.SelectedDate.ToString());
                    }
                    else
                    {
                        string begin = _PropertyPersistenceService.GetPropertyValue(this, "beginTime");
                        string end = _PropertyPersistenceService.GetPropertyValue(this, "endTime");

                        if (!String.IsNullOrEmpty(begin))
                        {
                            _beginDate.SelectedDate = Convert.ToDateTime(begin);
                        }

                        if (!String.IsNullOrEmpty(end))
                            _endDate.SelectedDate = Convert.ToDateTime(end);
                    }
                }

                TypeQueryField<DateTime> f = new TypeQueryField<DateTime>(_FieldName);

                CAMLExpression<object> expr = null;

                DateTime nowDate = DateTime.Now.Date;

                if (!_beginDate.IsDateEmpty || _beginDate.SelectedDate.Date != nowDate ) //默认值跟当前日期相同，不是默认值时说明选择了时间
                {
                    expr = f.MoreEqual(_beginDate.SelectedDate);
                }

                if (!_endDate.IsDateEmpty || _beginDate.SelectedDate.Date != nowDate)
                {
                    if (expr != null)
                        expr = expr & f.LessThan(_endDate.SelectedDate);
                    else
                        expr = f.LessThan(_endDate.SelectedDate);
                }

                return expr;
            }
        }

        private IPropertyPersistenceService _PropertyPersistenceService;
        public IPropertyPersistenceService PropertyPersistenceService
        {
            set
            {
                _PropertyPersistenceService = value;
            }
        }
    }



}
