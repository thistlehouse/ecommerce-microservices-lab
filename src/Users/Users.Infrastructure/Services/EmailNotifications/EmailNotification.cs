using System.Net;
using System.Net.Mail;
using Users.Application.Common.Abstractions.Services.EmailNotifications;

namespace Users.Infrastructure.Services.EmailNotifications;

public sealed class EmailNotification : IEmailNotification
{
    public async Task SendNotificationAsync(Message message)
    {
        using SmtpClient client = new(message.SmtpHost, message.Port);
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.EnableSsl = false;
        client.Credentials = null;
        client.Timeout = 5000;

        using MailMessage mailMessage = new(message.From, message.To, message.Subject, message.Body)
        {
            IsBodyHtml = true,
        };

        await client.SendMailAsync(mailMessage);
    }
}