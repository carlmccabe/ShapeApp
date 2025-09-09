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
}