namespace Application.Interfaces;

public interface ISlotAnswerService
{
    Task CreateSlotAnswer(SlotAnswerDTO answerDto);
    Task<List<SlotAnswerDTO>> GetSlotAnswer(int eventSlotId);
    Task UpdateSlotAnswer(SlotAnswerDTO slotAnswerDto, int slotAnswerId);
    void DeleteSlotAnswers(int EventId, string Email, List<SlotAnswerDTO> slotAnswerDtos);
}