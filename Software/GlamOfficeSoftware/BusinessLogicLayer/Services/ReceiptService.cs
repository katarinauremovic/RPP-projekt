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

        public async Task<Receipt> VoidReceiptAsync(int receiptId)
        {
            using(var repo = new ReceiptRepository())
            {
                var receipt = await repo.GetByIdAsync(receiptId);

                if (receipt != null)
                {
                    var voidReceipt = new Receipt
                    {
                        ReceiptNumber = receipt.ReceiptNumber,
                        TotalTreatmentAmount = -receipt.TotalTreatmentAmount,
                        GiftCardDiscount = receipt.GiftCardDiscount,
                        RewardDiscount = receipt.RewardDiscount,
                        TotalPrice = -receipt.TotalPrice,
                        Reservation_idReservation = receipt.Reservation_idReservation,
                        Reservation = receipt.Reservation
                    };


                    //RecoverGiftCard(int giftCardId);
                    //RecoverReward(int rewardId);
                    return voidReceipt;
                }

                return null;
            }
        }

        private string GenerateReceiptNumber()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
