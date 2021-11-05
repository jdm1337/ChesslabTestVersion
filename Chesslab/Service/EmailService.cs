using System;
using System.Threading.Tasks;
using Chesslab.Service;
using MailKit.Net.Smtp;
using MimeKit;
namespace ParseApi.Services
{
    public class EmailService : IMessageEmailService
    {
        public async Task SendMessage(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("ParseApi", "taramalytest@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 25, false);
                await client.AuthenticateAsync("taramalytest@gmail.com", "Zaqwsx1234Zaqwsx1234");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}