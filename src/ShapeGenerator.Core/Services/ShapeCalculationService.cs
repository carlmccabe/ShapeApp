using ShapeGenerator.Core.Models;

namespace ShapeGenerator.Core.Services;

public class ShapeCalculationService : IShapeCalculationService
{
    public Task<Shape> CalculatePointsAsync(Shape shape)
    {
        if (shape is null)
            throw new ArgumentNullException(nameof(shape));

        // Validation → Shape-specific calculation → Return updated shape
        return Task.FromResult(shape.Type switch
        {
            "Circle" => CalculateCircle(shape),
            "Square" => CalculateSquare(shape),
            "Rectangle" => CalculateRectangle(shape),
            "Equilateral Triangle" => CalculateEquilateralTriangle(shape),
            "Isosceles Triangle" => CalculateIsoscelesTriangle(shape),
            "Scalene Triangle" => CalculateScaleneTriangle(shape),
            "Pentagon" => CalculateRegularPolygon(shape, 5),
            "Hexagon" => CalculateRegularPolygon(shape, 6),
            "Heptagon" => CalculateRegularPolygon(shape, 7),
            "Octagon" => CalculateRegularPolygon(shape, 8),
            "Oval" => CalculateOval(shape),
            "Parallelogram" => CalculateParallelogram(shape),
            _ => throw new ArgumentException("Invalid shape type.", nameof(shape))
        });
    }

    private Shape CalculateCircle(Shape shape)
    {
        var radius = shape.Measurements["radius"];

        // Positon Circle so it's visually centered on the origin
        shape.SetCentre(new Point(radius, radius));

        return shape;
    }

    private Shape CalculateSquare(Shape shape)
    {
        var sideLength = shape.Measurements["side length"];

        var points = new List<Point>
        {
            new Point(0, 0), // bottom left
            new Point(sideLength, 0), // bottom right
            new Point(sideLength, sideLength), // Top right
            new Point(0, sideLength) // top left
        };

        shape.SetPoints(points);
        return shape;
    }

    private Shape CalculateRectangle(Shape shape)
    {
        var width = shape.Measurements["width"];
        var height = shape.Measurements["height"];

        var points = new List<Point>
        {
            new Point(0, 0), // Top left
            new Point(width, 0), // Top right
            new Point(width, height), // Bottom right
            new Point(0, height) // Bottom left
        };

        shape.SetPoints(points);
        return shape;
    }

    private Shape CalculateEquilateralTriangle(Shape shape)
    {
        var sideLength = shape.Measurements["side length"];

        var height = Math.Sqrt(3) * sideLength / 2;

        var points = new List<Point>
        {
            new Point(0, 0),
            new Point(sideLength, 0),
            new Point(sideLength / 2, height),
        };

        shape.SetPoints(points);
        return shape;
    }

    private Shape CalculateIsoscelesTriangle(Shape shape)
    {
        var height = shape.Measurements["height"];
        var width = shape.Measurements["width"];

        var points = new List<Point>
        {
            new Point(0, 0),
            new Point(width, 0),
            new Point(width / 2, height) // Bottom right
        };

        shape.SetPoints(points);
        return shape;
    }

    private Shape CalculateScaleneTriangle(Shape shape)
    {
        var side1 = shape.Measurements["side1"];
        var side2 = shape.Measurements["side2"];

        var points = new List<Point>
        {
            new Point(0, 0),
            new Point(side1, 0),
            new Point(side2 / 2, Math.Sqrt(Math.Pow(side2, 2) - Math.Pow(side2 / 2, 2)))
        };

        shape.SetPoints(points);
        return shape;
    }

    private Shape CalculateRegularPolygon(Shape shape, int numberOfSides)
    {
        var sideLength = shape.Measurements["side length"];

        // Calculate circumradius for regular polygon
        // R = side / (2 * sin(PI / n))
        var circumradius = sideLength / (2 * Math.Sin(Math.PI / numberOfSides));

        // Center the polygon
        var centerX = circumradius;
        var centerY = circumradius;

        var points = new List<Point>();
        var angleStep = 2 * Math.PI / numberOfSides;

        // Start from the top (angle = -PI/2) and go clockwise
        var startAngle = -Math.PI / 2;

        for (int i = 0; i < numberOfSides; i++)
        {
            var angle = startAngle + (i * angleStep);
            var x = centerX + circumradius * Math.Cos(angle);
            var y = centerY + circumradius * Math.Sin(angle);
            points.Add(new Point(x, y));
        }

        shape.SetPoints(points);
        return shape;
    }

    private Shape CalculateOval(Shape shape)
    {
        var width = shape.Measurements["width"];
        var height = shape.Measurements["height"];

        shape.SetCentre(new Point(width / 2, height / 2));
        return shape;
    }

    private Shape CalculateParallelogram(Shape shape)
    {
        var sideLength = shape.Measurements["side length"];
        var height = shape.Measurements["height"];

        var offset = sideLength * Math.Sin(Math.PI / 4); // 45 degrees

        var points = new List<Point>
        {
            new Point(0, 0),
            new Point(sideLength, 0),
            new Point(sideLength - offset, height),
            new Point(offset, height)
        };

        shape.SetPoints(points);
        return shape;
    }
}