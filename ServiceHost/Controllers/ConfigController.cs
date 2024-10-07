using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Controllers;

[ApiController]
[Route("api/config")]
public class ConfigController : ControllerBase
{
    [HttpGet("url")]
    public IActionResult GetBaseUrl()
    {
        var baseUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")?.Split(";").First();
        return Ok(new { url = baseUrl });
    }
}
