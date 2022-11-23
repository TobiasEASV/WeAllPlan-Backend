using Core;

namespace Application.Interfaces;

public interface IEventService
{
    Event CreateEvent(EventDTO eventDto);
}