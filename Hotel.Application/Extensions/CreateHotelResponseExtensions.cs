namespace Hotel.Application.Extensions;

public static class CreateHotelResponseExtensions
{
    public static CreateHotelResponseDto ToDto(this CreateHotelResponse source)
        => new()
        {
            Id = source.HotelId,
            HotelCreated = source.HotelCreated,
            Message = source.Message
        };

    public static HotelResponseDto ToDto(this HotelResponse source)
        => new()
        {
            Id = source.Id,
            HotelRetrieved = source.HotelRetrieved,
            Message = source.Message
        };
}