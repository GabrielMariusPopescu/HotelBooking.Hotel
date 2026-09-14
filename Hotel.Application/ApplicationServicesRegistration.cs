namespace Hotel.Application;

public static class ApplicationServicesRegistration
{
    public static void RegisterValidators(this IServiceCollection services)
    {
        var assembly = typeof(CreateHotelCommandValidator).Assembly;
        services.AddValidatorsFromAssembly(assembly);

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
    }
}