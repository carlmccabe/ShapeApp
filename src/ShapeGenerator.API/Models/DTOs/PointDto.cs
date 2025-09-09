namespace ShapeGenerator.API.Models.DTOs;

/// <summary>
/// Point DTO represents a point in 2D space.
/// </summary>
public class PointDto
{
    /// <summary>
    /// X coordinate.
    /// </summary>
    public double X { get; set; }

    /// <summary>
    /// Y coordinate.
    /// </summary>
    public double Y { get; set; }

    
    /// <summary>
    /// Assign Constructor.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public PointDto(double x, double y)
        => (X, Y) = (x, y);

    /// <summary>
    /// Default constructor.
    /// </summary>
    public PointDto()
    {
    }

}