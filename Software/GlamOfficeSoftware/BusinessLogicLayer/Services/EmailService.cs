using MailKit.Net.Imap;
using MailKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{
    public abstract class EmailService
    {
        protected readonly string _email = "glamoffice2025@gmail.com";
        protected readonly string _password = "zztv yfus xomu btrp";
        protected readonly string _smtpServer = "smtp.gmail.com";
        protected readonly int _smtpPort = 587;

        public abstract Task SendEmailAsync(string recipientEmail, string subject, string body);
    }
}
