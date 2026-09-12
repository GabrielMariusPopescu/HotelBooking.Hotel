namespace Hotel.Application.Responses;

public class CreateHotelResponse
{
    public Guid HotelId { get; private set; }

    public bool HotelCreated { get; private set; }

    public string Message { get; private set; }

    private CreateHotelResponse(Guid hotelId, bool hotelCreated, string message)
    {
        HotelId = hotelId;
        HotelCreated = hotelCreated;
        Message = message;
    }

    public static CreateHotelResponse Success(Guid hotelId)
        => new(hotelId, true, string.Empty);

    public static CreateHotelResponse Failure(Guid hotelId, string message)
        => new(hotelId, false, message);
}