namespace Hotel.Domain.Models;

public class Address(string street, string city, string zipCode, string countryCode)
{
    public string Street { get;  } = street;

    public string City { get; } = city;

    public string ZipCode { get; } = zipCode;

    public string CountryCode { get; } = countryCode;
}
