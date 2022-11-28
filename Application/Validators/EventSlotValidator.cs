using Application.DTO;
using FluentValidation;

namespace Application.Validators;

public class EventSlotValidator: AbstractValidator<EventSlotDTO>
{
    public EventSlotValidator()
    {
        
    }
}