using Application.DTO;
using Core;

namespace Application.Interfaces;

public interface IEventSlotService
{
    public Task CreateEventSlot(List<EventSlotDTO> eventSlotDtos, int eventId);
    public void UpdateEventSlot(List<EventSlotDTO> eventSlotDto, int userId);
    public Task<List<EventSlotDTO>> GetEventSlots(int eventId);
    public void DeleteEventSlots(List<EventSlotDTO> listToDelete, int userId);
}