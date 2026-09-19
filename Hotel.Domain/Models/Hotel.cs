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

    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public Address Address { get; private set; }
}