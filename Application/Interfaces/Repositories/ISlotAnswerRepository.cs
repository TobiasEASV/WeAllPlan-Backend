using Core;

namespace Application.Interfaces;

public interface ISlotAnswerRepository
{
    Task<SlotAnswer> CreateSlotAnswer(SlotAnswer slotAnswer);
}