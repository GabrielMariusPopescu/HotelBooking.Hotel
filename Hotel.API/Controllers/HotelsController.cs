namespace Hotel.API.Controllers;

[ApiController]
[Route("api/hotels")]
[Tags("Hotels")]
public class HotelsController(ISender mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(HotelResponseDto<Domain.Models.Hotel>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateHotel([FromBody] CreateHotelRequest request)
    {
        var command = new CreateHotelCommand(
        request.Name,
        request.Street,
        request.City,
        request.ZipCode,
        request.Country);
        
        var response = await mediator.Send(command);
        return response.IsSuccessful
            ? Created("api/hotels", response.ToDto()) 
            : BadRequest();
    }

    [HttpGet]
    [ProducesResponseType(typeof(HotelResponseDto<IEnumerable<Domain.Models.Hotel>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotels()
    {
        var query = new GetHotelsQuery();
        var response = await mediator.Send(query);
        return response.IsSuccessful
            ? Ok(response.ToDto())
            : NotFound();
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(HotelResponseDto<Domain.Models.Hotel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotel([FromRoute] Guid id)
    {
        var query = new GetHotelDetailsQuery(id);
        var response = await mediator.Send(query);
        return response.IsSuccessful
            ? Ok(response.ToDto()) 
            : NotFound();
    }
}