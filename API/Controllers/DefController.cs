using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class DefController : ControllerBase
{
    [HttpGet]
    [Route("Route/Path/Name")]
    public IActionResult GetSomResult()
    {
        return Ok();
    }

    [HttpPost]
    [Route("Route/Path/Name/2/{id:int}/{name}")]
    public IActionResult CreateResult([FromBody] int id, string name)
    {
        return Ok();
    }
}