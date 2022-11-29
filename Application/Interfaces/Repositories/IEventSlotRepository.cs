using Core;

namespace Application.Interfaces;

public interface IEventSlotRepository
{
    void CreateEventSlot(List<EventSlot> validatedEventSlots);
    Task<List<EventSlot>> GetAll();
    Task<List<EventSlot>> UpdateEvent(List<EventSlot> eventSlots);
}