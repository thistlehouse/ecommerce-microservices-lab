using Users.Domain.Enums;

namespace Users.Domain;

public abstract class Entity
{
    public Guid Id { get; set; }
    public ClientType ClientType { get; set; }
}