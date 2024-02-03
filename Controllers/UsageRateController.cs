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
        var usageRate = await apexTrackerService.GetUsageRates();
        if (usageRate is null)
        {
            return NotFound();
        }

        return Ok(usageRate);
    }
}
