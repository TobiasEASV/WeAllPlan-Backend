using System.Security.Authentication;
using Application.DTO;
using Application.Helpers;
using Application.Interfaces;
using AutoMapper;
using Core;
using FluentValidation;
using FluentValidation.Results;

namespace Application;

public class EventService : IEventService
{
    private IEventRepository _repository;
    private IMapper _mapper;
    private IValidator<EventDTO> _eventValidator;
    private IValidator<PostEventDTO> _createEventValidator;
    private CreateEventMapper _createEventMapper = new CreateEventMapper();

    public EventService(IEventRepository repository, IMapper mapper, IValidator<EventDTO> eventValidator,
        IValidator<PostEventDTO> createEventValidator)
    {
        if (repository is null)
            throw new NullReferenceException("Repository is null");
        if (mapper is null)
            throw new NullReferenceException("Mapper is null");
        if (eventValidator is null)
            throw new NullReferenceException("Validator is null");

        _repository = repository;
        _mapper = mapper;
        _eventValidator = eventValidator;
        _createEventValidator = createEventValidator;
    }

    public async Task CreateEvent(PostEventDTO eventDto)
    {
        var validation = _createEventValidator.Validate(eventDto);
        if (!validation.IsValid)
        {
            throw new ValidationException(validation.ToString());
        }

        Event testEvent = _createEventMapper.CrudEventDtoToEvent(eventDto); // Map the DTO to an actual Object.
        testEvent.User = setUserFromId(eventDto.OwnerId);
        await _repository.CreateEvent(testEvent);
    }

    public async Task<EventDTO> GetEvent(int id)
    {
        Event Event = _repository.GetEventById(id).Result;
        if (Event == null)
        {
            throw new NullReferenceException("Event doesn't exist");
        }

        EventDTO eventDto = _mapper.Map<EventDTO>(Event);
        eventDto.UserId = Event.User.Id;
        return await Task.Run(() => eventDto);
    }

    public async Task<List<EventDTO>> GetEventsFromUser(int userId)
    {
        return await Task.Run(() => _mapper.Map<List<EventDTO>>(_repository.GetEventByUserId(userId).Result));
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

        await _repository.UpdateEvent(testEvent);
    }

    public void DeleteEvent(int eventId, int userId)
    {
        Event eventToDelete = _repository.GetAll().Result.Find(Event => Event.Id == eventId);
        if (eventToDelete == null)
        {
            throw new NullReferenceException("Event does not exist");
        }

        if (userId != eventToDelete.User.Id)
        {
            throw new AuthenticationException("You do not own this Event");
        }

        _repository.Delete(eventToDelete);
    }

    private User setUserFromId(int userId)
    {
        return _repository.getUser(userId);
    }
}