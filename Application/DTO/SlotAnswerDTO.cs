using Core;

namespace Application;

public class SlotAnswerDTO
{
    public int Id { get; set; }
    
    public int Answer { get; set; }
    
    public string UserName { get; set; }

    public string Email { get; set; }
    
    public int EventSlotId { get; set; }
}