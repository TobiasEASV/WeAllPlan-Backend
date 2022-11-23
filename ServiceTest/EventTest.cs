using Application;
using Application.Interfaces;
using AutoMapper;
using Core;
using Infrastructure;
using Moq;
using Xunit.Abstractions;

namespace ServiceTest;

public class EventTest
{

    private IMapper _mapper;

    private readonly ITestOutputHelper _testOutputHelper;

    public EventTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        
        var config = new MapperConfiguration(conf => {
            conf.CreateMap<EventDTO, Event>(); //RegisterUserDto ==> User - create new user
            conf.CreateMap<Event,EventDTO>();
        });
        _mapper = config.CreateMapper();
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
        IEventService service = new EventService(mockRepo.Object,_mapper);
        

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
        var test = new Event()
        {
            Title = "topbias"
        };
        
        mockRepo.Setup(r => r.CreateEvent(test)).Returns(test);
        
        
        IEventService service = new EventService(mockRepo.Object, _mapper);

        //Act
        EventDTO tesst = service.CreateEvent(eventDto);
        
        
        
        //assert
        _testOutputHelper.WriteLine(tesst.Title);
        //Assert.Equal( "Kæmpe fedt event", tesst.Title);
        //_testOutputHelper.WriteLine(mockRepo.Object.CreateEvent(_mapper.Map<Event>(eventDto)).Title);
        mockRepo.Verify( repo => repo.CreateEvent(null), Times.Never);
        //Assert.True(mockRepo.Object.CreateEvent(_mapper.Map<Event>(eventDto)) is null);
        //Assert.True( tesst is EventDTO);
    }
    
    
}