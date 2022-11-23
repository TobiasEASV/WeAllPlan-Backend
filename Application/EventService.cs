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

    public EventDTO CreateEvent(EventDTO eventDto)
    {
        Event testEvent = _mapper.Map<Event>(eventDto);

        
        var test = new Event()
        {
            Title = "Tobiasssss"
        };
        var RepoEvent = _repository.CreateEvent(test);
        try
        {
            Console.WriteLine(RepoEvent.Title);
        }
        catch (Exception e)
        {
            throw new Exception("der er noget lort");
        }
       
       // EventDTO eventDTO = _mapper.Map<EventDTO>(test);
        return _mapper.Map<EventDTO>(test);
    }
}