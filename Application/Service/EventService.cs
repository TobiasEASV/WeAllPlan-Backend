using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Core;
using FluentValidation;
using ValidationException = FluentValidation.ValidationException;
using ValidationResult = FluentValidation.Results.ValidationResult;

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
        if(mapper is null)
            throw new NullReferenceException("Mapper is null");
        if (eventValidator is null)
            throw new NullReferenceException("Validator is null");
        
        _repository = repository;
        _mapper = mapper;
        _eventValidator = eventValidator;
    }

    public async Task CreateEvent(EventDTO eventDto)
    {
        var validation = _eventValidator.Validate(eventDto);
        if (!validation.IsValid)
        {
            throw new ValidationException(validation.ToString());
        }
        Event testEvent = _mapper.Map<Event>(eventDto); // Map the DTO to an actual Object.
        testEvent.User = setUserFromId(eventDto.UserId);
        await _repository.CreateEvent(testEvent);
        
    }

    public async Task<EventDTO> GetEvent(int id)
    {
        Event Event = _repository.GetEventById(id).Result;
        if (Event ==null)
        {
            throw new NullReferenceException("Event doesn't exist");
        }

        EventDTO eventDto = _mapper.Map<EventDTO>(Event);
        eventDto.UserId = Event.User.Id;
        return await Task.Run(() => eventDto);
    }

    public async Task<List<EventDTO>> GetEventsFromUser(int userId)
    {
        List<Event> eventTasks =  _repository.GetEventByUserId(userId).Result;

        List<EventDTO> listDtos = _mapper.Map<List<EventDTO>>(eventTasks);
        if (eventTasks.Count == 0)
        {
            return await Task.Run(() => new List<EventDTO>(){});
        }
        foreach (var dto in listDtos)
        {
            dto.UserId = eventTasks[0].User.Id;
        }
        return await Task.Run(() => listDtos);
    }

    public async Task UpdateEvent(EventDTO eventDto, int userId)
    {
        ValidationResult validationResult = await _eventValidator.ValidateAsync(eventDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.ToString());
        }

        if (eventDto.UserId != userId)
        {
            throw new AuthenticationException("Wrong User");
        }

        Event testEvent = _mapper.Map<Event>(eventDto);
        
        await _repository.UpdateEvent(testEvent, userId);
        
    }

    public void DeleteEvent(int eventId, int userId)
    {
        Event eventToDelete= _repository.GetAll().Result.Find(Event => Event.Id == eventId);
        if (eventToDelete == null)
        {
            throw new NullReferenceException("Event does not exist");
        }

        if (userId !=eventToDelete.User.Id)
        {
            throw new AuthenticationException("You do not own this Event");
        }
        _repository.Delete(eventToDelete);
    }

    public User setUserFromId(int userId)
    {
        return _repository.getUser(userId);
    }
}