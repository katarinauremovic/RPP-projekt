using EntityLayer.DTOs;
using EntityLayer.Entities;
using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IRewardService
    {
        Task<IEnumerable<Reward>> GetRewardsAsync();
        Task<IEnumerable<RewardDTO>> GetRewardsDtoByLoyaltyLevelNameAsync(LoyaltyLevels loyaltyLevelName);
        Task<IEnumerable<RewardDTO>> GetRewardsDtoWithinClientsLoyaltyLevelAsync(LoyaltyLevels loyaltyLevelName);
        Task<IEnumerable<RewardDTO>> GetRewardsDtoAsync();
    }
}
