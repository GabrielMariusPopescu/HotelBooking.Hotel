namespace Hotel.API.Tests.Controllers;

public class HotelsControllerTests
{
    private readonly Mock<ISender> _mediatorMock;
    private readonly HotelsController _sut;

    public HotelsControllerTests()
    {
        _mediatorMock = new Mock<ISender>();
        _sut = new HotelsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task CreateHotel_WhenValidRequest_SendsCommandAndReturnsDto()
    {
        // Arrange
        var request = new CreateHotelRequest
        {
            Name = "Grand Plaza",
            Street = "123 Main Street",
            City = "Dallas",
            ZipCode = "12345",
            Country = "United States"
        };

        // We assume CreateHotelResponse has a Success factory method based on the handler
        var expectedResponse = HotelResponse<Domain.Models.Hotel>.Success(null, request.ToHotel());

        _mediatorMock
            .Setup(sender => sender.Send(It.IsAny<CreateHotelCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _sut.CreateHotel(request);

        // Assert
        result.Should().NotBeNull();

        // Verify the command was dispatched with the exact parameters from the request
        _mediatorMock.Verify(sender => 
            sender.Send(It.Is<CreateHotelCommand>(command =>
            command.Name == request.Name &&
            command.Street == request.Street &&
            command.City == request.City &&
            command.ZipCode == request.ZipCode &&
            command.Country == request.Country
        ), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetHotel_WhenValidId_SendsQueryAndReturnsDto()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var response = HotelResponse<Domain.Models.Hotel>.Success(hotelId, default);

        _mediatorMock
            .Setup(sender => sender.Send(It.IsAny<GetHotelDetailsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await _sut.GetHotel(hotelId);

        // Assert
        result.Should().NotBeNull();

        // Verify the query was dispatched with the correct ID
        _mediatorMock.Verify(sender => 
            sender.Send(It.Is<GetHotelDetailsQuery>(query => 
                query.Id == hotelId), It.IsAny<CancellationToken>()), Times.Once);
    }
}