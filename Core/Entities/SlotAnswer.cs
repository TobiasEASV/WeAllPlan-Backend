using System.ComponentModel.DataAnnotations;

namespace Core;

public class SlotAnswer
{
    public int Id { get; set; }
    [Required] public int Answer { get; set; }
    [Required] [MaxLength(100)] public string UserName { get; set; }
    [Required] [EmailAddress] public string Email { get; set; }
    public EventSlot EventSlot { get; set; }
}