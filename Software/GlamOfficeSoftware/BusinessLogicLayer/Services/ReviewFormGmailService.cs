using EntityLayer.Entities;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;

namespace BusinessLogicLayer.Services
{
    public class ReviewFormGmailService : GmailService
    {
     
            private readonly string _googleFormBaseUrl = "https://docs.google.com/forms/d/e/1FAIpQLSfVN6cm9w7R7m6yNYQJikDMPNYENOiN3cYBldJvbQ3CGTWbQw/viewform?usp=pp_url";

            public string GenerateReviewFormLink(int reservationId, int treatmentId, int employeeId, int clientId)
            {
                return $"{_googleFormBaseUrl}&entry.229577371={reservationId}&entry.1655282895={treatmentId}&entry.728183292={employeeId}&entry.704643125={clientId}";
            }

            public async Task SendReviewRequestEmailAsync(string clientEmail, string clientName, int reservationId, List<(int treatmentId, string treatmentName, int employeeId)> treatments, int clientId)
            {
                string treatmentsListHtml = "";
                foreach (var treatment in treatments)
                {
                    string reviewLink = GenerateReviewFormLink(reservationId, treatment.treatmentId, treatment.employeeId, clientId);
                    treatmentsListHtml += $"<li>{treatment.treatmentName} – <a href='{reviewLink}'>Rate Here</a></li>";
                }

                string subject = "We'd love your feedback!";
                string body = $@"
        <p>Dear {clientName},</p>
        <p>Thank you for your recent visit! We value your opinion and would love to hear your feedback.</p>
        <p>Please take a moment to fill out our short review form for the treatments you received:</p>
        <ul>{treatmentsListHtml}</ul>
        <p>Your feedback helps us improve our services!</p>
        <p>Best regards,</p>
        <p><strong>Glam Office Team</strong></p>";

                await SendEmailAsync(clientEmail, subject, body);
            }

            public async Task<List<Review>> FetchReviewsFromEmailAsync()
            {
                var reviews = new List<Review>();

                using (var client = new ImapClient())
                {
                    await client.ConnectAsync("imap.gmail.com", 993, true);
                    await client.AuthenticateAsync("glamoffice2025@gmail.com", "zztv yfus xomu btrp");

                    var inbox = client.Inbox;
                    await inbox.OpenAsync(FolderAccess.ReadWrite);

                    var uids = await inbox.SearchAsync(SearchQuery.SubjectContains("New Form Response"));

                    foreach (var uid in uids)
                    {
                        var message = await inbox.GetMessageAsync(uid);

                        var review = ParseEmailBody(message.TextBody);
                        if (review != null)
                        {
                            reviews.Add(review);
                        }

                        await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
                    }

                    await client.DisconnectAsync(true);
                }

                return reviews;
            }

            private Review ParseEmailBody(string emailBody)
            {
                try
                {
                  

                    DateTime date = ExtractDateValue(emailBody, "Date:");
                    int reservationId = ExtractIntValue(emailBody, "Reservation ID:");
                    int treatmentId = ExtractIntValue(emailBody, "Treatment ID:");
                    int employeeId = ExtractIntValue(emailBody, "Employee ID:");
                    int clientId = ExtractIntValue(emailBody, "Client ID:");
                    int rating = ExtractIntValue(emailBody, "Rating:");
                    string comment = ExtractValue(emailBody, "Comment:");

                    Console.WriteLine($"Parsed Review - Date: {date}, Reservation ID: {reservationId}, Treatment ID: {treatmentId}, Employee ID: {employeeId}, Client ID: {clientId}, Rating: {rating}, Comment: {comment}");
                    if (reservationId == 0)
                    {
                        Console.WriteLine("Skipping review with invalid Reservation ID = 0.");
                        return null;
                    }
                    return new Review
                    {
                        Date = date,
                        Reservation_idReservation = reservationId,
                        Treatment_idTreatment = treatmentId,
                        Employee_idEmployee = employeeId,
                        Client_idClient = clientId,
                        Rating = rating,
                        Comment = comment
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing email: {ex.Message}");
                    return null;
                }
            }

            private int ExtractIntValue(string text, string field)
            {
                var match = Regex.Match(text, $"{field}\\s*(\\d+)");
                return match.Success ? int.Parse(match.Groups[1].Value.Trim()) : 0;
            }

            private string ExtractValue(string text, string field)
            {
                var match = Regex.Match(text, $"{field}\\s*(.+)");
                return match.Success ? match.Groups[1].Value.Trim() : "";
            }
            private DateTime ExtractDateValue(string text, string field)
            {
                var match = Regex.Match(text, $"{field}\\s*(\\d{{1,2}}\\.\\d{{1,2}}\\.\\d{{4}})"); 
                if (match.Success)
                {
                    return DateTime.ParseExact(match.Groups[1].Value.Trim(), "d.M.yyyy", CultureInfo.InvariantCulture);
                }
                return DateTime.UtcNow; 
            }

        }
    }
    

