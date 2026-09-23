namespace Hotel.API.Tests.Commands;

[ExcludeFromCodeCoverage]
public class UpdateHotelCommandHandlerTests
{
    private readonly Mock<IHotelRepository> _repositoryMock;
    private readonly UpdateHotelCommandHandler _sut;

    public UpdateHotelCommandHandlerTests()
    {
        _repositoryMock = new Mock<IHotelRepository>();
        _sut = new UpdateHotelCommandHandler(_repositoryMock.Object);
    }

    #region Handle Tests

    [Fact]
    public async Task Handle_WhenHotelExistsAndUpdateSucceeds_ReturnsSuccess()
    {
        // Arrange
        var command = CreateValidCommand();
        var existingHotel = CreateExistingHotel(command.Id);

        _repositoryMock
            .Setup(repository => repository.GetHotel(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingHotel);

        _repositoryMock
            .Setup(repository => repository.UpdateHotel(It.IsAny<Domain.Models.Hotel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var actual = await _sut.Handle(command, CancellationToken.None);

        // Assert
        actual.IsSuccessful.Should().BeTrue();
        actual.Id.Should().Be(command.Id);
        actual.Data.Should().NotBeNull();
        actual.Data!.Name.Should().Be(command.Name);
        actual.Data.Address.City.Should().Be(command.City);

        _repositoryMock.Verify(repository => repository.GetHotel(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(repository => repository.UpdateHotel(It.IsAny<Domain.Models.Hotel>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenHotelDoesNotExist_ReturnsFailureAndShortCircuits()
    {
        // Arrange
        var command = CreateValidCommand();

        _repositoryMock
            .Setup(repository => repository.GetHotel(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Models.Hotel?)null);

        // Act
        var actual = await _sut.Handle(command, CancellationToken.None);

        // Assert
        actual.IsSuccessful.Should().BeFalse();
        actual.Message.Should().Contain("not found");
        actual.Message.Should().Contain(command.Id.ToString());

        // Verify 
        _repositoryMock.Verify(repository => repository.GetHotel(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(repository => repository.UpdateHotel(It.IsAny<Domain.Models.Hotel>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenHotelCountryUpdateFails_ReturnsFailure()
    {
        // Arrange
        var command = CreateValidCommand();
        var existingHotel = CreateExistingHotel(command.Id);

        _repositoryMock
            .Setup(repository => repository.GetHotel(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingHotel);

        _repositoryMock
            .Setup(repository => repository.UpdateHotel(It.IsAny<Domain.Models.Hotel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var actual = await _sut.Handle(command, CancellationToken.None);

        // Assert
        actual.IsSuccessful.Should().BeFalse();
        actual.Message.Should().Contain("not updated");
        actual.Message.Should().Contain(command.Id.ToString());

        // Verify
        _repositoryMock.Verify(repository => repository.GetHotel(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(repository => repository.UpdateHotel(It.IsAny<Domain.Models.Hotel>(), It.IsAny<CancellationToken>()), Times.Once);

    }

    [Fact]
    public async Task Handle_WhenUpdateHotelFails_ReturnsFailure()
    {
        // Arrange
        var command = CreateValidCommand();
        var existingHotel = CreateExistingHotel(command.Id);

        _repositoryMock
            .Setup(repository => repository.GetHotel(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingHotel);

        existingHotel.Address = new Address("123 Old St", "New York", "12345", "Romania");

        _repositoryMock
            .Setup(repository => repository.UpdateHotel(It.IsAny<Domain.Models.Hotel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var actual = await _sut.Handle(command, CancellationToken.None);

        // Assert
        actual.IsSuccessful.Should().BeFalse();
        actual.Message.Should().Contain($"Country for hotel with '{existingHotel.Id}' identifier cannot be updated.");
        actual.Message.Should().Contain(command.Id.ToString());

        // Verify
        _repositoryMock.Verify(repository => repository.GetHotel(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(repository => repository.UpdateHotel(It.IsAny<Domain.Models.Hotel>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    #endregion

    #region Test Data Factories

    // Isolates the DTO/Command creation to satisfy the Open/Closed Principle.
    private static UpdateHotelCommand CreateValidCommand(Guid? id = null)
    {
        return new UpdateHotelCommand(
            id ?? Guid.NewGuid(),
            "Updated Grand Plaza",
            "456 New Street",
            "Metropolis",
            "54321",
            "US");
    }

    // Generates the initial state of the hotel before the update is applied.
    private static Domain.Models.Hotel CreateExistingHotel(Guid id)
    {
        return new Domain.Models.Hotel(
            id,
            "Old Grand Plaza",
            new Address("123 Old St", "New York", "12345", "US"));
    }

    #endregion
}