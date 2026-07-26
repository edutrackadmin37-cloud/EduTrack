using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace EduTrack.Helpers
{
    public static class ExportHelper
    {
        // Note: In production, install these NuGet packages:
        // Install-Package iTextSharp
        // Install-Package EPPlus

        public static byte[] ExportToPdf(string htmlContent)
        {
            // Placeholder - in production use iTextSharp or iText7
            // iTextSharp.text.Document doc = new iTextSharp.text.Document();
            // using (var stream = new MemoryStream())
            // {
            //     iTextSharp.text.pdf.PdfWriter.GetInstance(doc, stream);
            //     doc.Open();
            //     doc.Add(new iTextSharp.text.Paragraph(htmlContent));
            //     doc.Close();
            //     return stream.ToArray();
            // }
            return Encoding.UTF8.GetBytes(htmlContent);
        }

        public static byte[] ExportToExcel(string htmlContent)
        {
            // Placeholder - in production use EPPlus
            // using (var pck = new OfficeOpenXml.ExcelPackage())
            // {
            //     var ws = pck.Workbook.Worksheets.Add("Report");
            //     // Parse HTML and populate cells
            //     return pck.GetAsByteArray();
            // }
            return Encoding.UTF8.GetBytes(htmlContent);
        }

        public static byte[] ExportToWord(string htmlContent)
        {
            // For Word, we can return HTML with Word MIME type
            // Or use DocX library
            return Encoding.UTF8.GetBytes(htmlContent);
        }

        public static byte[] ExportDataTableToExcel(System.Data.DataTable dt)
        {
            // Using EPPlus:
            // using (var pck = new OfficeOpenXml.ExcelPackage())
            // {
            //     var ws = pck.Workbook.Worksheets.Add("Report");
            //     ws.Cells["A1"].LoadFromDataTable(dt, true);
            //     return pck.GetAsByteArray();
            // }
            return new byte[0];
        }
    }
}