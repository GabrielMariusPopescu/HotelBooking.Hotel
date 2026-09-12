namespace Hotel.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HotelsController(ISender mediator) : ControllerBase
{
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<CreateHotelResponseDto> CreateHotel(CreateHotelRequest request)
    {
        var result = await mediator.Send(
            new CreateHotel(
                request.Id ?? Guid.Empty,
                request.Name,
                request.Street,
                request.City,
                request.ZipCode,
                request.CountryCode));

        return result.ToDto();
    }
}