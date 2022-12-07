using Core;

namespace Application.DTO;

public class EventSlotDTO
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public List<SlotAnswer>? SlotAnswers { get; set; }
    public Boolean Confirmed { get; set; } = false;
}