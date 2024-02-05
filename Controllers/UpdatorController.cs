using Microsoft.AspNetCore.Mvc;

[Route("api/v1/admin")]
[ApiController]
public class UpdatorController : ControllerBase
{
    private readonly IUpdatorService _updatorService;

    public UpdatorController(IUpdatorService updatorService)
    {
        _updatorService = updatorService;
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update()
    {
        await _updatorService.Update();
        return Ok();
    }
}
