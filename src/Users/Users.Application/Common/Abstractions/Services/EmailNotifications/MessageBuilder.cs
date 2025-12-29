namespace Users.Application.Common.Abstractions.Services.EmailNotifications;

public sealed class MessageBuilder
{
    public string SmtpHost { get; set; } = string.Empty;
    public int Port { get; set; }
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;

    public static MessageBuilder New()
    {
        return new MessageBuilder();
    }

    public MessageBuilder WithSmtpHost(string smtpHost, int port)
    {
        SmtpHost = smtpHost;
        Port = port;
        return this;
    }

    public MessageBuilder FromEmail(string to)
    {
        To = to;
        return this;
    }

    public MessageBuilder ToEmail(string from)
    {
        From = from;
        return this;
    }

    public MessageBuilder WithSubject(string subject)
    {
        Subject = subject;
        return this;
    }

    public MessageBuilder WithBody(string body)
    {
        Body = body;
        return this;
    }

    public Message BuildEmailConfirmationMessage()
    {
        return EmailConfirmationMessage.Create(
            SmtpHost,
            Port,
            From,
            To,
            Subject,
            Body);
    }
}

public sealed class EmailConfirmationMessage : Message
{
    public string? Token { get; set; }

    private EmailConfirmationMessage(
        string smtpHost,
        int port,
        string from,
        string to,
        string subject,
        string body)
    {
        SmtpHost = smtpHost;
        Port = port;
        From = from;
        To = to;
        Subject = subject;
        Body = body;
    }

    public static EmailConfirmationMessage Create(
        string smtpHost,
        int port,
        string from,
        string to,
        string subject,
        string body) =>
        new(smtpHost, port, from, to, subject, body);
}
