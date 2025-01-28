using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Repositories;
using EntityLayer.DTOs;
using EntityLayer.Entities;
using EntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class RewardService : IRewardService
    {
        public async Task<IEnumerable<Reward>> GetRewardsAsync()
        {
            using (var repo = new RewardRepository())
            {
                return await repo.GetAllAsync();
            }
        }

        public async Task<Reward> GetRewardByIdAsync(int rewardId)
        {
            using (var repo = new RewardRepository())
            {
                return await repo.GetByIdAsync(rewardId);
            }
        }

        public async Task<IEnumerable<RewardDTO>> GetRewardsDtoByLoyaltyLevelNameAsync(LoyaltyLevels loyaltyLevelName)
        {
            using (var repo = new RewardRepository())
            {
                var strLoyaltyLevelName = loyaltyLevelName.ToString();
                var rewards = await repo.GetRewardsByLoyaltyLevelNameAsync(strLoyaltyLevelName);
                var rewardsDto = rewards.Select(ConvertRewardToRewardDto);
                return rewardsDto;
            }
        }

        public async Task<IEnumerable<RewardDTO>> GetRewardsDtoAsync()
        {
            using (var repo = new RewardRepository())
            {
                var rewards = await repo.GetAllAsync();
                var rewardsDto = rewards.Select(ConvertRewardToRewardDto);
                return rewardsDto;
            }
        }

        public async Task<IEnumerable<RewardDTO>> GetRewardsDtoWithinClientsPointsAsync(int points)
        {
            using (var repo = new RewardRepository())
            {
                var rewards = await repo.GetRewardsWithinClientsPointsAsync(points);
                var rewardsDto = rewards.Select(ConvertRewardToRewardDto);
                return rewardsDto;
            }
        }

        public async Task<IEnumerable<RewardDTO>> GetRewardsDtoWithinClientsLoyaltyLevelAsync(LoyaltyLevels loyaltyLevelName)
        {
            using (var repo = new RewardRepository())
            {
                ILoyaltyLevelService loyaltyLevelService = new LoyaltyLevelService();
                var loyaltyLevel = await loyaltyLevelService.GetLoyaltyLevelByNameAsync(loyaltyLevelName);
                
                var rewards = await repo.GetRewardsWithinClientsPointsAsync(loyaltyLevel.RequiredPoints);
                
                var rewardsDto = rewards.Select(ConvertRewardToRewardDto);
                return rewardsDto;
            }
        }

        private RewardDTO ConvertRewardToRewardDto(Reward reward)
        {
            return new RewardDTO
            {
                RewardId = reward.idReward,
                Name = reward.Name,
                Description = reward.Description,
                CostPoints = reward.CostPoints.Value,
                LoyaltyLevelName = reward.LoyaltyLevel.Name,
            };
        }
    }
}
