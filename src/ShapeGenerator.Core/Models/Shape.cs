namespace ShapeGenerator.Core.Models;

public class Shape(string shapeType, Dictionary<string, double>? measurements)
{
    public string Type { get; set; } =
        shapeType is null
            ? throw new ArgumentNullException(nameof(shapeType))
            : string.IsNullOrWhiteSpace(shapeType)
                ? throw new ArgumentException("Shape type cannot be null or empty.", nameof(shapeType))
                : shapeType;
    public Dictionary<string, double> Measurements { get; set; } = 
        measurements ?? new Dictionary<string, double>();
    public List<Point> Points { get; private set; } = [];
    public Point? Centre { get; private set; }

    public void SetPoints(List<Point>? points)
    {
        Points = points ?? [];
    }

    public void SetCentre(Point centre)
    {
        Centre = centre;
    }
}