using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Repositories;
using EntityLayer.DTOs;
using EntityLayer.Entities;
using EntityLayer.Enums;
using PdfFactory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class ReceiptService : IReceiptService
    {
        public async Task<IEnumerable<Receipt>> GetAllReceiptsAsync()
        {
            using (var repo = new ReceiptRepository())
            {
                return await repo.GetAllAsync();
            }
        }

        public async Task AddNewReceiptAsync(Receipt receipt)
        {
            using (var repo = new ReceiptRepository())
            {
                receipt.ReceiptNumber = GenerateReceiptNumber();
                await repo.AddAsync(receipt);
                await GenerateReceiptPdf(receipt);
            }
        }

        public async Task<Receipt> VoidReceiptAsync(int receiptId, bool wantsGiftCardRecover = false)
        {
            using (var repo = new ReceiptRepository())
            {
                var receipt = await repo.GetByIdAsync(receiptId);
                IReservationService reservationService = new ReservationService();

                if (receipt != null)
                {
                    var voidReceipt = new Receipt
                    {
                        ReceiptNumber = "-" + receipt.ReceiptNumber,
                        TotalTreatmentAmount = -receipt.TotalTreatmentAmount,
                        RewardDiscount = receipt.RewardDiscount,
                        Reservation_idReservation = receipt.Reservation_idReservation,
                        Reservation = receipt.Reservation
                    };

                    await HandleGiftCardRecoveryAsync(receipt, voidReceipt, wantsGiftCardRecover);
                    await reservationService.ChangeReservationStatusAsync(receipt.Reservation_idReservation, ReservationStatuses.Voided);
                    await GenerateReceiptPdf(voidReceipt);
                    await repo.AddAsync(voidReceipt);

                    return voidReceipt;
                }

                return null;
            }
        }

        public async Task<IEnumerable<ReceiptDTO>> GetAllReceiptsDTOAsync()
        {
            using (var repo = new ReceiptRepository())
            {
                var receipts = await repo.GetAllAsync();
                var euroCulture = new CultureInfo("de-DE");
                euroCulture.NumberFormat.CurrencyDecimalSeparator = ".";

                var receiptsDTO = receipts.Select(r => new ReceiptDTO
                {
                    Id = r.idReceipt,
                    ReceiptNumber = r.ReceiptNumber,
                    TotalTreatmentAmount = string.Format(euroCulture, "{0:C}", r.TotalTreatmentAmount.Value),
                    GiftCardDiscount = string.Format(euroCulture, "{0:C}", r.GiftCardDiscount.Value),
                    RewardDiscount = string.Format(euroCulture, "{0:C}", r.RewardDiscount.Value),
                    TotalPrice = string.Format(euroCulture, "{0:C}", r.TotalPrice.Value),
                    idReservation = r.Reservation_idReservation,
                    ReservationDate = r.Reservation.Date.Value.ToString("dd.MM.yyyy.", euroCulture),
                    Treatments = string.Join("\n", 
                    r.Reservation.Reservation_has_Treatment.Select(
                        rt => $"{rt.Treatment.Name} " +
                        $"(Qty: {rt.Amount}, " +
                        $"Price: {string.Format(euroCulture, "{0:C}", rt.Treatment.Price)}, " +
                        $"Total: {string.Format(euroCulture, "{0:C}", rt.Amount * rt.Treatment.Price)})")),
                    Client = string.Join(" ", r.Reservation.Client.Firstname, r.Reservation.Client.Lastname),
                    Employee = string.Join(" ", r.Reservation.Employee.Firstname, r.Reservation.Employee.Lastname)
                }).ToList();

                return receiptsDTO;
            }
        }

        private async Task<Receipt> ConvertReceiptDtoToReceipt(ReceiptDTO receiptDTO)
        {
            using (var repo = new ReceiptRepository())
            {
                var receipt = await repo.GetByIdAsync(receiptDTO.Id);

                if (receipt == null)
                {
                    throw new ClientNotFoundException($"Receipt with ID {receipt.idReceipt} does not exist.");
                }

                return receipt;
            }
        }

        private async Task GenerateReceiptPdf(Receipt receipt)
        {
            IPdfFactory<Receipt> pdfFactory = new ReceiptPdf();
            var pdfBytes = await pdfFactory.GeneratePdf(receipt);
            await Task.Run(() => File.WriteAllBytes($"Receipts/{receipt.ReceiptNumber}.pdf", pdfBytes));
        }

        private async Task HandleGiftCardRecoveryAsync(Receipt receipt, Receipt voidReceipt, bool wantsGiftCardRecover)
        {
            if (wantsGiftCardRecover)
            {
                var giftCardId = await GetGiftCardIdByReceiptAsync(receipt);
                if (giftCardId != null && receipt.GiftCardDiscount > 0)
                {
                    await RecoverGiftCardAsync(giftCardId.Value, (decimal)receipt.GiftCardDiscount);
                }

                voidReceipt.GiftCardDiscount = receipt.GiftCardDiscount;
                voidReceipt.TotalPrice = -receipt.TotalPrice;
            } else
            {
                voidReceipt.GiftCardDiscount = 0;
                voidReceipt.TotalPrice = -(receipt.TotalPrice + receipt.GiftCardDiscount);
            }
        }


        private async Task<int?> GetGiftCardIdByReceiptAsync(Receipt receipt)
        {
            using (var repo = new ReceiptRepository())
            {
                return await repo.GetGiftCardIdByReceiptAsync(receipt);
            }
        }

        private async Task RecoverGiftCardAsync(int giftCardId, decimal giftCardDiscount)
        {
            IGiftCardService service = new GiftCardService();
            await service.RecoverGiftCardAsync(giftCardId, giftCardDiscount);
        }

        private string GenerateReceiptNumber()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
