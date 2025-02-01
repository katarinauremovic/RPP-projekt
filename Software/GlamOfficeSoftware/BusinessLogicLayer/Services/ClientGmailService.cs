using EntityLayer.DTOs;
using EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class ClientGmailService : GmailService
    {
        public async Task FormatLoyaltyLevelUpgradeEmail(Client client, LoyaltyLevel loyaltyLevel, IEnumerable<RewardDTO> rewards)
        {
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
           
            await base.SendEmailAsync(client.Email, subject, body);
        }

        public async Task FormatLoyaltyLevelDowngradeEmail(Client client, LoyaltyLevel loyaltyLevel, IEnumerable<RewardDTO> lostRewards)
        {
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

            await base.SendEmailAsync(client.Email, subject, body);
        }
    }
}
