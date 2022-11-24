using Application;
using Application.Interfaces;
using AutoMapper;
using Core;
using FluentValidation;
using Infrastructure;
using Moq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ServiceTest;

public class EventTest
{
    
    private IMapper _mapper;
    private IValidator<EventDTO> _validator;
    private ITestOutputHelper _testOutputHelper;

    public EventTest(ITestOutputHelper testOutputHelper)
    {
        
        var config = new MapperConfiguration(conf => {
            conf.CreateMap<EventDTO, Event>(); //RegisterUserDto ==> User - create new user
            conf.CreateMap<Event,EventDTO>();
        });
        _mapper = config.CreateMapper();
        _validator = new EventValidator();
        _testOutputHelper = testOutputHelper;
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

    /// <summary>
    /// 2.1 & 2.2
    /// </summary>
    /// <param name="eventDto"></param>
    [Theory]
    [MemberData(nameof(TestData.CreateValidEventTestData), MemberType = typeof(TestData))]
    public void ValidEventCreationTest(EventDTO eventDto)
    {
        // Arrange
        Mock<IEventRepository> mockRepo = new Mock<IEventRepository>();
        
       IEventService service = new EventService(mockRepo.Object, _mapper,_validator);
        
        //Act
        service.CreateEvent(eventDto);

        //Assert
        mockRepo.Verify( repo => repo.CreateEvent(It.IsAny<Event>()), Times.Once);
        
    }
    
    /// <summary>
    /// 2.3
    /// </summary>
    /// <param name="eventDto"></param>
    /// <param name="expected"></param>
    [Theory]
    [MemberData(nameof(TestData.CreateInvalidEventTestData), MemberType = typeof(TestData))]
    public void InvalidEventCreationTest(EventDTO eventDto, string[] expected)
    {
        // Arrange
        Mock<IEventRepository> mockRepo = new Mock<IEventRepository>();

        IEventService service = new EventService(mockRepo.Object, _mapper,_validator);

        //Act + Assert
        ValidationException validationException = Assert.ThrowsAsync<ValidationException>(() => service.CreateEvent(eventDto)).Result;
        Assert.Equal(expected.First(), validationException.Message);
        mockRepo.Verify( repo => repo.CreateEvent(It.IsAny<Event>()), Times.Never);
    }

    /// <summary>
    /// 3.1
    /// </summary>
    [Fact]
    public void GetAValidEventTest()
    {
        // Arrange
        int id = 1;

        Mock<IEventRepository> mockRepo = new Mock<IEventRepository>();

        mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(_mapper.Map<List<Event>>(mockEvents));
        
        IEventService service = new EventService(mockRepo.Object, _mapper, _validator);

        // Act
        EventDTO eventDto = service.GetEvent(id).Result;

        // Assert
        Assert.Equal(eventDto.Id, id);
    }
    
    /// <summary>
    /// 3.2
    /// </summary>
    [Fact]
    public void GetAValidListOfEventsFromUserIdTest()
    {
        // Arrange
        int UserId = 1;

        List<EventDTO> expected = new List<EventDTO>()
        {
            new EventDTO()
            {
                Title = "eventTest", Id = 1, Description = "its so fun", Location = "India",
                User = new User()
                    { Id = 1, Name = "Peter", Email = "Min@email.com", Password = "hest", Salt = "HjemmeFest" }
            },
            new EventDTO()
            {
                Title = "anotherEvent", Id = 4, Description = "its also fun", Location = "Russia",
                User = new User()
                    { Id = 1, Name = "Peter", Email = "Min@email.com", Password = "hest", Salt = "HjemmeFest" }
            },
        };
        Mock<IEventRepository> mockRepo = new Mock<IEventRepository>();

        mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(_mapper.Map<List<Event>>(mockEvents));
        
        IEventService service = new EventService(mockRepo.Object, _mapper, _validator);

        // Act
        List<EventDTO> eventDtos = service.GetEventsFromUser(UserId).Result;

        // Assert
        Assert.True(eventDtos[0].Id == expected[0].Id && eventDtos[1].Id == expected[1].Id);
        Assert.Equal(eventDtos.Count, expected.Count);
    }
    
    /// <summary>
    /// 3.3
    /// </summary>
    [Fact]
    public void GetAValidEmptyListOfEventsFromUserIdTest()
    {
        // Arrange
        int UserId = 2;

        Mock<IEventRepository> mockRepo = new Mock<IEventRepository>();

        mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(_mapper.Map<List<Event>>(mockEvents));
        
        IEventService service = new EventService(mockRepo.Object, _mapper, _validator);

        // Act
        List<EventDTO> eventDtos = service.GetEventsFromUser(UserId).Result;

        // Assert
        Assert.Empty(eventDtos);
    }
    
    /// <summary>
    /// 3.4
    /// </summary>
    [Fact]
    public async Task GetInvalidEventFromEventIdTest()
    {
        // Arrange
        int eventId = 2;

        
        Mock<IEventRepository> mockRepo = new Mock<IEventRepository>();

        mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(_mapper.Map<List<Event>>(mockEvents));
        
        IEventService service = new EventService(mockRepo.Object, _mapper, _validator);

        // Act
        NullReferenceException nullReferenceException =
            await Assert.ThrowsAsync<NullReferenceException>(() =>service.GetEvent(eventId));
        string expected = "Event doesn't exist";

        // Assert
        Assert.Equal(expected,nullReferenceException.Message);
    }
    
    /// <summary>
    /// 4
    /// </summary>
    [Fact]
    public async Task GetEventListFromInvalidUserTest()
    {
        // Arrange
        int id = 1;
        string title = "shabuah";
        int userId = 1;
        string description = "kom gerne og vær med";
        string location = "Tyskland";

        Mock<IEventRepository> mockRepo = new Mock<IEventRepository>();

        mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(_mapper.Map<List<Event>>(mockEvents));
        
        IEventService service = new EventService(mockRepo.Object, _mapper, _validator);

        EventDTO eventdto = service.GetEvent(id).Result;

        eventdto.Title = title;
        eventdto.Description = description;
        eventdto.Location = location;
        
        // Act
        await service.UpdateEvent(eventdto);
        EventDTO actualEventDTO = service.GetEvent(1).Result;

        // Assert
        Assert.Equal(eventdto.Title, actualEventDTO.Title);
        Assert.Equal(eventdto.Description, actualEventDTO.Description);
        Assert.Equal(eventdto.Location, actualEventDTO.Location);
    }
    
    
    
    
    //Test Data
     List<EventDTO> mockEvents = new()
    {
        new EventDTO()
            { Title = "eventTest", Id = 1, Description = "its so fun", Location = "India", User = new User(){Id=1, Name = "Peter", Email = "Min@email.com",Password = "hest",Salt = "HjemmeFest"} },
        new EventDTO()
            { Title = "anotherEvent", Id = 4, Description = "its also fun", Location = "Russia", User = new User(){Id=1, Name = "Peter", Email = "Min@email.com",Password = "hest",Salt = "HjemmeFest" }}, 
        new EventDTO()
                {Title = "A THIRD EVENT", Id = 3, Description = "its also fun", Location = "Money Land", User = new User(){Id=3, Name = "Hans", Email = "Hans@email.com",Password = "MegetKortAdgangsordErBrugtHer",Salt = "UdeFest"}}
            };
    
}