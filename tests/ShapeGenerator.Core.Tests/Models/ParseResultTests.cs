using FluentAssertions;
using ShapeGenerator.Core.Models;

namespace ShapeGenerator.Core.Tests.Models;

public class ParseResultTests
{
    [Fact]
    public void Success_WhenCalledWithValidShape_ShouldReturnSuccessResult()
    {
        // Arrange
        var shape = new Shape("Circle", new Dictionary<string, double> { { "radius", 100 } });

        // Act
        var result = ParseResult.Success(shape);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Shape.Should().Be(shape);
        result.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public void Success_WhenCalledWithNullShape_ShouldThrowArgumentNullException()
    {
        // Arrange
        Shape? nullShape = null;

        // Act & Assert
        Action act = () => ParseResult.Success(nullShape!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Failure_WhenCalledWithErrorMessage_ShouldReturnFailureResult()
    {
        // Arrange
        var errorMessage = "Invalid command format";

        // Act
        var result = ParseResult.Failure(errorMessage);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be(errorMessage);
        result.Shape.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Failure_WhenCalledWithInvalidErrorMessage_ShouldThrowArgumentException(string? errorMessage)
    {
        // Act & Assert
        Action act = () => ParseResult.Failure(errorMessage!);
        act.Should().Throw<ArgumentException>()
            .WithMessage("Error message cannot be null or empty*");
    }

    [Fact]
    public void ParseResult_WhenSuccessful_ShouldHaveCorrectState()
    {
        // Arrange
        var shape = new Shape("Square", new Dictionary<string, double> { { "side length", 50 } });

        // Act
        var result = ParseResult.Success(shape);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Shape.Should().NotBeNull();
        result.ErrorMessage.Should().BeNull();
    }

    [Fact]
    public void ParseResult_WhenFailed_ShouldHaveCorrectState()
    {
        // Arrange
        var errorMessage = "Shape not supported";

        // Act
        var result = ParseResult.Failure(errorMessage);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Shape.Should().BeNull();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
    }
}