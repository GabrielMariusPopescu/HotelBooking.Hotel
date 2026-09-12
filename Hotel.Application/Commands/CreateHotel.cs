namespace Hotel.Application.Commands;

public class CreateHotel(
    Guid id, 
    string name, 
    string street, 
    string city, 
    string zipCode, 
    string countryCode) : IRequest<CreateHotelResponse>
{
    public Guid Id { get; } = id;

    public string Name { get; } = name;

    public string Street { get;  } = street;

    public string City { get; } = city;

    public string ZipCode { get;  } = zipCode;

    public string CountryCode { get; } = countryCode;
}
