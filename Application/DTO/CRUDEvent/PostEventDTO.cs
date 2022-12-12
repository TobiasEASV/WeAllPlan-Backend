namespace Application.DTO;

public class PostEventDTO
{
    public string Title { get; set; }
    
    public int OwnerId { get; set; }
    
    public string? Description { get; set; }
    
    public string? Location { get; set; }
    
    public List<TimeSlotDTO>? TimeSlots { get; set; }
}