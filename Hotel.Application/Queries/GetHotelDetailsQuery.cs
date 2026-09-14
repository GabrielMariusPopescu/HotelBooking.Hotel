namespace Hotel.Application.Queries;

public class GetHotelDetailsQuery(Guid id) : IRequest<HotelResponse>
{
    public Guid Id { get; set; } = id;
}