﻿using System.ComponentModel.DataAnnotations;
using System.Security.Authentication;
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
        _repository = repository;
        _mapper = mapper;
        _eventValidator = eventValidator;
    }

    public async Task<EventDTO> CreateEvent(EventDTO eventDto)
    {
        var validation = _eventValidator.Validate(eventDto);
        if (!validation.IsValid)
        {
            throw new ValidationException(validation.ToString());
        }
        Event testEvent = _mapper.Map<Event>(eventDto); // Map the DTO to an actual Object.
        Event RepoEvent = await _repository.CreateEvent(testEvent); // Get the actual object from the DB
        EventDTO eventDTO = _mapper.Map<EventDTO>(RepoEvent); // Map the actual object to a DTO and send it back
        return eventDTO ;
    }

    public async Task<EventDTO> GetEvent(int id)
    {
        Event Event = _repository.GetAll().Result.Find(Event => Event.Id.Equals(id));
        if (Event ==null)
        {
            throw new NullReferenceException("Event doesn't exist");
        }
        return await Task.Run(() => _mapper.Map<EventDTO>(Event));
    }

    public async Task<List<EventDTO>> GetEventsFromUser(int userId)
    {
        List<Event> eventTasks =  _repository.GetAll().Result.FindAll(e => e.User.Id.Equals(userId));
        
        
        return await Task.Run(() => _mapper.Map<List<EventDTO>>(eventTasks));
    }

    public Task<EventDTO> UpdateEvent(EventDTO eventDto, int userId)
    {
        ValidationResult validationResult = _eventValidator.Validate(eventDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.ToString());
        }

        if (eventDto.User.Id != userId)
        {
            throw new AuthenticationException("Wrong User");
        }
        Event Event = _repository.UpdateEvent(_mapper.Map<Event>(eventDto)).Result;
        return Task.Run( () => _mapper.Map<EventDTO>(Event));
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
}