using DataAccessLayer.Interfaces;
using EntityLayer.Entities;
using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class GiftCardRepository : Repository<GiftCard>, IGiftCardRepository
    {
        public async Task RecoverGiftCardAsync(GiftCard recoveredGiftCard)
        {
            var giftCardDb = await GetByIdAsync(recoveredGiftCard.idGiftCard);
            
            if (giftCardDb != null)
            {
                giftCardDb.Value = recoveredGiftCard.Value;
                giftCardDb.Status = recoveredGiftCard.Status;
                giftCardDb.ActivationDate = recoveredGiftCard.ActivationDate;
                giftCardDb.ExpirationDate = recoveredGiftCard.ExpirationDate;
                giftCardDb.RedemptionDate = recoveredGiftCard.RedemptionDate;
                giftCardDb.Description = recoveredGiftCard.Description;
                giftCardDb.PromoCode = recoveredGiftCard.PromoCode;

                await SaveChangesAsync();
            }
        }
    }
}
