using Application;
using Application.Interfaces;
using AutoMapper;
using Core;
using Moq;

namespace ServiceTest;

public class EventTest
{
    private IMapper mapper = new MapperConfiguration(config =>
    {
        config.CreateMap<EventDTO, Event>();
        
    }).CreateMapper();
    /// <summary>
    /// 1.1
    /// </summary>
    [Fact]
    public void ValidEventServiceTest()
    {
        // Arrange
        Mock<IEventRepository> mockRepo = new Mock<IEventRepository>();

        // Act
        IEventService service = new EventService(mockRepo.Object,mapper);
        

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
        var ex = Assert.Throws<NullReferenceException>(() => service = new EventService(null, null));
        Assert.Equal(expected, ex.Message);
    }

    [Theory]
    [MemberData(nameof(TestData.CreateValidEventTestData), MemberType = typeof(TestData))]
    public void ValidEventCreationTest(EventDTO eventDto)
    {
        // Arrange
        Mock<IEventRepository> mockRepo = new Mock<IEventRepository>();
        
        
        

        IEventService service = new EventService(mockRepo.Object, mapper);
        
        //Act
        Event testEvent = service.CreateEvent(eventDto);
        
        //assert
        Assert.True(testEvent is Event);
        
        
    }
    
    
}