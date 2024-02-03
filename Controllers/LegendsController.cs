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
    public async Task<IActionResult> Get([FromQuery] string? legendName)
    {
        var legends = await gamepediaService.GetLegends(legendName);
        if (legends is null)
        {
            return NotFound();
        }

        return Ok(legends);
    }
}
