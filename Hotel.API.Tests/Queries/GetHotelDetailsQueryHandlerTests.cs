namespace Hotel.API.Tests.Queries;

[ExcludeFromCodeCoverage]
public class GetHotelDetailsQueryHandlerTests
{
    private readonly Mock<IHotelRepository> _repositoryMock;
    private readonly GetHotelDetailsQueryHandler _sut;

    public GetHotelDetailsQueryHandlerTests()
    {
        _repositoryMock = new Mock<IHotelRepository>();
        _sut = new GetHotelDetailsQueryHandler(_repositoryMock.Object);
    }

    #region Handle Tests

    [Fact]
    public async Task Handle_WhenHotelExists_ReturnsSuccessResponse()
    {
        // Arrange
        var queryId = Guid.NewGuid();
        
        var query = new GetHotelDetailsQuery(queryId);
        var existingHotel = CreateValidHotel(queryId);

        _repositoryMock
            .Setup(r => r.GetHotel(queryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingHotel);

        // Act
        var actual = await _sut.Handle(query, CancellationToken.None);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccessful.Should().BeTrue();
        actual.Id.Should().Be(queryId);
        actual.Data.Should().BeEquivalentTo(existingHotel);

        // Verify
        _repositoryMock.Verify(r => r.GetHotel(queryId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenHotelDoesNotExist_ReturnsFailureResponse()
    {
        // Arrange
        var id = Guid.NewGuid();
        var query = new GetHotelDetailsQuery(id);

        _repositoryMock
            .Setup(r => r.GetHotel(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Models.Hotel?)null);

        // Act
        var result = await _sut.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();

        result.IsSuccessful.Should().BeFalse();
         result.Message.Should().Contain(id.ToString());
        result.Message.Should().Contain("could not be found");

        // Verify
        _repositoryMock.Verify(r => r.GetHotel(id, It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion

    #region Test Data Factories

    private static Domain.Models.Hotel CreateValidHotel(Guid id)
    {
        return new Domain.Models.Hotel(
            id,
            "The Grand Plaza",
            new Address("123 Ocean Blvd", "Metropolis", "12345", "US"));
    }

    #endregion
}