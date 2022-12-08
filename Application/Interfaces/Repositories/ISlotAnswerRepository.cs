using Core;

namespace Application.Interfaces;

public interface ISlotAnswerRepository
{
    Task CreateSlotAnswer(List<SlotAnswer> slotAnswer);
    Task<List<SlotAnswer>> GetAll();
    Task<SlotAnswer> UpdateSlotAnswer(SlotAnswer slotAnswer);
    void DeleteSlotAnswers(List<SlotAnswer> listToDelete);
    Task<EventSlot> getEventSlot(int eventSlotId);
}