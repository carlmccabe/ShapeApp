using FluentAssertions;
using ShapeGenerator.Core.Services;

namespace ShapeGenerator.Core.Tests.Services;

public class ShapeParsingServiceTests
{
    private readonly ShapeParsingService _sut = new();

    #region Single Measurement Commands

    [Theory]
    [InlineData("Draw a circle with a radius of 100", "Circle", "radius", 100)]
    [InlineData("Draw a square with a side length of 250", "Square", "side length", 250)]
    [InlineData("Draw a octagon with a side length of 200", "Octagon", "side length", 200)]
    [InlineData("Draw a hexagon with a side length of 120", "Hexagon", "side length", 120)]
    [InlineData("Draw a heptagon with a side length of 90", "Heptagon", "side length", 90)]
    public async Task ParseCommand_WithSingleDimension_ShouldReturnSuccessResult(string command, string shapeType,
        string dimensionName, double dimensionValue)
    {
        // Act
        var result = await _sut.ParseCommand(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Shape.Should().NotBeNull();
        result.Shape.Type.Should().Be(shapeType);
        result.Shape.Measurements.Should().ContainKey(dimensionName);
        result.Shape.Measurements[dimensionName].Should().Be(dimensionValue);
        result.ErrorMessage.Should().BeNullOrEmpty();
    }

    [Theory]
    [InlineData("Draw an oval with a width of 200", "Oval", "width", 200)]
    [InlineData("Draw a parallelogram with a side length of 100", "Parallelogram", "side length", 100)]
    public async Task ParseCommand_WithSpecialShapes_ShouldReturnCorrectShape(
        string command, string expectedShape, string expectedMeasurement, double expectedValue)
    {
        // Act
        var result = await _sut.ParseCommand(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Shape.Type.Should().Be(expectedShape);
        result.Shape.Measurements[expectedMeasurement].Should().Be(expectedValue);
    }

    #endregion

    #region Multiple Measurement Commands

    [Theory]
    [InlineData("Draw a rectangle with a width of 250 and a height of 400", "Rectangle", "width", 250, "height", 400)]
    // [InlineData("Draw an isosceles triangle with a height of 200 and a width of 100", "Isosceles Triangle", "height",
    //     200, "width", 100)]
    // [InlineData("Draw a scalene triangle with a side1 of 100 and a side2 of 150", "Scalene Triangle", "side1", 100, "side2", 150)]
    [InlineData("Draw an oval with a width of 300 and a height of 200", "Oval", "width", 300, "height", 200)]
    public async Task ParseCommand_WithTwoMeasurements_ShouldReturnCorrectShape(
        string command, string expectedShape, string measurement1, double value1, string measurement2, double value2)
    {
        // Act
        var result = await _sut.ParseCommand(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Shape.Should().NotBeNull();
        result.Shape.Type.Should().Be(expectedShape);
        result.Shape.Measurements.Should().HaveCount(2);
        result.Shape.Measurements.Should().ContainKey(measurement1);
        result.Shape.Measurements.Should().ContainKey(measurement2);
        result.Shape.Measurements[measurement1].Should().Be(value1);
        result.Shape.Measurements[measurement2].Should().Be(value2);
    }

    // [Fact]
    // public void ParseCommand_WithEquilateralTriangle_ShouldHandleSingleMeasurement()
    // {
    //     // Arrange
    //     var command = "Draw an equilateral triangle with a side length of 100";
    //
    //     // Act
    //     var result = _await sut.ParseCommand(command);
    //
    //     // Assert
    //     result.IsSuccess.Should().BeTrue();
    //     result.Shape.Type.Should().Be("Equilateral Triangle");
    //     result.Shape.Dimensions.Should().ContainKey("side length");
    //     result.Shape.Dimensions["side length"].Should().Be(100);
    // }

    #endregion

    #region Invalid Input Handling

    [Fact]
    public async Task ParseCommand_WithNullInput_ShouldReturnFailure()
    {
        // Act
        var result = await _sut.ParseCommand(null!);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Command cannot be null or empty");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("hello world")]
    [InlineData("Draw something")]
    [InlineData("Draw a shape")]
    [InlineData("Make a circle")]
    public async Task ParseCommand_WithInvalidFormat_ShouldReturnFailure(string invalidCommand)
    {
        // Act
        var result = await _sut.ParseCommand(invalidCommand);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().NotBeNullOrEmpty();
        result.Shape.Should().BeNull();
    }

    [Theory]
    [InlineData("Draw a circle with a radius of -50")]
    [InlineData("Draw a square with a side length of -100")]
    [InlineData("Draw a rectangle with a width of 100 and a height of -50")]
    public async Task ParseCommand_WithNegativeMeasurement_ShouldReturnFailure(string command)
    {
        // Act
        var result = await _sut.ParseCommand(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("must be positive");
        result.Shape.Should().BeNull();
    }

    [Theory]
    [InlineData("Draw a circle with a radius of 0")]
    [InlineData("Draw a square with a side length of 0")]
    public async Task ParseCommand_WithZeroMeasurement_ShouldReturnFailure(string command)
    {
        // Act
        var result = await _sut.ParseCommand(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("must be positive");
    }

    [Theory]
    [InlineData("Draw a dodecagon with a side length of 100")]
    [InlineData("Draw a star with a radius of 50")]
    [InlineData("Draw a heart with a size of 100")]
    public async Task ParseCommand_WithUnsupportedShape_ShouldReturnFailure(string command)
    {
        // Act
        var result = await _sut.ParseCommand(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("not supported");
        result.Shape.Should().BeNull();
    }

    #endregion

    #region Edge Cases

    [Theory]
    [InlineData("Draw a Circle with a Radius of 100")] // Mixed case
    [InlineData("draw a circle with a radius of 100")] // Lowercase
    [InlineData("DRAW A CIRCLE WITH A RADIUS OF 100")] // Uppercase
    public async Task ParseCommand_WithDifferentCasing_ShouldBeHandledCorrectly(string command)
    {
        // Act
        var result = await _sut.ParseCommand(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Shape.Type.Should().Be("Circle");
    }

    [Theory]
    [InlineData("Draw a circle with a radius of 100.5")]
    [InlineData("Draw a rectangle with a width of 150.25 and a height of 200.75")]
    public async Task ParseCommand_WithDecimalMeasurements_ShouldParseCorrectly(string command)
    {
        // Act
        var result = await _sut.ParseCommand(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Shape.Should().NotBeNull();
    }

    [Theory]
    [InlineData("  Draw a circle with a radius of 100  ")] // Leading/trailing spaces
    [InlineData("Draw  a  circle  with  a  radius  of  100")] // Multiple spaces
    public async Task ParseCommand_WithExtraWhitespace_ShouldParseCorrectly(string command)
    {
        // Act
        var result = await _sut.ParseCommand(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Shape.Type.Should().Be("Circle");
    }

    [Theory]
    [InlineData("Draw a circle with a radius of abc")]
    [InlineData("Draw a square with a side length of xyz")]
    public async Task ParseCommand_WithNonNumericMeasurement_ShouldReturnFailure(string command)
    {
        // Act
        var result = await _sut.ParseCommand(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Contain("Invalid measurement value");
    }

    #endregion

    #region Required Shape Coverage

    [Fact]
    public async Task ParseCommand_ShouldSupportAllRequiredBasicShapes()
    {
        // Arrange
        var basicShapes = new[]
        {
            ("Draw a circle with a radius of 100", "Circle"),
            ("Draw a square with a side length of 100", "Square"),
            ("Draw a rectangle with a width of 100 and a height of 150", "Rectangle"),
            ("Draw an oval with a width of 100", "Oval")
        };

        foreach (var (command, expectedType) in basicShapes)
        {
            // Act
            var result = await _sut.ParseCommand(command);

            // Assert
            result.IsSuccess.Should().BeTrue($"Failed to parse: {command}");
            result.Shape.Type.Should().Be(expectedType);
        }
    }

    // [Fact]
    // public void ParseCommand_ShouldSupportAllRequiredTriangles()
    // {
    //     // Arrange
    //     var triangles = new[]
    //     {
    //         ("Draw an isosceles triangle with a height of 100 and a width of 50", "Isosceles Triangle"),
    //         ("Draw a scalene triangle with a side1 of 100 and a side2 of 150", "Scalene Triangle"),
    //         ("Draw an equilateral triangle with a side length of 100", "Equilateral Triangle")
    //     };
    //
    //     foreach (var (command, expectedType) in triangles)
    //     {
    //         // Act
    //         var result = _await sut.ParseCommand(command);
    //
    //         // Assert
    //         result.IsSuccess.Should().BeTrue($"Failed to parse: {command}");
    //         result.Shape!.Type.Should().Be(expectedType);
    //     }
    // }
    //
    // [Fact]
    // public void ParseCommand_ShouldSupportAllRequiredPolygons()
    // {
    //     // Arrange
    //     var polygons = new[]
    //     {
    //         ("Draw a pentagon with a side length of 100", "Pentagon"),
    //         ("Draw a hexagon with a side length of 100", "Hexagon"),
    //         ("Draw a heptagon with a side length of 100", "Heptagon"),
    //         ("Draw an octagon with a side length of 100", "Octagon")
    //     };
    //
    //     foreach (var (command, expectedType) in polygons)
    //     {
    //         // Act
    //         var result = _await sut.ParseCommand(command);
    //
    //         // Assert
    //         result.IsSuccess.Should().BeTrue($"Failed to parse: {command}");
    //         result.Shape!.Type.Should().Be(expectedType);
    //     }
    // }

    #endregion
}