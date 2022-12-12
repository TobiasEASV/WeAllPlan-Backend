namespace Application.Interfaces;

public interface ISlotAnswerService
{
    public Task CreateSlotAnswer(List<SlotAnswerDTO> answerDto);
    public Task<List<SlotAnswerDTO>> GetSlotAnswer(int eventSlotId);
    public Task UpdateSlotAnswer(SlotAnswerDTO slotAnswerDto, int slotAnswerId);
    public void DeleteSlotAnswers(int EventId, string Email, List<SlotAnswerDTO> slotAnswerDtos);
}