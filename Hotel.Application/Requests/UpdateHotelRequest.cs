namespace Hotel.Application.Requests;

public class UpdateHotelRequest
{
    public required Guid Id { get; set; }
    
    public required string Name { get; set; }

    public required string Street { get; set; }

    public required string City { get; set; }

    public required string ZipCode { get; set; }

    public required string Country { get; set; }
}