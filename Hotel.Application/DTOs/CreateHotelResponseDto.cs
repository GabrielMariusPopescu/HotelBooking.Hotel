namespace Hotel.Application.DTOs;

public class CreateHotelResponseDto
{
    public Guid HotelId { get; set; }

    public bool HotelCreated { get; set; }

    public string Message { get; set; } = string.Empty;
}