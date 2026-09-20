namespace Hotel.Domain.Models;

public class Hotel
{
    public Hotel()
    {
        
    }
    
    public Hotel(Guid id, string name, Address address)
    {
        Id = id;
        Name = name;
        Address = address;
    }

    public Hotel(string name, Address address)
    {
        Name = name;
        Address = address;
    }

    public Guid Id { get; set; }

    public string Name { get; set; }

    public Address Address { get; set; }
}