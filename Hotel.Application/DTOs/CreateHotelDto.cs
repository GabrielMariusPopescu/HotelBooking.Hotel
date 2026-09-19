namespace Hotel.Application.DTOs;

public class CreateHotelDto
{
    public Guid? Id { get; set; }

    public required string Name { get; set; }

    public required string Street { get; set; }

    public required string City { get; set; }

    public required string ZipCode { get; set; }

    public required string Country { get; set; }
}