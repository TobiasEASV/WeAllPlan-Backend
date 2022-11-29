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
        if (allEventSlots.Count !=0)
        {
            foreach (var eventSlotDto in eventSlotDtos)
            {
                foreach (var eventSlotsDb in allEventSlots)
                {
                    var validation = await _validator.ValidateAsync(eventSlotDto);
                    if (!validation.IsValid)
                    {
                        eventSlotDtos.Remove(eventSlotDto);
                    }
                    if (eventSlotDto.StartTime == eventSlotsDb.StartTime && eventSlotDto.EndTime == eventSlotsDb.EndTime)
                    {
                        eventSlotDtos.Remove(eventSlotDto);
                    }
                }
            }
        }

        List<EventSlot> eventSlots = new List<EventSlot>(){};
        if (eventSlotDtos.Count !=0)
        {
            _repository.CreateEventSlot(_mapper.Map<List<EventSlot>>(eventSlotDtos)); // Create EventSlots
            foreach (var eventSlotMemory in eventSlotDtos)
            {
            
                var eventSlotDb =  _repository.GetAll().Result.Find(e =>
                    e.Event.Id == eventSlotMemory.Event.Id && e.StartTime == eventSlotMemory.StartTime && e.EndTime == eventSlotMemory.EndTime);
                eventSlots.Add(eventSlotDb);
            }
            return await Task.Run(() => _mapper.Map<List<EventSlotDTO>>(eventSlots));
        }



        return await Task.Run(() => new List<EventSlotDTO>());
    }

    public Task<List<EventSlotDTO>> UpdateEventSlot(List<EventSlotDTO> eventSlotDto, int userId)
    {
        throw new NotImplementedException();
    }
}