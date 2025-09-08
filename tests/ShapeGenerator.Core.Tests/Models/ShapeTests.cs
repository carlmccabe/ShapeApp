using FluentAssertions;
using ShapeGenerator.Core.Models;

namespace ShapeGenerator.Core.Tests.Models;

public class ShapeTests
{
    [Fact]
    public void Shape_WhenCreatedWithValidParameters_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var shapeType = "Circle";
        var measurements = new Dictionary<string, double> { { "radius", 100 } };

        // Act
        var shape = new Shape(shapeType, measurements);

        // Assert
        shape.Type.Should().Be(shapeType);
        shape.Measurements.Should().ContainKey("radius");
        shape.Measurements["radius"].Should().Be(100);
    }

    [Fact]
    public void Shape_WhenCreatedWithNullType_ShouldThrowArgumentNullException()
    {
        // Arrange
        string? shapeType = null;
        var measurements = new Dictionary<string, double>();

        // Act & Assert
        Action act = () => new Shape(shapeType!, measurements);
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'shapeType')");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Shape_WhenCreatedWithEmptyType_ShouldThrowArgumentException(string emptyType)
    {
        // Arrange
        var measurements = new Dictionary<string, double>();

        // Act & Assert
        Action act = () => new Shape(emptyType, measurements);
        act.Should().Throw<ArgumentException>()
            .WithMessage("Shape type cannot be null or empty*");
    }

    [Fact]
    public void Shape_WhenCreatedWithNullMeasurements_ShouldCreateEmptyMeasurements()
    {
        // Arrange
        var shapeType = "Circle";
        Dictionary<string, double>? measurements = null;

        // Act
        var shape = new Shape(shapeType, measurements!);

        // Assert
        shape.Measurements.Should().BeEmpty();
        shape.Measurements.Should().NotBeNull();
    }

    [Fact]
    public void Shape_WhenCreated_ShouldCreateEmptyPointsList()
    {
        // Arrange
        var shapeType = "Square";
        var measurements = new Dictionary<string, double> { { "side length", 100 } };

        // Act
        var shape = new Shape(shapeType, measurements);

        shape.Points.Should().NotBeNull();
        shape.Points.Should().BeEmpty();
    }

    [Fact]
    public void SetPoints_WhenCalledWithValidPoints_ShouldUpdatePointsList()
    {
        // Arrange
        var shape = new Shape("Square", new Dictionary<string, double>());
        var points = new List<Point>
        {
            new Point(0, 0),
            new Point(100, 0),
            new Point(100, 100),
            new Point(0, 100)
        };

        // Act
        shape.SetPoints(points);

        // Assert
        shape.Points.Should().HaveCount(4);
        shape.Points.Should().BeEquivalentTo(points);
    }

    [Fact]
    public void SetPoints_WhenCalledWithNull_ShouldSetEmptyList()
    {
        // Arrange
        var shape = new Shape("Circle", new Dictionary<string, double>());

        // Act
        shape.SetPoints(null!);

        // Assert
        shape.Points.Should().NotBeNull();
        shape.Points.Should().BeEmpty();
    }

    [Fact]
    public void SetCentre_WhenCalledWithValidPoint_ShouldUpdateCentre()
    {
        // Arrange
        var shape = new Shape("Circle", new Dictionary<string, double>());
        var centre = new Point(50, 50);
        
        // Act
        shape.SetCentre(centre);
        
        shape.Centre.Should().Be(centre);
        shape.Centre.Should().NotBeNull();
    }
}