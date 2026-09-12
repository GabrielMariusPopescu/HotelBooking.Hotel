namespace Hotel.Persistence.Repositories;

public class HotelRepository(HotelDbContext context) : IHotelRepository
{
    public async Task<bool> Save(Domain.Models.Hotel hotel)
    {
        await context.Hotels.AddAsync(hotel); 
        var row = await context.SaveChangesAsync();
        return row > 0;
    }
}