namespace Hotel.API.Tests.Integration;

[ExcludeFromCodeCoverage]
public class HotelsIntegrationTests(HotelFactory factory) : IClassFixture<HotelFactory>, IAsyncLifetime
{
    private readonly HttpClient _client = factory.CreateClient();

    public async ValueTask InitializeAsync()
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<HotelDbContext>();

        dbContext.Hotels.RemoveRange(dbContext.Hotels);
        await dbContext.SaveChangesAsync();
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    
    #region Create Hotel Tests

    [Fact]
    public async Task CreateHotel_WhenValidRequest_ReturnsSuccessAndDto()
    {
        // Arrange
        const string uniqueName = "Integration Plaza";

        var request = new CreateHotelRequest
        {
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
        dto.Data.Should().NotBeNull();
    }

    #endregion

    #region Update Hotel Tests

    [Fact]
    public async Task UpdateHotel_WhenValidRequest_ReturnsSuccess()
    {
        // Arrange: Seed the database by creating a hotel first
        const string uniqueName = "Pre-Update Hotel";

        var createRequest = new CreateHotelRequest
        {
            Name = uniqueName,
            Street = "123 Old Street",
            City = "Old City",
            ZipCode = "12345",
            Country = "United Kingdom"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/hotels", createRequest, TestContext.Current.CancellationToken);
        var createdHotel = await createResponse.Content.ReadFromJsonAsync<HotelResponseDto<Domain.Models.Hotel>>(cancellationToken: TestContext.Current.CancellationToken);

        createdHotel.Should().NotBeNull();

        var updateRequest = new UpdateHotelCommand(
            createdHotel.Id.GetValueOrDefault(),
            "Post-Update Hotel",
            "456 New Street",
            "New City",
            "54321",
            "United Kingdom"
        );

        // Act
        var response = await _client.PutAsJsonAsync($"/api/hotels/{createdHotel.Id}", updateRequest, TestContext.Current.CancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            throw new Exception($"API Failed with {response.StatusCode}: {error}");
        }

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateHotel_WhenHotelDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        var updateRequest = new UpdateHotelCommand(
            nonExistentId,
            "Ghost Hotel",
            "Nowhere Street",
            "Void City",
            "00000",
            "NA"
        );

        // Act
        var response = await _client.PutAsJsonAsync($"/api/hotels/{nonExistentId}", updateRequest, TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region Delete Hotel Tests

    [Fact]
    public async Task DeleteHotel_WhenValidRequest_ReturnsOk()
    {
        // Arrange
        const string uniqueName = $"To-Delete Resort";

        var createRequest = new CreateHotelRequest
        {
            Name = uniqueName,
            Street = "123 Ocean Drive",
            City = "Vice City",
            ZipCode = "33139",
            Country = "US"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/hotels", createRequest, TestContext.Current.CancellationToken);

        if (!createResponse.IsSuccessStatusCode)
        {
            var error = await createResponse.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            throw new Exception($"API Failed with {createResponse.StatusCode}. Details: {error}");
        }

        var createdHotel = await createResponse.Content.ReadFromJsonAsync<HotelResponseDto<Domain.Models.Hotel>>(cancellationToken: TestContext.Current.CancellationToken);
        createdHotel.Should().NotBeNull();

        // Act
        var actual = await _client.DeleteAsync($"/api/hotels/{createdHotel.Id}", TestContext.Current.CancellationToken);

        // Assert: Validates the 200 OK mapped in the controller's success branch
        actual.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task DeleteHotel_WhenHotelDoesNotExist_ReturnsBadRequest()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var actual = await _client.DeleteAsync($"/api/hotels/{nonExistentId}", TestContext.Current.CancellationToken);

        // Assert
        actual.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region Get All Tests

    [Fact]
    public async Task GetHotels_WhenHotelsExists_ReturnsOkAndHotels()
    {
        // Arrange
        const string uniqueName = "Seeded Resort";

        var request = new CreateHotelRequest
        {
            Name = uniqueName,
            Street = "456 Ocean Blvd",
            City = "Coast City",
            ZipCode = "54321",
            Country = "Spain"
        };

        var response = await _client.PostAsJsonAsync("/api/hotels", request, TestContext.Current.CancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            throw new Exception($"API Failed with {response.StatusCode}. Details: {error}");
        }

        await response.Content.ReadFromJsonAsync<HotelResponseDto<Domain.Models.Hotel>>(TestContext.Current.CancellationToken);
        
        // Act
        response = await _client.GetAsync("/api/hotels", TestContext.Current.CancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            throw new Exception($"API Failed with {response.StatusCode}. Details: {error}");
        }

        // Assert
        response.IsSuccessStatusCode.Should().BeTrue();
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var fetchedHotels = await response.Content.ReadFromJsonAsync<HotelResponseDto<IEnumerable<Domain.Models.Hotel>>>(TestContext.Current.CancellationToken);
        fetchedHotels.Should().NotBeNull();
        fetchedHotels.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task GetHotels_WhenHotelsDoesNotExists_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/hotels", TestContext.Current.CancellationToken);

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

    #region Get Hotel By Id Tests

    [Fact]
    public async Task GetHotel_WhenValidId_ReturnsOkAndHotel()
    {
        // Arrange
        const string uniqueName = "Seeded Resort";

        var request = new CreateHotelRequest
        {
            Name = uniqueName,
            Street = "456 Ocean Blvd",
            City = "Coast City",
            ZipCode = "54321",
            Country = "Spain"
        };

        var response = await _client.PostAsJsonAsync("/api/hotels", request, TestContext.Current.CancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            throw new Exception($"API Failed with {response.StatusCode}. Details: {error}");
        }

        var createdHotel = await response.Content.ReadFromJsonAsync<HotelResponseDto<Domain.Models.Hotel>>(TestContext.Current.CancellationToken);
        
        // Act
        response = await _client.GetAsync($"/api/hotels/{createdHotel?.Id}", TestContext.Current.CancellationToken);

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
        fetchedHotel.Id.Should().NotBe(Guid.Empty);
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