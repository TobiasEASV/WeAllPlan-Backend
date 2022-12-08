using Application.DTO;
using Core;

namespace Application.Helpers;

public class CreateEventMapper
{
    public Event CrudEventDtoToEvent(CRUDEventDTO eventDto)
    {
        var eventSlotList = eventDto.TimeSlots.Select(TimeSlot => new EventSlot()
            {
                Event = new Event(),
                Confirmed = false,
                StartTime = TimeSlot.StartTime,
                EndTime = TimeSlot.EndTime,
                SlotAnswers = new List<SlotAnswer>()
            })
            .ToList();


        return new Event()
        {
            Title = eventDto.Title,
            Location = eventDto.Location,
            Description = eventDto.Description,
            User = new User()
            {
                Id = eventDto.OwnerId
            },
            EventSlots = eventSlotList
        };
    }
}