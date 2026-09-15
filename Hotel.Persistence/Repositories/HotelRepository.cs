namespace Hotel.Persistence.Repositories;

public class HotelRepository(HotelDbContext context) : IHotelRepository
{
    public async Task<bool> SaveHotel(Domain.Models.Hotel hotel, CancellationToken cancellationToken)
    {
        await context.Hotels.AddAsync(hotel, cancellationToken); 
        var row = await context.SaveChangesAsync(cancellationToken);
        return row > 0;
    }

    public async Task<bool> SaveCountry(Country country, CancellationToken cancellationToken)
    {
        await context.Countries.AddAsync(country, cancellationToken);
        var row = await context.SaveChangesAsync(cancellationToken);
        return row > 0;
    }
    
    public async Task<Domain.Models.Hotel?> GetHotel(Guid id, CancellationToken cancellationToken)
    {
        var hotel = await context.Hotels.FindAsync(id, cancellationToken);
        return hotel ?? null;
    }

    public async Task<Country?> GetCountry(string name, CancellationToken cancellationToken)
    {
        var country = await context.Countries.FirstOrDefaultAsync(country => country.Name == name, cancellationToken);
        return country ?? null;
    }

    public async Task<IEnumerable<Country>> GetCountries(CancellationToken cancellationToken) 
        => await context
            .Countries
            .AsNoTracking()
            .Where(country => !country.Disabled)
            .OrderBy(country => country.Name)
            .ToListAsync(cancellationToken);
}