namespace Hotel.Application.Commands;

public class CreateHotelCommandHandler(IHotelRepository repository) : IRequestHandler<CreateHotelCommand, CreateHotelResponse>
{
    public async Task<CreateHotelResponse> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
    {
        var address = new Address(request.Street, request.City, request.ZipCode, request.CountryCode);
        var hotel = new Domain.Models.Hotel(request.Id, request.Name, address);

        var saved = await repository.Save(hotel, cancellationToken);
        return saved 
            ? CreateHotelResponse.Success(request.Id) 
            : CreateHotelResponse.Failure(request.Id, $"Create '{request.Name}' hotel failed."); 
    }
}