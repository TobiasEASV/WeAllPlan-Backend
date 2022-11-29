using Application.Helpers;
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
    private EmailValidator _emailValidator;

    public SlotAnswerService(ISlotAnswerRepository slotAnswerRepository, IMapper mapper, IValidator<SlotAnswerDTO> validator)
    {
        if (slotAnswerRepository is null   )
        {
            throw new NullReferenceException("Repository is null");
        }

        if (mapper is null)
        {
            throw new NullReferenceException("Mapper is null");
        }

        if (validator is null)
        {
            throw new NullReferenceException("Validator is null");
        }
        _slotAnswerRepository = slotAnswerRepository;
        _mapper = mapper;
        _validator = validator;
        _emailValidator = new EmailValidator();
    }

    public async Task<SlotAnswerDTO> CreateSlotAnswer(SlotAnswerDTO answerDto)
    {
        var validation = _validator.Validate(answerDto);
        if (!validation.IsValid)
        {
            throw new ValidationException(validation.ToString());
        }

        if (!_emailValidator.IsValidEmail(answerDto.Email))
        {
            throw new ValidationException("E-mail has to be of a correct format");
        }
            SlotAnswer slotAnswer = _mapper.Map<SlotAnswer>(answerDto); // Map the DTO to an actual Object.
        SlotAnswer repoAnswer = await _slotAnswerRepository.CreateSlotAnswer(slotAnswer); // Get the actual object from the DB
        SlotAnswerDTO answerDTO = _mapper.Map<SlotAnswerDTO>(repoAnswer); // Map the actual object to a DTO and send it back
        return answerDTO;
    }

    public async Task<List<SlotAnswerDTO>> GetSlotAnswer(int eventSlotId)
    {
        List<SlotAnswer> slotAnswers = _slotAnswerRepository.GetAll().Result
            .FindAll(answer => answer.EventSlot.Id.Equals(eventSlotId));
        return await Task.Run(() => _mapper.Map<List<SlotAnswerDTO>>(slotAnswers));
    }

    public async Task UpdateSlotAnswer(SlotAnswerDTO slotAnswerDto, int slotAnswerId)
    {
        var validation = _validator.Validate(slotAnswerDto);
        if (slotAnswerId != slotAnswerDto.Id)
        {
            throw new ValidationException("You can only change your own answers");
        }
            if (!validation.IsValid)
        {
            throw new ValidationException(validation.ToString());
        }

        if (!_emailValidator.IsValidEmail(slotAnswerDto.Email))
        {
            throw new ValidationException("E-mail has to be of a correct format");
        }
        await _slotAnswerRepository.UpdateSlotAnswer(_mapper.Map<SlotAnswer>(slotAnswerDto));
      
    }

    public void DeleteSlotAnswers(int eventId, string email, List<SlotAnswerDTO> slotAnswerDtos)
    {
        List<SlotAnswer> listOfSlotAnswers = _slotAnswerRepository.GetAll().Result.FindAll(s => s.EventSlot.Event.Id == eventId );
        List<SlotAnswer> listToDelete = new List<SlotAnswer>();
        foreach (var dto in slotAnswerDtos)
        {
            foreach (var slotAnswers in listOfSlotAnswers)
            {
                if (dto.Id == slotAnswers.Id)
                { if ((slotAnswers.Email != email && slotAnswers.EventSlot.Event.User.Email == email)
                        || (slotAnswers.EventSlot.Event.User.Email != email && slotAnswers.Email == email)
                        || (slotAnswers.EventSlot.Event.User.Email == email && slotAnswers.Email == email))
                    {
                        listToDelete.Add(slotAnswers);
                    }
                    else throw new ValidationException("You do not have permission to delete these answers");
                }
                
            }
        }
        _slotAnswerRepository.DeleteSlotAnswers(listToDelete);
    }
}