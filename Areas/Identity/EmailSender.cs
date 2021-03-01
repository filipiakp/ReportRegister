using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;

namespace ReportRegister.Areas.Identity
{
    public class EmailSender : IEmailSender
    {
        private IConfiguration _configuration;
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.Sender = MailboxAddress.Parse(_configuration["SMTP_Mail"]);
            mimeMessage.To.Add(MailboxAddress.Parse(email));
            mimeMessage.Subject = subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = htmlMessage;
            mimeMessage.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_configuration["SMTP_Host"], int.Parse(_configuration["SMTP_Port"]), SecureSocketOptions.StartTls);
            smtp.Authenticate(_configuration["SMTP_Mail"], _configuration["SMTP_Password"]);
            await smtp.SendAsync(mimeMessage);
            smtp.Disconnect(true);
        }
    }
}
