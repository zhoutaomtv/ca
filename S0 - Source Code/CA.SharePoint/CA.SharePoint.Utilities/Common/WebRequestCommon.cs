using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Web;

namespace CA.SharePoint
{
    public class WebRequestCommon
   {
       public WebRequestCommon()
       { }

       /// <summary>
       /// 按指定页面发送方式和参数值对获得指定页面的Html代码
       /// </summary>
       /// <param name="url"></param>
       /// <param name="mothod"></param>
       /// <param name="paramList"></param>
       /// <returns></returns>
       public static string GetRequestPageInnerHtml(string url, string mothod, NameValueCollection paramList, WebProxy wp)
       {
           if (url == null || url == "")
               return "";

           WebRequest request = null;
           string strParam = "";

           try
           {
               if (mothod.ToLower() == "post")	//post发送方式
               {
                   request = WebRequest.Create(url);
                   request.Timeout = 10000;
                   foreach (string key in paramList.Keys)
                   {
                       strParam += key + "=" + paramList.Get(key) + "&";
                   }
                   if (strParam.Length > 0)
                   {
                       strParam = strParam.Substring(0, strParam.Length - 1);
                   }
                   byte[] postData = System.Text.Encoding.Default.GetBytes(strParam);
                   request.ContentLength = postData.Length;
                   Stream postStream = request.GetRequestStream();
                   postStream.Write(postData, 0, postData.Length);
                   postStream.Close();
                    
                   WebResponse res = request.GetResponse(); 

                   StreamReader reader = new StreamReader(res.GetResponseStream(), System.Text.Encoding.Default);

                   string html = reader.ReadToEnd();

                   reader.Close();
                   res.Close();

                   return html;

               }
               else				//get发送方式
               {
                   if (paramList != null && paramList.Count > 0)
                   {

                       foreach (string key in paramList.Keys)
                       {
                           strParam += key + "=" + HttpUtility.UrlEncode(paramList.Get(key), Encoding.GetEncoding("utf-8")) + "&";
                       }

                       if (strParam.Length > 0)
                       {
                           strParam = strParam.Substring(0, strParam.Length - 1);
                           strParam = "?" + strParam;
                       }

                   }

                   strParam = url + strParam;
                   request = WebRequest.Create(strParam);

                   //读取HTML内容
                   if(wp!=null)
                    request.Proxy = wp;
                   WebResponse response = request.GetResponse();

                   StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));

                   string html = reader.ReadToEnd();

                   reader.Close();
                   response.Close();

                   return html;
               }
           }
           catch (Exception e)
           {
               throw e;
               //return e.Message;
           }

       }

       /// <summary>
       /// 按指定页面和参数值对获得指定页面的XML文档
       /// </summary>
       /// <param name="url">页面URL</param>
       /// <param name="paramList">参数值</param>
       /// <param name="wp">代理信息</param>
       /// <returns>XmlDocument</returns>
       public static XmlDocument GetRequestPageInnerXML(string url, NameValueCollection paramList, WebProxy wp)
       {
           XmlDocument xmlDoc = null;
           if (url == null || url == "")
               return xmlDoc;

           HttpWebRequest request = null;
           string strParam = "";
           try
           {
               if (paramList != null && paramList.Count > 0)
               {
                   foreach (string key in paramList.Keys)
                   {
                       strParam += key + "=" + HttpUtility.UrlEncode(paramList.Get(key), Encoding.GetEncoding("utf-8")) + "&";
                   }
                   if (strParam.Length > 0)
                   {
                       strParam = strParam.Substring(0, strParam.Length - 1);
                       strParam = "?" + strParam;
                   }
                   strParam = url + strParam;

                   request = (HttpWebRequest)WebRequest.Create(strParam);
                   request.UserAgent = @"Mozilla/5.0 (Windows; U; Windows NT 6.0; en-US; rv:1.8.1.4) Gecko/20070515 Firefox/2.0.0.4";
                   if (wp != null)
                       request.Proxy = wp;
                   HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                   xmlDoc = new XmlDocument();
                   xmlDoc.Load(response.GetResponseStream());

                   response.Close();
               }
           }
           catch (Exception e)
           {
               throw e;
           }
           return xmlDoc;
       }

       /// <summary>
       /// 获得指定起始和结束字符串之间的字符串
       /// </summary>
       /// <param name="html"></param>
       /// <param name="beginStr"></param>
       /// <param name="endStr"></param>
       /// <returns></returns>
       public static string GetSubString(string html, string beginStr, string endStr)
       {
           int iBegin = GetIndexBySubString(html, beginStr);
           int iEnd = GetIndexBySubString(html, endStr);
           if (iBegin == -1 || iEnd == -1)
               return "";
           return GetSubString(html, iBegin, iEnd);
       }

       /// <summary>
       /// 获得指定字符串的指定位置之间的字符串
       /// </summary>
       /// <param name="html"></param>
       /// <param name="iBegin"></param>
       /// <param name="iEnd"></param>
       /// <returns></returns>
       public static string GetSubString(string html, int iBegin, int iEnd)
       {
           if (html == "")
               return "";
           if (iBegin == -1 || iEnd == -1)
               return "";
           return html.Substring(iBegin, iEnd - iBegin);
       }

       /// <summary>
       /// 获得指定html代码的第iDivIndex个div之间字符串
       /// </summary>
       /// <param name="html"></param>
       /// <param name="iDivIndex"></param>
       /// <returns></returns>
       public static string GetSubString(string html, int iDivIndex)
       {
           if (html == "" || iDivIndex < 1)
               return "";

           string strTemp = html.ToLower(),
               strBeginDiv = "<div",
               strEndDiv = "</div>";

           int iBegin = -1, iEnd = -1, iStartIndex;

           for (int i = 1; i <= iDivIndex; i++)
           {
               if (i == 1)
               {
                   iEnd = 0;
                   iStartIndex = 0;
               }
               else
               {
                   iStartIndex = iBegin + strBeginDiv.Length;
                   iEnd = iEnd + strEndDiv.Length;
               }
               iBegin = strTemp.IndexOf(strBeginDiv, iStartIndex);
               iEnd = strTemp.IndexOf(strEndDiv, iEnd);

               if (iBegin == -1 || iEnd == -1)
                   return "";

           }
           strTemp = html.Substring(iBegin - 1, iEnd - iBegin);

           return strTemp + strEndDiv;

       }

       /// <summary>
       /// 获得指定字符在指定字符串的匹配位数
       /// </summary>
       /// <param name="html"></param>
       /// <param name="subStr"></param>
       /// <returns></returns>
       public static int GetIndexBySubString(string html, string subStr)
       {
           return html.IndexOf(subStr);
       }

       /// <summary>
       /// 将所有匹配项替换为其他指定的 Unicode 字符或 String
       /// </summary>
       /// <param name="html"></param>
       /// <param name="sourceString"></param>
       /// <returns></returns>
       public static string ReplaceString(string html, string sourceString, string targetString)
       {
           return html.Replace(sourceString, targetString);
       }
   }

}
