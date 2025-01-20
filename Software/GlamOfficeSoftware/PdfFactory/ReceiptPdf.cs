using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PdfFactory
{
    public class ReceiptPdf : PdfFactory<Receipt>
    {
        public override async Task<byte[]> GeneratePdf(Receipt receipt)
        {
            string reservationDetails = receipt.Reservation != null
                ? $@"
                    Reservation Details:
                    Client Name: $'{receipt.Reservation.Client.Firstname} {receipt.Reservation.Client.Lastname}'
                    Reservation Date: {receipt.Reservation.Date}
                    Services: {string.Join(", ", receipt.Reservation.Reservation_has_Treatment.Select(rt => rt.Treatment.Name))}
                    "
                : "No reservation details available.";

            string content = $@"
                Receipt Number: {receipt.ReceiptNumber}
                ----------------------------------------
                Total Treatment Amount: {receipt.TotalTreatmentAmount:C}
                Gift Card Discount: {receipt.GiftCardDiscount:C}
                Reward Discount: {receipt.RewardDiscount:C}
                ----------------------------------------
                Total Price: {receipt.TotalPrice:C}
                ----------------------------------------
                {reservationDetails}
                ";

            return await ConvertToPdf(content);
        }
    }
}
