using Microsoft.AspNetCore.Mvc;

[Route("api/v1")]
[ApiController]
public class UsageRateController : ControllerBase
{
    private readonly IApexTrackerService apexTrackerService;

    public UsageRateController(IApexTrackerService _apexTrackerService)
    {
        apexTrackerService = _apexTrackerService;
    }

    [HttpGet("usage-rate")]
    public async Task<IActionResult> Get([FromQuery] string? legendName)
    {
        var usageRate = await apexTrackerService.GetUsageRates(legendName);
        if (usageRate is null)
        {
            return NotFound();
        }

        return Ok(usageRate);
    }
}
