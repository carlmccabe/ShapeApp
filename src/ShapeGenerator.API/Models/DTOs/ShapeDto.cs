using ShapeGenerator.Core.Models;

namespace ShapeGenerator.API.Models.DTOs;

/// <summary>
/// DTO for a shape object with its properties.
/// </summary>
public class ShapeDto
{

    /// <summary>
    /// Shape type. Example: "Circle"
    /// </summary>
    public string? Type { get; set; }
    
    /// <summary>
    /// Shape measurements. Example: {"radius": 100}.
    /// N.B. Only 2D shapes are supported. Will only use a max of 2 measurements for 2D shapes.
    /// </summary>
    public Dictionary<string, double>? Measurements { get; set; }
    
    /// <summary>
    /// Centre of the shape.
    /// </summary>
    public PointDto? Centre { get; set; }
    
    /// <summary>
    /// Coordinates of the shape.
    /// </summary>
    public List<PointDto>? Points { get; set; }

    /// <summary>
    /// Creates a new ShapeDto from a Shape object.
    /// </summary>
    /// <param name="shape"></param>
    /// <returns></returns>
    public static ShapeDto FromShape(Shape shape)
    {
        var dto = new ShapeDto
        {
            Type = shape.Type,
            Measurements = shape.Measurements,
            Points = shape.Points.Select(p => new PointDto(p.X, p.Y)).ToList()
        };
        
        if (shape.Centre != null)
            dto.Centre = new PointDto(shape.Centre.X, shape.Centre.Y);
        else
            dto.Centre = null;
        
        return dto;
    }
}