namespace Hotel.Application.Commands;

public class DeleteHotelCommandHandler(IHotelRepository repository) : IRequestHandler<DeleteHotelCommand, HotelResponse<Guid>>
{
    public async Task<HotelResponse<Guid>> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
    {
        var hotel = await repository.GetHotel(request.Id, cancellationToken);
        if (hotel == null)
            return HotelResponse<Guid>.Failure(request.Id, $"Hotel with '{request.Id}' identifier could not be found.");

        var deleted = await repository.DeleteHotel(hotel, cancellationToken);
        return deleted
            ? HotelResponse<Guid>.Success(null, hotel.Id)
            : HotelResponse<Guid>.Failure(null, $"Hotel with '{request.Id}' identifier not updated.");
    }
}