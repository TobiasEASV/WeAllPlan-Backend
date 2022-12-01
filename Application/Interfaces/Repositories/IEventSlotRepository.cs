using Core;

namespace Application.Interfaces;

public interface IEventSlotRepository
{
    void CreateEventSlot(List<EventSlot> validatedEventSlots);
    Task<List<EventSlot>> GetAll();
    
    void UpdateEventSlot(List<EventSlot> isAny);
    void DeleteEventSlot(List<EventSlot> isAny);
    Task<Event> getEventFromId(int eventId);
}