namespace Hotel.Application.Queries;

public class GetHotelDetailsQueryHandler(IHotelRepository repository) : IRequestHandler<GetHotelDetailsQuery, HotelResponse>
{
    public async Task<HotelResponse> Handle(GetHotelDetailsQuery request, CancellationToken cancellationToken)
    {
        var hotel = await repository.GetHotel(request.Id, cancellationToken);
        return hotel != null
            ? HotelResponse.Success(request.Id)
            : HotelResponse.Failure(request.Id, $"Retrieve hotel with '{request.Id}' identifier failed.");
    }
}