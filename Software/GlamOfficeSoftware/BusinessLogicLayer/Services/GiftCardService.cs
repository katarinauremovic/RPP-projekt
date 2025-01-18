using DataAccessLayer.Repositories;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class GiftCardService
    {
        public async Task RecoverGiftCardAsync(int giftCardId)
        {
            using (var repo = new GiftCardRepository())
            {
                var giftCard = await repo.GetByIdAsync(giftCardId);

                if (giftCard != null)
                {
                    var recoveredGiftCard = new GiftCard
                    {
                        
                    };
                }
            }
        }
    }
}
