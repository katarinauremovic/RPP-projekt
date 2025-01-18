using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class GiftCardService : IGiftCardService
    {
        public async Task RecoverGiftCardAsync(int giftCardId, decimal giftCardDiscount)
        {
            using (var repo = new GiftCardRepository())
            {
                var giftCard = await repo.GetByIdAsync(giftCardId);

                if (giftCard != null)
                {
                    var recoveredGiftCard = new GiftCard
                    {
                        Value = giftCardDiscount,
                        Status = CheckGiftCardStatus(giftCard.ActivationDate, giftCard.ExpirationDate, giftCard.RedemptionDate).ToString(),
                        ActivationDate = giftCard.ActivationDate,
                        ExpirationDate = giftCard.ExpirationDate,
                        RedemptionDate = null,
                        Description = giftCard.Description,
                        PromoCode = giftCard.PromoCode
                    };

                    await repo.RecoverGiftCardAsync(recoveredGiftCard);
                }
            }
        }

        private GiftCardStatuses CheckGiftCardStatus(DateTime? activationDate, DateTime? expirationDate, DateTime? redemptionDate)
        {
            if (redemptionDate.HasValue && expirationDate.HasValue && DateTime.Now > expirationDate.Value)
            {
                return GiftCardStatuses.Expired;
            }

            return GiftCardStatuses.Active;
        }
    }
}
