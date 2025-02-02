using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public class PromotionEmailService : GmailService
    {
        public async Task SendPromotionalEmailAsync(string recipientEmail, string promotionName, string amount, string description, string endDate)
        {
            string emailBody = GenerateEmailBody(promotionName, amount, description, endDate);
            await SendEmailAsync(recipientEmail, "Exclusive Promotion for You!", emailBody);
        }

        private string GenerateEmailBody(string promotionName, string amount, string description, string endDate)
        {
            return $@"
                <html>
                    <body style='font-family: Arial, sans-serif; color: #333;'>
                        <h2>Exclusive Promotion for You!</h2>
                        <p>Dear [ClientName],</p>
                        <p>We have an amazing offer just for you:</p>
                        <ul>
                            <li><strong>Promotion:</strong> {promotionName}</li>
                            <li><strong>Amount:</strong> {amount}€ off</li>
                            <li><strong>Description:</strong> {description}</li>
                            <li><strong>Valid Until:</strong> {endDate}</li>
                        </ul>
                        <p>Hurry up, this offer is limited!</p>
                        <p>Best regards,</p>
                        <p><strong>Glam Office Team</strong></p>
                    </body>
                </html>";
        }
    }
}
