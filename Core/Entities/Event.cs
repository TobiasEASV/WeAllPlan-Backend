using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace Core;

public class Event
{
    
    public int Id { get; set; }
    
    [Required]
    public string Title { get; set; }
    [Required]
    public User User { get; set; }
    
    
    public string? Description { get; set; }
    public string? Location { get; set; }
    public List<EventSlot>? EventSlots { get; set; }
    
    
}