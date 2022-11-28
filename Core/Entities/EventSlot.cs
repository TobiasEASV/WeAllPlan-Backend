using System.ComponentModel.DataAnnotations;

namespace Core;

public class EventSlot
{
    public int Id { get; set; }
    
    public Event Event { get; set; }
    [Required]
    public DateTime StartTime { get; set; }
    [Required]
    public DateTime EndTime { get; set; }
    
    public List<SlotAnswer> SlotAnswers{ get; set; }
    public Boolean Confirmed { get; set; } = false;

}