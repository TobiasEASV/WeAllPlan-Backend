using Application.DTO;

namespace Core.Interfaces;

public interface IAuthenticationService
{
    public string Register(RegisterUserDto dto);

    public LoggedInUserDto Login(LoginUserDto dto);
}