using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Data;

namespace CA.WorkFlow.UI.Code
{
    public class ExportPDF
    {

        public static void CreatePDF(DataTable dt1,DataTable dt2,DataTable dt3,DataTable dt4, string strTitle, string strSaveFilePath, string strPicPath,string strPicCode)
        {
            try
            {
                //Rectangle pageSize = new Rectangle(1024, 780);
                Document document = new Document();
                PdfWriter.GetInstance(document, new FileStream(strSaveFilePath, FileMode.Create));
                document.Open();
                BaseFont bfChinese = BaseFont.CreateFont(@"C:\WINDOWS\Fonts\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                //BaseFont bfChinese = CreateChineseFont();
                Font fontChinese = new Font(bfChinese, 12, Font.NORMAL, new BaseColor(0, 0, 0));

                float titleLineHeight = 45f, normalLineHeight = 25f;
                iTextSharp.text.Font titleFont = new iTextSharp.text.Font(bfChinese, 22, Font.BOLD);
                iTextSharp.text.Font normalFont = new Font(bfChinese, 12);
                Paragraph titleP = new Paragraph(strTitle, titleFont);
                titleP.Leading = titleLineHeight; titleP.Alignment = Element.ALIGN_CENTER;
                document.Add(titleP);

                Paragraph pBlank = new Paragraph(" ", normalFont);
                pBlank.Leading = normalLineHeight;
                document.Add(pBlank);
                //document.Add(new Paragraph(strTitle, fontChinese));
                
                PdfPTable table = new PdfPTable(dt1.Columns.Count);

                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    for (int j = 0; j < dt1.Columns.Count; j++)
                    {
                        table.AddCell(new Phrase(dt1.Rows[i][j].ToString(), fontChinese));
                    }
                }
                document.Add(table);
                document.Add(pBlank);
                table = new PdfPTable(dt2.Columns.Count);

                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    for (int j = 0; j < dt2.Columns.Count; j++)
                    {
                        table.AddCell(new Phrase(dt2.Rows[i][j].ToString(), fontChinese));
                    }
                }
                document.Add(table);
                document.Add(pBlank);
                table = new PdfPTable(dt3.Columns.Count);

                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    for (int j = 0; j < dt3.Columns.Count; j++)
                    {
                        table.AddCell(new Phrase(dt3.Rows[i][j].ToString(), fontChinese));
                    }
                }
                document.Add(table);
                document.Add(pBlank);
                table = new PdfPTable(dt4.Columns.Count);

                for (int i = 0; i < dt4.Rows.Count; i++)
                {
                    for (int j = 0; j < dt4.Columns.Count; j++)
                    {
                        table.AddCell(new Phrase(dt4.Rows[i][j].ToString(), fontChinese));
                    }
                }
                document.Add(table);
                document.Add(pBlank);
                //if (!string.IsNullOrEmpty(strPicCode))
                //{
                //    Paragraph PicP = new Paragraph("名片颜色:" + strPicCode, normalFont);
                //    document.Add(PicP);
                //}
                //if (!string.IsNullOrEmpty(strPicPath))
                //{
                //    iTextSharp.text.Image jpeg = iTextSharp.text.Image.GetInstance(strPicPath);
                //    document.Add(jpeg);
                //}
                document.Close();
            }
            catch (DocumentException de)
            {
                throw de;
            }
        }

        /// <summary>
        /// 创建中文字体(实现中文)
        /// </summary>
        /// <returns></returns>
        public static BaseFont CreateChineseFont()
        {
            BaseFont.AddToResourceSearch("iTextAsian.dll");
            BaseFont.AddToResourceSearch("iTextAsianCmaps.dll");
            BaseFont baseFT = BaseFont.CreateFont("STSong-Light", "UniGB-UCS2-H", BaseFont.EMBEDDED);
            return baseFT;
        }
    }
}
