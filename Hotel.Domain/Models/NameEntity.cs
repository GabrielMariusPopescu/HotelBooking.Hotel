namespace Hotel.Domain.Models;

public abstract class NameEntity
{
    public Guid Id { get; protected set; }

    public string Name { get; protected set; } = string.Empty;
}