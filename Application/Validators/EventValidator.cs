using FluentValidation;

namespace Application;

public class EventValidator : AbstractValidator<EventDTO>
{

    public EventValidator()
    {
        RuleFor(Event => Event.Title)
            .NotEmpty()
            .WithMessage("The event needs a title");

        RuleFor(Event => Event.UserId)
            .NotNull()
            .GreaterThan(0)
            .WithMessage("Event must have an Event Owner");

    }
}