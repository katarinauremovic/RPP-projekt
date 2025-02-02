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
                    giftCard.Status = CheckGiftCardStatus(giftCard.ActivationDate, giftCard.ExpirationDate, giftCard.RedemptionDate).ToString();
                    UpdateGiftCardAmount(giftCard, giftCardDiscount);
                    giftCard.RedemptionDate = null;       

                    await repo.RecoverGiftCardAsync(giftCard);
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

        private void UpdateGiftCardAmount(GiftCard giftCard, decimal giftCardDiscount)
        {
            if (giftCard.Status == GiftCardStatuses.Expired.ToString())
            {
                giftCard.ToSpend = 0;
            } else
            {
                giftCard.ToSpend = giftCardDiscount;
            }
        }

        public async Task<IEnumerable<GiftCard>> GetAllGiftCardsAsync()
        {
            using (var repo = new GiftCardRepository())
            {
                return await repo.GetAllGiftCardsAsync();
            }
        }

        public async Task<IEnumerable<GiftCard>> GetGiftCardsByPromoCodeAsync(string promocode)
        {
            using (var repo = new GiftCardRepository())
            {
                return await repo.GetGiftCardsByPromoCodeAsync(promocode);
            }
        }

        public async Task AddNewGiftCardAsync(GiftCard giftCard)
        {
            using (var repo = new GiftCardRepository())
            {
                await repo.AddAsync(giftCard);
            }
        }

        public async Task UpdateGiftCardAsync(GiftCard giftCard)
        {
            using (var repo = new GiftCardRepository())
            {
                await repo.UpdateGiftCardAsync(giftCard);
            }
        }
    }
}
