using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace apex_legends_buddy_api.Controllers;

[ApiController]
[Route("api/v1/legends")]
public class LegendsController(IGamepediaService gamepediaService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var legends = await gamepediaService.GetLegends();

        if (legends is null)
        {
            return NotFound();
        }

        return Ok(legends);
    }
}