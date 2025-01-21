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

                var receiptDTO = ConvertReceiptToReceiptDto(receipt);
                await GenerateReceiptPdf(receiptDTO);
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
                    var voidReceiptDTO = ConvertReceiptToReceiptDto(voidReceipt);
                    await GenerateReceiptPdf(voidReceiptDTO);
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
                
                var receiptsDTO = receipts.Select(receipt => ConvertReceiptToReceiptDto(receipt)).ToList();

                return receiptsDTO;
            }
        }

        private ReceiptDTO ConvertReceiptToReceiptDto(Receipt receipt)
        {
            var euroCulture = new CultureInfo("de-DE");
            euroCulture.NumberFormat.CurrencyDecimalSeparator = ".";

            var receiptDTO = new ReceiptDTO
            {
                Id = receipt.idReceipt,
                ReceiptNumber = receipt.ReceiptNumber,
                TotalTreatmentAmount = string.Format(euroCulture, "{0:C}", receipt.TotalTreatmentAmount.Value),
                GiftCardDiscount = string.Format(euroCulture, "{0:C}", receipt.GiftCardDiscount.Value),
                RewardDiscount = string.Format(euroCulture, "{0:C}", receipt.RewardDiscount.Value),
                TotalPrice = string.Format(euroCulture, "{0:C}", receipt.TotalPrice.Value),
                idReservation = receipt.Reservation_idReservation,
                ReservationDate = receipt.Reservation.Date.Value.ToString("dd.MM.yyyy.", euroCulture),
                Treatments = string.Join("\n",
                    receipt.Reservation.Reservation_has_Treatment.Select(
                        rt => $"{rt.Treatment.Name} " +
                              $"(Qty: {rt.Amount}, " +
                              $"Price: {string.Format(euroCulture, "{0:C}", rt.Treatment.Price)}, " +
                              $"Total: {string.Format(euroCulture, "{0:C}", rt.Amount * rt.Treatment.Price)})")),
                Client = string.Join(" ", receipt.Reservation.Client.Firstname, receipt.Reservation.Client.Lastname),
                Employee = string.Join(" ", receipt.Reservation.Employee.Firstname, receipt.Reservation.Employee.Lastname)
            };

            return receiptDTO;
        }

        public async Task<string> LoadReceiptInReceiptFormat(ReceiptDTO receiptDTO)
        {
            IPdfFactory<ReceiptDTO> pdfFactory = new ReceiptPdf();
            var pdfBytes = await pdfFactory.GeneratePdf(receiptDTO);
            string base64Pdf = Convert.ToBase64String(pdfBytes);

            return base64Pdf;
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

        private async Task GenerateReceiptPdf(ReceiptDTO receiptDto)
        {
            IPdfFactory<ReceiptDTO> pdfFactory = new ReceiptPdf();
            var pdfBytes = await pdfFactory.GeneratePdf(receiptDto);
            await Task.Run(() => File.WriteAllBytes($"Receipts/{receiptDto.ReceiptNumber}.pdf", pdfBytes));
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
