using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShapeGenerator.API.Controllers;
using ShapeGenerator.API.Models.DTOs;
using ShapeGenerator.Core.Models;
using ShapeGenerator.Core.Services;

namespace ShapeGenerator.API.Tests.Controllers;

public class ShapeControllerTests
{
    private readonly Mock<IShapeParsingService> shapeParsingServiceMock = new();
    private readonly Mock<IShapeCalculationService> shapeCalculationServiceMock = new();
    private readonly ShapeController controller;

    public ShapeControllerTests()
    {
        controller = new ShapeController(shapeParsingServiceMock.Object, shapeCalculationServiceMock.Object);
    }

    #region Successful Tests

    [Fact]
    public async Task ParseShape_WithValidCommand_ShouldReturnOkWithShapeData()
    {
        // Arrange
        const string command = "Draw a circle with a radius of 100";

        var request = new ParseShapeRequest
        {
            Command = command
        };
        var parsedShape = new Shape("Circle", new Dictionary<string, double> { { "radius", 100 } });
        var calculatedShape = new Shape("Circle", new Dictionary<string, double> { { "radius", 100 } });
        calculatedShape.SetCentre(new Point(100, 100));

        shapeParsingServiceMock
            .Setup(parsingService => parsingService.ParseCommand(request.Command))
            .Returns(ParseResult.Success(parsedShape));

        shapeCalculationServiceMock
            .Setup(calculationService => calculationService.CalculatePoints(parsedShape))
            .Returns(calculatedShape);

        // Act
        var result = await controller.ParseShape(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var response = okResult!.Value as ParseShapeResponse;

        response.Should().NotBeNull();
        response.Success.Should().BeTrue();
        response.Shape.Should().NotBeNull();
        response.Shape!.Type.Should().NotBeNullOrEmpty();
        response.Shape.Measurements.Should().ContainKey("radius");
        response.Shape.Measurements["radius"].Should().Be(100);
        response.Shape.Centre.Should().NotBeNull();
        response.ErrorMessage.Should().BeNullOrEmpty();
    }

    [Theory]
    [InlineData("Draw a circle with a radius of 100", "Circle")]
    [InlineData("Draw a square with a side length of 200", "Square")]
    [InlineData("Draw a rectangle with a width of 250 and a height of 400", "Rectangle")]
    [InlineData("Draw an octagon with a side length of 200", "Octagon")]
    [InlineData("Draw an isosceles triangle with a height of 200 and a height of 100", "Isosceles Triangle")]
    public async Task ParseShape_WithDifferentValidCommands_ShouldReturnOkWithShapeDataWithCorrectType(string command,
        string shapeType)
    {
        // Arrange
        var request = new ParseShapeRequest { Command = command };
        var parsedShape = new Shape("TestShape", new Dictionary<string, double>());
        var calculatedShape = new Shape("TestShape", new Dictionary<string, double>());

        shapeParsingServiceMock
            .Setup(parsingService => parsingService.ParseCommand(request.Command))
            .Returns(ParseResult.Success(parsedShape));
        shapeCalculationServiceMock
            .Setup(calculationService => calculationService.CalculatePoints(parsedShape))
            .Returns(calculatedShape);

        // Act
        var result = await controller.ParseShape(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var response = okResult!.Value as ParseShapeResponse;
        response!.Success.Should().BeTrue();
        response.Shape!.Type.Should().Be(shapeType);
        response.ErrorMessage.Should().BeNullOrEmpty();
    }

    #endregion

    #region Parsing Errors

    [Fact]
    public async Task ParseShape_WithInvalidCommand_ShouldReturnBadRequestWithError()
    {
        // Arrange
        var request = new ParseShapeRequest { Command = "An Invalid Command" };
        var expectedErrorMessage = "Invalid command format";

        shapeParsingServiceMock
            .Setup(parsingService => parsingService.ParseCommand(request.Command))
            .Returns(ParseResult.Failure(expectedErrorMessage));

        // Act
        var result = await controller.ParseShape(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        var response = badRequestResult!.Value as ParseShapeResponse;

        response!.Success.Should().BeFalse();
        response.ErrorMessage.Should().Be(expectedErrorMessage);
        response.Shape.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData("Draw a star with a radius of 50")]
    [InlineData("Draw a heart with a size of 100")]
    [InlineData("Draw a circle with a radius of -100")]
    public async Task ParseShape_WithVariousInvalidCommand_ShouldReturnBadRequestWithError(string invalidCommand)
    {
        // Arrange
        var request = new ParseShapeRequest { Command = invalidCommand };

        shapeParsingServiceMock
            .Setup(parsingService => parsingService.ParseCommand(request.Command))
            .Returns(ParseResult.Failure("An Error depending on reason"));
        // Act
        var result = await controller.ParseShape(request);

        // Assert  
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        var response = badRequestResult!.Value as ParseShapeResponse;

        response!.Success.Should().BeFalse();
        response.ErrorMessage.Should().NotBeNullOrEmpty();
        response.Shape.Should().BeNull();
    }

    #endregion

    #region Request Validation Errors

    [Fact]
    public async Task ParseShape_WithNullCommand_ShouldReturnBadRequestWithError()
    {
        // Arrange
        var request = new ParseShapeRequest { Command = null! };

        // Act
        var result = await controller.ParseShape(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        var response = badRequestResult!.Value as ParseShapeResponse;

        response!.Success.Should().BeFalse();
        response.ErrorMessage.Should().NotBeNullOrEmpty();
        response.ErrorMessage.Should().Contain("Command cannot be null");
        response.Shape.Should().BeNull();
    }

    [Fact]
    public async Task ParseShape_WithNullRequest_ShouldReturnBadRequestWithError()
    {
        // Arrange
        // Request is null
        // Act
        var result = await controller.ParseShape(null!);

        // Assert   
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        var response = badRequestResult!.Value as ParseShapeResponse;

        response!.Success.Should().BeFalse();
        response.ErrorMessage.Should().NotBeNullOrEmpty();
        response.ErrorMessage.Should().Contain("Request cannot be null");
        response.Shape.Should().BeNull();
    }

    #endregion

    #region Service Errors

    [Fact]
    public async Task ParseShape_WhenShapeCalculationServiceThrows_ShouldReturnInternalServerError()
    {
        // Arrange
        var request = new ParseShapeRequest { Command = "Draw a circle with a radius of 100" };

        shapeParsingServiceMock
            .Setup(parsingService => parsingService.ParseCommand(It.IsAny<string>()))
            .Throws(new Exception("Unexpected parsing error"));

        // Act
        var result = await controller.ParseShape(request);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult!.StatusCode.Should().Be(500);

        var response = objectResult.Value as ParseShapeResponse;
        response!.Success.Should().BeFalse();
        response.ErrorMessage.Should().NotBeNullOrEmpty();
        response.ErrorMessage.Should().Contain("An error occurred while calculating the shape");
        response.Shape.Should().BeNull();
    }

    [Fact]
    public async Task ParseShape_WhenShapeParsingServiceThrows_ShouldReturnInternalServerError()
    {
        // Arrange
        var request = new ParseShapeRequest { Command = "Draw a circle with a radius of 100" };
        var parsedShape = new Shape("Circle", new Dictionary<string, double> { { "radius", 100 } });

        shapeParsingServiceMock
            .Setup(parsingService => parsingService.ParseCommand(request.Command))
            .Returns(ParseResult.Success(parsedShape));

        shapeCalculationServiceMock
            .Setup(calculationService => calculationService.CalculatePoints(It.IsAny<Shape>()))
            .Throws(new Exception("Unexpected calculation error"));

        // Act
        var result = await controller.ParseShape(request);

        // Assert   
        result.Should().BeOfType<ObjectResult>();
        var objectResult = result as ObjectResult;
        objectResult!.StatusCode.Should().Be(500);

        var response = objectResult.Value as ParseShapeResponse;
        response!.Success.Should().BeFalse();
        response.ErrorMessage.Should().NotBeNullOrEmpty();
        response.ErrorMessage.Should().Contain("An error occurred while parsing the shape");
        response.Shape.Should().BeNull();
    }

    #endregion

    #region Response Formatting

    [Fact]
    public async Task ParseShape_WithSuccessfulParsing_ShouldReturnCompleteShapeData()
    {
        // Arrange
        var request = new ParseShapeRequest { Command = "Draw a rectangle with a width of 200 and a height of 100" };
        var parsedShape = new Shape("Rectangle", new Dictionary<string, double>
        {
            { "width", 200 }, { "height", 100 }
        });
        var calculatedShape = new Shape("Rectangle", new Dictionary<string, double>
        {
            { "width", 200 }, { "height", 100 }
        });

        calculatedShape.SetPoints([
            new Point(0, 0),
            new Point(200, 0),
            new Point(200, 100),
            new Point(0, 100)
        ]);
        
        shapeParsingServiceMock
            .Setup(parsingService => parsingService.ParseCommand(request.Command))
            .Returns(ParseResult.Success(parsedShape));
        shapeCalculationServiceMock
            .Setup(calculationService => calculationService.CalculatePoints(parsedShape))
            .Returns(calculatedShape);
        
        // Act
        var result = await controller.ParseShape(request);
        
        // Assert 
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var response = okResult!.Value as ParseShapeResponse;
        response!.Success.Should().BeTrue();
        response.Shape!.Type.Should().Be("Rectangle");
        response.Shape.Measurements.Should().ContainKey("width");
        response.Shape.Measurements.Should().ContainKey("height");
        response.Shape.Measurements["width"].Should().Be(200);
        response.Shape.Measurements["height"].Should().Be(100);
        response.Shape.Centre.Should().NotBeNull();
        response.ErrorMessage.Should().BeNullOrEmpty();
        response.Shape.Points.Should().HaveCount(4);
    }

    [Fact]
    public async Task ParseShape_WithCircle_ShouldReturnCenterInsteadOfPoints()
    {
        // Arrange
        var request = new ParseShapeRequest { Command = "Draw a circle with a radius of 100" };
        var parsedShape = new Shape("Circle", new Dictionary<string, double> { { "radius", 100 } });
        var calculatedShape = new Shape("Circle", new Dictionary<string, double> { { "radius", 100 } });
        calculatedShape.SetCentre(new Point(100, 100));
        
        shapeParsingServiceMock
            .Setup(parsingService => parsingService.ParseCommand(request.Command))
            .Returns(ParseResult.Success(parsedShape));
        shapeCalculationServiceMock
            .Setup(calculationService => calculationService.CalculatePoints(parsedShape))
            .Returns(calculatedShape);
        
        // Act
        var result = await controller.ParseShape(request);
        
        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        var response = okResult!.Value as ParseShapeResponse;
        response!.Success.Should().BeTrue();
        response.Shape!.Type.Should().Be("Circle");
        response.Shape.Measurements.Should().ContainKey("radius");
        response.Shape.Measurements["radius"].Should().Be(100);
        response.Shape.Centre.Should().NotBeNull();
        response.ErrorMessage.Should().BeNullOrEmpty();
        response.Shape.Points.Should().BeEmpty();
    }

    #endregion

    #region HTTP Status Codes

    [Fact]
    public async Task ParseShape_WithSuccessfulParsing_ShouldReturnOk200()
    {
        // Arrange
        var request = new ParseShapeRequest { Command = "Draw a circle with a radius of 100" };
        var parsedShape = new Shape("Circle", new Dictionary<string, double> { { "radius", 100 } });
        var calculatedShape = new Shape("Circle", new Dictionary<string, double> { { "radius", 100 } });
        calculatedShape.SetCentre(new Point(100, 100));
        
        shapeParsingServiceMock
            .Setup(parsingService => parsingService.ParseCommand(request.Command))
            .Returns(ParseResult.Success(parsedShape));
        shapeCalculationServiceMock
            .Setup(calculationService => calculationService.CalculatePoints(parsedShape));
        
        // Act
        var result = await controller.ParseShape(request);
        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var okResult = result as OkObjectResult;
        okResult!.StatusCode.Should().Be(200);
    }

    [Fact]
    public async Task ParseShape_WithParsingError_ShouldReturnBadRequest400()
    {
        // Arrange
        var request = new ParseShapeRequest { Command = "An Invalid Command" };
        var expectedErrorMessage = "Invalid command format";
        
        shapeParsingServiceMock
            .Setup(parsingService => parsingService.ParseCommand(request.Command))
            .Returns(ParseResult.Failure(expectedErrorMessage));
        
        // Act
        var result = await controller.ParseShape(request);
        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult!.StatusCode.Should().Be(400);
    }

    #endregion
}