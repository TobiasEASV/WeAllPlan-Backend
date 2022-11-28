using Application.Interfaces;
using AutoMapper;
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

    public void CreateSlotAnswer(SlotAnswerDTO answerDto)
    {
        throw new NotImplementedException();
    }
}