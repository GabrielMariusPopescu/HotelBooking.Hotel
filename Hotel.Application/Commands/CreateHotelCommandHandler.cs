namespace Hotel.Application.Commands;

public class CreateHotelCommandHandler(IHotelRepository repository) : IRequestHandler<CreateHotelCommand, HotelResponse<Domain.Models.Hotel>>
{
    public async Task<HotelResponse<Domain.Models.Hotel>> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
    {
        var address = new Address(request.Street, request.City, request.ZipCode, request.Country);
        var country = new Country(request.Country.ToDeterministicGuid(), request.Country);
        bool saved;
        
        var dbCountry = await repository.GetCountry(request.Country, cancellationToken);
        if (dbCountry == null)
        {
            saved = await repository.SaveCountry(country, cancellationToken);
            if (!saved)
                return HotelResponse<Domain.Models.Hotel>.Failure(request.Id, $"Create '{request.Country}' country failed.");
        }

        var hotel = new Domain.Models.Hotel(request.Id, request.Name, address);
        saved = await repository.SaveHotel(hotel, cancellationToken);
        return saved 
            ? HotelResponse<Domain.Models.Hotel>.Success(request.Id, hotel) 
            : HotelResponse<Domain.Models.Hotel>.Failure(request.Id, $"Create '{request.Name}' hotel failed.");
    }
}