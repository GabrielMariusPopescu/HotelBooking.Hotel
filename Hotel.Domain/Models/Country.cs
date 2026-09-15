namespace Hotel.Domain.Models;

public class Country : NameEntity
{
    public bool Disabled { get; private set; }

    public Country()
    {
        
    }
    
    public Country(Guid id, string name)
    {
        Id = id;
        Name = name;
        Disabled = false;
    }
}