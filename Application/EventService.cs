using Application.DTO;
using Application.Interfaces;
using Core;
using Core.Interfaces;

namespace Application;

public class EventService: IEventService
{
    private readonly IEventRepository _eventRepository;

    public EventService(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }


    public EventDto CreateEvent(EventDto eventDto)
    {
        
        var test = _eventRepository.CreateEvent(null);

        return new EventDto()
        {
            Id = test.Id,
            Description = test.Description,
            Location = test.Location,
            Title = test.Title,
            User = test.User,
            EventSlots = test.EventSlots
        };
    }
}