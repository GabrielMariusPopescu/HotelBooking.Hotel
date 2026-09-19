namespace Hotel.Application.Queries;

public class GetHotelsQueryHandler(IHotelRepository repository) : IRequestHandler<GetHotelsQuery, HotelResponse<IEnumerable<Domain.Models.Hotel>>>
{
    public async Task<HotelResponse<IEnumerable<Domain.Models.Hotel>>> Handle(GetHotelsQuery request, CancellationToken cancellationToken)
    {
        var hotels = (await repository.GetHotels(cancellationToken)).ToList();
        return hotels.Any() 
            ? HotelResponse<IEnumerable<Domain.Models.Hotel>>.Success(null, hotels)
            : HotelResponse<IEnumerable<Domain.Models.Hotel>>.Failure(null, "No hotel was found.");
    }
}