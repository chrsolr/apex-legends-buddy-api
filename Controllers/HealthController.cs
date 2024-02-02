using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/")]
public class HealthController : ControllerBase
{
    [HttpGet("health")]
    public ActionResult<dynamic> GetVersion()
    {
        var version = GetType().Assembly.GetName().Version;
        if (version == null)
        {
            return NotFound("Version not found");
        }

        var name = GetType().Assembly.GetName().Name;
        if (name is null)
        {
            return NotFound("Name not found");
        }

        return Ok(new { name = name.ToString(), version = version.ToString() });
    }
}
