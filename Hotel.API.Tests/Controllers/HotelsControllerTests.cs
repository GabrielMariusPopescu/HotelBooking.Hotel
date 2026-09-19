namespace Hotel.API.Tests.Controllers;

[ExcludeFromCodeCoverage]
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
    public async Task CreateHotel_WhenValidRequest_SendsCommandAndReturnsSuccess()
    {
        // Arrange
        CreateHotelRequest request = new()
        {
            Name = "Grand Plaza",
            Street = "123 Main Street",
            City = "Dallas",
            ZipCode = "12345",
            Country = "United States"
        };

        var response = HotelResponse<Domain.Models.Hotel>.Success(null, request.ToHotel());

        _mediatorMock
            .Setup(sender => sender.Send(It.IsAny<CreateHotelCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var actual = await _sut.CreateHotel(request);

        // Assert
        actual.Should().NotBeNull();
        var result = actual.Should().BeOfType<CreatedResult>().Subject;
        result.StatusCode.Should().Be(StatusCodes.Status201Created);
        
        // Verify
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
    public async Task CreateHotel_WhenInvalidRequest_SendsCommandAndReturnsFailure()
    {
        // Arrange
        CreateHotelRequest request = new()
        {
            Name = "Astoria", 
            Street = "123 Main Street", 
            City = "Belfast", 
            ZipCode = "BT4 1HH", 
            Country = "United Kingdom"
        };

        var response = HotelResponse<Domain.Models.Hotel>.Failure(null, $"Create '{request.Name}' hotel failed.");

        _mediatorMock
            .Setup(sender => sender.Send(It.IsAny<CreateHotelCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var actual = await _sut.CreateHotel(request);

        // Assert
        actual.Should().NotBeNull();
        var result = actual.Should().BeOfType<BadRequestResult>().Subject;
        result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);

        // Verify
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
    public async Task GetHotels_WhenExisting_SendsQueryAndReturnsSuccess()
    {
        // Arrange
        List<Domain.Models.Hotel> hotels = [new Domain.Models.Hotel()];
        var response = HotelResponse<IEnumerable<Domain.Models.Hotel>>.Success(null, hotels);

        _mediatorMock
            .Setup(sender => sender.Send(It.IsAny<GetHotelsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        
        // Act
        var actual = await _sut.GetHotels();
        
        // Assert
        actual.Should().NotBeNull();
        var result = actual.Should().BeOfType<OkObjectResult>().Subject;
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
    }

    [Fact]
    public async Task GetHotels_WhenNotExisting_SendQueryAndReturnsFailure()
    {
        // Arrange
        var response = HotelResponse<IEnumerable<Domain.Models.Hotel>>.Failure(null, "No hotels are found.");

        _mediatorMock
            .Setup(sender => sender.Send(It.IsAny<GetHotelsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
        
        // Act
        var actual = await _sut.GetHotels();

        // Assert
        actual.Should().NotBeNull();
        var result = actual.Should().BeOfType<NotFoundResult>().Subject;
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }
    
    [Fact]
    public async Task GetHotel_WhenValidId_SendsQueryAndReturnsSuccess()
    {
        // Arrange
        var hotelId = Guid.NewGuid();
        var response = HotelResponse<Domain.Models.Hotel>.Success(hotelId, default);

        _mediatorMock
            .Setup(sender => sender.Send(It.IsAny<GetHotelDetailsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var actual = await _sut.GetHotel(hotelId);

        // Assert
        actual.Should().NotBeNull();
        var result = actual.Should().BeOfType<OkObjectResult>().Subject;
        result.StatusCode.Should().Be(StatusCodes.Status200OK);

        // Verify
        _mediatorMock.Verify(sender => 
            sender.Send(It.Is<GetHotelDetailsQuery>(query => 
                query.Id == hotelId), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetHotel_WhenInvalidId_SendsQueryAndReturnsFailure()
    {
        // Arrange
        var id = Guid.NewGuid();
        var response = HotelResponse<Domain.Models.Hotel>.Failure(null, $"Hotel with '{id}' unique identifier could not be found.");

        _mediatorMock
            .Setup(sender => sender.Send(It.IsAny<GetHotelDetailsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var actual = await _sut.GetHotel(id);

        // Assert
        actual.Should().NotBeNull();
        var result = actual.Should().BeOfType<NotFoundResult>().Subject;
        result.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        // Verify
        _mediatorMock.Verify(sender =>
            sender.Send(It.Is<GetHotelDetailsQuery>(query =>
                query.Id == id), It.IsAny<CancellationToken>()), Times.Once);
    }
}