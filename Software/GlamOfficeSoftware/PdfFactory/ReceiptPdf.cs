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
                document.Add(new Paragraph($"Receipt Issuance: {receipt.ReceiptIssueDateTime.ToString("dd.MM.yyyy. HH:mm:ss")}", regularFont));
                document.Add(new Chunk("\n"));

                document.Add(new Paragraph("Client Information:", subHeaderFont));
                document.Add(new Paragraph($"Name: {receipt.strClient}", regularFont));
                document.Add(new Paragraph($"E-mail: {receipt.Client.Email}", regularFont));
                document.Add(new Paragraph($"Phone Number: {receipt.Client.PhoneNumber}", regularFont));
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

        public override Task<string> GenerateStr(ReceiptDTO data)
        {
            var stringBuilder = new StringBuilder();

            // Info
            stringBuilder.AppendLine("                   Glam Office d.o.o.        ");
            stringBuilder.AppendLine("                     Julija Merlića 9        ");
            stringBuilder.AppendLine("            42000, Varaždin, Hrvatska        ");

            // Header
            stringBuilder.AppendLine("==========================");
            stringBuilder.AppendLine("                          Receipt        ");
            stringBuilder.AppendLine("==========================");
            stringBuilder.AppendLine($"Receipt No: {data.ReceiptNumber}");
            stringBuilder.AppendLine($"Date: {data.ReceiptIssueDateTime.ToString("dd.MM.yyyy. HH:mm")}");
            stringBuilder.AppendLine("---------------------------------------------");

            // Client Information
            stringBuilder.AppendLine("Client Info:");
            stringBuilder.AppendLine($"Name: {data.strClient}");
            stringBuilder.AppendLine($"Email: {data.Client.Email}");
            stringBuilder.AppendLine($"Phone: {data.Client.PhoneNumber}");
            stringBuilder.AppendLine("---------------------------------------------");

            // Reservation Information
            stringBuilder.AppendLine("Reservation:");
            stringBuilder.AppendLine($"Date: {data.ReservationDate}");
            stringBuilder.AppendLine("---------------------------------------------");

            // Treatments
            stringBuilder.AppendLine("Treatments:");
            stringBuilder.AppendLine("---------------------------------------------");

            // Column headers with left alignment
            stringBuilder.AppendLine(string.Format("{0,-20} {1,2} {2,5} {3,8}", "Treatment", "Qty", "Unit Price", "Total"));

            // Loop through each treatment and fill columns
            foreach (var treatment in data.Treatments)
            {
                // Limiting column lengths
                string treatmentName = LimitStringLength(treatment.Name, 20);
                string quantity = LimitStringLength(treatment.Quantity.ToString(), 5);
                string unitPrice = LimitStringLength(treatment.UnitPrice, 10);
                string totalPrice = LimitStringLength(treatment.TotalPrice, 10);

                stringBuilder.AppendLine(string.Format("{0,-20} {1,5} {2,10} {3,10}", treatmentName, quantity, unitPrice, totalPrice));
            }

            stringBuilder.AppendLine("---------------------------------------------");

            // Pricing Information
            stringBuilder.AppendLine("Pricing:");
            stringBuilder.AppendLine($"Total: {data.TotalTreatmentAmount}");
            stringBuilder.AppendLine($"Gift Card: {data.GiftCardDiscount}");
            stringBuilder.AppendLine($"Reward: {data.RewardDiscount}");
            stringBuilder.AppendLine($"Total Price: {data.TotalPrice}");
            stringBuilder.AppendLine("---------------------------------------------");

            // Employee Information
            stringBuilder.AppendLine("Employee:");
            stringBuilder.AppendLine($"{data.Employee}");
            stringBuilder.AppendLine("==========================");
            stringBuilder.AppendLine("                        THANK YOU!        ");
            stringBuilder.AppendLine("==========================");

            // Return the result as a string
            return Task.FromResult(stringBuilder.ToString());
        }

        private string LimitStringLength(string value, int maxLength)
        {
            if (value.Length > maxLength)
            {
                return value.Substring(0, maxLength);
            }
            return value.PadRight(maxLength);
        }
    }
}
