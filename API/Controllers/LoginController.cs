using Application;
using Application.DTO;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[Controller]")]
public class LoginController : ControllerBase
{
    private IAuthenticationService _authenticationService;

    public LoginController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    public async Task<ActionResult<LoggedInUserDto>> Login(LoginUserDto userDto)
    {
        try
        {
            return  Ok(await _authenticationService.Login(userDto));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    
}