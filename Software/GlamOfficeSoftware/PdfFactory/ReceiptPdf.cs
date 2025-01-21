using EntityLayer.DTOs;
using EntityLayer.Entities;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfFactory
{
    public class ReceiptPdf : PdfFactory<ReceiptDTO>
    {
        public override async Task<byte[]> GeneratePdf(ReceiptDTO receipt)
        {
            using (var memoryStream = new MemoryStream())
            {
                var document = new Document(PageSize.A4, 40, 40, 40, 40);

                var writer = PdfWriter.GetInstance(document, memoryStream);

                document.Open();

                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                var subHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                var regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
                var boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);

                var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "GlamOfficeIcon.png");
                var logo = Image.GetInstance(logoPath);
                logo.ScaleToFit(100f, 100f);
                logo.Alignment = Element.ALIGN_LEFT;
                document.Add(logo);

                document.Add(new Paragraph("Glam Office d.o.o.", headerFont) { Alignment = Element.ALIGN_LEFT });
                document.Add(new Paragraph("Julija Merlica 9", regularFont) { Alignment = Element.ALIGN_LEFT });
                document.Add(new Paragraph("Varaždin, 42000, Croatia", regularFont) { Alignment = Element.ALIGN_LEFT });
                document.Add(new Chunk("\n"));

                document.Add(new Paragraph($"Receipt Number: {receipt.ReceiptNumber}", boldFont));
                document.Add(new Paragraph($"Date: {DateTime.Now.ToString("dd.MM.yyyy.")}", regularFont));
                document.Add(new Chunk("\n"));

                document.Add(new Paragraph("Client Information:", subHeaderFont));
                document.Add(new Paragraph($"Name: {receipt.Client}", regularFont));
                document.Add(new Chunk("\n"));

                var table = new PdfPTable(4)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                table.SetWidths(new float[] { 2f, 1f, 1f, 1f });

                table.AddCell(new Phrase("Service", boldFont));
                table.AddCell(new Phrase("Quantity", boldFont));
                table.AddCell(new Phrase("Unit Price", boldFont));
                table.AddCell(new Phrase("Total Price", boldFont));

                foreach (var treatment in receipt.Treatments)
                {
                    table.AddCell(new Phrase(treatment.Name, regularFont));
                    table.AddCell(new Phrase(treatment.Quantity.ToString(), regularFont));
                    table.AddCell(new Phrase(treatment.UnitPrice, regularFont));
                    table.AddCell(new Phrase(treatment.TotalPrice, regularFont));
                }

                document.Add(table);
                document.Add(new Paragraph($"Total Treatment Amount: {receipt.TotalTreatmentAmount}", boldFont));
                document.Add(new Chunk("\n"));
                document.Add(new Paragraph($"Gift Card Discount: {receipt.GiftCardDiscount}", regularFont));
                document.Add(new Paragraph($"Reward Discount: {receipt.RewardDiscount}", regularFont));
                document.Add(new Chunk("\n"));
                document.Add(new Paragraph($"Total Price: {receipt.TotalPrice}", boldFont));

                document.Close();

                return await Task.Run(() => memoryStream.ToArray());
            }
        }
    }
}
