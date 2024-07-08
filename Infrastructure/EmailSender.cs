using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Contracts.Infrastructure;
using Application.Models;

namespace Infrastructure
{
    public class EmailSender : IEmailSender
    {
        private EmailSettings _emailSettings { get; }
        private readonly SendGridClient client;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
            client = new SendGridClient(_emailSettings.ApiKey);
        }

        public async Task<bool> SendEmail(Email email)
        {
            if (_emailSettings.IsTest)
            {
                return await SendTestEmail(email);
            }

            var to = new EmailAddress(email.To);
            var from = new EmailAddress
            {
                Email = _emailSettings.SenderEmail,
                Name = _emailSettings.SenderName
            };

            var message = MailHelper.CreateSingleEmail(from, to, email.Subject, email.Body, email.Body);
            var response = await client.SendEmailAsync(message);

            return response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Accepted;
        }

        private async Task<bool> SendTestEmail(Email email)
        {
            email.Body += $"<br><br>This email is intended for {email.To}";

            var tos = new List<EmailAddress>();

            var emailList = _emailSettings.TestEmail.Split(';').ToList();

            emailList.ForEach(email => tos.Add(new EmailAddress(email)));

            var from = new EmailAddress
            {
                Email = _emailSettings.SenderEmail,
                Name = _emailSettings.SenderName
            };

            var message = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, email.Subject, email.Body, email.Body);
            var response = await client.SendEmailAsync(message);

            return response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Accepted;
        }
    }
}
