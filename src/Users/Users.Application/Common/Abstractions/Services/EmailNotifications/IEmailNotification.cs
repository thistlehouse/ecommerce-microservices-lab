namespace Users.Application.Common.Abstractions.Services.EmailNotifications;

public interface IEmailNotification
{
    void SendNotification(Message message);
}