using FluentValidation;

namespace Application;

public class EventValidator : AbstractValidator<EventDTO>
{

    public EventValidator()
    {
        RuleFor(Event => Event.Title)
            .NotEmpty()
            .WithMessage("The Event Needs a Title");
        
        RuleFor(Event => Event.Title)
            .NotNull()
            .WithMessage("The Event Needs a Title");

        RuleFor(Event => Event.User)
            .NotNull()
            .WithMessage("Event Must have a Event Owner");

    }
}