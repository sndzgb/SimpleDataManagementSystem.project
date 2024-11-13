using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SimpleDataManagementSystem.Backend.Logic.Options;
using SimpleDataManagementSystem.Backend.Logic.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleDataManagementSystem.Backend.Logic.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IOptionsSnapshot<EmailClientOptions> _emailClientOptionsSnapshot;


        public EmailService(ILogger<EmailService> logger, IOptionsSnapshot<EmailClientOptions> emailClientOptionsSnapshot)
        {
            _logger = logger;
            _emailClientOptionsSnapshot = emailClientOptionsSnapshot;
        }


        public void Send(Email email)
        {
            var emailOptions = _emailClientOptionsSnapshot.Value;

            MailMessage mailMessage = new MailMessage(email.From, email.To, email.Subject, email.Body);
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = Encoding.UTF8;

            using (var smtpClient = new SmtpClient(emailOptions.Host, emailOptions.Port) { })
            {
#if DEVELOPMENT
                SendDevelopmentEmail(smtpClient, mailMessage);
#else
                smtpClient.Send(mailMessage);
#endif
            }
        }

        public Task SendAsync(Email email)
        {
            Send(email);
            return Task.CompletedTask;
        }

        private void SendDevelopmentEmail(SmtpClient smtpClient, MailMessage mailMessage)
        {
            if (smtpClient == null)
                return;

            if (mailMessage == null)
                return;

            var exe = Assembly.GetExecutingAssembly();
            var exeDirPath = Path.GetDirectoryName(exe.Location);

            var di = Directory.CreateDirectory(Path.Combine(exeDirPath, "Emails"));

            smtpClient.PickupDirectoryLocation = di.FullName;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;

            smtpClient.Send(mailMessage);
        }
    }

    public class Email
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
