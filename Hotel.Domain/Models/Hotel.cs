namespace Hotel.Domain.Models;

public class Hotel(string name, Address address)
{
    public string Name { get; set; } = name;

    public Address Address { get; set; } = address;
}