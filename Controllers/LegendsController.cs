using Microsoft.AspNetCore.Mvc;

[Route("api/v1")]
[ApiController]
public class LegendsController : ControllerBase
{
    private readonly ILogger<LegendsController> logger;
    private readonly ILegendService legendService;

    public LegendsController(ILogger<LegendsController> _logger, ILegendService _legendService)
    {
        logger = _logger;
        legendService = _legendService;
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
}
