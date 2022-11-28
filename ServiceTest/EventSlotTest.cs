using Application;
using Application.DTO;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Core;
using FluentValidation;
using Moq;

namespace ServiceTest;

public class EventSlotTest
{
    private IMapper _mapper;
    private IValidator<EventSlotDTO> _validator;
    public EventSlotTest()
    {
        var config = new MapperConfiguration(conf => {
            conf.CreateMap<EventSlotDTO, EventSlot>(); //RegisterUserDto ==> User - create new user
            conf.CreateMap<EventSlot,EventSlotDTO>();
        });
        _mapper = config.CreateMapper();
        _validator = new EventSlotValidator();
        
    }
    
    
    /// <summary>
    /// 1.1
    /// </summary>
    [Fact]
    public void ValidEventServiceTest()
    {
        // Arrange
        Mock<IEventSlotRepository> mockRepo = new Mock<IEventSlotRepository>();
      
        // Act
        IEventSlotService service = new EventSlotService(mockRepo.Object,_mapper,_validator);
        

        // Assert
        Assert.NotNull(service);
        Assert.True(service is EventSlotService);
    }
    
    /// <summary>
    /// 1.2
    /// </summary>
    [Fact]
    public void InvalidEventServiceTest()
    {
        // Arrange
        IEventService service;
        Mock<IEventSlotRepository> mock = new Mock<IEventSlotRepository>();
        
        // Act + Assert
        NullReferenceException noMock = Assert.Throws<NullReferenceException>(() => new EventSlotService(null, _mapper,_validator ));
        NullReferenceException noMapper = Assert.Throws<NullReferenceException>(() => new EventSlotService(mock.Object, null,_validator ));
        NullReferenceException noValidator = Assert.Throws<NullReferenceException>(() => new EventSlotService(mock.Object, _mapper,null ));
        Assert.Equal("Repository is null", noMock.Message);
        Assert.Equal("Mapper is null", noMapper.Message);
        Assert.Equal("Validator is null", noValidator.Message);
        
    }
}