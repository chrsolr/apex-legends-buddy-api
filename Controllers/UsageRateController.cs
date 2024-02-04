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
    public async Task<IActionResult> Get()
    {
        var usageRate = await apexTrackerService.GetLegendUsageRates();
        if (usageRate is null)
        {
            return NotFound();
        }

        return Ok(usageRate);
    }

    [HttpGet("usage-rate/{legendName}")]
    public async Task<IActionResult> GetByName(string legendName)
    {
        var usageRate = await apexTrackerService.GetLegendUsageRateByName(legendName);
        if (usageRate is null)
        {
            return NotFound();
        }

        return Ok(usageRate);
    }

    // TODO: Add Authorize and Authentication
    [HttpPost("usage-rate")]
    public async Task<IActionResult> Post()
    {
        await apexTrackerService.UpdateLegendUsageRates();
        return Ok();
    }
}
