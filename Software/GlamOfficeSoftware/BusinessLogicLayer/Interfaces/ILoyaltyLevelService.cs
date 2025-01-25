using EntityLayer.Entities;
using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface ILoyaltyLevelService
    {
        Task<LoyaltyLevel> GetLoyaltyLevelByNameAsync(LoyaltyLevels loyaltyLevel);
        LoyaltyLevels CheckLoyaltyLevel(int points);
    }
}
