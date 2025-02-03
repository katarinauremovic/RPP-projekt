using MailKit.Net.Imap;
using MailKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;

namespace BusinessLogicLayer.Services
{
    public abstract class GmailService
    {
        private readonly string _email = "glamoffice2025@gmail.com";
        private readonly string _password = "zztv yfus xomu btrp";
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;

        public virtual async Task SendEmailAsync(string recipientEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Glam Office", _email));
            email.To.Add(new MailboxAddress("", recipientEmail));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            using (var smtp = new SmtpClient())
            {
                try
                {
                    await smtp.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(_email, _password);
                    await smtp.SendAsync(email);
                } finally
                {
                    await smtp.DisconnectAsync(true);
                }
            }
        }
    }
}
