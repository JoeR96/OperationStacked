namespace OperationStacked.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class VersionController : ControllerBase
{
    private readonly string _appVersion;


    public VersionController(IConfiguration configuration)
    {
        _appVersion = configuration["AppVersion"];
    }

    [HttpGet]
    public IActionResult GetVersion()
    {
        return Ok(_appVersion);
    }
}
