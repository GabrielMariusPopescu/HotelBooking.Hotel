namespace Hotel.Application.Queries;

public class GetHotelDetailsQueryHandler(IHotelRepository repository) : IRequestHandler<GetHotelDetailsQuery, HotelResponse<Domain.Models.Hotel>>
{
    public async Task<HotelResponse<Domain.Models.Hotel>> Handle(GetHotelDetailsQuery request, CancellationToken cancellationToken)
    {
        var hotel = await repository.GetHotel(request.Id, cancellationToken);
        return hotel != null
            ? HotelResponse<Domain.Models.Hotel>.Success(request.Id, hotel)
            : HotelResponse<Domain.Models.Hotel>.Failure(request.Id, $"Retrieve hotel with '{request.Id}' identifier failed.");
    }
}