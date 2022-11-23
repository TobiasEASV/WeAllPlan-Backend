using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.DTO;
using Application.Helpers;
using Application.Interfaces;
using Core;
using Core.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Application;

public class AuthenticationService: IAuthenticationService
{

    private readonly IUserRepository _userRepository;
    private readonly AppSettings _appSettings;

    public AuthenticationService(IUserRepository userRepository, IOptions<AppSettings> appSettings)
    {
        if (appSettings is null)
        {
            throw new NullReferenceException("Can't create service object without secret file (appsetting).");
        }
        if (userRepository is null)
        {
            throw new NullReferenceException("Can't create service object with null repository.");
        }
        _appSettings = appSettings.Value;
        _userRepository = userRepository;
    }


    public string Register(RegisterUserDto dto)
    {
        if (dto.Email is null || dto.Name is null || dto.Password is null)
        {
            throw new NullReferenceException("null is an invalid input.");
        }
        
        try
        {
            _userRepository.GetUserByEmail(dto.Email);
        }
        catch (KeyNotFoundException e)
        {
            var salt = RandomNumberGenerator.GetBytes(32).ToString();
            var user = new User
            {
                Email = dto.Email,
                Salt = salt,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password + salt)
            };

            _userRepository.CreateNewUser(user);
            return "User successfully registered";
        }

        throw new Exception(dto.Email + " is already in use.");
    }

    public LoggedInUserDto Login(LoginUserDto dto)
    {
        throw new NotImplementedException();
    }

    public string GenerateToken(User user)
    {
        var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("Email", user.Email)}),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }
}