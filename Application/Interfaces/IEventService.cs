using Core;

namespace Application.Interfaces;

public interface IEventService
{
    EventDTO CreateEvent(EventDTO eventDto);
}