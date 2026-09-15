namespace Hotel.API.Tests;

public class HotelsIntegrationTests(HotelFactory factory) : IClassFixture<HotelFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    #region CreateHotel Tests

    [Fact]
    public async Task CreateHotel_WhenValidRequest_ReturnsSuccessAndDto()
    {
        // Arrange
        var id = "Integration Plaza".ToDeterministicGuid();
        const string uniqueName = "Integration Plaza";

        var request = new CreateHotelRequest
        {
            Id = id,
            Name = uniqueName,
            Street = "123 Test Ave",
            City = "Test ville",
            ZipCode = "AB1 BA1",
            Country = "United Kingdom"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/hotels/create", request, CancellationToken.None);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(CancellationToken.None);
            throw new Exception($"API Failed with {response.StatusCode}. Details: {error}");
        }
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();

        var dto = await response.Content.ReadFromJsonAsync<CreateHotelResponseDto>(CancellationToken.None);
        dto.Should().NotBeNull();
        dto.Id.Should().Be(id);
    }

#endregion

#region GetHotel Tests

    [Fact]
    public async Task GetHotel_WhenValidId_ReturnsOkAndHotel()
    {
        // Arrange
        var id = "Seeded Resort".ToDeterministicGuid();
        const string uniqueName = "Seeded Resort";

        var request = new CreateHotelRequest
        {
            Id = id,
            Name = uniqueName,
            Street = "456 Ocean Blvd",
            City = "Coast City",
            ZipCode = "54321",
            Country = "Spain"
        };

        await _client.PostAsJsonAsync("/api/hotels/create", request, CancellationToken.None);

        // Act
        var response = await _client.GetAsync($"/api/hotels/{id}", CancellationToken.None);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(CancellationToken.None);
            throw new Exception($"API Failed with {response.StatusCode}. Details: {error}");
        }

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var fetchedHotel = await response.Content.ReadFromJsonAsync<HotelResponseDto>(CancellationToken.None);
        fetchedHotel.Should().NotBeNull();
        fetchedHotel.Id.Should().Be(id);
    }

    [Fact]
    public async Task GetHotel_WhenHotelDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/hotels/{id}", CancellationToken.None);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(CancellationToken.None);
            throw new Exception($"API Failed with {response.StatusCode}. Details: {error}");
        }

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

#endregion
}