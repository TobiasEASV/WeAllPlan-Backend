using Application.Interfaces;

namespace Application;

public class EventService : IEventService
{
    private IRepository _repository;

    public EventService(IRepository repository)
    {
        if (repository is null)
            throw new NullReferenceException("Repository is null");
        _repository = repository;
    }
    
}