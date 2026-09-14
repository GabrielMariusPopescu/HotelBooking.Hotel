namespace Hotel.Application.Responses;

public class HotelResponse
{
    public Guid Id { get; private set; }

    public bool HotelRetrieved { get; private set; }

    public string Message { get; private set; }

    private HotelResponse(Guid id, bool hotelRetrieved, string message)
    {
        Id = id;
        HotelRetrieved = hotelRetrieved;
        Message = message;
    }

    public static HotelResponse Success(Guid id)
        => new(id, true, string.Empty);

    public static HotelResponse Failure(Guid id, string message)
        => new(id, false, message);
}