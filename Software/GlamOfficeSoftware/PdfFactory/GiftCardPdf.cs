using EntityLayer.DTOs;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer.Entities;
using System.Globalization;

namespace PdfFactory
{
    public class GiftCardPdf : PdfFactory<GiftCard>
    {
        public override async Task<byte[]> GeneratePdf(GiftCard giftCard)
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

                var culture = new CultureInfo("de-DE");

                var logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "GlamOfficeIcon.png");
                if (File.Exists(logoPath))
                {
                    var logo = Image.GetInstance(logoPath);
                    logo.ScaleToFit(100f, 100f);
                    logo.Alignment = Element.ALIGN_LEFT;
                    document.Add(logo);
                }

                
                document.Add(new Paragraph("Glam Office d.o.o.", headerFont) { Alignment = Element.ALIGN_LEFT });
                document.Add(new Paragraph("Julija Merlića 9", regularFont) { Alignment = Element.ALIGN_LEFT });
                document.Add(new Paragraph("Varaždin, 42000, Croatia", regularFont) { Alignment = Element.ALIGN_LEFT });
                document.Add(new Chunk("\n"));

                document.Add(new Paragraph("Gift Card Information", subHeaderFont) { Alignment = Element.ALIGN_CENTER });
                document.Add(new Chunk("\n"));

                
                var table = new PdfPTable(2)
                {
                    WidthPercentage = 100,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                table.SetWidths(new float[] { 2f, 3f });

                AddTableRow(table, "Value:", giftCard.Value?.ToString("C", culture) ?? "N/A", boldFont, regularFont);
                AddTableRow(table, "Amount to Spend:", giftCard.ToSpend?.ToString("C", culture) ?? "N/A", boldFont, regularFont);
                AddTableRow(table, "Status:", giftCard.Status ?? "N/A", boldFont, regularFont);
                AddTableRow(table, "Activation Date:", giftCard.ActivationDate?.ToString("dd.MM.yyyy") ?? "N/A", boldFont, regularFont);
                AddTableRow(table, "Expiration Date:", giftCard.ExpirationDate?.ToString("dd.MM.yyyy") ?? "N/A", boldFont, regularFont);
                AddTableRow(table, "Redemption Date:", giftCard.RedemptionDate?.ToString("dd.MM.yyyy") ?? "Not Redeemed", boldFont, regularFont);
                AddTableRow(table, "Promo Code:", giftCard.PromoCode ?? "N/A", boldFont, regularFont);
                AddTableRow(table, "Description:", giftCard.Description ?? "N/A", boldFont, regularFont);

                document.Add(table);
                document.Close();

                return await Task.Run(() => memoryStream.ToArray());
            }
        }

        private void AddTableRow(PdfPTable table, string label, string value, Font labelFont, Font valueFont)
        {
            table.AddCell(new PdfPCell(new Phrase(label, labelFont)) { Border = 0, Padding = 5 });
            table.AddCell(new PdfPCell(new Phrase(value, valueFont)) { Border = 0, Padding = 5 });
        }

        public override Task<string> GenerateStr(GiftCard data)
        {
            var stringBuilder = new StringBuilder();
            var culture = new CultureInfo("de-DE");


            stringBuilder.AppendLine("                   Glam Office d.o.o.        ");
            stringBuilder.AppendLine("                     Julija Merlića 9        ");
            stringBuilder.AppendLine("            42000, Varaždin, Hrvatska        ");
            stringBuilder.AppendLine("==========================");
            stringBuilder.AppendLine("                    GIFT CARD       ");
            stringBuilder.AppendLine("==========================");
            stringBuilder.AppendLine($"Value: {data.Value?.ToString("C", culture) ?? "N/A"}");
            stringBuilder.AppendLine($"Amount to Spend: {data.ToSpend?.ToString("C", culture) ?? "N/A"}");
            stringBuilder.AppendLine($"Status: {data.Status ?? "N/A"}");
            stringBuilder.AppendLine($"Activation Date: {data.ActivationDate?.ToString("dd.MM.yyyy") ?? "N/A"}");
            stringBuilder.AppendLine($"Expiration Date: {data.ExpirationDate?.ToString("dd.MM.yyyy") ?? "N/A"}");
            stringBuilder.AppendLine($"Redemption Date: {data.RedemptionDate?.ToString("dd.MM.yyyy") ?? "Not Redeemed"}");
            stringBuilder.AppendLine($"Promo Code: {data.PromoCode ?? "N/A"}");
            stringBuilder.AppendLine($"Description: {data.Description ?? "N/A"}");
            stringBuilder.AppendLine("==========================");
            stringBuilder.AppendLine("                        THANK YOU!        ");
            stringBuilder.AppendLine("==========================");

            return Task.FromResult(stringBuilder.ToString());
        }
    }
}
