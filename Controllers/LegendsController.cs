using Microsoft.AspNetCore.Mvc;

[Route("api/v1")]
[ApiController]
public class LegendsController : ControllerBase
{
    private readonly ILogger<LegendsController> logger;
    private readonly ILegendService legendService;
    private readonly IGamepediaService gamepediaService;

    public LegendsController(
        ILogger<LegendsController> _logger,
        ILegendService _legendService,
        IGamepediaService _gamepediaService
    )
    {
        logger = _logger;
        legendService = _legendService;
        gamepediaService = _gamepediaService;
    }

    [HttpGet("legends")]
    public async Task<IActionResult> Get()
    {
        var legends = await legendService.GetLegends();
        if (legends is null)
        {
            return NotFound();
        }

        return Ok(legends);
    }

    [HttpGet("legends/{legendName}")]
    public async Task<IActionResult> GetByName(string legendName)
    {
        var legend = await legendService.GetLegendsByName(legendName);
        if (legend is null)
        {
            return NotFound();
        }

        return Ok(legend);
    }

    [HttpPut("legends")]
    public async Task<IActionResult> UpdateLegends()
    {
        await gamepediaService.UpdateLegends(null);
        return Ok();
    }

    [HttpPut("legends/{legendName}")]
    public async Task<IActionResult> UpdateLegend(string legendName)
    {
        await gamepediaService.UpdateLegends(legendName);
        return Ok();
    }
}
