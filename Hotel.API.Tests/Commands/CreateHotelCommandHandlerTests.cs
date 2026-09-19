namespace Hotel.API.Tests.Commands;

public class CreateHotelCommandHandlerTests
{
    private readonly Mock<IHotelRepository> _repositoryMock;
    private readonly CreateHotelCommandHandler _sut;

    public CreateHotelCommandHandlerTests()
    {
        _repositoryMock = new Mock<IHotelRepository>();
        _sut = new CreateHotelCommandHandler(_repositoryMock.Object);
    }

    #region Handle Tests

    [Fact]
    public async Task Handle_WhenCountryExistsAndHotelSaves_ReturnsSuccess()
    {
        // Arrange
        var command = CreateValidCommand();
        
        // Simulating that the database already has this country tracked
        var existingCountry = new Country(command.Country.ToDeterministicGuid(), command.Country);

        _repositoryMock
            .Setup(r => r.GetCountry(command.Country, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCountry);

        _repositoryMock
            .Setup(r => r.SaveHotel(It.IsAny<Hotel.Domain.Models.Hotel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();
        result.Id.Should().Be(command.Id);

        // Verify that SaveCountry was bypassed because it already exists
        _repositoryMock.Verify(r => r.SaveCountry(It.IsAny<Country>(), It.IsAny<CancellationToken>()), Times.Never);
        _repositoryMock.Verify(r => r.SaveHotel(It.IsAny<Hotel.Domain.Models.Hotel>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCountryDoesNotExistAndIsSuccessfullySaved_ReturnsSuccess()
    {
        // Arrange
        var command = CreateValidCommand();

        _repositoryMock
            .Setup(r => r.GetCountry(command.Country, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Country?)null);

        // Simulating successful creation of the new country
        _repositoryMock
            .Setup(r => r.SaveCountry(It.IsAny<Country>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _repositoryMock
            .Setup(r => r.SaveHotel(It.IsAny<Hotel.Domain.Models.Hotel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeTrue();

        // Verify both save operations occurred
        _repositoryMock.Verify(r => r.SaveCountry(It.IsAny<Country>(), It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.SaveHotel(It.IsAny<Hotel.Domain.Models.Hotel>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenSaveCountryFails_ReturnsFailureAndShortCircuits()
    {
        // Arrange
        var command = CreateValidCommand();

        _repositoryMock
            .Setup(r => r.GetCountry(command.Country, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Country?)null);

        // Simulate database failure when trying to save the new country
        _repositoryMock
            .Setup(r => r.SaveCountry(It.IsAny<Country>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeFalse();
        result.Message.Should().Contain(command.Country); // Validates the custom error message logic

        // Verify that Hotel persistence was never attempted due to the short-circuit
        _repositoryMock.Verify(r => r.SaveHotel(It.IsAny<Hotel.Domain.Models.Hotel>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenSaveHotelFails_ReturnsFailure()
    {
        // Arrange
        var command = CreateValidCommand();
        var existingCountry = new Country(command.Country.ToDeterministicGuid(), command.Country);

        _repositoryMock
            .Setup(r => r.GetCountry(command.Country, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCountry);

        // Simulate database failure when saving the hotel entity
        _repositoryMock
            .Setup(r => r.SaveHotel(It.IsAny<Hotel.Domain.Models.Hotel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccessful.Should().BeFalse();
        result.Message.Should().Contain(command.Name); // Validates the custom error message logic

        _repositoryMock.Verify(r => r.SaveHotel(It.IsAny<Hotel.Domain.Models.Hotel>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    #endregion

    #region Test Data Factories

    // Isolates the DTO creation to satisfy the Open/Closed Principle.
    private static CreateHotelCommand CreateValidCommand(Guid? id = null) 
        => new(id ?? Guid.NewGuid(),
            "Grand Plaza",
            "123 Main St",
            "Metropolis",
            "12345",
            "United States");

    #endregion
}