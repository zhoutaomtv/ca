
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;
using System.Threading;
using System.IO;
using Microsoft.SharePoint;

namespace CA.SharePoint.Utilities.Common
{
    public class Util
    {
        public static string GetDataOrEmpty(object obj)
        {
            return obj == null ? string.Empty : obj.ToString();
        }

        /// <summary> 
        /// word 转成html 
        /// </summary> 
        /// <param name="path">实际目录</param> 
        /// <param name="fileName">文件名</param>
        /// <param name="path">文件相对路径</param> 
        public static string WordToHtml(string path, string fileName, string fileRelaUrl)
        {
            string wordFileUrl = path + fileName;  //文件实际路径
            string htmlFileUrl = fileEndWithHtml(wordFileUrl.ToString());  //将要生成的html文件实际路径

            //if (!File.Exists(htmlFileUrl))
            //{
            try
            {

                SPSecurity.RunWithElevatedPrivileges(delegate()
            {
              //在此处放置用户代码以初始化页面 
              ApplicationClass word = new ApplicationClass();
              Type wordType = word.GetType();
              Documents docs = word.Documents;

              //打开文件 
              Type docsType = docs.GetType();
              Document doc = (Document)docsType.InvokeMember("Open",
              System.Reflection.BindingFlags.InvokeMethod, null, docs, new Object[] { wordFileUrl, true, true });

              //转换格式，另存为 
              Type docType = doc.GetType();

              docType.InvokeMember("SaveAs", System.Reflection.BindingFlags.InvokeMethod,
               null, doc, new object[] { htmlFileUrl, WdSaveFormat.wdFormatFilteredHTML });

              docType.InvokeMember("Close", System.Reflection.BindingFlags.InvokeMethod,
               null, doc, null);

              //退出 Word 
              wordType.InvokeMember("Quit", System.Reflection.BindingFlags.InvokeMethod,
               null, word, null);

              //Thread.Sleep(3000); //为了使退出完全，这里阻塞3秒
            });
            }

            catch (Exception ex)
            {
                throw ex;
            }

            //}

            return fileEndWithHtml(fileRelaUrl);    //html文件相对路径

        }

        public static string fileEndWithHtml(string fileName)
        {
            if (fileName.EndsWith(".doc"))
            {
                return fileName.Substring(0, fileName.Length - 3) + "html";
            }
            else if (fileName.EndsWith(".docx"))
            {
                return fileName.Substring(0, fileName.Length - 4) + "html";
            }
            return fileName;
        }
    }
}
