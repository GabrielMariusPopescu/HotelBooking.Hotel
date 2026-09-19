namespace Hotel.Application.DTOs;

public class HotelResponseDto<T>
{
    public Guid? Id { get; set; }
    
    public bool IsSuccessful { get; set; }
    
    public string Message { get; set; } = string.Empty;
    
    public T? Data { get; set; }
}