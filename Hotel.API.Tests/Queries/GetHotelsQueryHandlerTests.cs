namespace Hotel.API.Tests.Queries;

[ExcludeFromCodeCoverage]
public class GetHotelsQueryHandlerTests
{
    private readonly Mock<IHotelRepository> _repositoryMock;
    private readonly GetHotelsQueryHandler _sut;

    public GetHotelsQueryHandlerTests()
    {
        _repositoryMock = new Mock<IHotelRepository>();
        _sut = new GetHotelsQueryHandler(_repositoryMock.Object);
    }

    #region Handle Tests

    [Fact]
    public async Task Handle_WhenHotelsExist_ReturnsSuccessResponse()
    {
        // Arrange
        var query = new GetHotelsQuery();
        var expectedHotels = new List<Domain.Models.Hotel>
        {
            CreateValidHotel()
        };

        _repositoryMock
            .Setup(r => r.GetHotels(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedHotels);

        // Act
        var actual = await _sut.Handle(query, CancellationToken.None);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccessful.Should().BeTrue();
        actual.Data.Should().BeEquivalentTo(expectedHotels);

        // Verify
        _repositoryMock.Verify(r => r.GetHotels(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenNoHotelsExist_ReturnsFailureResponse()
    {
        // Arrange
        var query = new GetHotelsQuery();

        _repositoryMock
            .Setup(r => r.GetHotels(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Domain.Models.Hotel>()); // Simulates empty dataset[cite: 4]

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();

        result.IsSuccessful.Should().BeFalse();
         result.Message.Should().Be("No hotels found.");

         // Verify
        _repositoryMock.Verify(r => r.GetHotels(It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region Test Data Factories

    private static Domain.Models.Hotel CreateValidHotel()
    {
        return new Domain.Models.Hotel(
            Guid.NewGuid(),
            "The Grand Plaza",
            new Address("123 Ocean Blvd", "Metropolis", "12345", "US"));
    }

    #endregion
}