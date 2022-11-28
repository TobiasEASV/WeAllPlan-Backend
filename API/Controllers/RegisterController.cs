using Application;
using Application.DTO;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[Controller]")]
public class RegisterController : ControllerBase
{
    private IAuthenticationService _authenticationService;

    public RegisterController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    public async Task<ActionResult<string>> Register(RegisterUserDto registerUserDto)
    {
        try
        {
            return Ok( await _authenticationService.Register(registerUserDto));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
       
    }
    
}