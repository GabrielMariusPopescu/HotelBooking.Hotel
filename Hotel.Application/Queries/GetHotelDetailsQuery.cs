namespace Hotel.Application.Queries;

public class GetHotelDetailsQuery(Guid id) : IRequest<HotelResponse<Domain.Models.Hotel>>
{
    public Guid Id { get; set; } = id;
}