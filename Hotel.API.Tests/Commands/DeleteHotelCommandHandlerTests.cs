namespace Hotel.API.Tests.Commands;

[ExcludeFromCodeCoverage]
public class DeleteHotelCommandHandlerTests
{
    private readonly Mock<IHotelRepository> _repositoryMock;
    private readonly DeleteHotelCommandHandler _sut;

    public DeleteHotelCommandHandlerTests()
    {
        _repositoryMock = new Mock<IHotelRepository>();
        _sut = new DeleteHotelCommandHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WhenHotelExistsAndDeleteSucceeds_ReturnsSuccessResponse()
    {
        // Arrange
        var command = new DeleteHotelCommand(Guid.NewGuid());
        var existingHotel = CreateValidHotel(command.Id);

        _repositoryMock
            .Setup(r => r.GetHotel(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingHotel);

        _repositoryMock
            .Setup(r => r.DeleteHotel(It.IsAny<Domain.Models.Hotel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var actual = await _sut.Handle(command, CancellationToken.None);

        // Assert
        actual.Data.Should().NotBeEmpty();
        actual.IsSuccessful.Should().BeTrue();
        actual.Data.Should().Be(existingHotel.Id);

        // Verify
        _repositoryMock.Verify(r => r.GetHotel(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.DeleteHotel(It.IsAny<Domain.Models.Hotel>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenHotelDoesNotExist_ReturnsFailureAndShortCircuits()
    {
        // Arrange
        var command = new DeleteHotelCommand(Guid.NewGuid());

        _repositoryMock
            .Setup(r => r.GetHotel(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Models.Hotel?)null);

        // Act
        var actual = await _sut.Handle(command, CancellationToken.None);

        // Assert
        actual.Should().NotBeNull();
        actual.IsSuccessful.Should().BeFalse();
        actual.Message.Should().Contain(command.Id.ToString());
        actual.Message.Should().Contain("could not be found");

        // Verify
        _repositoryMock.Verify(r => r.GetHotel(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.DeleteHotel(It.IsAny<Domain.Models.Hotel>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenDeleteHotelFails_ReturnsFailureResponse()
    {
        // Arrange
        var command = new DeleteHotelCommand(Guid.NewGuid());
        var existingHotel = CreateValidHotel(command.Id);

        _repositoryMock
            .Setup(r => r.GetHotel(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingHotel);

        _repositoryMock
            .Setup(r => r.DeleteHotel(It.IsAny<Domain.Models.Hotel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccessful.Should().BeFalse();
        result.Message.Should().Contain(command.Id.ToString());
        result.Message.Should().Contain("not updated");

        // Verify
        _repositoryMock.Verify(r => r.GetHotel(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.DeleteHotel(It.IsAny<Domain.Models.Hotel>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    // Encapsulates test data creation to strictly maintain the Single Responsibility Principle.
    private static Domain.Models.Hotel CreateValidHotel(Guid id)
    {
        return new Domain.Models.Hotel(
            id,
            "The Grand Plaza",
            new Address("123 Ocean Blvd", "Metropolis", "12345", "US"));
    }
}