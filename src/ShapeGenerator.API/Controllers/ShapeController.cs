using Microsoft.AspNetCore.Mvc;
using ShapeGenerator.API.Models.DTOs;
using ShapeGenerator.Core.Services;

namespace ShapeGenerator.API.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
public class ShapeController : ControllerBase
{
    private readonly IShapeParsingService _shapeParsingService;
    private readonly IShapeCalculationService _shapeCalculationService;
    private readonly ILogger<ShapeController> _logger;
    
    public ShapeController(IShapeParsingService shapeParsingService, IShapeCalculationService shapeCalculationService, ILogger<ShapeController> logger)
    {
        _shapeParsingService = shapeParsingService ?? throw new ArgumentNullException(nameof(shapeParsingService));
        _shapeCalculationService = shapeCalculationService ?? throw new ArgumentNullException(nameof(shapeCalculationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Parse a natural language shape description and return a shape object with the calculated measurements.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <response code="200">Returns the parsed shape object</response>
    /// <response code="400">Returns an error message</response>
    [HttpPost("parse")]
    [ProducesResponseType(typeof(ParseShapeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ParseShapeResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ParseShapeResponse), StatusCodes.Status500InternalServerError)] // For now, we'll just return a generic error message'
    public async Task<IActionResult> ParseShape([FromBody] ParseShapeRequest? request)
    {
        try
        {
            // validate request
            if (request == null)
                return BadRequest(ParseShapeResponse.CreateFailureResponse("Request cannot be null."));
            
            if (string.IsNullOrWhiteSpace(request.Command))
                return BadRequest(ParseShapeResponse.CreateFailureResponse("Command cannot be null or empty."));
            
            _logger.LogInformation("Parsing shape command: {Command}", request.Command);
            
            // Parse shape
            var parseResult = await _shapeParsingService.ParseCommand(request.Command);
            
            if (!parseResult.IsSuccess)
            {
                _logger.LogError("Failed to parse shape command: {ErrorMessage}", parseResult.ErrorMessage);
                return BadRequest(ParseShapeResponse.CreateFailureResponse(parseResult.ErrorMessage!));           
            }
            
            _logger.LogInformation("Successfully parsed shape command: {Command}", request.Command);
            
            // Calculate measurements
            var calculatedShape = await _shapeCalculationService.CalculatePointsAsync(parseResult.Shape);
            
            // Convert to response DTO
            var shapeDto = ShapeDto.FromShape(calculatedShape);
            var response = ParseShapeResponse.CreateSuccessResponse(shapeDto);
            
            return Ok(response);
        }
        catch (NotSupportedException ex)
        {
            _logger.LogError(ex, "An error occurred while parsing the shape.");
            return BadRequest(ParseShapeResponse.CreateFailureResponse("An error occurred while parsing the shape."));
        }
        catch (ArgumentException ex)
        {
            _logger.LogError(ex, "An error occurred while parsing the shape.");
            return BadRequest(ParseShapeResponse.CreateFailureResponse("An error occurred while parsing the shape."));       
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while parsing the shape.");
            return StatusCode(500, ParseShapeResponse.CreateFailureResponse("An unexpected error occurred while parsing the shape."));
        }
    }
}