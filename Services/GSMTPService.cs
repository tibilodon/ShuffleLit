using Microsoft.Extensions.Options;
using ShuffleLit.Helpers;
using ShuffleLit.Interfaces;
using System.Net;
using System.Net.Mail;

namespace ShuffleLit.Services
{
    public class GSMTPService : IGSMTPService
    {
        public GSMTPSettings Options { get; set; }
        public GSMTPService(IOptions<GSMTPSettings> config)
        {
            Options = config.Value;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (string.IsNullOrEmpty(Options.ApiKey))
            {
                throw new Exception("Null gsmtp api key");
            }
            await Execute(Options.FromMail, Options.ApiKey, subject, message, toEmail);
        }

        private async Task Execute(string fromMail, string apiKey, string subject, string message, string toEmail)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, apiKey),
                EnableSsl = true
            };
            MailMessage GSMTPMessage = new MailMessage();
            GSMTPMessage.From = new MailAddress(fromMail);
            GSMTPMessage.Subject = subject;
            GSMTPMessage.To.Add(new MailAddress(toEmail));
            GSMTPMessage.Body = message;
            GSMTPMessage.IsBodyHtml = true;
            //  send mail
            smtpClient.Send(GSMTPMessage);
        }
    }
}


