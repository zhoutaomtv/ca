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
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using System.Net;

namespace CA.SharePoint
{
    /// <summary>
    /// 天气预报webPart - http://www.google.com/ig/api?weather=shanghai&hl=zh-cn
    /// 若需要使用代理，可以设置webpart的属性，或者，配置到web.config中:
    /// <![CDATA[
    /// <system.net>
    ///<defaultProxy enabled="true" useDefaultCredentials="true">
    ///  <proxy proxyaddress="http://ssahn095.smc.saicmotor.com:8080"  />    
    ///</defaultProxy>    
    ///</system.net>
    /// ]]>
    /// </summary>
    public class WeatherWebPart : BaseSPWebPart
    {
        //private const string strRequestUrl = "http://php.weather.sina.com.cn/search.php";  //数据源位置
        const string strRequestPrifix = "http://www.google.com";
        private const string strRequestUrl = "http://www.google.com/ig";  //数据源位置

        #region webpart公开属性
        private const string defaultSelectedCity = "上海";//默认选择的城市
        private string _SelectedCity = defaultSelectedCity;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [WebDisplayName("默认城市")]
        public string SelectedCity
        {
            get
            {
                return _SelectedCity;
            }

            set
            {
                if (HttpContext.Current.Cache["WeatherWebPart_Weather_"+_SelectedCity] != null)
                    HttpContext.Current.Cache.Remove("WeatherWebPart_Weather_"+_SelectedCity);
                _SelectedCity = value;
            }
        }

        private string _ProxyUrl;//代理服务器
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [WebDisplayName("代理服务器Url")]
        [Category("代理设置")]
        public string ProxyUrl
        {
            get
            {
                return _ProxyUrl;
            }

            set
            {
                _ProxyUrl = value;
            }
        }

        private const int defaultProxyPort = 8080;//默认代理服务器端口
        private int _ProxyPort = defaultProxyPort;//代理服务器端口
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [WebDisplayName("代理服务器端口")]
        [Category("代理设置")]
        public int ProxyPort
        {
            get
            {
                return _ProxyPort;
            }

            set
            {
                _ProxyPort = value;
            }
        }

        private string _ProxyAccount;//代理服务器登陆帐号
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [WebDisplayName("代理服务器登陆帐号")]
        [Category("代理设置")]
        public string ProxyAccount
        {
            get
            {
                return _ProxyAccount;
            }

            set
            {
                _ProxyAccount = value;
            }
        }

        private string _ProxyPassword;//代理服务器登陆密码
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [WebDisplayName("代理服务器登陆密码")]
        [Category("代理设置")]
        public string ProxyPassword
        {
            get
            {
                return _ProxyPassword;
            }

            set
            {
                _ProxyPassword = value;
            }
        }

        private string _ProxyDomain;//代理服务器域
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [WebDisplayName("代理服务器域")]
        [Category("代理设置")]
        public string ProxyDomain
        {
            get
            {
                return _ProxyDomain;
            }

            set
            {
                _ProxyDomain = value;
            }
        }

        private bool _IsShowExtendContent = false;
        [Personalizable(PersonalizationScope.Shared)]
        [WebBrowsable]
        [WebDisplayName("显示扩展信息")]
        public bool IsShowExtendContent
        {
            get
            {
                return _IsShowExtendContent;
            }

            set
            {
                _IsShowExtendContent = value;
            }
        }
        #endregion

        /// <summary>
        /// 城市拼音
        /// </summary>
        /// <returns>Hashtable</returns>
        private Hashtable Cities
        {
            get
            {
                Hashtable city = new Hashtable();
                city.Add("北京", "beijing");
                city.Add("上海", "shanghai");
                city.Add("天津", "tianjin");
                city.Add("重庆", "chongqing");
                city.Add("石家庄", "shijiazhuang");
                city.Add("太原", "taiyuan");
                city.Add("呼和浩特", "huhehaote");
                city.Add("沈阳", "changchun");
                city.Add("长春", "changchun");
                city.Add("哈尔滨", "haerbin");
                city.Add("南京", "nanjing");
                city.Add("杭州", "hangzhou");
                city.Add("合肥", "hefei");
                city.Add("福州", "fuzhou");
                city.Add("南昌", "nanchang");
                city.Add("济南", "jinan");
                city.Add("郑州", "zhengzhou");
                city.Add("武汉", "wuhan");
                city.Add("长沙", "changsha");
                city.Add("广州", "guangzhou");
                city.Add("南宁", "nanning");
                city.Add("海口", "haikou");
                city.Add("成都", "chengdu");
                city.Add("贵阳", "guiyang");
                city.Add("昆明", "kunming");
                city.Add("拉萨", "lhasa");
                city.Add("西安", "xian");
                city.Add("兰州", "lanzhou");
                city.Add("西宁", "xining");
                city.Add("银川", "yinchuan");
                city.Add("乌鲁木齐", "wulumuqi");
                city.Add("香港", "hongkong");
                city.Add("澳门", "macau");
                city.Add("台北", "taipei");
                city.Add("台湾", "taiwan");
                return city;
            }
        }

        //protected override void OnPreRender(EventArgs e)
        //{
        //    base.OnPreRender(e);
        //    RegisterJS();
        //}

        protected override void RenderContents(HtmlTextWriter writer)
        {
            string cacheKey = "WeatherWebPart_Weather_" + _SelectedCity;

            try
            {             

                string strWeacherContent = "" + HttpContext.Current.Cache[cacheKey];

                if (String.IsNullOrEmpty(strWeacherContent))
                {
                    strWeacherContent = GetWeatherStr();
                    //strWeacherContent = strWeacherContent.Replace("http://image2.sina.com.cn/dy/weather/images/figure/", "/_layouts/MCS_Resources/theme1/");
                    HttpContext.Current.Cache.Add(cacheKey, strWeacherContent, null, System.DateTime.Now.AddHours(2), 
                        System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, null);
                    //strWeacherContent = strWeacherContent.Replace("Weather3DBlk", "WeatherWebPart_" + this.ClientID);
                }                

                writer.Write("<table width='100%' border='0' cellpadding='0' cellspacing='0'");
                writer.Write("  <tr>");
                writer.Write("      <td height='5'>");
                writer.Write("      </td>");
                writer.Write("  </tr>");
                //writer.Write("  <tr>");
                //writer.Write("      <td align='right'>");
                //writer.Write("          <table border='0' cellpadding='0' cellspacing='0'>");
                //writer.Write("              <tr>");
                //writer.Write("                  <td onclick='javascript:" + this.ClientID + "_showWeather(0);' style='cursor:hand;'>今天|</td>");
                //writer.Write("                  <td onclick='javascript:" + this.ClientID + "_showWeather(4);' style='cursor:hand;'>明天|</td>");
                //writer.Write("                  <td onclick='javascript:" + this.ClientID + "_showWeather(8);' style='cursor:hand;'>后天</td>");
                //writer.Write("              </tr>");
                //writer.Write("          </table>");
                //writer.Write("      </td>");
                //writer.Write("  </tr>");
                writer.Write("  <tr>");
                writer.Write("      <td align='center'>");            

                if (strWeacherContent == "")
                    writer.Write("找不到 " + _SelectedCity + " 城市的天气预报.");

                writer.Write(strWeacherContent);
                writer.Write("      </td>");
                writer.Write("  </tr>");
                writer.Write("  <tr>");
                writer.Write("      <td height='5'>");
                writer.Write("      </td>");
                writer.Write("  </tr>");
                writer.Write("</table>");

            }
            catch (Exception e)
            {                
                base.RenderError(e, writer);

                HttpContext.Current.Cache.Add(cacheKey , "发生错误:" + e.Message, null, System.DateTime.Now.AddMinutes(5),
                    System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Low, null);
            }
        }

        private string GetWeatherStr()
        {
            string weather = string.Empty;
            XmlDocument xmlDoc = GetGoogleApiWeather();

            if (xmlDoc != null)
            {
                XmlNodeList nodeList = xmlDoc.SelectNodes("xml_api_reply/weather");
                if (nodeList != null)
                {
                    XmlNode node = nodeList.Item(0).SelectSingleNode("current_conditions");
                    if (node != null)
                    {
                        weather = _topHtml;
                        string icon = strRequestPrifix + node.SelectSingleNode("icon").Attributes["data"].InnerText;
                        string condition = node.SelectSingleNode("condition").Attributes["data"].InnerText;
                        string temp = node.SelectSingleNode("temp_c").Attributes["data"].InnerText;
                        string wind = node.SelectSingleNode("wind_condition").Attributes["data"].InnerText;
                        string humidity = node.SelectSingleNode("humidity").Attributes["data"].InnerText;
                        weather = weather.Replace("$$City", _SelectedCity);
                        weather = weather.Replace("$$current_conditions_icon_data", icon);
                        weather = weather.Replace("$$current_conditions_condition_data", condition);
                        weather = weather.Replace("$$current_conditions_temp_c_data", temp);
                        weather = weather.Replace("$$current_conditions_wind_condition_data", wind).Replace("、", "<br />");
                        weather = weather.Replace("$$current_conditions_humidity_data", humidity);
                    }
                    XmlNodeList nodes = nodeList.Item(0).SelectNodes("forecast_conditions");
                    if (nodes != null && nodes.Count > 0)
                    {
                        weather += _leftHtml;
                        for (int iCount = 0; iCount < nodes.Count; iCount++)
                        {
                            if (iCount > 2)
                                continue;
                            string contentHtml = _contentHtml;
                            string day = nodes[iCount].SelectSingleNode("day_of_week").Attributes["data"].InnerText;
                            string icon = strRequestPrifix + nodes[iCount].SelectSingleNode("icon").Attributes["data"].InnerText;
                            string condition = nodes[iCount].SelectSingleNode("condition").Attributes["data"].InnerText;
                            string low = nodes[iCount].SelectSingleNode("low").Attributes["data"].InnerText;
                            string high = nodes[iCount].SelectSingleNode("high").Attributes["data"].InnerText;
                            contentHtml = contentHtml.Replace("$$forecast_conditions_day", day);
                            contentHtml = contentHtml.Replace("$$forecast_conditions_icon_data", icon);
                            contentHtml = contentHtml.Replace("$$forecast_conditions_condition_data", condition);
                            contentHtml = contentHtml.Replace("$$forecast_conditions_low_data", low);
                            contentHtml = contentHtml.Replace("$$forecast_conditions_high_data", high);
                            weather += contentHtml;
                        }
                        weather += _rigthHtml;
                    }
                }
            }
            return weather;
        }

        private XmlDocument GetGoogleApiWeather()
        {
            string cityCode = string.Empty;
            if (this.Cities.ContainsKey(_SelectedCity))
            {
                cityCode = this.Cities[_SelectedCity].ToString();
            }
            else
            {
                cityCode = _SelectedCity;
            }
            NameValueCollection collection = new NameValueCollection();
            collection.Add("weather", cityCode);
            collection.Add("hl", "zh-cn");

            //获取代理信息
            System.Net.WebProxy wp;
            if (string.IsNullOrEmpty(_ProxyUrl))
            {
                wp = null;
            }
            else
            {
                wp = new System.Net.WebProxy(_ProxyUrl, _ProxyPort);
                wp.BypassProxyOnLocal = true;

                if (String.IsNullOrEmpty(_ProxyAccount))
                {
                    wp.Credentials = System.Net.CredentialCache.DefaultCredentials;
                }
                else
                {
                    System.Net.ICredentials credentials = new System.Net.NetworkCredential(_ProxyAccount, _ProxyPassword, _ProxyDomain);
                    wp.Credentials = credentials;
                }
            }


            //获取天气预报页面HTML内容
            string url = strRequestUrl + "/api";
            XmlDocument xmlDoc = WebRequestCommon.GetRequestPageInnerXML(url, collection, wp);

            return xmlDoc;
        }

        #region "HTML Template"
        private const string _topHtml = @"<div style='color:#000000;width:95%'>
    <div id='weatherAJAX'>
		<div id='ctl00_divCurrentWeather' style='padding-bottom: 5px; margin-bottom: 5px; border-bottom: dotted 1px #999999; overflow:auto; zoom:1;'>
    	<div style='width:50%; float:left; text-align:center;'>
        	<strong>$$City</strong><br />
        	<img src='$$current_conditions_icon_data' style='border-width:0px;' /><br />$$current_conditions_condition_data 
		</div>
    	<div style='float:left; line-height: 170%; text-align: left'>
        气温：$$current_conditions_temp_c_data (度)<br />
        $$current_conditions_wind_condition_data<br />
        $$current_conditions_humidity_data
    </div>
</div>";

        private const string _leftHtml = @"<div id='ctl00_divForecastWeather'>
    <table border='0' cellpadding='0' cellspacing='0' width='100%' style='margin-left: auto;margin-right: auto;'>
        <tr>";

        private const string _rigthHtml = @"        </tr>
    </table>
</div>";

        private const string _contentHtml = @"<td style='text-align: center;'>
                <strong>
                   $$forecast_conditions_day
                </strong><br />
                <img src='$$forecast_conditions_icon_data' /><br />
                $$forecast_conditions_condition_data<br />
                <span style='font-size: 8pt'>$$forecast_conditions_low_data/$$forecast_conditions_high_data&#176;C</span>
            </td>";
        #endregion

        #region "Google数据源返回格式"
        /*
          <?xml version="1.0" ?> 
        - <xml_api_reply version="1">
        - <weather module_id="0" tab_id="0">
        - <forecast_information>
              <city data="shanghai" /> 
              <postal_code data="shanghai" /> 
              <latitude_e6 data="" /> 
              <longitude_e6 data="" /> 
              <forecast_date data="2008-12-10" /> 
              <current_date_time data="2008-12-10 17:00:00 +0000" /> 
              <unit_system data="SI" /> 
          </forecast_information>
        - <current_conditions>
              <condition data="晴" /> 
              <temp_f data="64" /> 
              <temp_c data="18" /> 
              <humidity data="湿度： 32%" /> 
              <icon data="/images/weather/sunny.gif" /> 
              <wind_condition data="风向： 北、风速：6 (公里/小时）" /> 
          </current_conditions>
        - <forecast_conditions>
              <day_of_week data="今天" /> 
              <low data="8" /> 
              <high data="19" /> 
              <icon data="/images/weather/mostly_sunny.gif" /> 
              <condition data="以晴为主" /> 
          </forecast_conditions>
        - <forecast_conditions>
              <day_of_week data="周四" /> 
              <low data="6" /> 
              <high data="14" /> 
              <icon data="/images/weather/mostly_sunny.gif" /> 
              <condition data="以晴为主" /> 
          </forecast_conditions>
        - <forecast_conditions>
              <day_of_week data="周五" /> 
              <low data="10" /> 
              <high data="16" /> 
              <icon data="/images/weather/sunny.gif" /> 
              <condition data="晴" /> 
          </forecast_conditions>
        - <forecast_conditions>
              <day_of_week data="周六" /> 
              <low data="7" /> 
              <high data="15" /> 
              <icon data="/images/weather/chance_of_rain.gif" /> 
              <condition data="可能有雨" /> 
          </forecast_conditions>
          </weather>
          </xml_api_reply>
        */
        #endregion

        #region "Old"
        //       /// <summary>
//       /// 注册客户端js脚本
//       /// </summary>
//        private void RegisterJS()
//        {
//            string js = "<script language='javascript'>";
//            if (_IsShowExtendContent)
//                js += "var $$_IsShowExtendContent = 'true';";
//            else
//                js += "var $$_IsShowExtendContent = 'false';";

//            js += @"function $$_showWeather(number)
//                            {
//                                var obj = document.getElementById('WeatherWebPart_$$').childNodes;
//                                for(i =0;i<obj.length;i++)
//                                {
//                                    obj[i].style.display='none';
//                                }
//                                obj[number].style.display=''
//                                if($$_IsShowExtendContent=='false')
//                                {
//                                    var objChildNodes = obj[number].childNodes;
//                                    objChildNodes[1].style.display='none';
//                                }
//                            }
//                            $$_showWeather(0);
//                        </script>";
//            js = js.Replace("$$",this.ClientID);

//            Page.ClientScript.RegisterStartupScript(this.GetType(), "WeatherWebPart_JS_" + this.ClientID, js);
//        }

//        /// <summary>
//        /// 获取sina天气预报html脚本中指定位置的html脚本
//        /// </summary>
//        /// <returns></returns>
//        private string GetWeatherStr()
//        {
//            //获取天气所在table的html代码

//            string html = GetSinaWeather();

//            //string weather = WebRequestCommon.GetSubString(html, "<!-- 天气状况 begin -->", "<!-- 天气状况 end -->");

//            return weather;

//        }

//        /// <summary>
//        /// 获得天气预报的html脚本
//        /// </summary>
//        private string GetSinaWeather()
//        {
//            NameValueCollection collection = new NameValueCollection();

//            //collection.Add("city", _SelectedCity);

//            //IWebProxy proxy = WebRequest.DefaultWebProxy;// GlobalProxySelection.Select;

//            //if( proxy != null )
//            //    proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;

//            //获取代理信息
//            System.Net.WebProxy wp;
//            if (string.IsNullOrEmpty(_ProxyUrl))
//            {
//                wp = null;
//            }
//            else
//            {
//                wp = new System.Net.WebProxy(_ProxyUrl, _ProxyPort);
//                wp.BypassProxyOnLocal = true;

//                if (String.IsNullOrEmpty(_ProxyAccount))
//                {
//                    wp.Credentials = System.Net.CredentialCache.DefaultCredentials;
//                }
//                else
//                {
//                    System.Net.ICredentials credentials = new System.Net.NetworkCredential(_ProxyAccount, _ProxyPassword, _ProxyDomain);
//                    wp.Credentials = credentials;
//                }
//            }


//            //获取天气预报页面HTML内容
//            string html = WebRequestCommon.GetRequestPageInnerHtml(strRequestUrl, "get", collection, wp);

//            return html;
        //        }
        #endregion
    }
}
