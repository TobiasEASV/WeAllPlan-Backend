using Application.DTO;
using Core;

namespace Application.Interfaces;

public interface IEventSlotService
{
    Task<List<EventSlotDTO>> CreateEventSlot(List<EventSlotDTO> eventSlotDtos, int eventId);

    Task<List<EventSlotDTO>>UpdateEventSlot(List<EventSlotDTO> eventSlotDto, int userId);
}