namespace Hotel.Application.DTOs;

public class CountryResponseDto
{
    public Guid Id { get; set; }

    public bool CountryRetrieved { get; set; }

    public string Message { get; set; } = string.Empty;

}