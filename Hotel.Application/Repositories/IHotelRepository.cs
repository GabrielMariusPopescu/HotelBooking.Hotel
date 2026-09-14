namespace Hotel.Application.Repositories;

public interface IHotelRepository
{
    Task<bool> Save(Domain.Models.Hotel hotel, CancellationToken cancellationToken);

    Task<Domain.Models.Hotel?> Get(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<Country>> GetCountries(CancellationToken cancellationToken);
}