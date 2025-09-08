using FluentAssertions;
using ShapeGenerator.Core.Models;
using ShapeGenerator.Core.Services;

namespace ShapeGenerator.Core.Tests.Services;

public class ShapeCalculationServiceTests
{
    private readonly ShapeCalculationService sut = new();

    #region Circle Tests

    [Fact]
    public void CalculatePoints_ForCircle_ShouldSetCentreAndRadiusOnly()
    {
        // Arrange
        var shape = new Shape("Circle", new Dictionary<string, double> { { "radius", 100 } });

        // Act
        var result = sut.CalculatePoints(shape);

        // Assert
        result.Centre.Should().NotBeNull();
        result.Centre!.X.Should().Be(100);
        result.Centre.Y.Should().Be(100);
        result.Points.Should().BeEmpty();
    }

    [Theory]
    [InlineData(25, 25, 25)]
    [InlineData(100, 100, 100)]
    [InlineData(100.5, 100.5, 100.5)]
    public void CalculatePoints_ForCircle_ShouldSetCorrectly(double radius, double expectedX, double expectedY)
    {
        // Arrange
        var shape = new Shape("Circle", new Dictionary<string, double> { { "radius", radius } });

        // Act
        var result = sut.CalculatePoints(shape);

        // Assert
        result.Centre.Should().NotBeNull();
        result.Centre!.X.Should().Be(expectedX);
        result.Centre.Y.Should().Be(expectedY);
        result.Points.Should().BeEmpty();
    }

    #endregion

    #region Square Tests

    [Fact]
    public void CalculatePoints_ForSquare_ShouldReturnFourPoints()
    {
        // Arrange
        var shape = new Shape("Square", new Dictionary<string, double> { { "side length", 100 } });

        // Act
        var result = sut.CalculatePoints(shape);

        // Assert
        result.Points.Should().NotBeEmpty();
        result.Points.Count.Should().Be(4);

        var points = result.Points.ToList();
        points[0].Should().BeEquivalentTo(new Point(0, 0));
        points[1].Should().BeEquivalentTo(new Point(100, 0));
        points[2].Should().BeEquivalentTo(new Point(100, 100));
        points[3].Should().BeEquivalentTo(new Point(0, 100));
    }

    [Fact]
    public void CalculatePoints_ForSquare_ShouldScaleCorrectly()
    {
        // Arrange
        var shape = new Shape("Square", new Dictionary<string, double> { { "side length", 200 } });

        // Act
        var result = sut.CalculatePoints(shape);

        // Assert
        result.Points.Should().NotBeEmpty();
        result.Points.Count.Should().Be(4);

        var points = result.Points.ToList();

        (points[1].X - points[0].X).Should().Be(200);
        (points[2].Y - points[0].Y).Should().Be(200);
    }

    #endregion

    #region Rectangle Tests

    [Fact]
    public void CalculatePoints_ForRectangle_ShouldReturn4PointsWithCorrectDimensions()
    {
        // Arrange
        var shape = new Shape("Rectangle", new Dictionary<string, double>
        {
            { "width", 150 },
            { "height", 100 }
        });

        // Act
        var result = sut.CalculatePoints(shape);

        // Assert
        result.Points.Should().HaveCount(4);
        var points = result.Points;

        points[0].Should().BeEquivalentTo(new Point(0, 0)); // Top-left
        points[1].Should().BeEquivalentTo(new Point(150, 0)); // Top-right
        points[2].Should().BeEquivalentTo(new Point(150, 100)); // Bottom-right
        points[3].Should().BeEquivalentTo(new Point(0, 100)); // Bottom-left
    }

    [Theory]
    [InlineData(200, 300)]
    [InlineData(50, 75)]
    [InlineData(100.5, 200.7)]
    public void CalculatePoints_ForRectangleWithDifferentDimensions_ShouldScaleCorrectly(double width, double height)
    {
        // Arrange
        var shape = new Shape("Rectangle", new Dictionary<string, double>
        {
            { "width", width },
            { "height", height }
        });

        // Act
        var result = sut.CalculatePoints(shape);

        // Assert
        var points = result.Points;
        (points[1].X - points[0].X).Should().Be(width); // Width check
        (points[3].Y - points[0].Y).Should().Be(height); // Height check
    }

    #endregion

    #region Triangle Tests

    [Fact]
    public void CalculatePoints_ForEquilateralTriangle_ShouldReturnThreePointsFormingEquilateralTriangle()
    {
        // Arrange
        var shape = new Shape("Equilateral Triangle", new Dictionary<string, double> { { "side length", 100 } });

        // Act
        var result = sut.CalculatePoints(shape);

        // Assert
        result.Points.Should().NotBeEmpty();
        result.Points.Should().HaveCount(3);

        // All sides should be equal
        var points = result.Points.ToList();
        var side1Length = Math.Sqrt(Math.Pow(points[1].X - points[0].X, 2) + Math.Pow(points[1].Y - points[0].Y, 2));
        var side2Length = Math.Sqrt(Math.Pow(points[2].X - points[1].X, 2) + Math.Pow(points[2].Y - points[1].Y, 2));
        var side3Length = Math.Sqrt(Math.Pow(points[0].X - points[2].X, 2) + Math.Pow(points[0].Y - points[2].Y, 2));

        side1Length.Should().Be(side2Length);
        side2Length.Should().BeApproximately(100, 0.01);
        side3Length.Should().BeApproximately(100, 0.01);
    }

    [Fact]
    public void CalculatePoints_ForIsoscelesTriangle_ShouldReturn3PointsWithCorrectDimensions()
    {
        // Arrange
        var shape = new Shape("Isosceles Triangle", new Dictionary<string, double>
        {
            { "height", 100 },
            { "width", 80 }
        });

        // Act
        var result = sut.CalculatePoints(shape);

        // Assert
        result.Points.Should().HaveCount(3);
        var points = result.Points;

        // Base should be 80 units wide, height should be 100
        var baseWidth = points[1].X;
        var height = points[2].Y;

        baseWidth.Should().Be(80);
        Math.Abs(height).Should().Be(100);
    }

    [Fact]
    public void CalculatePoints_ForScaleneTriangle_ShouldReturn3PointsWithSpecifiedSides()
    {
        // Arrange
        var shape = new Shape("Scalene Triangle", new Dictionary<string, double>
        {
            { "side1", 100 },
            { "side2", 150 }
        });

        // Act
        var result = sut.CalculatePoints(shape);

        // Assert
        result.Points.Should().HaveCount(3);
        // Note: Implementation will need to calculate third side and position points accordingly
    }

    #endregion

    #region Regular Polygon Tests

    [Theory]
    [InlineData("Pentagon", 5)]
    [InlineData("Hexagon", 6)]
    [InlineData("Heptagon", 7)]
    [InlineData("Octagon", 8)]
    public void CalculatePoints_ForRegularPolygon_ShouldReturnCorrectNumberOfPoints(string shapeType,
        int expectedPointCount)
    {
        // Arrange
        var shape = new Shape(shapeType, new Dictionary<string, double> { { "side length", 100 } });

        // Act
        var result = sut.CalculatePoints(shape);

        // Assert
        result.Points.Should().HaveCount(expectedPointCount);
    }

    [Fact]
    public void CalculatePoints_ForPentagon_ShouldFormRegularPentagon()
    {
        // Arrange
        var shape = new Shape("Pentagon", new Dictionary<string, double> { { "side length", 100 } });

        // Act
        var result = sut.CalculatePoints(shape);

        // Assert
        result.Points.Should().HaveCount(5);

        // Verify points are arranged in a circle (regular polygon property)
        var center = CalculatePolygonCenter(result.Points);
        var distances = result.Points.Select(p =>
            Math.Sqrt(Math.Pow(p.X - center.X, 2) + Math.Pow(p.Y - center.Y, 2))).ToList();

        // All points should be equidistant from center
        distances.Should().AllSatisfy(d => d.Should().BeApproximately(distances[0], 0.1));
    }

    [Fact]
    public void CalculatePoints_ForOctagon_ShouldFormRegularOctagon()
    {
        // Arrange
        var shape = new Shape("Octagon", new Dictionary<string, double> { { "side length", 50 } });

        // Act
        var result = sut.CalculatePoints(shape);

        // Assert
        result.Points.Should().HaveCount(8);

        // All sides should be approximately equal
        var sideLengths = new List<double>();
        for (int i = 0; i < result.Points.Count; i++)
        {
            var current = result.Points[i];
            var next = result.Points[(i + 1) % result.Points.Count];
            var sideLength = Math.Sqrt(Math.Pow(next.X - current.X, 2) + Math.Pow(next.Y - current.Y, 2));
            sideLengths.Add(sideLength);
        }

        sideLengths.Should().AllSatisfy(length => length.Should().BeApproximately(50, 1.0));
    }

    #endregion

    #region Special Shape Tests

    [Fact]
    public void CalculatePoints_ForOval_ShouldSetCenterAndNotUsePoints()
    {
        // Arrange
        var shape = new Shape("Oval", new Dictionary<string, double>
        {
            { "width", 200 },
            { "height", 100 }
        });

        // Act
        var result = sut.CalculatePoints(shape);

        // Assert
        result.Centre.Should().NotBeNull();
        result.Points.Should().BeEmpty(); // Ovals use center and dimensions, not point arrays
    }

    [Fact]
    public void CalculatePoints_ForParallelogram_ShouldReturn4PointsFormingParallelogram()
    {
        // Arrange
        var shape = new Shape("Parallelogram",
            new Dictionary<string, double> { { "side length", 100 }, { "height", 50 } });

        // Act
        var result = sut.CalculatePoints(shape);

        // Assert
        result.Points.Should().HaveCount(4);

        // Opposite sides should be parallel and equal
        var points = result.Points;
        var side1Vector = new { X = points[1].X - points[0].X, Y = points[1].Y - points[0].Y };
        var side3Vector = new { X = points[2].X - points[3].X, Y = points[2].Y - points[3].Y };

        // Opposite sides should be equal (parallel)
        side1Vector.X.Should().BeApproximately(side3Vector.X, 0.1);
        side1Vector.Y.Should().BeApproximately(side3Vector.Y, 0.1);
    }

    #endregion

    #region Error Handling Tests

    [Fact]
    public void CalculatePoints_WithNullShape_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Action act = () => sut.CalculatePoints(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CalculatePoints_WithUnsupportedShape_ShouldThrowNotSupportedException()
    {
        // Arrange
        var shape = new Shape("Dodecagon", new Dictionary<string, double> { { "side length", 100 } });

        // Act & Assert
        Action act = () => sut.CalculatePoints(shape);
        act.Should().Throw<NotSupportedException>()
            .WithMessage("Shape type 'Dodecagon' is not supported for coordinate calculation");
    }

    [Fact]
    public void CalculatePoints_WithMissingRequiredDimension_ShouldThrowArgumentException()
    {
        // Arrange - Circle without radius
        var shape = new Shape("Circle", new Dictionary<string, double>());

        // Act & Assert
        Action act = () => sut.CalculatePoints(shape);
        act.Should().Throw<ArgumentException>()
            .WithMessage("Required dimension 'radius' not found for shape 'Circle'");
    }

    #endregion

    #region Helper Methods

    private static Point CalculatePolygonCenter(List<Point> points)
    {
        var centerX = points.Average(p => p.X);
        var centerY = points.Average(p => p.Y);
        return new Point(centerX, centerY);
    }

    #endregion
}