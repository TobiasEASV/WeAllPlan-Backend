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

    public SlotAnswerService(ISlotAnswerRepository slotAnswerRepository, IMapper mapper,
        IValidator<SlotAnswerDTO> validator)
    {
        if (slotAnswerRepository is null)
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

    public async Task CreateSlotAnswer(List<SlotAnswerDTO> answerDto)
    {
        List<SlotAnswer> existingAnswers = _slotAnswerRepository.GetAll().Result
            .FindAll(s => s.EventSlot.Id == answerDto[0].EventSlotId);
        if (existingAnswers.FindAll(s=> s.Email == answerDto[0].Email).Count < 0)
        {
            throw new ValidationException("There has been an entry for this email.");
        }
        foreach (var answer in answerDto)
        {
            var validation = _validator.Validate(answer);
            if (!validation.IsValid)
            {
                throw new ValidationException(validation.ToString());
            }

            if (!_emailValidator.IsValidEmail(answer.Email))
            {
                throw new ValidationException("E-mail has to be of a correct format");
            }
        }

        int counter = 0;
        List<SlotAnswer> slotAnswerList = _mapper.Map<List<SlotAnswer>>(answerDto); // Map the DTO to an actual Object.
        foreach (var slotAnswer in slotAnswerList)
        {
            setEventSlot(slotAnswer, answerDto[counter].EventSlotId);
            counter++;
        }

        await _slotAnswerRepository.CreateSlotAnswer(slotAnswerList); // Get the actual object from the DB
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
        List<SlotAnswer> listOfSlotAnswers =
            _slotAnswerRepository.GetAll().Result.FindAll(s => s.EventSlot.Event.Id == eventId);
        List<SlotAnswer> listToDelete = new List<SlotAnswer>();
        foreach (var dto in slotAnswerDtos)
        {
            foreach (var slotAnswers in listOfSlotAnswers)
            {
                if (dto.Id == slotAnswers.Id)
                {
                    if ((slotAnswers.Email != email && slotAnswers.EventSlot.Event.User.Email == email)
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

    public void setEventSlot(SlotAnswer slotAnswer, int eventSlotId)
    {
        slotAnswer.EventSlot = _slotAnswerRepository.getEventSlot(eventSlotId).Result;
    }
}