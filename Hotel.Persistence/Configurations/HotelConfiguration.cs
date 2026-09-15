namespace Hotel.Persistence.Configurations;

public class HotelConfiguration : IEntityTypeConfiguration<Domain.Models.Hotel>
{
    public void Configure(EntityTypeBuilder<Domain.Models.Hotel> builder)
    {
        builder.HasKey(hotel => hotel.Id);

        builder.Property(hotel => hotel.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.ComplexProperty(hotel => hotel.Address, addressBuilder =>
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
                .Property(address => address.Country)
                .HasColumnName("Address_Country")
                .HasMaxLength(50);
        });
    }
}