namespace Users.Application.Common.Abstractions.Services.EmailNotifications;

public abstract class Message
{
    public string SmtpHost { get; set; } = string.Empty;
    public int Port { get; set; }
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}