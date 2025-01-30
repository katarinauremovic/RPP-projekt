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

            if(loyaltyLevel.Id != client.LoyaltyLevel_id)
            {
                await SendLoyaltyLevelChangeEmailAsync(client, loyaltyLevel);
            }

            return loyaltyLevel.Id;
        }

        public async Task SendLoyaltyLevelChangeEmailAsync(Client client, LoyaltyLevel loyaltyLevel)
        {
            // Fetching rewards for the loyalty level
            var loyaltyLevelName = (LoyaltyLevels)Enum.Parse(typeof(LoyaltyLevels), client.LoyaltyLevel.Name);
            var rewards = await _rewardService.GetRewardsDtoWithinClientsLoyaltyLevelAsync(loyaltyLevelName);

            // Constructing the email body with a table
            string subject = $"Congratulations! Your new Loyalty Level is {loyaltyLevel.Name}";

            string body = $"Hello {client.Firstname},<br><br>" +
                          $"Your loyalty level has been upgraded to: <b>{loyaltyLevel.Name}</b>!<br>" +
                          $"With this level, you are eligible for the following rewards:<br><br>" +
                          "<table border='1' cellpadding='10' style='border-collapse: collapse;'>" +
                          "<thead><tr>" +
                          "<th>Reward</th>" +
                          "<th>Description</th>" +
                          "<th>Cost (Points)</th>" +
                          "<th>Reward Amount</th>" +
                          "</tr></thead>" +
                          "<tbody>";

            // Adding rows for each reward
            foreach (var reward in rewards)
            {
                body += $"<tr>" +
                        $"<td>{reward.Name}</td>" +
                        $"<td>{reward.Description}</td>" +
                        $"<td>{reward.CostPoints.ToString()}</td>" +
                        "</tr>";
            }

            body += "</tbody></table><br><br>" +
                    "Thank you for being a loyal customer!";

            // Sending the email
            var gmailService = new GmailService(); // Assuming there's an existing EmailService implementation
            await gmailService.SendEmailAsync(client.Email, subject, body);

            Console.WriteLine("Loyalty level change email successfully sent.");
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
            var loyaltyLevel = (LoyaltyLevels)Enum.Parse(typeof(LoyaltyLevels), client.LoyaltyLevel.Name);
            var rewards = (await _rewardService.GetRewardsWithinClientsLoyaltyLevelAsync(loyaltyLevel))
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
