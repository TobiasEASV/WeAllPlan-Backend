using FluentValidation;

namespace Application;

public class SlotAnswerValidator: AbstractValidator<SlotAnswerDTO>
{
    public SlotAnswerValidator()
    {
        RuleFor(slotAnswer => slotAnswer.Answer)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(2)
            .WithMessage("Answer has to be no, maybe or yes");

        RuleFor(slotanswer => slotanswer.UserName)
            .NotEmpty()
            .WithMessage("Username cannot be empty");
    }
}