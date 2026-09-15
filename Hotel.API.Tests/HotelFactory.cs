namespace Hotel.API.Tests;

public class HotelFactory : WebApplicationFactory<ApiMaker>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<HotelDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<HotelDbContext>(options =>
            {
                options.UseInMemoryDatabase("HotelMicroserviceIntegrationTestDb");
            });
        });
    }
}