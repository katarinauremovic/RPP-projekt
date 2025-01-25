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
    public class LoyaltyLevelService : ILoyaltyLevelService
    {
        public async Task<LoyaltyLevel> GetLoyaltyLevelByNameAsync(LoyaltyLevels loyaltyLevel)
        {
            using (var repo = new LoyaltyLevelRepository())
            {
                var strLoyaltyLevel = loyaltyLevel.ToString();
                return await repo.GetLoyaltyLevelByNameAsync(strLoyaltyLevel);
            }
        }

        public LoyaltyLevels CheckLoyaltyLevel(int points)
        {
            if (points >= 20000)
            {
                return LoyaltyLevels.VIP;
            } else if (points >= 10000 && points < 20000)
            {
                return LoyaltyLevels.Platinum;
            } else if (points >= 5000 && points < 10000)
            {
                return LoyaltyLevels.Gold;
            } else if (points >= 1000 && points < 5000)
            {
                return LoyaltyLevels.Silver;
            } else if (points >= 500 && points < 1000)
            {
                return LoyaltyLevels.Bronze;
            } else
            {
                return LoyaltyLevels.None;
            }
        }
    }
}
