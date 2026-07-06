using Microsoft.AspNetCore.Mvc;

namespace DocLint.WebAPI.Controllers;

[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet("/healthz")]
    public IActionResult Get()
    {
        return Ok("Healthy");
    }
}
