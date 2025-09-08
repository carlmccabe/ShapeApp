using Moq;
using ShapeGenerator.API.Controllers;
using ShapeGenerator.Core.Services;

namespace ShapeGenerator.API.Tests.Controllers;

public class ShapeControllerTests
{
    private readonly Mock<IShapeParsingService> _shapeParsingServiceMock = new();
    private readonly Mock<IShapeCalculationService> _shapeCalculationServiceMock = new();
    private readonly ShapeController _controller;

    public ShapeControllerTests()
    {
        _controller = new ShapeController(_shapeParsingServiceMock.Object, _shapeCalculationServiceMock.Object);
    }

    #region Successful Tests

    [Fact]
    public async Task ParseShape_WithValidCommand_ShouldReturnOkWithShapeData()
    {
        // Arrange
        var command = "Draw a circle with a radius of 100";
    }

    [Theory]
    [InlineData("Draw a circle with a radius of 100")]
    public async Task ParseShape_WithDifferentValidCommands_ShouldReturnOkWithShapeDataWithCorrectType(string command)
    {
        // Arrange
        // Act
        // Assert
    }

    #endregion

    #region Parsing Errors

    [Fact]
    public async Task ParseShape_WithInvalidCommand_ShouldReturnBadRequestWithError()
    {
        // Arrange
        // Act
        // Assert
    }

    [Fact]
    public async Task ParseShape_WithVariousInvalidCommand_ShouldReturnBadRequestWithError()
    {
        // Arrange
        // Act
        // Assert  
    }

    #endregion

    #region Request Validation Errors

    [Fact]
    public async Task ParseShape_WithNullCommand_ShouldReturnBadRequestWithError()
    {
        // Arrange
        // Act
        // Assert
    }

    [Fact]
    public async Task ParseShape_WithNullRequest_ShouldReturnBadRequestWithError()
    {
        // Arrange
        // Act
        // Assert   
    }

    #endregion

    #region Service Errors

    [Fact]
    public async Task ParseShape_WhenShapeCalculationServiceThrows_ShouldReturnInternalServerError()
    {
        // Arrange
        // Act
        // Assert
    }

    [Fact]
    public async Task ParseShape_WhenShapeParsingServiceThrows_ShouldReturnInternalServerError()
    {
        // Arrange
        // Act
        // Assert   
    }

    #endregion

    #region Response Formatting

    [Fact]
    public async Task ParseShape_WithSuccessfulParsing_ShouldReturnCompleteShapeData()
    {
        // Arrange
        // Act
        // Assert 
    }

    [Fact]
    public async Task ParseShape_WithCircle_ShouldReturnCenterInsteadOfPoints()
    {
        // Arrange
        // Act
        // Assert
    }

    #endregion

    #region HTTP Status Codes

    [Fact]
    public async Task ParseShape_WithSuccessfulParsing_ShouldReturnOk200()
    {
        // Arrange
        // Act
    }

    [Fact]
    public async Task ParseShape_WithParsingError_ShouldReturnBadRequest400()
    {
        // Arrange
    }

    #endregion
}