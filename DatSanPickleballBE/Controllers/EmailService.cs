using MailKit.Net.Smtp;
using System.Net.Mail;
using MimeKit;
using System.Threading.Tasks;
using System.Net;

namespace DatSanPickleballBE.Controllers
{
    public class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com"; // SMTP server Gmail
        private readonly int _smtpPort = 587; // TLS
        private readonly string _fromEmail = "0899244124tuan@gmail.com"; // email gửi
        private readonly string _fromPassword = "tlex kadd netn kwbf"; // App Password

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using (var client = new System.Net.Mail.SmtpClient(_smtpServer, _smtpPort))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(_fromEmail, _fromPassword);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true // cho phép HTML
                };

                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
