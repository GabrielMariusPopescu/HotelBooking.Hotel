namespace Hotel.Persistence;

public class HotelDbContext : DbContext
{
    public DbSet<Domain.Models.Hotel> Hotels { get; set; }

    public DbSet<Address> Addresses { get; set; }
}
