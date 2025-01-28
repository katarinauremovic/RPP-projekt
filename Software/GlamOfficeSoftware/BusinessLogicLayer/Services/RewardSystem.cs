using BusinessLogicLayer.Interfaces;
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
    public class RewardSystem
    {
        private IClientService _clientService;
        private IReceiptService _receiptService;
        private IRewardService _rewardService;
        private ILoyaltyLevelService _loyaltyLevelService;
        private IClientHasRewardService _clientHasRewardService;

        public RewardSystem()
        {
            _clientService = new ClientService();
            _receiptService = new ReceiptService();
            _rewardService = new RewardService();
            _loyaltyLevelService = new LoyaltyLevelService();
            _clientHasRewardService = new ClientHasRewardService();
        }

        public async Task<bool> IsClientInTheRewardSystemAsync(int clientId)
        {
            return await _clientService.IsClientInTheRewardSystemAsync(clientId);
        }

        public async Task AddClientToRewardSystemAsync(int clientId)
        {
            await _clientService.AddClientToRewardSystemAsync(clientId);
        }

        public async Task AddPointsToClientAsync(int clientId, decimal totalAmount)
        {
            var points = CalculatePoints(totalAmount);
            await _clientService.AddPointsToClientAsync(clientId, points);
        }

        public async Task UpdateClientsLoyaltyLevelAsync(Client client)
        {
            var loyaltyLevelName = _loyaltyLevelService.CheckLoyaltyLevel(client.Points.Value);
            var loyaltyLevel = await _loyaltyLevelService.GetLoyaltyLevelByNameAsync(loyaltyLevelName);

            client.LoyaltyLevel = loyaltyLevel;
            client.LoyaltyLevel_id = loyaltyLevel.Id;
        }

        public async Task<IEnumerable<RewardDTO>> GetRewardsDtoForClientAsync(ClientDTO client)
        {
            var clientsRewards = await _clientHasRewardService.GetClientHasRewardsForClientAsync(client.Id);
            var loyaltyLevel = (LoyaltyLevels)Enum.Parse(typeof(LoyaltyLevels), client.LoyaltyLevel);
            var rewards = await _rewardService.GetRewardsDtoWithinClientsLoyaltyLevelAsync(loyaltyLevel);

            var rewardsDto = rewards.Select(r => new RewardDTO
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                CostPoints = r.CostPoints,
                LoyaltyLevelName = r.LoyaltyLevelName,
                RewardAmount = r.RewardAmount,
                ReedemCode = clientsRewards.Where(cr => cr.Reward_idReward == r.Id).Select(cr => cr.ReedemCode).FirstOrDefault() ?? "",
                Status = clientsRewards.Where(cr => cr.Reward_idReward == r.Id).Select(cr => cr.Status).FirstOrDefault() ?? ""
            });

            foreach (var rd in rewardsDto)
            {
                Console.WriteLine(rd.Name.ToString());
            }

            return rewardsDto;
        }

        private int CalculatePoints(decimal totalAmount)
        {
            return (int)Math.Round(totalAmount * 10);
        }
    }
}
