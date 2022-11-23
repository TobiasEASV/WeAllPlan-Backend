using Application.Interfaces;
using AutoMapper;
using Core;

namespace Application;

public class EventService : IEventService
{
    private IEventRepository _repository;
    private IMapper _mapper;

    public EventService(IEventRepository repository, IMapper mapper)
    {
        if (repository is null)
            throw new NullReferenceException("Repository is null");
        _repository = repository;
        _mapper = mapper;
    }

    public Event CreateEvent(EventDTO eventDto)
    {
        Event testEvent = _mapper.Map<Event>(eventDto);
        
        return _repository.CreateEvent(testEvent);
    }
}