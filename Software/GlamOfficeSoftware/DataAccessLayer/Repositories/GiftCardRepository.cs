using DataAccessLayer.Interfaces;
using EntityLayer.DTOs;
using EntityLayer.Entities;
using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class GiftCardRepository : Repository<GiftCard>
    {
        public async Task RecoverGiftCardAsync(GiftCard recoveredGiftCard)
        {
            var giftCardDb = await GetByIdAsync(recoveredGiftCard.idGiftCard);
            
            if (giftCardDb != null)
            {
                giftCardDb = recoveredGiftCard;
                await SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<GiftCard>> GetAllGiftCardsAsync()
        {
            return await context.GiftCards.ToListAsync();
        }

        public async Task<IEnumerable<GiftCard>> GetGiftCardsByPromoCodeAsync(string promocode)
        {
            promocode = promocode.Trim().ToLower();

            var filtered = await context.GiftCards
                                        .Where(g => g.PromoCode != null && g.PromoCode.ToLower().Contains(promocode))
                                        .ToListAsync();

            return filtered;
        }

        public async Task UpdateGiftCardAsync(GiftCard giftCard)
        {
            var existingGiftCard = await items.FirstOrDefaultAsync(g => g.idGiftCard == giftCard.idGiftCard);

            if (existingGiftCard != null)
            {
                existingGiftCard.Value = giftCard.Value;
                existingGiftCard.ToSpend = giftCard.ToSpend;
                existingGiftCard.Description = giftCard.Description;
                existingGiftCard.PromoCode = giftCard.PromoCode;
                existingGiftCard.ActivationDate = giftCard.ActivationDate;
                existingGiftCard.ExpirationDate = giftCard.ExpirationDate;
                existingGiftCard.RedemptionDate = giftCard.RedemptionDate;
                existingGiftCard.Status = giftCard.Status;

                await SaveChangesAsync();
            }
        }

        public async Task DeleteGiftCardAsync(int giftCardId)
        {
            var giftCard = await items.FirstOrDefaultAsync(g => g.idGiftCard == giftCardId);

            if (giftCard != null)
            {
                items.Remove(giftCard);
                await SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Gift card with ID {giftCardId} does not exist.");
            }
        }

        public async Task<GiftCard> GetOneGiftCardByPromoCodeAsync(string promoCode)
        {
            promoCode = promoCode.Trim().ToLower();

            var giftCard = await context.GiftCards
                                        .FirstOrDefaultAsync(g => g.PromoCode.ToLower() == promoCode);

            return giftCard;
        }

        public async Task<bool> RedeemGiftCardAsync(string promoCode)
        {
            promoCode = promoCode.Trim().ToLower();

            var giftCard = await context.GiftCards
                                        .FirstOrDefaultAsync(g => g.PromoCode.ToLower() == promoCode);

            if (giftCard == null)
            {
                return false; 
            }

            if (giftCard.Status == "Redeemed")
            {
                throw new InvalidOperationException("Gift card has already been redeemed.");
            }

            giftCard.Status = "Redeemed";
            giftCard.RedemptionDate = DateTime.UtcNow; 

            await SaveChangesAsync();

            return true;
        }


    }
}
