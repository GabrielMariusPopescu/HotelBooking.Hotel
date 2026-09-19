namespace Hotel.Application.Responses;

public class HotelResponse<T>
{
    public Guid? Id { get; private set; }

    public bool IsSuccessful { get; private set; }

    public string Message { get; private set; }

    public T? Data { get; private set; }
    
    private HotelResponse(Guid? id, bool isSuccessful, string message, T? data)
    {
        Id = id;
        IsSuccessful = isSuccessful;
        Message = message;
        Data = data;
    }
    
    public static HotelResponse<T> Success(Guid? id, T? data) 
        => new(id, true, string.Empty, data);

    public static HotelResponse<T> Failure(Guid? id, string message)
        => new(id, false, message, default);
}