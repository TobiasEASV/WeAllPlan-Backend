using Core;

namespace Application.Interfaces;

public interface IEventService
{
    Task<EventDTO> CreateEvent(EventDTO eventDto);
    Task<EventDTO> GetEvent(int id);
}