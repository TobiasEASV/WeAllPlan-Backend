using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using FluentValidation;

namespace Application;

public class EventSlotService : IEventSlotService
{
    private IEventSlotRepository _repository;
    private IMapper _mapper;
    private IValidator<EventSlotDTO> _validator;

    public EventSlotService(IEventSlotRepository repository, IMapper mapper, IValidator<EventSlotDTO> validator)
    {
        if (repository is null)
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
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }
}