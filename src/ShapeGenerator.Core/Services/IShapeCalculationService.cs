using ShapeGenerator.Core.Models;

namespace ShapeGenerator.Core.Services;

/// <summary>
/// Service for calculating coordinate points for shapes based on their dimensions
/// </summary>
public interface IShapeCalculationService
{
    /// <summary>
    /// Calculates the coordinate points for a shape and updates the shape object
    /// </summary>
    /// <param name="shape">Shape object with type and dimensions</param>
    /// <returns>The same shape object with calculated points and/or center coordinates</returns>
    /// <exception cref="ArgumentNullException">Thrown when shape is null</exception>
    /// <exception cref="NotSupportedException">Thrown when shape type is not supported</exception>
    /// <exception cref="ArgumentException">Thrown when required dimensions are missing</exception>
    Task<Shape> CalculatePointsAsync(Shape shape);
}