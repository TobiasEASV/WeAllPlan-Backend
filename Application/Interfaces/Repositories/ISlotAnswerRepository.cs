using Core;

namespace Application.Interfaces;

public interface ISlotAnswerRepository
{
    Task<SlotAnswer> CreateSlotAnswer(SlotAnswer slotAnswer);
    Task<List<SlotAnswer>> GetAll();
    Task<SlotAnswer> UpdateSlotAnswer(SlotAnswer slotAnswer);
    void DeleteSlotAnswers(List<SlotAnswer> listToDelete);
    Task<EventSlot> getEventSlot(int eventSlotId);
}