using Application.DTO;
using Core;

namespace Application.Interfaces;

public interface IEventSlotService
{
    Task CreateEventSlot(List<EventSlotDTO> eventSlotDtos, int eventId);

    void UpdateEventSlot(List<EventSlotDTO> eventSlotDto, int userId);
    Task<List<EventSlotDTO>> GetEventSlots(int eventId);

    void DeleteEventSlots(List<EventSlotDTO> listToDelete, int userId);
}