using EntityLayer.DTOs;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfFactory
{
    public class ReceiptPdf : PdfFactory<ReceiptDTO>
    {
        public override async Task<byte[]> GeneratePdf(ReceiptDTO receipt)
        {
            string content = $@"
            Receipt Number: {receipt.ReceiptNumber}
            ----------------------------------------
            Total Treatment Amount: {receipt.TotalTreatmentAmount}
            Gift Card Discount: {receipt.GiftCardDiscount}
            Reward Discount: {receipt.RewardDiscount}
            ----------------------------------------
            Total Price: {receipt.TotalPrice}
            ----------------------------------------
            Reservation Details:
            Client Name: {receipt.Client}
            Reservation Date: {receipt.ReservationDate}
            Services: {receipt.Treatments}
            Employee: {receipt.Employee}
        ";

            return await ConvertToPdf(content);
        }
    }
}
