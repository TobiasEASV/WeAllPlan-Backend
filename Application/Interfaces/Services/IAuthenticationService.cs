using Application.DTO;

namespace Core.Interfaces;

public interface IAuthenticationService
{
    public Task<string> Register(RegisterUserDto dto);

    public Task<LoggedInUserDto> Login(LoginUserDto dto);

}