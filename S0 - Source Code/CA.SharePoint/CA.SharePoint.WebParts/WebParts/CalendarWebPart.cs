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
using System.Data;
using System.IO;
using System.Xml;
using Microsoft.VisualBasic;

namespace CA.SharePoint
{
    /// <summary>
    /// 部门日历webPart
    /// </summary>
    public class CalendarWebPart : BaseSPListWebPart
    {

        private SPListItemCollection _Items;

        private SPListItemCollection _DeleteEventItems = null;

        private string _ViewUrl;

        protected override void CreateChildControls()
        {

            if (base.ListName == null || base.ListName == "")
                return;

            if (base.ChildControlsCreated) return;

            base.CreateChildControls();

            Calendar cal = new Calendar();
            cal.CellPadding = 0;
            cal.CssClass = "ms-picker-table";
            cal.BorderWidth = new Unit(0);
            cal.Style.Clear();
            cal.TitleStyle.CssClass = "ms-picker-header";
            cal.DayHeaderStyle.CssClass = "ms-picker-dayheader";

            cal.PrevMonthText = "<img border=0 alt='Previous Month' src='/_layouts/images/pickback.gif' >";
            cal.NextMonthText = "<img border=0 alt='Next Month' src='/_layouts/images/pickforward.gif' >";

            if (!this.Width.IsEmpty)
                cal.Width = this.Width;
            else
                cal.Width = new Unit("100%");
            if (!this.Height.IsEmpty)
                cal.Height = this.Height;
            else
                cal.Height = new Unit("100%");

            cal.DayRender += new DayRenderEventHandler(cal_DayRender);

            this.Controls.Add(cal);

            base.ChildControlsCreated = true;

        }

        private void InitData(DayRenderEventArgs e)
        {
            if (_Items != null)
                return;

            SPList list = base.GetCurrentSPList();

            SPQuery q = new SPQuery();

            string sq = @"<Where>
                                    <Or>
                                        <And>
                                            <And>
                                                <Geq>
                                                    <FieldRef Name='EventDate'/><Value Type='DateTime'>{0}</Value>
                                                </Geq>
                                                <Leq>
                                                    <FieldRef Name='EventDate'/><Value Type='DateTime'>{1}</Value>
                                                </Leq>
                                            </And>
                                            <Eq>
                                                <FieldRef Name='fRecurrence'/><Value Type='Boolean'>0</Value>
                                            </Eq>
                                        </And>
                                        <And>
                                            <And>
                                                <Or>
                                                    <Or>
                                                        <And>
                                                            <Leq>
                                                                <FieldRef Name='EventDate'/><Value Type='DateTime'>{2}</Value>
                                                            </Leq>
                                                            <And>
                                                                <Geq>
                                                                    <FieldRef Name='EndDate'/><Value Type='DateTime'>{3}</Value>
                                                                </Geq>
                                                                <Leq>
                                                                    <FieldRef Name='EndDate'/><Value Type='DateTime'>{4}</Value>
                                                                </Leq>
                                                            </And>
                                                        </And>
                                                        <And>
                                                            <Geq>
                                                                    <FieldRef Name='EventDate'/><Value Type='DateTime'>{5}</Value>
                                                            </Geq>
                                                            <Leq>
                                                                    <FieldRef Name='EndDate'/><Value Type='DateTime'>{6}</Value>
                                                            </Leq>
                                                        </And>
                                                    </Or>
                                                    <Or>
                                                        <And>
                                                            <And>
                                                                 <Geq>
                                                                    <FieldRef Name='EventDate'/><Value Type='DateTime'>{7}</Value>
                                                                </Geq>
                                                                <Leq>
                                                                    <FieldRef Name='EventDate'/><Value Type='DateTime'>{8}</Value>
                                                                </Leq>
                                                            </And>
                                                            <Geq>
                                                                    <FieldRef Name='EndDate'/><Value Type='DateTime'>{9}</Value>
                                                            </Geq>
                                                        </And>
                                                        <And>
                                                            <Leq>
                                                                    <FieldRef Name='EventDate'/><Value Type='DateTime'>{10}</Value>
                                                            </Leq>
                                                            <Geq>
                                                                    <FieldRef Name='EndDate'/><Value Type='DateTime'>{11}</Value>
                                                            </Geq>
                                                        </And>
                                                    </Or>
                                                </Or>
                                                <Neq>
                                                    <FieldRef Name='fRecurrence'/><Value Type='Boolean'>0</Value>
                                                </Neq>
                                            </And>
                                            <IsNull>
                                                <FieldRef Name='MasterSeriesItemID'/></Value>
                                            </IsNull>
                                        </And>
                                    </Or>
                              </Where>";

            DateTime firstDay = DateTime.Parse(e.Day.Date.Year + "-" + e.Day.Date.Month + "-1");

            DateTime endDay = firstDay.AddMonths(1);

            q.Query = String.Format(sq, firstDay, endDay, firstDay, firstDay, endDay, firstDay, endDay, firstDay, endDay, endDay, firstDay, endDay);

            _Items = list.GetItems(q);

            //StreamWriter sw = new StreamWriter(@"c:\b.xml",false, Encoding.UTF8);
            //sw.WriteLine(_Items.Xml);
            //sw.Flush();
            //sw.Close();

            //StreamWriter sw2 = new StreamWriter(@"c:\c.xml", false, Encoding.UTF8);
            //sw2.WriteLine(list.Items.Xml);
            //sw2.Flush();
            //sw2.Close();

            //DataTable dt = _Items.GetDataTable();

            //dt.WriteXml(@"c:\a.xml");

            _ViewUrl = list.DefaultViewUrl;

        }

        private void IntiDeleteEvent(DayRenderEventArgs e)
        {
            try
            {
                SPList list = base.GetCurrentSPList();

                SPQuery q = new SPQuery();

                string sq = @"<Where>
                                    <And>
                                        <And>
                                            <Or>
                                                <Or>
                                                    <And>
                                                        <Leq>
                                                            <FieldRef Name='EventDate'/><Value Type='DateTime'>{0}</Value>
                                                        </Leq>
                                                        <And>
                                                            <Geq>
                                                                <FieldRef Name='EndDate'/><Value Type='DateTime'>{1}</Value>
                                                            </Geq>
                                                            <Leq>
                                                                <FieldRef Name='EndDate'/><Value Type='DateTime'>{2}</Value>
                                                            </Leq>
                                                        </And>
                                                    </And>
                                                    <And>
                                                        <Geq>
                                                                <FieldRef Name='EventDate'/><Value Type='DateTime'>{3}</Value>
                                                        </Geq>
                                                        <Leq>
                                                                <FieldRef Name='EndDate'/><Value Type='DateTime'>{4}</Value>
                                                        </Leq>
                                                    </And>
                                                </Or>
                                                <Or>
                                                    <And>
                                                        <And>
                                                             <Geq>
                                                                <FieldRef Name='EventDate'/><Value Type='DateTime'>{5}</Value>
                                                            </Geq>
                                                            <Leq>
                                                                <FieldRef Name='EventDate'/><Value Type='DateTime'>{6}</Value>
                                                            </Leq>
                                                        </And>
                                                        <Geq>
                                                                <FieldRef Name='EndDate'/><Value Type='DateTime'>{7}</Value>
                                                        </Geq>
                                                    </And>
                                                    <And>
                                                        <Leq>
                                                                <FieldRef Name='EventDate'/><Value Type='DateTime'>{8}</Value>
                                                        </Leq>
                                                        <Geq>
                                                                <FieldRef Name='EndDate'/><Value Type='DateTime'>{9}</Value>
                                                        </Geq>
                                                    </And>
                                                </Or>
                                            </Or>
                                            <Neq>
                                                <FieldRef Name='fRecurrence'/><Value Type='Boolean'>0</Value>
                                            </Neq>
                                        </And>
                                        <IsNotNull>
                                            <FieldRef Name='MasterSeriesItemID'/></Value>
                                        </IsNotNull>
                                    </And>
                              </Where>";

                DateTime firstDay = DateTime.Parse(e.Day.Date.Year + "-" + e.Day.Date.Month + "-1");

                DateTime endDay = firstDay.AddMonths(1);

                q.Query = String.Format(sq, firstDay, firstDay, endDay, firstDay, endDay, firstDay, endDay, endDay, firstDay, endDay);

                _DeleteEventItems = list.GetItems(q);
            }
            catch (Exception ex)
            {
                base.RegisterError(ex);
            }

        }

        private string GetDayHtml(DateTime datetime)
        {
            string html = "";
            foreach (SPListItem item in _Items)
            {
                DateTime t = Convert.ToDateTime(item["EventDate"]);

                if (item["fRecurrence"].ToString().ToUpper() == "FALSE")
                {
                    if (t.Day == datetime.Day && t.Month == datetime.Month)
                    {
                        html += item.Title + " ("
                                + ((DateTime)item["EventDate"]).ToShortTimeString() + "~"
                                + ((DateTime)item["EndDate"]).ToShortTimeString() + ")"
                                + "\n";
                    }
                }
                else
                {
                    if (IsDateOverlaped(datetime, item))
                    {
                        html += item.Title + " ("
                                + ((DateTime)item["EventDate"]).ToShortTimeString() + "~"
                                + ((DateTime)item["EndDate"]).ToShortTimeString() + ")"
                                + "\n";
                    }
                }
            }
            return html;
        }

        private void cal_DayRender(object sender, DayRenderEventArgs e)
        {

            if (e.Day.IsOtherMonth)
            {
                e.Cell.Text = "";
                return;
            }
            else
                e.Cell.Attributes.Add("class", "ms-picker-daycenter");

            if(e.Day.IsToday)
                e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#edb244");

            InitData(e);

            IntiDeleteEvent(e);

            e.Cell.Text = e.Day.Date.Day.ToString();

            string html = GetDayHtml(e.Day.Date);

            if (html != "")
            {
                e.Cell.Attributes.Add("title", html);

                if (e.Day.IsToday)
                    e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#edb244");
                else
                    e.Cell.BackColor = System.Drawing.Color.Yellow;
            }

            e.Cell.Attributes.Add("style", "cursor:hand;");

            e.Cell.Attributes.Add("onclick", "javascript:window.location.href='" + _ViewUrl + "?CalendarPeriod=day&CalendarDate=" + e.Day.Date.Year + "%2F" + e.Day.Date.Month + "%2F" + e.Day.Date.Day + "';return false;");

        }

        private bool IsDateOverlaped(DateTime dtCurrent, SPListItem item)
        {
            DateTime dtStart = ((DateTime)item["EventDate"]).Date;
            DateTime dtEnd = ((DateTime)item["EndDate"]).Date;
            string strXML = item["RecurrenceData"].ToString();

            if (dtCurrent.CompareTo(dtEnd) > 0 || dtCurrent.CompareTo(dtStart) < 0)
                return false;

            foreach (SPListItem i in _DeleteEventItems)
            {
                if (i["MasterSeriesItemID"].ToString() == item["ID"].ToString())
                {
                    if (((DateTime)i["EventDate"]).Date.CompareTo(dtCurrent) == 0)
                        return false;
                }
            }

            double dFrequency;

            TimeSpan TS = new TimeSpan(dtCurrent.Ticks - dtStart.Ticks);

            object[] arrWeekday = {
									  new object[] {"su", DayOfWeek.Sunday},
									  new object[] {"mo", DayOfWeek.Monday},
									  new object[] {"tu", DayOfWeek.Tuesday},
									  new object[] {"we", DayOfWeek.Wednesday},
									  new object[] {"th", DayOfWeek.Thursday},
									  new object[] {"fr", DayOfWeek.Friday},
									  new object[] {"sa", DayOfWeek.Saturday}
									};


            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(strXML);

            XmlNode repeatChildNode = xmlDoc.SelectSingleNode("/recurrence/rule/repeat").FirstChild;

            switch (repeatChildNode.Name)
            {
                case "daily":
                    dFrequency = Convert.ToDouble(repeatChildNode.Attributes["dayFrequency"].InnerText);
                    if ((TS.TotalDays % dFrequency) == 0 && (TS.TotalDays / dFrequency) >= 0)
                        return true;
                    break;
                case "weekly":
                    foreach (object[] oWeekday in arrWeekday)
                    {
                        if (dtCurrent.DayOfWeek != (DayOfWeek)oWeekday[1])
                            continue;
                        if (repeatChildNode.Attributes.GetNamedItem(oWeekday[0].ToString()) == null)
                            continue;
                        if (repeatChildNode.Attributes[oWeekday[0].ToString()].InnerText == "TRUE")
                        {
                            dFrequency = Convert.ToDouble(repeatChildNode.Attributes["weekFrequency"].InnerText);
                            if ((Convert.ToInt32(TS.TotalDays / 7) % dFrequency) == 0 && (TS.TotalDays / 7) >= 0)
                                return true;
                        }
                    }
                    break;
                case "monthly":
                    if (dtCurrent.Day.ToString() != repeatChildNode.Attributes["day"].InnerText)
                        return false;
                    dFrequency = Convert.ToDouble(repeatChildNode.Attributes["monthFrequency"].InnerText);
                    if (DateAndTime.DateDiff(DateInterval.Month, dtStart, dtCurrent, Microsoft.VisualBasic.FirstDayOfWeek.Monday, Microsoft.VisualBasic.FirstWeekOfYear.System) % dFrequency == 0)
                        return true;
                    break;
                case "monthlyByDay":
                    if (GetWeekdayOfMonth(dtCurrent).IndexOf(repeatChildNode.Attributes["weekdayOfMonth"].InnerText) == -1)
                        return false;

                    foreach (object[] oWeekday in arrWeekday)
                    {
                        if (dtCurrent.DayOfWeek != (DayOfWeek)oWeekday[1])
                            continue;
                        if (repeatChildNode.Attributes.GetNamedItem(oWeekday[0].ToString()) == null)
                            continue;
                        if (repeatChildNode.Attributes[oWeekday[0].ToString()].InnerText == "TRUE")
                        {
                            dFrequency = Convert.ToDouble(repeatChildNode.Attributes["monthFrequency"].InnerText);
                            if (DateAndTime.DateDiff(DateInterval.Month, dtStart, dtCurrent, Microsoft.VisualBasic.FirstDayOfWeek.Monday, Microsoft.VisualBasic.FirstWeekOfYear.System) % dFrequency == 0)
                                return true;
                        }
                    }
                    break;
                case "yearly":
                    if (dtCurrent.Month != dtStart.Month || dtCurrent.Day != dtStart.Day)
                        return false;
                    dFrequency = Convert.ToDouble(repeatChildNode.Attributes["yearFrequency"].InnerText);
                    if ((dtCurrent.Year - dtStart.Year) % dFrequency == 0)
                        return true;
                    break;
            }

            return false;
        }

        private string GetWeekdayOfMonth(DateTime dtCurrent)
        {
            DateTime dtFirstDayOfMonth = new DateTime(dtCurrent.Year, dtCurrent.Month, 1);
            TimeSpan TS = new TimeSpan(dtCurrent.Ticks - dtFirstDayOfMonth.Ticks);

            DateTime dtLastDayOfMonth = dtFirstDayOfMonth.AddMonths(1).AddDays(-1);
            TimeSpan tsLast = new TimeSpan(dtLastDayOfMonth.Ticks - dtCurrent.Ticks);

            string sRet = "";

            switch (Convert.ToInt16(TS.TotalDays / 7))
            {
                case 0:
                    sRet += "first";
                    break;
                case 1:
                    sRet += "second";
                    break;
                case 2:
                    sRet += "third";
                    break;
                case 3:
                    sRet += "fourth";
                    break;
            }

            // Maybe the last week also is the fourth week.
            if (Convert.ToInt16(tsLast.TotalDays / 7) == 0)
                sRet += ";last";

            return sRet;
        }

    }
}