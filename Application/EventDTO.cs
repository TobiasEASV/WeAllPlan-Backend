using Core;

namespace Application;

public class EventDTO
{
    public string Title { get; set; }
    public User User { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public List<EventSlot>? EventSlots { get; set; }

    public EventDTO(string title, User user)
    {
        Title = title;
        User = user;
    }
}