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
using PDFGenerator.Models.ViewModels;
using iText.Layout.Properties;
using iText.Kernel.Pdf.Canvas.Draw;
using PDFGenerator.Models.ClientModels;
using iText.Kernel.Font;
using iText.IO.Font;
using iText.IO.Font.Constants;

namespace PDFGenerator.Services
{
    public class PdfMaker
    {
        public static void CreatePdf(string name, PdfMakerModel model)
        {
            var file = File.Create("PDFs/" + name + ".pdf");
            PdfWriter writer = new PdfWriter(file);
            ConverterProperties converterProperties = new ConverterProperties();
            PdfDocument pdf = new PdfDocument(writer);
            pdf.SetDefaultPageSize(new PageSize(PageSize.A4));
            pdf.AddNewPage();
            Document doc = new Document(pdf);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA, PdfEncodings.CP1257);
            Rectangle pageSize = pdf.GetDefaultPageSize();

            Paragraph header = new Paragraph("Zlecenie nr." + (string)name + " dla zleceniobiorcy")
                .SetFont(font)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetFontSize(18)
                .SetBold()
                .SetMarginTop(3f);
            doc.Add(header);

            SolidLine line = new SolidLine(1f);
            LineSeparator lineSeparator = new LineSeparator(line)
                .SetWidth(pageSize.GetWidth()/2)
                .SetMarginTop(8);
            doc.Add(lineSeparator);

            Paragraph employer = new Paragraph("Przyjmujący: " + model.AppUser.FirstName + " " + model.AppUser.SurName)
                .SetFont(font)
                .SetFontSize(12)
                .SetMultipliedLeading(1f);
            Paragraph client = new Paragraph("Imię i nazwisko: " + model.Client.FirstName + " " + model.Client.SurName)
                .SetFont(font)
                .SetFontSize(12)
                .SetMultipliedLeading(1f);
            Paragraph clientPhone = new Paragraph("Numer telefonu: " + model.Client.PhoneNumber)
                .SetFont(font)
                .SetFontSize(12)
                .SetMultipliedLeading(1f);
            Paragraph itemToFix = new Paragraph("Do naprawy: " + model.Fix.ItemToFix + " Nr." + model.Fix.ID)
                .SetFont(font)
                .SetFontSize(12)
                .SetMultipliedLeading(1f);
            doc.Add(employer);
            doc.Add(client);
            doc.Add(clientPhone);
            doc.Add(itemToFix);

            int accesoriesCounter = model.Accesories.Count();
            for (var i = 0; i < accesoriesCounter; i++)
            {
                Paragraph accesoryParagraph = new Paragraph("Akcesorie nr." + model.Accesories.ElementAt(i).ID + " Nazwa: " +
                    model.Accesories.ElementAt(i).NameOfAccesory)
                    .SetFont(font)
                    .SetFontSize(12)
                    .SetMultipliedLeading(1f);
                doc.Add(accesoryParagraph);
            }

            int fixesCounter = model.FixNames.Count();
            for (var i = 0; i < fixesCounter; i++)
            {
                Paragraph fixParagraph = new Paragraph("Do naprawy: " + model.FixNames.ElementAt(i))
                    .SetFont(font)
                    .SetFontSize(12)
                    .SetMultipliedLeading(1f);
                doc.Add(fixParagraph);
            }

            DashedLine line2 = new DashedLine(3f);
            LineSeparator lineSeparator2 = new LineSeparator(line2)
                .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                .SetWidth(pageSize.GetWidth() / 2 + 15f)
                .SetMarginTop(8);
            doc.Add(lineSeparator2);

            Paragraph header2 = new Paragraph("Zlecenie nr." + (string)name + " dla zleceniodawcy")
                .SetFont(font)
                .SetFontSize(18)
                .SetBold()
                .SetTextAlignment(TextAlignment.CENTER)
                .SetMarginTop(3f);
            doc.Add(header2);
            doc.Add(lineSeparator);
            doc.Add(employer);
            doc.Add(client);
            doc.Add(clientPhone);
            doc.Add(itemToFix);
            for (var i = 0; i < accesoriesCounter; i++)
            {
                Paragraph accesoryParagraph = new Paragraph("Akcesorie nr." + model.Accesories.ElementAt(i).ID + " Nazwa: " +
                    model.Accesories.ElementAt(i).NameOfAccesory)
                    .SetFont(font)
                    .SetFontSize(12)
                    .SetMultipliedLeading(1f);
                doc.Add(accesoryParagraph);
            }
            for (var i = 0; i < fixesCounter; i++)
            {
                Paragraph fixParagraph = new Paragraph("Do naprawy: " + model.FixNames.ElementAt(i))
                    .SetFont(font)
                    .SetFontSize(12)
                    .SetMultipliedLeading(1f);
                doc.Add(fixParagraph);
            }

            iText.Barcodes.Barcode128 bar = new iText.Barcodes.Barcode128(pdf);
            bar.SetCode(model.Fix.Barcode);
            var barcodeImg = new Image(bar.CreateFormXObject(pdf)).SetMarginTop(2.5f);
            doc.Add(barcodeImg);

            doc.Close();
        }
    }
}
