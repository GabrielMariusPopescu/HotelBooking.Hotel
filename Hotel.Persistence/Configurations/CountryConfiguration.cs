namespace Hotel.Persistence.Configurations;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(hotel => hotel.Id);

        builder.Property(hotel => hotel.Name)
            .IsRequired()
            .HasMaxLength(200);
    }
}