using Microsoft.AspNetCore.Mvc;

namespace FileValidator.WebApi.Controllers;

[Route("api/info")]
[ApiController]
public class InfoController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var info = new 
        {
            Version = Helper.GetVersion()
        };

        return Ok(info);
    }
}
