using Core;

namespace Application.Interfaces;

public interface IEventRepository
{
    Task<Event> CreateEvent(Event testEvent);
    Task<List<Event>> GetAll();
}