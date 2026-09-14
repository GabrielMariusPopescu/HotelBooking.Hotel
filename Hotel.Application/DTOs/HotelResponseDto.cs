namespace Hotel.Application.DTOs;

public class HotelResponseDto
{
    public Guid Id { get; set; }
    
    public bool HotelRetrieved { get; set; }
    
    public string Message { get; set; } = string.Empty;
}