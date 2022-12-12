using Application.DTO;
using FluentValidation;

namespace Application.Validators;

public class CreateEventValidator: AbstractValidator<PostEventDTO>
{
    public CreateEventValidator()
    {
        RuleFor(Event => Event.Title)
            .NotEmpty()
            .WithMessage("The event needs a title");

        RuleFor(Event => Event.OwnerId)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("Event must have an Event Owner");
    }
}