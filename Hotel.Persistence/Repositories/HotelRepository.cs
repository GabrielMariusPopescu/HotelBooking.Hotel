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

    public async Task<bool> UpdateHotel(Domain.Models.Hotel hotel, CancellationToken cancellationToken)
    {
        context.Hotels.Update(hotel);
        var row = await context.SaveChangesAsync(cancellationToken);
        return row > 0;
    }

    public async Task<bool> DeleteHotel(Domain.Models.Hotel hotel, CancellationToken cancellationToken)
    {
        context.Hotels.Remove(hotel);
        var row = await context.SaveChangesAsync(cancellationToken);
        return row > 0;
    }

    public async Task<Domain.Models.Hotel?> GetHotel(Guid id, CancellationToken cancellationToken) 
        => await context.Hotels.FirstOrDefaultAsync(hotel => hotel.Id == id, cancellationToken);

    public async Task<IEnumerable<Domain.Models.Hotel>> GetHotels(CancellationToken cancellationToken)
        => await context.Hotels.ToListAsync(cancellationToken);

    public async Task<Country?> GetCountry(string name, CancellationToken cancellationToken) 
        => await context.Countries.FirstOrDefaultAsync(country => country.Name == name, cancellationToken);
}