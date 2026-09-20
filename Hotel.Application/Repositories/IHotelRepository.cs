namespace Hotel.Application.Repositories;

public interface IHotelRepository
{
    Task<bool> SaveHotel(Domain.Models.Hotel hotel, CancellationToken cancellationToken);

    Task<bool> UpdateHotel(Domain.Models.Hotel hotel, CancellationToken cancellationToken);

    Task<bool> SaveCountry(Country country, CancellationToken cancellationToken);
    
    Task<Domain.Models.Hotel?> GetHotel(Guid id, CancellationToken cancellationToken);
    
    Task<IEnumerable<Domain.Models.Hotel>> GetHotels(CancellationToken cancellationToken);

    Task<Country?> GetCountry(string name, CancellationToken cancellationToken);
}