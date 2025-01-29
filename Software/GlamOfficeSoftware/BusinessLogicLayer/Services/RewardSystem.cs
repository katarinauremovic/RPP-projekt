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
            await reservationService.ChangeReservationStatusAsync(receiptDb.Reservation_idReservation, ReservationStatuses.Completed);

            RewardSystem rewardSystem = new RewardSystem();

            var clientId = receiptDb.Reservation.Client_idClient.Value;
            var isClientInTheRewardSystem = await rewardSystem.IsClientInTheRewardSystemAsync(clientId);

            if (isClientInTheRewardSystem)
            {
                await rewardSystem.AddPointsToClientAsync(clientId, receiptDb.TotalTreatmentAmount.Value);
            } else
            {
                await rewardSystem.AddClientToRewardSystemAsync(clientId);
                await rewardSystem.AddPointsToClientAsync(clientId, receiptDb.TotalTreatmentAmount.Value);
            }   
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

        public async Task<int> UpdateClientsLoyaltyLevelAsync(Client client)
        {
            var loyaltyLevelName = _loyaltyLevelService.CheckLoyaltyLevel(client.Points.Value);
            var loyaltyLevel = await _loyaltyLevelService.GetLoyaltyLevelByNameAsync(loyaltyLevelName);

            return loyaltyLevel.Id;
        }

        public async Task<IEnumerable<RewardDTO>> GetRewardsDtoForClientAsync(int clientId)
        {
            var client = await _clientService.GetClientByIdAsync(clientId);
            var clientsRewards = await _clientHasRewardService.GetClientHasRewardsForClientAsync(client.idClient);

            // Dohvati sve nagrade koje su kupljene
            var additionalRewards = await Task.WhenAll(
                clientsRewards.Select(cr => _rewardService.GetRewardByIdAsync(cr.Reward_idReward))
            );

            // Ukloni kupljene nagrade koje već postoje u rewards listi
            var rewards = (await _rewardService.GetRewardsWithinClientsLoyaltyLevelAsync((LoyaltyLevels)Enum.Parse(typeof(LoyaltyLevels), client.LoyaltyLevel.Name)))
                .Where(r => !additionalRewards.Any(ar => ar.idReward == r.idReward)) // Filtriraj da ne uključuje već kupljene
                .ToList();

            // Dodaj kupljene nagrade u rewards listu
            rewards.InsertRange(0, additionalRewards); // Dodaj kupljene nagrade na početak

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
            });

            return rewardsDto;
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
