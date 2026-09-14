namespace Hotel.Persistence.Configurations;

public class HotelConfiguration : IEntityTypeConfiguration<Domain.Models.Hotel>
{
    public void Configure(EntityTypeBuilder<Domain.Models.Hotel> builder)
    {
        builder.HasKey(hotel => hotel.Id);

        builder.Property(hotel => hotel.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.ComplexProperty(h => h.Address, addressBuilder =>
        {
            addressBuilder.IsRequired();
            
            addressBuilder
                .Property(address => address.Street)
                .HasColumnName("Address_Street")
                .HasMaxLength(250);
            
            addressBuilder
                .Property(address => address.City)
                .HasColumnName("Address_City")
                .HasMaxLength(100);
            
            addressBuilder
                .Property(address => address.ZipCode)
                .HasColumnName("Address_ZipCode")
                .HasMaxLength(20);
            
            addressBuilder
                .Property(address => address.CountryCode)
                .HasColumnName("Address_CountryCode")
                .HasMaxLength(2);
        });

        //List<Country> countries =
        //[
        //    new Country("England".ToDeterministicGuid(), "England", "EN"),
        //    new Country("Northern Ireland".ToDeterministicGuid(), "Northern Ireland", "NI"),
        //    new Country("Wales".ToDeterministicGuid(), "Wales", "WS"),
        //    new Country("Scotland".ToDeterministicGuid(), "Scotland", "SC"),
        //    new Country("Bulgaria".ToDeterministicGuid(), "Bulgaria", "BG"),
        //    new Country("France".ToDeterministicGuid(), "France", "FR"),
        //    new Country("Romania".ToDeterministicGuid(), "Romania", "RO"),
        //    new Country("Iceland".ToDeterministicGuid(), "Iceland", "IC"),
        //    new Country("Israel".ToDeterministicGuid(), "Israel", "IR"),
        //    new Country("Austria".ToDeterministicGuid(), "Austria", "AT")
        //];
        //builder.HasData(countries);
    }
}