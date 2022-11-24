using FluentValidation;

namespace Application;

public class EventValidator : AbstractValidator<EventDTO>
{

    public EventValidator()
    {
        RuleFor(Event => Event.Title)
            .NotEmpty()
            .WithMessage("The event needs a title");

        RuleFor(Event => Event.User)
            .NotNull()
            .WithMessage("Event must have an Event Owner");

    }
}