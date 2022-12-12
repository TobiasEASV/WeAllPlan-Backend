using Core;

namespace Application.Interfaces;

public interface IEventSlotRepository
{
    public void CreateEventSlot(List<EventSlot> validatedEventSlots);
    public Task<List<EventSlot>> GetAll();
    public void UpdateEventSlot(List<EventSlot> isAny);
    public void DeleteEventSlot(List<EventSlot> isAny);
    public Task<Event> getEventFromId(int eventId);
}