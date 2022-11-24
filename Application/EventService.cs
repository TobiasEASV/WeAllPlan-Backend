using System.ComponentModel.DataAnnotations;
using Application.Interfaces;
using AutoMapper;
using Core;
using FluentValidation;
using ValidationException = FluentValidation.ValidationException;

namespace Application;

public class EventService : IEventService
{
    private IEventRepository _repository;
    private IMapper _mapper;
    private IValidator<EventDTO> _eventValidator ;

    public EventService(IEventRepository repository, IMapper mapper, IValidator<EventDTO> eventValidator)
    {
        if (repository is null)
            throw new NullReferenceException("Repository is null");
        _repository = repository;
        _mapper = mapper;
        _eventValidator = eventValidator;
    }

    public EventDTO CreateEvent(EventDTO eventDto)
    {
        var validation = _eventValidator.Validate(eventDto);
        if (!validation.IsValid)
        {
            throw new ValidationException(validation.ToString());
        }
        Event testEvent = _mapper.Map<Event>(eventDto); // Map the DTO to an actual Object.
        Event RepoEvent = _repository.CreateEvent(testEvent); // Get the actual object from the DB
        EventDTO eventDTO = _mapper.Map<EventDTO>(RepoEvent); // Map the actual object to a DTO and send it back
        return eventDTO ;
    }
}