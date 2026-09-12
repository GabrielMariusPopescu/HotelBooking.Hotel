namespace Hotel.Application.Extensions;

public static class CreateHotelResponseExtensions
{
    public static CreateHotelResponseDto ToDto(this CreateHotelResponse source)
        => new()
        {
            HotelId = source.HotelId,
            HotelCreated = source.HotelCreated,
            Message = source.Message
        };
}