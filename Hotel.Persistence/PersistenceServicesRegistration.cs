namespace Hotel.Persistence;

public static class PersistenceServicesRegistration
{
    public static void RegisterRepositories(this IServiceCollection services) 
        => services.AddTransient<IHotelRepository, HotelRepository>();

    public static void RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<HotelDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var assemblyName = typeof(HotelDbContext).Assembly.FullName;
            options.UseNpgsql(connectionString, builder =>
            {
                builder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                builder.MigrationsAssembly(assemblyName);
            });
            options.ConfigureWarnings(warnings =>
            {
                warnings.Log(CoreEventId.ManyServiceProvidersCreatedWarning);
                warnings.Log(RelationalEventId.MultipleCollectionIncludeWarning);
            });
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });
    }
}