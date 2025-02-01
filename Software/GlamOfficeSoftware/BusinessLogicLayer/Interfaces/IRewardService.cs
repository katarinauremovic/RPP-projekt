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
        Task<Reward> GetRewardByIdAsync(int rewardId);
        Task<IEnumerable<RewardDTO>> GetRewardsDtoByLoyaltyLevelNameAsync(LoyaltyLevels loyaltyLevelName);
        Task<IEnumerable<RewardDTO>> GetRewardsDtoWithinClientsLoyaltyLevelAsync(LoyaltyLevels loyaltyLevelName);
        Task<IEnumerable<Reward>> GetRewardsWithinClientsLoyaltyLevelAsync(LoyaltyLevels loyaltyLevelName);
        Task AddRewardAsync(Reward reward);
        Task<IEnumerable<RewardDTO>> GetRewardsDtoAsync();
    }
}
