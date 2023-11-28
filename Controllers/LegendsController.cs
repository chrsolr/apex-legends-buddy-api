using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace apex_legends_buddy_api.Controllers;

[ApiController]
[Route("api/v1/legends")]
public class LegendsController(ILogger<LegendsController> logger, GamepediaService gamepediaService) : ControllerBase
{
    private readonly ILogger<LegendsController> _logger = logger;
    private readonly GamepediaService _gamepediaService = gamepediaService;

    [HttpGet]
    public async Task<IActionResult> Get()
    {

        var legends = await _gamepediaService.GetLegends();

        if (legends is null)
        {
            return NotFound();
        }

        return Ok(legends);
    }
}