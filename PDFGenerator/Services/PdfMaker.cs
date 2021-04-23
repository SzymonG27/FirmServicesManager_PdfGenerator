using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Layout.Element;

namespace PDFGenerator.Services
{
    public class PdfMaker
    {
        public static void CreatePdf(string name, string barcode)
        {
            var file = File.Create("PDFs/" + name + ".pdf");
            PdfWriter writer = new PdfWriter(file);
            ConverterProperties converterProperties = new ConverterProperties();
            PdfDocument pdf = new PdfDocument(writer);
            //string src = "Views/Client/PdfTemplate.cshtml";
            pdf.SetDefaultPageSize(new PageSize(PageSize.A4));
            //Document doc = HtmlConverter.ConvertToDocument(htmlCode, pdf, converterProperties);
            Document doc = new Document(pdf);
            HtmlConverter.ConvertToPdf("Views/Client/PdfTemplate.xhtml", writer);
            iText.Barcodes.Barcode128 bar = new iText.Barcodes.Barcode128(pdf);
            bar.SetCode(barcode);
            var barcodeImg = new Image(bar.CreateFormXObject(pdf));
            doc.Add(barcodeImg);
            //doc.Close();
        }
    }
}
