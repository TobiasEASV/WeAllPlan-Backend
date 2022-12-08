using Application.DTO;

namespace Application.Interfaces;

public interface IEventService
{
    Task CreateEvent(CRUDEventDTO eventDto);
    Task<EventDTO> GetEvent(int id);
    Task<List<EventDTO>> GetEventsFromUser(int userId);
    Task UpdateEvent(EventDTO eventDto, int userId);
    void DeleteEvent(int eventId, int userId);
}