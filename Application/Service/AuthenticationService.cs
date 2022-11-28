﻿using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Application.DTO;
using Application.Helpers;
using Application.Interfaces;
using Core;
using Core.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Application;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    private readonly AppSettings _appSettings;
    private readonly IValidator<RegisterUserDto> _userValidator;
    private readonly EmailValidator _emailValidator;

    public AuthenticationService(IUserRepository userRepository, IOptions<AppSettings> appSettings,
        IValidator<RegisterUserDto> userValidator)
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
        _userValidator = userValidator;
        _emailValidator = new EmailValidator();
    }


    public async Task<string> Register(RegisterUserDto dto)
    {
        if (dto.Email is null || dto.Name is null || dto.Password is null)
        {
            throw new NullReferenceException("null is an invalid input.");
        }

        ValidationResult Validation = _userValidator.Validate(dto);

        if (!Validation.IsValid)
        {
            throw new ValidationException(Validation.ToString());
        }

        if (!_emailValidator.IsValidEmail(dto.Email))
        {
            throw new ValidationException("invalid email, email must follow pattern John.doe@example.com");
        }

        try
        {
            await _userRepository.GetUserByEmail(dto.Email);
        }
        catch (KeyNotFoundException)
        {
            var salt = RandomNumberGenerator.GetBytes(32).ToString();
            var user = new User
            {
                Email = dto.Email,
                Name = dto.Name,
                Salt = salt,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password + salt)
            };

            await _userRepository.CreateNewUser(user);
            return "User successfully registered";
        }

        throw new Exception(dto.Email + " is already in use.");
    }

    public async Task<string> Login(LoginUserDto dto)
    {
        var user = await _userRepository.GetUserByEmail(dto.Email);
        if (BCrypt.Net.BCrypt.Verify(dto.Password + user.Salt, user.Password))
        {
            return GenerateToken(user);
        }

        throw new Exception("Invalid login");
    }

    public string GenerateToken(User user)
    {
        var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { 
                new Claim("Email", user.Email),
                new Claim("Id", user.Id.ToString()),
                new Claim("UserName", user.Name)
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }
}