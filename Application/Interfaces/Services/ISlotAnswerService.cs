namespace Application.Interfaces;

public interface ISlotAnswerService
{
    Task<SlotAnswerDTO> CreateSlotAnswer(SlotAnswerDTO answerDto);
}