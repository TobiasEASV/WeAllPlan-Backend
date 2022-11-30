using System.ComponentModel.DataAnnotations;
using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Core;
using FluentValidation;
using ValidationException = FluentValidation.ValidationException;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Application;

public class EventSlotService : IEventSlotService
{
    private IEventSlotRepository _repository;
    private IMapper _mapper;
    private IValidator<EventSlotDTO> _validator;

    public EventSlotService(IEventSlotRepository repository, IMapper mapper, IValidator<EventSlotDTO> validator)
    {
        if (repository is null)
        {
            throw new NullReferenceException("Repository is null");
        }

        if (mapper is null)
        {
            throw new NullReferenceException("Mapper is null");
        }

        if (validator is null)
        {
            throw new NullReferenceException("Validator is null");
        }

        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task CreateEventSlot(List<EventSlotDTO> eventSlotDtos, int eventId)
    {
        List<EventSlot> allEventSlots = _repository.GetAll().Result.FindAll(e => e.Event.Id == eventId);
        List<EventSlotDTO> createDTOs = new List<EventSlotDTO>();
        if (allEventSlots.Count != 0)
        {
            foreach (var eventSlotDto in eventSlotDtos)
            {
                foreach (var eventSlotsDb in allEventSlots)
                {
                    var validation = await _validator.ValidateAsync(eventSlotDto);
                    if (!validation.IsValid || (eventSlotDto.StartTime == eventSlotsDb.StartTime &&
                                                eventSlotDto.EndTime == eventSlotsDb.EndTime))
                    {
                       // Do nothing
                    }
                    else
                    {
                        createDTOs.Add(eventSlotDto);
                    }
                }
            }
        }
        else
        {
            foreach (var eventSlotDto in eventSlotDtos)
            {
                var validation = await _validator.ValidateAsync(eventSlotDto);
                if (!validation.IsValid)
                {
                    eventSlotDtos.Remove(eventSlotDto);
                }
            }

            List<EventSlot> createList = _mapper.Map<List<EventSlot>>(eventSlotDtos);
            getEventsToEventSlot(createList, eventId);
            _repository.CreateEventSlot(createList);
        }
        if (createDTOs.Count != 0)
        {
            List<EventSlot> createList = _mapper.Map<List<EventSlot>>(createDTOs);
            getEventsToEventSlot(createList, eventId);
            _repository.CreateEventSlot(createList); // Create EventSlots
        }
    }

    public void UpdateEventSlot(List<EventSlotDTO> eventSlotDto, int userId)
    {
        bool dropList = false;
       List<EventSlot> listToCheck = _repository.GetAll().Result.FindAll(e => e.Event.Id == eventSlotDto[0].EventId);
       
       foreach (var eventSlot in listToCheck)
       {
           if (eventSlot.Event.User.Id != userId)
           {
               dropList = true;
           }
       }

       if (dropList) { }
       else
       {
           List<EventSlotDTO> listToUpdate = new List<EventSlotDTO>();
           foreach (var dto in eventSlotDto)
           {
               ValidationResult validate = _validator.Validate(dto);
               if (validate.IsValid)
               {
                   listToUpdate.Add(dto);
               }
           }

           if (listToUpdate.Count != 0)
           {
               _repository.UpdateEventSlot(_mapper.Map<List<EventSlot>>(listToUpdate));
           }
       }
    }

    public async Task<List<EventSlotDTO>> GetEventSlots(int eventId)
    {
        List<EventSlot> eventSlots = _repository.GetAll().Result.FindAll(e => e.Id == eventId);
        return await Task.Run( () =>_mapper.Map<List<EventSlotDTO>>(eventSlots));
    }
    

    public void DeleteEventSlots(List<EventSlotDTO> listToDelete, int userId)
    {
        Event Event = getEventFromId(GetEventSlots(listToDelete[0].Id).Result[0].EventId).Result;
        foreach (var dto in listToDelete)
        {
            if (Event.User.Id != userId)
            {
                throw new ValidationException("You do not own this Event.");
            }
        }
        _repository.DeleteEventSlot(_mapper.Map<List<EventSlot>>(listToDelete));
    }

    public async Task<Event> getEventFromId(int eventId)
    {
        return await _repository.getEventFromId(eventId);
    }

    public async void getEventsToEventSlot(List<EventSlot> list, int eventId)
    {
        foreach (var eventSlot in list)
        {
            eventSlot.Event = await _repository.getEventFromId(eventId);
        }
    }
}