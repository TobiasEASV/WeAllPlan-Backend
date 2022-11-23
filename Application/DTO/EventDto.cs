using Core;

namespace Application.DTO;

public class EventDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public User User { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public List<EventSlot>? EventSlots { get; set; }

}