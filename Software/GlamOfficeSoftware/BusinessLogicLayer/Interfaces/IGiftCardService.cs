using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IGiftCardService
    {
        Task RecoverGiftCardAsync(int giftCardId, decimal giftCardDiscount);

        Task<IEnumerable<GiftCard>> GetAllGiftCardsAsync(); 

        Task<IEnumerable<GiftCard>> GetGiftCardsByPromoCodeAsync(string promocode);

        Task AddNewGiftCardAsync(GiftCard giftCard);

        Task UpdateGiftCardAsync(GiftCard giftCard);

        Task<string> GenerateGiftCardInStringFormat(GiftCard giftCard);

        Task GenerateGiftCardInPdf(GiftCard giftCard);

        Task DeleteGiftCardAsync(int giftCardId);

        Task<GiftCard> GetOneGiftCardByPromoCodeAsync(string promoCode);
        Task<bool> RedeemGiftCardAsync(string promoCode);
    }
}
