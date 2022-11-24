using Application;
using Application.Interfaces;
using AutoMapper;
using Core;
using FluentValidation;
using Infrastructure;
using Moq;

namespace ServiceTest;

public class EventTest
{
    
    private IMapper _mapper;
    private IValidator<EventDTO> _validator;

    public EventTest()
    {
        
        var config = new MapperConfiguration(conf => {
            conf.CreateMap<EventDTO, Event>(); //RegisterUserDto ==> User - create new user
            conf.CreateMap<Event,EventDTO>();
        });
        _mapper = config.CreateMapper();
        _validator = new EventValidator();
    }
    
    /// <summary>
    /// 1.1
    /// </summary>
    [Fact]
    public void ValidEventServiceTest()
    {
        // Arrange
        Mock<IEventRepository> mockRepo = new Mock<IEventRepository>();
      
        // Act
        IEventService service = new EventService(mockRepo.Object,_mapper,_validator);
        

        // Assert
        Assert.NotNull(service);
        Assert.True(service is EventService);
    }
    
    /// <summary>
    /// 1.2
    /// </summary>
    [Fact]
    public void InvalidEventServiceTest()
    {
        // Arrange
        IEventService service;
        string expected = "Repository is null";
        
        // Act + Assert
        var ex = Assert.Throws<NullReferenceException>(() => service = new EventService(null, null,null));
        Assert.Equal(expected, ex.Message);
    }

    [Theory]
    [MemberData(nameof(TestData.CreateValidEventTestData), MemberType = typeof(TestData))]
    public void ValidEventCreationTest(EventDTO eventDto)
    {
        // Arrange
        Mock<IEventRepository> mockRepo = new Mock<IEventRepository>();
        
        Event testEvent = _mapper.Map<Event>(eventDto);
        mockRepo.Setup(r => r.CreateEvent(testEvent)).Returns(testEvent);
        
        IEventService service = new EventService(mockRepo.Object, _mapper,_validator);
        
        //Act
        EventDTO tesst = service.CreateEvent(eventDto);
        
        
        
        //Assert
        mockRepo.Verify( repo => repo.CreateEvent(It.IsAny<Event>()), Times.Once);
        
    }
    
    
}