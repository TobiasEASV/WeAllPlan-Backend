using Core;

namespace Application.Interfaces;

public interface IEventRepository
{
    public Task<Event> CreateEvent(Event testEvent);
    public Task<List<Event>> GetAll();
    public Task<Event> UpdateEvent(Event Event);
    public void Delete(Event Event);

    public User getUser(int userId);
    public Task<Event> GetEventById(int id);
    public Task<List<Event>> GetEventByUserId(int userId);
}