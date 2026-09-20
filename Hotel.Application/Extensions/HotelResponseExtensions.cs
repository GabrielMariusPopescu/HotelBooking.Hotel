using Hotel.Application.Requests;

namespace Hotel.Application.Extensions;

public static class HotelResponseExtensions
{
    public static HotelResponseDto<Domain.Models.Hotel> ToDto(this HotelResponse<Domain.Models.Hotel> source)
        => new()
        {
            Id = source.Id,
            Message = source.Message,
            Data = source.Data,
            IsSuccessful = source.IsSuccessful
        };
    
    public static HotelResponseDto<IEnumerable<Domain.Models.Hotel>> ToDto(this HotelResponse<IEnumerable<Domain.Models.Hotel>> source)
        => new()
        {
            Data = source.Data,
            IsSuccessful = source.IsSuccessful,
            Message = source.Message
        };

    public static Domain.Models.Hotel ToHotel(this CreateHotelRequest request)
        => new(
            request.Name,
            new Address(
                request.Street,
                request.City,
                request.ZipCode,
                request.Country)
            );

    public static Domain.Models.Hotel ToHotel(this UpdateHotelRequest request)
        => new(
            request.Id,
            request.Name,
            new Address(
                request.Street, 
                request.City, 
                request.ZipCode, 
                request.Country)
            );
}