namespace Users.Application.Common.Abstractions.Services.EmailNotifications;

public sealed class MessageBuilder
{
    public string SmtpHost { get; private set; } = string.Empty;
    public int Port { get; private set; }
    public string From { get; private set; } = string.Empty;
    public string To { get; private set; } = string.Empty;
    public string Subject { get; private set; } = string.Empty;
    public string Body { get; private set; } = string.Empty;
    public string Code { get; private set; } = string.Empty;

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
        Body = $"{body}";
        return this;
    }

    public MessageBuilder WithCode(string code)
    {
        Code = code;
        return this;
    }

    public Message BuildEmailConfirmationMessage()
    {
        Body += " " + Code;

        return EmailConfirmationMessage.Create(
            SmtpHost,
            Port,
            From,
            To,
            Subject,
            Body,
            Code);
    }
}

public sealed class EmailConfirmationMessage : Message
{
    public string Code { get; private set; }

    private EmailConfirmationMessage(
        string smtpHost,
        int port,
        string from,
        string to,
        string subject,
        string body,
        string code)
    {
        SmtpHost = smtpHost;
        Port = port;
        From = from;
        To = to;
        Subject = subject;
        Body = body;
        Code = code;
    }

    public static EmailConfirmationMessage Create(
        string smtpHost,
        int port,
        string from,
        string to,
        string subject,
        string body,
        string code) =>
        new(smtpHost, port, from, to, subject, body, code);
}
