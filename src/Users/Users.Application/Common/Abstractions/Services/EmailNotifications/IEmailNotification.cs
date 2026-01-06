namespace Users.Application.Common.Abstractions.Services.EmailNotifications;

public interface IEmailNotification
{
    Task SendNotificationAsync(Message message);
}