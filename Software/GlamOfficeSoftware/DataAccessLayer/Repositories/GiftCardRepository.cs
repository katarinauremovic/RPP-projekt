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
            promocode = promocode.Trim().ToUpper(); 

            var giftCards = await context.GiftCards
                                         .Where(g => g.PromoCode == promocode)
                                         .ToListAsync();
            return giftCards;
        }
    }
}
