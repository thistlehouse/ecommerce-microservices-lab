using Users.Application.Common.Abstractions.Services;

namespace Users.Infrastructure;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
