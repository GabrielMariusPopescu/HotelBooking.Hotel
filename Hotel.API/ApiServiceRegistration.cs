namespace Hotel.API;

public static class ApiServiceRegistration
{
    public static void RegisterMediator(this IServiceCollection services)
    {
        var assembly = typeof(CreateHotelCommand).Assembly;
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(assembly));
    }
}