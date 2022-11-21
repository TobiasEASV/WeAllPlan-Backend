namespace Core;

public class EventSlot
{
    public int Id { get; set; }
    public Event Event { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public List<SlotAnswer> SlotAnswers{ get; set; }
    public Boolean Confirmed { get; set; } = false;

}