using Core;

namespace Application.Interfaces;

public interface ISlotAnswerRepository
{
    public  Task CreateSlotAnswer(List<SlotAnswer> slotAnswer);
    public Task<List<SlotAnswer>> GetAll();
    public Task<SlotAnswer> UpdateSlotAnswer(SlotAnswer slotAnswer);
    public void DeleteSlotAnswers(List<SlotAnswer> listToDelete);
    public Task<EventSlot> getEventSlot(int eventSlotId);
}