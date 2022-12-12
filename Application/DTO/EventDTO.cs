using Core;

namespace Application;

public class EventDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int UserId { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    
    public List<EventSlot>? EventSlots { get; set; }

    
}