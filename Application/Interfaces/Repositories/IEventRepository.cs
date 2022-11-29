using Core;

namespace Application.Interfaces;

public interface IEventRepository
{
    Task<Event> CreateEvent(Event testEvent);
    Task<List<Event>> GetAll();
    Task<Event> UpdateEvent(Event Event);
    void Delete(Event Event);

    User getUser(int userId);
}