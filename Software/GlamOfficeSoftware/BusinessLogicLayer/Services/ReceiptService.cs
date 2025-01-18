using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            }
        }

        public async Task<Receipt> VoidReceiptAsync(int receiptId, bool wantsGiftCardRecover = false)
        {
            using (var repo = new ReceiptRepository())
            {
                var receipt = await repo.GetByIdAsync(receiptId);

                if (receipt != null)
                {
                    var voidReceipt = new Receipt
                    {
                        ReceiptNumber = receipt.ReceiptNumber,
                        TotalTreatmentAmount = -receipt.TotalTreatmentAmount,
                        RewardDiscount = receipt.RewardDiscount,
                        Reservation_idReservation = receipt.Reservation_idReservation,
                        Reservation = receipt.Reservation
                    };

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

                    await repo.AddAsync(voidReceipt);
                    
                    return voidReceipt;                   
                }

                return null;
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
