using System.Net;
using System.Net.Mail;

namespace Feed_Bridge.Services
{
    public class EmailSender
    {
        private readonly string _smtpServer = "smtp.gmail.com"; // السيرفر
        private readonly int _smtpPort = 587; // البورت
        private readonly string _fromEmail = "s0$$95320@gmail.com"; // ايميلك
        private readonly string _password = "ajdw nbrm pndi zjmy"; // باسورد او App Password

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.Credentials = new NetworkCredential(_fromEmail, _password);
                client.EnableSsl = true;

                var message = new MailMessage(_fromEmail, toEmail, subject, body);
                message.IsBodyHtml = true; // لو محتاج HTML

                await client.SendMailAsync(message);
            }
        }
    }
}
