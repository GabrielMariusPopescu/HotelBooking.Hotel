namespace Hotel.API;

public static class ApiServiceRegistration
{
    public static void RegisterMediator(this IServiceCollection services)
    {
        var assembly = typeof(CreateHotelCommand).Assembly;
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(assembly));
    }
    public static void UseRequestLogging(this IApplicationBuilder app)
        => app.UseMiddleware<RequestLoggingMiddleware>();

    public static void RegisterSerilog(this WebApplicationBuilder builder)
    {
        var columnOptions = new Dictionary<string, ColumnWriterBase>()
        {
            { "message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
            { "message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
            { "level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
            { "time_stamp", new TimestampColumnWriter(NpgsqlDbType.TimestampTz) },
            { "exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
            { "properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) }
        };

        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext();
                //.WriteTo.Console(); 

            if (context.HostingEnvironment.IsDevelopment())
            {
                configuration.WriteTo.File(
                    path: "logs/hotel-api-log-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7
                );
            }
            else
            {
                var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

                configuration.WriteTo.PostgreSQL(
                    connectionString: connectionString,
                    tableName: "Logs",
                    columnOptions: columnOptions,
                    needAutoCreateTable: true
                );
            }
        });
    }
}