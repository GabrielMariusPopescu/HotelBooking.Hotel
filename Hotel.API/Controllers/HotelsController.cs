namespace Hotel.API.Controllers;

[ApiController]
[Route("api/hotels")]
[Tags("Hotels")]
public class HotelsController(ISender mediator) : ControllerBase
{
    [HttpPost("create")]
    [ProducesResponseType(typeof(CreateHotelResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<CreateHotelResponseDto> CreateHotel([FromBody] CreateHotelRequest request)
    {
        var command = new CreateHotelCommand(
        request.Id ?? Guid.Empty,
        request.Name,
        request.Street,
        request.City,
        request.ZipCode,
        request.Country);
        
        var response = await mediator.Send(command);

        return response.ToDto();

    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(HotelResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<HotelResponseDto> GetHotel([FromRoute] Guid id)
    {
        var query = new GetHotelDetailsQuery(id);
        var response = await mediator.Send(query);

        return response.ToDto();
    }
}