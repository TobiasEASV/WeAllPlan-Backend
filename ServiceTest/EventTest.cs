using Application;
using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Core;
using Core.Interfaces;
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
            conf.CreateMap<EventDto, Event>(); //RegisterUserDto ==> User - create new user
            conf.CreateMap<Event,EventDto>();
        });
        _mapper = config.CreateMapper();
    }
    
    [Fact]
    [MemberData(nameof(TestData.CreateValidEventTestData), MemberType = typeof(TestData))]
    public void ValidEventCreationTest()
    {
        var test = new Event()
        {
            Id = 1,
            Description = "testDescription",
            Location = "testLocation",
            Title = "testTitle",
            User = new User(),
            EventSlots = new List<EventSlot>()
        };
        // Arrange
        Mock<IEventRepository> mockRepo = new Mock<IEventRepository>();
        
        mockRepo.Setup(repo => repo.CreateEvent(It.IsAny<Event>())).Returns<Event>((testEvent) => test);
        
        
        IEventService service = new EventService(mockRepo.Object);

        //Act
        EventDto tesst = service.CreateEvent(new EventDto());
        
        
        //assert
        _testOutputHelper.WriteLine(tesst.Title);
        Assert.Equal(tesst.Description, "testDescription");
        mockRepo.Verify( repo => repo.CreateEvent(null), Times.Once);
        
    }
}