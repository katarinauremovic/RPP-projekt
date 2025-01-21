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
using System.Diagnostics;
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

        public async Task<string> GenerateReceiptInStringFormat(ReceiptDTO receiptDTO)
        {
            IPdfFactory<ReceiptDTO> pdfFactory = new ReceiptPdf();
            var receipt = await pdfFactory.GenerateStr(receiptDTO);
            return receipt;
        }

        public async Task GenerateReceiptPdf(ReceiptDTO receiptDTO)
        {
            IPdfFactory<ReceiptDTO> pdfFactory = new ReceiptPdf();
            var pdfBytes = await pdfFactory.GeneratePdf(receiptDTO);
            await OpenReceiptPdfAsync(receiptDTO, pdfBytes);
        }

        private ReceiptDTO ConvertReceiptToReceiptDto(Receipt receipt)
        {
            var euroCulture = CreateEuroCulture();

            return new ReceiptDTO
            {
                Id = receipt.idReceipt,
                ReceiptNumber = receipt.ReceiptNumber,
                TotalTreatmentAmount = FormatCurrency(receipt.TotalTreatmentAmount.Value, euroCulture),
                GiftCardDiscount = FormatCurrency(receipt.GiftCardDiscount.Value, euroCulture),
                RewardDiscount = FormatCurrency(receipt.RewardDiscount.Value, euroCulture),
                TotalPrice = FormatCurrency(receipt.TotalPrice.Value, euroCulture),
                ReceiptIssueDateTime = receipt.IssueDateTime,
                idReservation = receipt.Reservation_idReservation,
                ReservationDate = FormatDate(receipt.Reservation.Date.Value, euroCulture),
                Treatments = ConvertTreatments(receipt.Reservation.Reservation_has_Treatment, euroCulture),
                strTreatments = GenerateTreatmentsString(receipt.Reservation.Reservation_has_Treatment, euroCulture),
                Client = ConvertClient(receipt.Reservation.Client),
                strClient = FormatFullName(receipt.Reservation.Client.Firstname, receipt.Reservation.Client.Lastname),
                Employee = FormatFullName(receipt.Reservation.Employee.Firstname, receipt.Reservation.Employee.Lastname)
            };
        }

        private ClientReceiptDTO ConvertClient(Client client)
        {
            return new ClientReceiptDTO
            {
                Firstname = client.Firstname,
                Lastname = client.Lastname,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber
            };
        }

        private CultureInfo CreateEuroCulture()
        {
            var euroCulture = new CultureInfo("de-DE");
            euroCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            return euroCulture;
        }

        private string FormatCurrency(decimal value, CultureInfo culture)
        {
            return string.Format(culture, "{0:C}", value);
        }

        private string FormatDate(DateTime date, CultureInfo culture)
        {
            return date.ToString("dd.MM.yyyy.", culture);
        }

        private List<TreatmentReceiptDTO> ConvertTreatments(IEnumerable<Reservation_has_Treatment> treatments, CultureInfo culture)
        {
            return treatments.Select(rt => new TreatmentReceiptDTO
            {
                Name = rt.Treatment.Name,
                Quantity = rt.Amount,
                UnitPrice = FormatCurrency((decimal)rt.Treatment.Price, culture),
                TotalPrice = FormatCurrency((decimal)(rt.Amount * rt.Treatment.Price), culture)
            }).ToList();
        }

        private string GenerateTreatmentsString(IEnumerable<Reservation_has_Treatment> treatments, CultureInfo culture)
        {
            return string.Join("\n", treatments.Select(rt =>
                $"{rt.Treatment.Name} " +
                $"(Qty: {rt.Amount}, " +
                $"Price: {FormatCurrency((decimal)rt.Treatment.Price, culture)}, " +
                $"Total: {FormatCurrency((decimal)(rt.Amount * rt.Treatment.Price), culture)})"));
        }

        private string FormatFullName(string firstname, string lastname)
        {
            return string.Join(" ", firstname, lastname);
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

        private async Task OpenReceiptPdfAsync(ReceiptDTO receiptDTO, byte[] pdfBytes)
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), $"{receiptDTO.ReceiptNumber}.pdf");

            await Task.Run(() => File.WriteAllBytes(tempFilePath, pdfBytes));
            await Task.Run(() => Process.Start(new ProcessStartInfo(tempFilePath) { UseShellExecute = true }));
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
