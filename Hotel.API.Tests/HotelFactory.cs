namespace Hotel.API.Tests;

public class HotelFactory : WebApplicationFactory<ApiMaker>
{
    private SqliteConnection? _connection;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<HotelDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            services.AddDbContext<HotelDbContext>(options =>
            {
                options.UseSqlite(_connection);
            });

            using var scope = services.BuildServiceProvider().CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<HotelDbContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        });
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (!disposing)
            return;

        _connection?.Close();
        _connection?.Dispose();
    }
}