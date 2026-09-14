namespace Hotel.API.Controllers;

[ApiController]
[Route("api/countries")]
[Tags("Countries")]
public class CountriesController(ISender mediator) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CountryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCountries(CancellationToken cancellationToken)
    {
        var query = new GetAllActiveCountriesQuery();
        var countries = await mediator.Send(query, cancellationToken);

        return countries.Any() ? Ok(countries) : NotFound();
    }
}
