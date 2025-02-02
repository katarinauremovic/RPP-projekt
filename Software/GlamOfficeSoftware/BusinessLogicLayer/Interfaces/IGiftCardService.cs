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
    }
}
