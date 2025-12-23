namespace Users.Application.Common.Abstractions.Services;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}