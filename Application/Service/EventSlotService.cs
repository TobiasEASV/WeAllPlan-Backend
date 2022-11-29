using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Core;
using FluentValidation;

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
    
    public async Task<List<EventSlotDTO>> CreateEventSlot(List<EventSlotDTO> eventSlotDtos, int eventId)
    {
        List<EventSlot> allEventSlots = _repository.GetAll().Result.FindAll(e => e.Event.Id == eventId);
        foreach (var eventSlot in eventSlotDtos)
        {
            foreach (var eventSlotsDB in allEventSlots)
            {
                if (eventSlot.StartTime == eventSlotsDB.StartTime && eventSlot.EndTime == eventSlotsDB.EndTime)
                {
                    throw new ValidationException("EventSlot already exists on this event");
                }
            }
            var validation = await _validator.ValidateAsync(eventSlot);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation.ToString());
            }
        }
        _repository.CreateEventSlot(_mapper.Map<List<EventSlot>>(eventSlotDtos)); // Create EventSlots
        
        List<EventSlot> eventSlots = new List<EventSlot>();
        
        
        foreach (var eventSlotMemory in eventSlotDtos)
        {
            var eventSlotDb =  _repository.GetAll().Result.Find(e =>
                e.Event.Id == eventSlotMemory.Event.Id && e.StartTime == eventSlotMemory.StartTime && e.EndTime == eventSlotMemory.EndTime);
            eventSlots.Add(eventSlotDb);
        }

        return await Task.Run(() => _mapper.Map<List<EventSlotDTO>>(eventSlots));
    }
    
}