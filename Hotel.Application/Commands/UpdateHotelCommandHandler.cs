namespace Hotel.Application.Commands;

public class UpdateHotelCommandHandler(IHotelRepository repository) : IRequestHandler<UpdateHotelCommand, HotelResponse<Domain.Models.Hotel>>
{
    public async Task<HotelResponse<Domain.Models.Hotel>> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
    {
        var dbHotel = await repository.GetHotel(request.Id, cancellationToken);
        if (dbHotel == null)
            return HotelResponse<Domain.Models.Hotel>.Failure(request.Id, $"Hotel with '{request.Id}' identifier not found.");

        if (dbHotel.Address.Country != request.Country)
            return HotelResponse<Domain.Models.Hotel>.Failure(request.Id, $"Country for hotel with '{request.Id}' identifier cannot be updated.");

        dbHotel.Name = request.Name;
            dbHotel.Address = 
            new Address(
                request.Street, 
                request.City, 
                request.ZipCode, 
                request.Country);
            
        var updated = await repository.UpdateHotel(dbHotel, cancellationToken);
        return updated
            ? HotelResponse<Domain.Models.Hotel>.Success(request.Id, dbHotel)
            : HotelResponse<Domain.Models.Hotel>.Failure(request.Id, $"Hotel with '{dbHotel.Id}' identifier not updated.");
    }
}