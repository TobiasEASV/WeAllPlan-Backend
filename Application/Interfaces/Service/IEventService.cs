using Application.DTO;

namespace Core.Interfaces;

public interface IEventService
{
    EventDto CreateEvent(EventDto eventDto);
}