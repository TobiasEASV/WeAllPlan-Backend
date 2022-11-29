using System.Security.Authentication;
using Application;
using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Core;
using FluentValidation;
using Moq;
using Xunit.Abstractions;

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
        Mock<IEventRepository> mock = new Mock<IEventRepository>();
        
        // Act + Assert
        NullReferenceException noMock = Assert.Throws<NullReferenceException>(() => new EventService(null, _mapper,_validator ));
        NullReferenceException noMapper = Assert.Throws<NullReferenceException>(() => new EventService(mock.Object, null,_validator ));
        NullReferenceException noValidator = Assert.Throws<NullReferenceException>(() => new EventService(mock.Object, _mapper,null ));
        Assert.Equal("Repository is null", noMock.Message);
        Assert.Equal("Mapper is null", noMapper.Message);
        Assert.Equal("Validator is null", noValidator.Message);
        
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
        Task<ValidationException> validationException = Assert.ThrowsAsync<ValidationException>(() => service.CreateEvent(eventDto));
        Assert.Equal(expected.First(), validationException.Result.Message);
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

        mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(mockEvents);
        
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
                UserId = 1
            },
            new EventDTO()
            {
                Title = "anotherEvent", Id = 4, Description = "its also fun", Location = "Russia",
                UserId = 1
            },
        };
        Mock<IEventRepository> mockRepo = new Mock<IEventRepository>();

        mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(mockEvents);
        mockRepo.Setup(mockRepo => mockRepo.getUser(UserId)).Returns(userIdOne);
        
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

        mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(mockEvents);
        
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

        mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(mockEvents);
        
        IEventService service = new EventService(mockRepo.Object, _mapper, _validator);

        // Act
        NullReferenceException nullReferenceException =
            await Assert.ThrowsAsync<NullReferenceException>(() =>service.GetEvent(eventId));
        string expected = "Event doesn't exist";

        // Assert
        Assert.Equal(expected,nullReferenceException.Message);
    }
    
    /// <summary>
    /// 4.1 - 4.3
    /// </summary>
    [Fact]
    public async Task UpdateValidEventTest()
    {
        // Arrange
        EventDTO eventDTO = new EventDTO()
        {
            Id = 1,
            Location = "Tyskland",
            Title = "shabuah",
            Description = "kom gerne og vær med",
            UserId = 1
        };
        int id = 1;
        string title = "shabuah";
        int userId = 1;
        string description = "kom gerne og vær med";
        string location = "Tyskland";

        Mock<IEventRepository> mockRepo = new Mock<IEventRepository>();

        mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(mockEvents);
        
       

        mockRepo.Setup(repo => repo.getUser(userId)).Returns(userIdOne);
        
        IEventService service = new EventService(mockRepo.Object, _mapper, _validator);

        //EventDTO eventdto = service.GetEvent(id).Result;
        
        // Act
         await service.UpdateEvent(eventDTO, userId);
        
        // Assert
        mockRepo.Verify(repo => repo.UpdateEvent(It.IsAny<Event>()), Times.Once);
        
    }
    
    /// <summary>
    /// 4.7
    /// </summary>
    [Fact]
    public async Task UpdateInvalidWithBadTitleEventTest()
    {
        Mock<IEventRepository> mockRepo = new Mock<IEventRepository>();

        mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(mockEvents);
        IEventService service = new EventService(mockRepo.Object, _mapper, _validator);

        EventDTO eventdto = service.GetEvent(1).Result;
        eventdto.Title = null;

        string expected = "The event needs a title";
        
        // Act + Assert
        ValidationException validationException =
            Assert.ThrowsAsync<ValidationException>(() => service.UpdateEvent(eventdto,eventdto.UserId)).Result;
        Assert.Equal(expected,validationException.Message);

    }
    
    /// <summary>
    /// 4.8
    /// </summary>
    [Fact]
    public async Task UpdateInvalidWithBadUserIdEventTest()
    {
        Mock<IEventRepository> mockRepo = new Mock<IEventRepository>();

        mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(mockEvents);
        IEventService service = new EventService(mockRepo.Object, _mapper, _validator);

        EventDTO eventdto = service.GetEvent(1).Result;
        

        string expected = "Wrong User";
        
        // Act + Assert
        AuthenticationException validationException =
            Assert.ThrowsAsync<AuthenticationException>(() => service.UpdateEvent(eventdto,5)).Result;
        Assert.Equal(expected,validationException.Message);

    }

    /// <summary>
    /// 5.1
    /// </summary>
    [Fact]
    public void DeleteValidEventTest()
    {
        //Assert
        Mock<IEventRepository> mockRepo = new Mock<IEventRepository>();
        mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(_mapper.Map<List<Event>>(mockEvents));
        
        IEventService service = new EventService(mockRepo.Object, _mapper,_validator);
        int eventId = 1;
        int userId = 1;

        //Act
        service.DeleteEvent(eventId, userId);
        
        //Assert
        mockRepo.Verify(repo => repo.Delete(It.IsAny<Event>()), Times.Once);
    }
    
    /// <summary>
    /// 5.2
    /// </summary>
    [Fact]
    public void DeleteInvalidEventTest()
    {
        //Assert
        Mock<IEventRepository> mockRepo = new Mock<IEventRepository>();
        mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(mockEvents);
        
        IEventService service = new EventService(mockRepo.Object, _mapper,_validator);
        int eventId = 1;
        int userId = 5;

        string expected = "You do not own this Event";

        //Act + Assert
        AuthenticationException authenticationException =
            Assert.Throws<AuthenticationException>(() => service.DeleteEvent(eventId, userId));
        Assert.Equal(expected,authenticationException.Message);
    }
    
    
    //Test Data
     List<Event> mockEvents = new()
    {
        new Event()
            { Title = "eventTest", Id = 1, Description = "its so fun", Location = "India", User= userIdOne },
        new Event()
            { Title = "anotherEvent", Id = 4, Description = "its also fun", Location = "Russia", User= userIdOne}, 
        new Event()
                {Title = "A THIRD EVENT", Id = 3, Description = "its also fun", Location = "Money Land", User=userIdThree}
            };

     static User userIdOne = new User()
     {
         Email = "Mig@Hotmail.com", Id = 1, Name = "Mikkel", Password = "PASSS", Salt = "SAAALT"
     };
    static private User userIdThree = new User()
     {
         Email = "dig@Hotmail.com", Id = 3, Name = "Mikkeline", Password = "PASSssap", Salt = "SAAALT"
     };

}