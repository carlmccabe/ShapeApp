using System.ComponentModel.DataAnnotations;

namespace ShapeGenerator.API.Models.DTOs;

/// <summary>
/// Request model for parsing a shape string into a shape object, from natural language.
/// </summary>
public class ParseShapeRequest
{
    /// <summary>
    /// The shape string to parse. Example: "Draw a circle with a radius of 100"
    /// </summary>
    [Required(ErrorMessage = "Command is required")]
    public string? Command { get; set; }
}