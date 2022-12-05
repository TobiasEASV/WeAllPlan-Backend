using Application;
using Application.DTO;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    public async Task<ActionResult<string>> Login(LoginUserDto userDto)
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
    

    [HttpPost]
    [Route("LoginWithGoogle")]
    public async Task<ActionResult<string>> LoginWithGoogle([FromBody]CredentialDTO dto)
    {
        try
        {
            return  Ok(await _authenticationService.LogInWithGoogle(dto.credential));
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }
    
    
}