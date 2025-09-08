using ShapeGenerator.Core.Models;

namespace ShapeGenerator.Core.Services;

public class ShapeCalculationService : IShapeCalculationService
{
    public Shape CalculatePoints(Shape shape)
    {
        // Validation → Shape-specific calculation → Return updated shape
        return shape;
    }
}