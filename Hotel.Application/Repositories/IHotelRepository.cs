namespace Hotel.Application.Repositories;

public interface IHotelRepository
{
    Task<bool> Save(Domain.Models.Hotel hotel);
}