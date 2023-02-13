using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeQuest.Models;
using HomeQuest.Models.Settings;
using HomeQuest.Services;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace HomeQuest.Services
{
    public class MailService : IMailService, IEmailSender
    {
        private readonly MailSettings _settings;
        private readonly ILogger<MailService> _logger;

        public MailService(IOptions<MailSettings> settings, ILogger<MailService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            await SendAsync(new MailData(
                to: mailRequest.UserEmail,
                subject: mailRequest.OfferAmount.ToString(),
                body: mailRequest.OfferMessage));
        }

        // used by the Identity framework
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            await SendAsync(new MailData(to, subject, body));
        }

        public async Task SendAsync(MailData mailData)
        {
            try
            {
                var mail = new MimeMessage();
                mail.Sender = new MailboxAddress(_settings.DisplayName, _settings.From);
                mail.To.Add(MailboxAddress.Parse(mailData.To));
                mail.Subject = mailData.Subject;

                var body = new BodyBuilder();
                body.HtmlBody = mailData.Body;
                if (mailData.Attachments != null)
                {
                    byte[] attachmentFileByteArray;
                    foreach (var attachment in mailData.Attachments)
                    {
                        if (attachment.Length > 0)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                attachment.CopyTo(memoryStream);
                                attachmentFileByteArray = memoryStream.ToArray();
                            }
                            body.Attachments.Add(attachment.FileName, attachmentFileByteArray, ContentType.Parse(attachment.ContentType));
                        }
                    }
                }
                mail.Body = body.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_settings.UserName, _settings.Password);
                await smtp.SendAsync(mail);
                await smtp.DisconnectAsync(true);

                _logger.LogInformation($"Email sent to {mailData.To} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Email sent to {mailData.To} failed.");
                throw ex;
            }
        }

        public async Task SendWelcomeEmailAsync(WelcomeRequest request)
        {
            string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\WelcomeTemplate.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();
            MailText = MailText.Replace("[username]", request.UserName).Replace("[email]", request.ToEmail);
            await SendAsync(new MailData(
                to: request.ToEmail,
                subject: $"Welcome {request.UserName}",
                body: MailText));
        }
    }
}