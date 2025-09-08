using FluentAssertions;
using ShapeGenerator.Core.Models;

namespace ShapeGenerator.Core.Tests.Models;

public class PointTests
{
    [Fact]
    public void Point_WhenCreatedWithCoordinates_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var x = 10.5;
        var y = 20.7;

        // Act
        var point = new Point(x, y);
        
        // Assert
        point.X.Should().Be(x);
        point.Y.Should().Be(y);
    }
    
    [Theory]
    [InlineData(0, 0)]
    [InlineData(-10, -20)]
    [InlineData(100.5, 200.8)]
    [InlineData(double.MaxValue, double.MinValue)]
    public void Point_WhenCreatedWithVariousCoordinates_ShouldSetCorrectly(double x, double y)
    {
        // Act
        var point = new Point(x, y);
            
        // Assert
        point.X.Should().Be(x);
        point.Y.Should().Be(y);
    }

}