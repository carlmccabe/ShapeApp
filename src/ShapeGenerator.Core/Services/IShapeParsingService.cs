using ShapeGenerator.Core.Models;

namespace ShapeGenerator.Core.Services;

public interface IShapeParsingService
{
    /// <summary>
    ///  Service to parse a shape string into a shape object, from natural language.
    /// </summary>
    /// <param name="command"></param>
    /// <returns>ParseResult indicating success/failure and contains the parsed shape</returns>
    Task<ParseResult> ParseCommand(string command);
}