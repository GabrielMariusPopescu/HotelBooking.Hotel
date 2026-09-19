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
        var response = await _client.PostAsJsonAsync("/api/hotels", request, TestContext.Current.CancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            throw new Exception($"API Failed with {response.StatusCode}. Details: {error}");
        }
        
        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var dto = await response.Content.ReadFromJsonAsync<HotelResponseDto<Domain.Models.Hotel>>(TestContext.Current.CancellationToken);
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

        await _client.PostAsJsonAsync("/api/hotels", request, TestContext.Current.CancellationToken);

        // Act
        var response = await _client.GetAsync($"/api/hotels/{id}", TestContext.Current.CancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            throw new Exception($"API Failed with {response.StatusCode}. Details: {error}");
        }

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var fetchedHotel = await response.Content.ReadFromJsonAsync<HotelResponseDto<Domain.Models.Hotel>>(TestContext.Current.CancellationToken);
        fetchedHotel.Should().NotBeNull();
        fetchedHotel.Id.Should().Be(id);
    }

    [Fact]
    public async Task GetHotel_WhenHotelDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/hotels/{id}", TestContext.Current.CancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            throw new Exception($"API Failed with {response.StatusCode}. Details: {error}");
        }

        // Assert
        response.IsSuccessStatusCode.Should().BeFalse();
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

#endregion
}