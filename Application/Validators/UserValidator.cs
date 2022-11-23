using Application.DTO;
using FluentValidation;

namespace Application.Validators;

public class UserValidator : AbstractValidator<RegisterUserDto>
{
    public UserValidator()
    {
        RuleFor(user => user.Password.Length)
            .GreaterThan(7)
            .WithMessage("invalid password, password must be greater than eight characters.");

        RuleFor(user => user.Name)
            .NotEmpty()
            .WithMessage("invalid name, name cannot be empty");
    }

}