using Application.DTO;

namespace Core.Interfaces;

public interface IAuthenticationService
{
    public Task<string> Register(RegisterUserDto dto);

    public Task<string> Login(LoginUserDto dto);

}