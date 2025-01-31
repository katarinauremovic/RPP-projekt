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
    public class RewardSystem
    {
        private IClientService _clientService;
        private IRewardService _rewardService;
        private ILoyaltyLevelService _loyaltyLevelService;
        private IClientHasRewardService _clientHasRewardService;

        public RewardSystem()
        {
            _clientService = new ClientService();
            _rewardService = new RewardService();
            _loyaltyLevelService = new LoyaltyLevelService();
            _clientHasRewardService = new ClientHasRewardService();
        }

        public async Task PurchaseReward(int clientId, int rewardId)
        {
            var client = await _clientService.GetClientByIdAsync(clientId);
            var reward = await _rewardService.GetRewardByIdAsync(rewardId);
            if (client.Points < reward.CostPoints)
            {
                throw new Exception("Client does not have enough points to purchase this reward.");
            }
            var clientHasReward = new Client_has_Reward
            {
                Client_idClient = clientId,
                Reward_idReward = rewardId,
                SpentPoints = reward.CostPoints.Value,
                PurchaseDate = DateTime.Now,
                ReedemCode = GenerateRedeemCode(),
                Status = ClientHasRewardStatuses.Active.ToString()
            };

            await _clientHasRewardService.AddClientHasRewardAsync(clientHasReward);
            await _clientService.SubtractPointsFromClientAsync(clientId, reward.CostPoints.Value);
        }

        public async Task ProcessReceiptAsync(Receipt receiptDb)
        {
            IReservationService reservationService = new ReservationService();
            await reservationService.ChangeReservationStatusAndPaymentAsync(receiptDb.Reservation_idReservation, ReservationStatuses.Completed, true);

            var clientId = receiptDb.Reservation.Client_idClient.Value;
            var isClientInTheRewardSystem = await IsClientInTheRewardSystemAsync(clientId);

            if (isClientInTheRewardSystem)
            {
                await AddPointsToClientAsync(clientId, receiptDb.TotalTreatmentAmount.Value);
            } else
            {
                await AddClientToRewardSystemAsync(clientId);
                await AddPointsToClientAsync(clientId, receiptDb.TotalTreatmentAmount.Value);
            }   
        }

        public async Task<int> UpdateClientsLoyaltyLevelAsync(Client client)
        {
            var loyaltyLevelName = _loyaltyLevelService.CheckLoyaltyLevel(client.Points.Value);
            var loyaltyLevel = await _loyaltyLevelService.GetLoyaltyLevelByNameAsync(loyaltyLevelName);

            await HandleLoyaltyLevelChangeAsync(client, loyaltyLevel);

            return loyaltyLevel.Id;
        }

        private async Task HandleLoyaltyLevelChangeAsync(Client client, LoyaltyLevel loyaltyLevel)
        {
            if (loyaltyLevel.Level > client.LoyaltyLevel.Level)
            {
                await _clientService.UpdateClientsLoyaltyLevelAsync(client.idClient, loyaltyLevel.Id);
                await SendLoyaltyLevelUpgradeEmailAsync(client, loyaltyLevel);
            } else if (loyaltyLevel.Level < client.LoyaltyLevel.Level)
            {
                await SendLoyaltyLevelDowngradeEmailAsync(client, loyaltyLevel);
            }
        }

        private async Task SendLoyaltyLevelUpgradeEmailAsync(Client client, LoyaltyLevel loyaltyLevel)
        { 
            var rewards = await GetRewardsDtoForClientAsync(client.idClient);

            var clientGmailService = new ClientGmailService();
            await clientGmailService.FormatLoyaltyLevelUpgradeEmail(client, loyaltyLevel, rewards);

            Console.WriteLine("Loyalty level change email successfully sent.");
        }

        private async Task SendLoyaltyLevelDowngradeEmailAsync(Client client, LoyaltyLevel loyaltyLevel)
        {
            var loyaltyLevelName = (LoyaltyLevels)Enum.Parse(typeof(LoyaltyLevels), client.LoyaltyLevel.Name);

            var rewardsUpperLevel = await _rewardService.GetRewardsDtoByLoyaltyLevelNameAsync(loyaltyLevelName);
            var clientsRewards = await _clientHasRewardService.GetClientHasRewardsForClientAsync(client.idClient);

            var lostRewards = rewardsUpperLevel
                .Where(rul => !clientsRewards.Any(cr => cr.Reward_idReward == rul.RewardId))
                .ToList();

            lostRewards = lostRewards.Distinct().ToList();

            var clientGmailService = new ClientGmailService();
            await clientGmailService.FormatLoyaltyLevelDowngradeEmail(client, loyaltyLevel, lostRewards);

            Console.WriteLine("Loyalty level downgrade email successfully sent.");
        }

        public async Task<IEnumerable<RewardDTO>> GetRewardsDtoForClientAsync(int clientId)
        {
            var client = await _clientService.GetClientByIdAsync(clientId);
            var clientsRewards = await _clientHasRewardService.GetClientHasRewardsForClientAsync(client.idClient);

            var rewards = await GetClientRewardsWithPurchasedAsync(client);

            var rewardsDto = GetRewardsDtoForClientAsync(client, rewards, clientsRewards);

            return rewardsDto;
        }

        private IEnumerable<RewardDTO> GetRewardsDtoForClientAsync(Client client, List<Reward> rewards, IEnumerable<Client_has_Reward> clientsRewards)
        {
            var rewardsDto = rewards.Select(r => new RewardDTO
            {
                ClientId = client.idClient,
                RewardId = r.idReward,
                Name = r.Name,
                Description = r.Description,
                CostPoints = r.CostPoints ?? 0,
                LoyaltyLevelName = r.LoyaltyLevel.Name,
                RewardAmount = r.RewardAmount ?? 0,
                ReedemCode = clientsRewards.FirstOrDefault(cr => cr.Reward_idReward == r.idReward)?.ReedemCode,
                Status = clientsRewards.FirstOrDefault(cr => cr.Reward_idReward == r.idReward)?.Status
            }).ToList();

            return rewardsDto;
        }


        private async Task<List<Reward>> GetClientRewardsWithPurchasedAsync(Client client)
        {
            var additionalRewards = await Task.WhenAll(
                client.Client_has_Reward.Select(cr => _rewardService.GetRewardByIdAsync(cr.Reward_idReward))
            );

            var loyaltyLevel = (LoyaltyLevels)Enum.Parse(typeof(LoyaltyLevels), client.LoyaltyLevel.Name);

            var rewards = (await _rewardService.GetRewardsWithinClientsLoyaltyLevelAsync(loyaltyLevel))
                .Where(r => !additionalRewards.Any(ar => ar.idReward == r.idReward)) 
                .ToList();

            rewards.InsertRange(0, additionalRewards);

            return rewards;
        }

        private async Task AddPointsToClientAsync(int clientId, decimal totalAmount)
        {
            var points = CalculatePoints(totalAmount);
            await _clientService.AddPointsToClientAsync(clientId, points);
        }

        private async Task<bool> IsClientInTheRewardSystemAsync(int clientId)
        {
            return await _clientService.IsClientInTheRewardSystemAsync(clientId);
        }

        private async Task AddClientToRewardSystemAsync(int clientId)
        {
            await _clientService.AddClientToRewardSystemAsync(clientId);
        }

        private int CalculatePoints(decimal totalAmount)
        {
            return (int)Math.Round(totalAmount * 10);
        }

        private string GenerateRedeemCode()
        {
            return NStringGenerator.NStringGenerator.Generate();
        }
    }
}
