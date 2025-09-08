namespace ShapeGenerator.Core.Models;

public class ParseResult(Shape shape, string? errorMessage = null)
{
    public bool IsSuccess { get; set; }
    public Shape Shape { get; set; } = shape;
    public string? ErrorMessage { get; set; } = errorMessage;
    
    
    public static ParseResult Success(Shape shape)
    {
        if (shape is null)
            throw new ArgumentNullException(nameof(shape));

        return new ParseResult(shape) { IsSuccess = true };
    }

    public static ParseResult Failure(string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(errorMessage))
            throw new ArgumentException("Error message cannot be null or empty", nameof(errorMessage));

        return new ParseResult(shape: null!, errorMessage);
    }
}