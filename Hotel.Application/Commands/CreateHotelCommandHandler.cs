namespace Hotel.Application.Commands;

public class CreateHotelCommandHandler(IHotelRepository repository) : IRequestHandler<CreateHotel, CreateHotelResponse>
{
    public async Task<CreateHotelResponse> Handle(CreateHotel request, CancellationToken cancellationToken)
    {
        var address = new Address(request.Street, request.City, request.ZipCode, request.CountryCode);
        var hotel = new Domain.Models.Hotel(request.Name, address);

        var saved = await repository.Save(hotel);
        return saved 
            ? CreateHotelResponse.Success(request.Id) 
            : CreateHotelResponse.Failure(request.Id, $"Create '{request.Name}' hotel failed."); 
    }
}