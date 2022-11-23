using Core;

namespace Application.Interfaces;

public interface IEventRepository
{
    Event CreateEvent(Event testEvent);
}