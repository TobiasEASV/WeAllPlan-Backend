using Application.DTO;
using FluentValidation;

namespace Application.Validators;

public class EventSlotValidator: AbstractValidator<EventSlotDTO>
{
    public EventSlotValidator()
    {
        RuleFor(eventSlotStart => eventSlotStart.StartTime)
            .GreaterThanOrEqualTo(DateTime.UtcNow.AddMinutes(15))
            .LessThanOrEqualTo(eventSlotEnd => eventSlotEnd.EndTime.AddMinutes(-15));
    }
}