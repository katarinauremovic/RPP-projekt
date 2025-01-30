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
                await SendLoyaltyLevelUpgradeEmailAsync(client, loyaltyLevel);
            } else if (loyaltyLevel.Level < client.LoyaltyLevel.Level)
            {
                await SendLoyaltyLevelDowngradeEmailAsync(client, loyaltyLevel);
            }
        }

        private async Task SendLoyaltyLevelUpgradeEmailAsync(Client client, LoyaltyLevel loyaltyLevel)
        { 
            var rewards = await GetRewardsDtoForClientAsync(client.idClient);

            string subject = $"Congratulations! Your new Loyalty Level is {loyaltyLevel.Name}";

            string body = $"Hello {client.Firstname} {client.Lastname},<br><br>" +
                          $"Your loyalty level has been upgraded to: <b>{loyaltyLevel.Name}</b>!<br>" +
                          $"With this level, you are eligible for the following rewards:<br><br>" +
                          "<table border='1' cellpadding='10' style='border-collapse: collapse;'>" +
                          "<thead><tr>" +
                          "<th>Reward</th>" +
                          "<th>Description</th>" +
                          "<th>Cost (Points)</th>" +
                          "<th>Reedem Code</th>" +
                          "<th>Status</th>" +
                          "</tr></thead>" +
                          "<tbody>";

            foreach (var reward in rewards)
            {
                body += $"<tr>" +
                        $"<td>{reward.Name}</td>" +
                        $"<td>{reward.Description}</td>" +
                        $"<td>{reward.CostPoints}</td>" +
                        $"<td>{reward.ReedemCode}</td>" +
                        $"<td>{reward.Status}</td>" +
                        "</tr>";
            }

            body += "</tbody></table><br><br>" +
                    "Thank you for being a loyal customer!";

            var gmailService = new GmailService();
            gmailService.SendEmailAsync(client.Email, subject, body).Wait();

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

            string subject = $"Notice: Your Loyalty Level has been downgraded to {loyaltyLevel.Name}";
            string body = $"Hello {client.Firstname} {client.Lastname},<br><br>" +
                          $"Your loyalty level has been changed to: <b>{loyaltyLevel.Name}</b>.<br>" +
                          $"We value your continued support and encourage you to keep earning points to unlock more rewards.<br><br>";

            if (lostRewards.Any())
            {
                body += "Unfortunately, with this change, you no longer have access to the following rewards:<br><br>" +
                        "<table border='1' cellpadding='10' style='border-collapse: collapse;'>" +
                        "<thead><tr>" +
                        "<th>Reward</th>" +
                        "<th>Description</th>" +
                        "<th>Cost (Points)</th>" +
                        "</tr></thead>" +
                        "<tbody>";

                foreach (var reward in lostRewards)
                {
                    body += $"<tr>" +
                            $"<td>{reward.Name}</td>" +
                            $"<td>{reward.Description}</td>" +
                            $"<td>{reward.CostPoints}</td>" +
                            "</tr>";
                }

                body += "</tbody></table><br>";
            }

            body += "<br>Thank you for your loyalty!<br>" +
                    "Keep earning points to reach higher levels again.<br><br>" +
                    "Best regards,<br>Your Loyalty Program Team";

            var gmailService = new GmailService();
            await gmailService.SendEmailAsync(client.Email, subject, body);

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
            // Fetching all purchased rewards for the client
            var additionalRewards = await Task.WhenAll(
                client.Client_has_Reward.Select(cr => _rewardService.GetRewardByIdAsync(cr.Reward_idReward))
            );

            // Parsing the loyalty level for the client
            var loyaltyLevel = (LoyaltyLevels)Enum.Parse(typeof(LoyaltyLevels), client.LoyaltyLevel.Name);

            // Fetching the available rewards for the client's current loyalty level
            var rewards = (await _rewardService.GetRewardsWithinClientsLoyaltyLevelAsync(loyaltyLevel))
                .Where(r => !additionalRewards.Any(ar => ar.idReward == r.idReward)) // Filter out already purchased rewards
                .ToList();

            // Adding the purchased rewards at the beginning of the list
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
