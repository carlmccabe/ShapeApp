using System.Text.RegularExpressions;
using ShapeGenerator.Core.Models;

namespace ShapeGenerator.Core.Services;

public partial class ShapeParsingService : IShapeParsingService
{
    private readonly Regex shapePattern = ShapePattern();
    private readonly Regex singleMeasurementPattern = SingleMeasurementPattern();
    private readonly Regex dualMeasurementPattern = DualMeasurementPattern();

    private readonly Dictionary<string, string> supportedShapes = new(StringComparer.OrdinalIgnoreCase)
    {
        // Basic shapes
        { "circle", "Circle" },
        { "square", "Square" },
        { "rectangle", "Rectangle" },
        { "parallelogram", "Parallelogram" },
        { "oval", "Oval" },
            
        // Triangles
        { "isosceles triangle", "Isosceles Triangle" },
        { "scalene triangle", "Scalene Triangle" },
        { "equilateral triangle", "Equilateral Triangle" },
            
        // Polygons
        { "pentagon", "Pentagon" },
        { "hexagon", "Hexagon" },
        { "heptagon", "Heptagon" },
        { "octagon", "Octagon" }
        
    };

    public ParseResult ParseCommand(string command)
    {
        // Validate command
        if (string.IsNullOrWhiteSpace(command))
            return ParseResult.Failure("Command cannot be null or empty.");

        // Normalise command
        command = command.Trim();
        command = Regex.Replace(command, @"\s+", " ");
        command = command.ToLowerInvariant();

        // validate shape
        var shapeMatch = shapePattern.Match(command);
        if (!shapeMatch.Success)
            return ParseResult.Failure("Command must be in the format 'Draw a <shape> with a <measurement> of <value>'");

        var shapeTypeRequest = shapeMatch.Groups[1].Value.Trim().ToLowerInvariant();
        if (!supportedShapes.TryGetValue(shapeTypeRequest, out var shapeType))
            return ParseResult.Failure($"'{shapeTypeRequest}' is not supported shape.");

        var measurements = new Dictionary<string, double>();

        // try dual Measurement
        var dualMatch = dualMeasurementPattern.Match(command);
        if (dualMatch.Success)
        {
            var firstMeasurement = dualMatch.Groups[1].Value.Trim();
            var firstValueString = dualMatch.Groups[2].Value;
            var secondMeasurement = dualMatch.Groups[3].Value.Trim();
            var secondValueString = dualMatch.Groups[4].Value;

            // validate measurements
            if (!double.TryParse(firstValueString, out double firstValue))
                return ParseResult.Failure($"Invalid measurement value for {firstMeasurement}.");
            if (!double.TryParse(secondValueString, out double secondValue))
                return ParseResult.Failure($"Invalid measurement value for {secondMeasurement}.");

            if (firstValue <= 0)
                return ParseResult.Failure($"Value for {firstMeasurement} must be positive.");
            if (secondValue <= 0)
                return ParseResult.Failure($"Value for {secondMeasurement} must be positive.");

            // add measurements
            measurements.Add(firstMeasurement, firstValue);
            if (firstMeasurement != secondMeasurement)
                measurements.Add(secondMeasurement, secondValue);
        }
        else
        {
            // try single measurement
            var singleMatch = singleMeasurementPattern.Match(command);
            if (!singleMatch.Success)
                return ParseResult.Failure(
                    "Command must be in the format 'Draw a <shape> with a <measurement> of <value>'");
            var measurement = singleMatch.Groups[1].Value.Trim();
            var valueString = singleMatch.Groups[2].Value;

            // validate measurement
            if (!double.TryParse(valueString, out double value))
                return ParseResult.Failure($"Invalid measurement value for {measurement}.");

            if (value <= 0)
                return ParseResult.Failure($"Value for {measurement} must be positive.");

            // add measurement
            measurements.Add(measurement, value);
        }

        // Create shape
        var shape = new Shape(shapeType, measurements);

        // return result
        return ParseResult.Success(shape);
    }


    [GeneratedRegex(@"with\s+an?\s+(.+?)\s+of\s+(.+?)\s+and\s+an?\s+(.+?) of (.+?)$")]
    private static partial Regex DualMeasurementPattern();

    [GeneratedRegex(@"with\s+an?\s+(.+?)\s+of\s+(.+?)$")]
    private static partial Regex SingleMeasurementPattern();
    
    [GeneratedRegex(@"draw\s+an?\s+(.+?)\s+with", RegexOptions.IgnoreCase)]
    private static partial Regex ShapePattern();
}