namespace Hotel.Persistence.Repositories;

public class HotelRepository(HotelDbContext context) : IHotelRepository
{
    public async Task<bool> Save(Domain.Models.Hotel hotel, CancellationToken cancellationToken)
    {
        await context.Hotels.AddAsync(hotel, cancellationToken); 
        var row = await context.SaveChangesAsync(cancellationToken);
        return row > 0;
    }

    public async Task<Domain.Models.Hotel?> Get(Guid id, CancellationToken cancellationToken)
    {
        var hotel = await context.Hotels.FindAsync(id, cancellationToken);
        return hotel ?? null;
    }

    public async Task<IEnumerable<Country>> GetCountries(CancellationToken cancellationToken) 
        => await context
            .Countries
            .AsNoTracking()
            .Where(country => !country.Disabled)
            .OrderBy(country => country.Name)
            .ToListAsync(cancellationToken);
}