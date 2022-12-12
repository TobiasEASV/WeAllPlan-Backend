using Application.DTO;

namespace Application.Interfaces;

public interface IEventService
{
    public Task CreateEvent(PostEventDTO eventDto);
    public Task<EventDTO> GetEvent(int id);
    public Task<List<EventDTO>> GetEventsFromUser(int userId);
    public Task UpdateEvent(EventDTO eventDto, int userId);
    public void DeleteEvent(int eventId, int userId);
}