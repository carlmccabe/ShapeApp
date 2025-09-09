using Microsoft.AspNetCore.Mvc;
using ShapeGenerator.API.Models.DTOs;
using ShapeGenerator.Core.Services;

namespace ShapeGenerator.API.Controllers;

public class ShapeController(IShapeParsingService shapeParsingService, IShapeCalculationService shapeCalculationService)
{
    public async Task<IActionResult> ParseShape(ParseShapeRequest request)
    {
        throw new NotImplementedException();
    }
}