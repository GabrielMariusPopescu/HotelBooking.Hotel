namespace Hotel.Application.Queries;

public class GetAllActiveCountriesQueryHandler(IHotelRepository repository) : IRequestHandler<GetAllActiveCountriesQuery, IEnumerable<Country>>
{
    public async Task<IEnumerable<Country>> Handle(GetAllActiveCountriesQuery request, CancellationToken cancellationToken) 
        => await repository.GetCountries(cancellationToken);
}