namespace Hotel.Domain.Models;

public class Country : NameEntity
{
    public string CountryCode { get; private set; }

    public bool Disabled { get; private set; }

    public Country()
    {
        
    }
    
    public Country(Guid id, string name, string countryCode)
    {
        Id = id;
        Name = name;
        CountryCode = countryCode;
        Disabled = false;
    }
}