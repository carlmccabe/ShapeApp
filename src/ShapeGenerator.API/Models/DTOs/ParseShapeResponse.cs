namespace ShapeGenerator.API.Models.DTOs;

/// <summary>
/// Response model for parsing a shape string into a shape object, from natural language.
/// </summary>
public class ParseShapeResponse
{
    /// <summary>
    /// Indicates whether the parsing was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Parsed shape object.
    /// </summary>
    public ShapeDto? Shape { get; set; }

    /// <summary>
    /// Error message if parsing failed.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Creates a successful response with the given shape.
    /// </summary>
    /// <param name="shape"></param>
    /// <returns></returns>
    public static ParseShapeResponse CreateSuccessResponse(ShapeDto shape)
    {
        return new ParseShapeResponse
        {
            Success = true,
            Shape = shape
        };
    }

    /// <summary>
    /// Creates a failiure response with the given error message.
    /// </summary>
    /// <param name="errorMessage"></param>
    public static ParseShapeResponse CreateFailureResponse(string errorMessage)
    {
        return new ParseShapeResponse
        {
            Success = false,
            ErrorMessage = errorMessage
        };
    }
}