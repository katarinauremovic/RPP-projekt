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


    }
}
