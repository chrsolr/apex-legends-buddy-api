using Microsoft.AspNetCore.Mvc;

[Route("api/v1/gamepedia")]
[ApiController]
public class GamepediaController : ControllerBase
{
    private readonly ILogger<GamepediaController> logger;
    private readonly IGamepediaService gamepediaService;

    public GamepediaController(
        ILogger<GamepediaController> _logger,
        IGamepediaService _gamepediaService
    )
    {
        logger = _logger;
        gamepediaService = _gamepediaService;
    }

    [HttpPatch("legends")]
    public async Task<IActionResult> UpdateLegends()
    {
        await gamepediaService.UpdateLegends(null);
        return Ok();
    }

    [HttpPatch("legends/{legendName}")]
    public async Task<IActionResult> UpdateLegend(string legendName)
    {
        await gamepediaService.UpdateLegends(legendName);
        return Ok();
    }
}
