using Microsoft.AspNetCore.Mvc;

[Route("api/v1")]
[ApiController]
public class LegendsController : ControllerBase
{
    private readonly IGamepediaService gamepediaService;

    public LegendsController(IGamepediaService _gamepediaService)
    {
        gamepediaService = _gamepediaService;
    }

    [HttpGet("legends")]
    public async Task<IActionResult> Get()
    {
        var legends = await gamepediaService.GetLegends();
        if (legends is null)
        {
            return NotFound();
        }

        return Ok(legends);
    }

    [HttpGet("legends/{legendName}")]
    public async Task<IActionResult> GetByName(string legendName)
    {
        var legend = await gamepediaService.GetLegendsByName(legendName);
        if (legend is null)
        {
            return NotFound();
        }

        return Ok(legend);
    }

    // TODO: Add Authorize and Authentication
    [HttpPost("legends")]
    public async Task<IActionResult> Post([FromQuery] string? legendName)
    {
        await gamepediaService.UpdateLegends(legendName);
        return Ok();
    }
}
