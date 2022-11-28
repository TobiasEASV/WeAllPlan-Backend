using Application.Interfaces;
using AutoMapper;
using Core;
using FluentValidation;

namespace Application;

public class SlotAnswerService : ISlotAnswerService
{
    private ISlotAnswerRepository _slotAnswerRepository;
    private IMapper _mapper;
    private IValidator<SlotAnswerDTO> _validator;

    public SlotAnswerService(ISlotAnswerRepository slotAnswerRepository, IMapper mapper, IValidator<SlotAnswerDTO> validator)
    {
        if (slotAnswerRepository == null || mapper ==null || validator ==null)
        {
            throw new NullReferenceException("Service can not be created without all parameters.");
        }
        _slotAnswerRepository = slotAnswerRepository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<SlotAnswerDTO> CreateSlotAnswer(SlotAnswerDTO answerDto)
    {
        var validation = _validator.Validate(answerDto);
        if (!validation.IsValid)
        {
            throw new ValidationException(validation.ToString());
        }
        SlotAnswer slotAnswer = _mapper.Map<SlotAnswer>(answerDto); // Map the DTO to an actual Object.
        SlotAnswer repoAnswer = await _slotAnswerRepository.CreateSlotAnswer(slotAnswer); // Get the actual object from the DB
        SlotAnswerDTO answerDTO = _mapper.Map<SlotAnswerDTO>(repoAnswer); // Map the actual object to a DTO and send it back
        return answerDTO;
    }
}