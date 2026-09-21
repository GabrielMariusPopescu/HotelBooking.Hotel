namespace Hotel.Application.Commands;

public class DeleteHotelCommand(Guid id) : IRequest<HotelResponse<Guid>>
{
    public Guid Id { get; } = id;
}