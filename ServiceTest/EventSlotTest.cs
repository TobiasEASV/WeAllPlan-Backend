using Application;
using Application.DTO;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Core;
using FluentValidation;
using Moq;
using Xunit.Abstractions;

namespace ServiceTest;

public class EventSlotTest
{
    private IMapper _mapper;
    private IValidator<EventSlotDTO> _validator;
    private ITestOutputHelper _outcomeHelper;

    public EventSlotTest(ITestOutputHelper testOutputHelper)
    {
        var config = new MapperConfiguration(conf =>
        {
            conf.CreateMap<EventSlotDTO, EventSlot>();
            conf.CreateMap<EventSlot, EventSlotDTO>();
        });
        _mapper = config.CreateMapper();
        _validator = new EventSlotValidator();
        _outcomeHelper = testOutputHelper;
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
        IEventSlotService service = new EventSlotService(mockRepo.Object, _mapper, _validator);


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
        NullReferenceException noMock =
            Assert.Throws<NullReferenceException>(() => new EventSlotService(null, _mapper, _validator));
        NullReferenceException noMapper =
            Assert.Throws<NullReferenceException>(() => new EventSlotService(mock.Object, null, _validator));
        NullReferenceException noValidator =
            Assert.Throws<NullReferenceException>(() => new EventSlotService(mock.Object, _mapper, null));
        Assert.Equal("Repository is null", noMock.Message);
        Assert.Equal("Mapper is null", noMapper.Message);
        Assert.Equal("Validator is null", noValidator.Message);
    }

    /// <summary>
    /// 2.1
    /// </summary>
    [Fact]
    public void ValidCreateEventSlot()
    {
        // Arrange
        Mock<IEventSlotRepository> mockRepo = new Mock<IEventSlotRepository>();

        mockRepo.Setup(mockRepo => mockRepo.GetAll())
            .ReturnsAsync(_mapper.Map<List<EventSlot>>(new List<EventSlotDTO>()));
        IEventSlotService service = new EventSlotService(mockRepo.Object, _mapper, _validator);

        // Act
        service.CreateEventSlot(validEventSlotDTOs, 1);

        // Assert
        mockRepo.Verify(repo => repo.CreateEventSlot(It.IsAny<List<EventSlot>>()), Times.Once);
    }

    /// <summary>
    /// 2.2
    /// </summary>
    [Theory]
    [MemberData(nameof(TestData.InvalidEventSlots), MemberType = typeof(TestData))]
    public void InvalidCreateEventSlot(List<EventSlotDTO> dtos)
    {
        // Arrange
        Mock<IEventSlotRepository> mockRepo = new Mock<IEventSlotRepository>();

        List<EventSlotDTO> alreadyExists = new List<EventSlotDTO>()
        {
            validEventSlotDTOs[1]
        };

        mockRepo.Setup(mockRepo => mockRepo.GetAll()).ReturnsAsync(_mapper.Map<List<EventSlot>>(alreadyExists));
        IEventSlotService service = new EventSlotService(mockRepo.Object, _mapper, _validator);

        // Act 
        service.CreateEventSlot(dtos, 1);

        //Assert
        mockRepo.Verify(repo => repo.CreateEventSlot(It.IsAny<List<EventSlot>>()), Times.Never);
    }

    /// <summary>
    /// 3.1
    /// </summary>
    [Fact]
    public void GetAllEventSlotsFromEventID()
    {
        //Arrange
        Mock<IEventSlotRepository> mockRepo = new Mock<IEventSlotRepository>();
        mockRepo.Setup(mockRepo => mockRepo.GetAll()).ReturnsAsync(validEventSlot);

        int EventId = 1;

        IEventSlotService service = new EventSlotService(mockRepo.Object, _mapper, _validator);
        //Act
        List<EventSlotDTO> eventSlotDtos = service.GetEventSlots(EventId).Result;

        //Assert
        mockRepo.Verify(repo => repo.GetAll(), Times.Once);
    }

    /// <summary>
    /// 3.2
    /// </summary>
    [Fact]
    public void GetAllEventSlotsFromEventIDWithNoEventSlots()
    {
        //Arrange
        Mock<IEventSlotRepository> mockRepo = new Mock<IEventSlotRepository>();
        mockRepo.Setup(mockRepo => mockRepo.GetAll()).ReturnsAsync(validEventSlot);

        int EventId = 2;

        IEventSlotService service = new EventSlotService(mockRepo.Object, _mapper, _validator);
        //Act
        List<EventSlotDTO> eventSlotDtos = service.GetEventSlots(EventId).Result;

        //Assert
        mockRepo.Verify(repo => repo.GetAll(), Times.Once);
    }

    /// <summary>
    /// 4.1
    /// </summary>
    [Fact]
    public void ValidUpdateEventSlotTest()
    {
        //Arrange
        Mock<IEventSlotRepository> mockRepo = new Mock<IEventSlotRepository>();
        mockRepo.Setup(mockRepo => mockRepo.GetAll()).ReturnsAsync(validEventSlot);


        IEventSlotService service = new EventSlotService(mockRepo.Object, _mapper, _validator);

        EventSlotDTO eventSlotDto = validEventSlotDTOs[0];
        eventSlotDto.StartTime = DateTime.Now.AddDays(2);
        eventSlotDto.EndTime = DateTime.Now.AddDays(20);

        int UserId = 1;
        List<EventSlotDTO> eventSlotDtos = new List<EventSlotDTO>()
        {
            eventSlotDto
        };
        //Act
        service.UpdateEventSlot(eventSlotDtos, UserId);

        //Arrange
        mockRepo.Verify(repo => repo.UpdateEventSlot(It.IsAny<List<EventSlot>>()), Times.Once);
    }

    /// <summary>
    /// 4.2-4.4
    /// </summary>
    [Fact]
    public void InvalidUpdateEventSlotTest()
    {
        //Arrange
        Mock<IEventSlotRepository> mockRepo = new Mock<IEventSlotRepository>();
        mockRepo.Setup(mockRepo => mockRepo.GetAll()).ReturnsAsync(validEventSlot);


        IEventSlotService service = new EventSlotService(mockRepo.Object, _mapper, _validator);

        EventSlotDTO eventSlotDto = validEventSlotDTOs[0];
        eventSlotDto.StartTime = DateTime.Now.AddHours(2);
        eventSlotDto.EndTime = DateTime.Now.AddDays(-5); //Error

        EventSlotDTO eventSlotDto1 = validEventSlotDTOs[0];
        eventSlotDto1.StartTime = DateTime.Now; //Error
        eventSlotDto1.EndTime = DateTime.Now.AddDays(5);

        int UserId = 1;
        List<EventSlotDTO> eventSlotDtos = new List<EventSlotDTO>()
        {
            eventSlotDto, eventSlotDto1
        };
        //Act
        service.UpdateEventSlot(eventSlotDtos, UserId);

        //Arrange
        mockRepo.Verify(repo => repo.UpdateEventSlot(It.IsAny<List<EventSlot>>()), Times.Never);
    }

    /// <summary>
    /// 5.1
    /// </summary>

    [Fact]
    public void ValidDeleteEventSlotTest()
    {
        //Arrange
        Mock<IEventSlotRepository> mockRepo = new Mock<IEventSlotRepository>();
        mockRepo.Setup(mockRepo => mockRepo.GetAll()).ReturnsAsync(validEventSlot);
        mockRepo.Setup(mockRepo => mockRepo.getEventFromId(It.IsAny<int>())).ReturnsAsync(eventIdOne);
        List<EventSlotDTO> listToDelete = new List<EventSlotDTO>()
        {
            validEventSlotDTOs[1]
        };
        int userId = 1;

        IEventSlotService service = new EventSlotService(mockRepo.Object, _mapper, _validator);
        //Act
        service.DeleteEventSlots(listToDelete, userId);

        //Assert
        mockRepo.Verify(repo => repo.DeleteEventSlot(It.IsAny<List<EventSlot>>()), Times.Once);
    }
    
    /// <summary>
    /// 5.2
    /// </summary>
    [Fact]
    public void InvalidDeleteEventSlotTest()
    {
        //Arrange
        Mock<IEventSlotRepository> mockRepo = new Mock<IEventSlotRepository>();
        mockRepo.Setup(mockRepo => mockRepo.GetAll()).ReturnsAsync(validEventSlot);
        mockRepo.Setup(mockRepo => mockRepo.getEventFromId(It.IsAny<int>())).ReturnsAsync(eventIdOne);
        List<EventSlotDTO> listToDelete = new List<EventSlotDTO>()
        {
            validEventSlotDTOs[1]
        };
        int userId = 5;

        IEventSlotService service = new EventSlotService(mockRepo.Object, _mapper, _validator);

        string expected = "You do not own this Event.";
        //Act + Assert
        ValidationException actual = Assert.Throws<ValidationException>(() => service.DeleteEventSlots(listToDelete,userId));
        Assert.Equal(expected,actual.Message);
    }

    // DATA

    //Valid List<EventSlot>
    private List<EventSlotDTO> validEventSlotDTOs = new List<EventSlotDTO>()
    {
        new EventSlotDTO()
        {
            Confirmed = false,
            EventId =1,
            Id = 1,
            EndTime = DateTime.Parse("20/02/2500 07:22:16"),
            SlotAnswers = new List<SlotAnswer>()
            {
                new SlotAnswer()
                {
                    Answer = 1, Email = "Anders@hotmail.com", Id = 1, UserName = "AndersAnd"
                },
                new SlotAnswer()
                {
                    Answer = 2, Email = "Thomas@yahoo.com", Id = 2, UserName = "ThomasTog"
                }
            },
            StartTime = DateTime.Parse("18/02/2500 07:22:16")
        },
        new EventSlotDTO()
        {
            Confirmed = false,
            EventId = 1,
            Id = 1,
            EndTime = DateTime.Parse("08/07/2500 07:22:16"),
            SlotAnswers = new List<SlotAnswer>()
            {
                new SlotAnswer()
                {
                    Answer = 0, Email = "Anders@hotmail.com", Id = 1, UserName = "AndersAnd"
                },
                new SlotAnswer()
                {
                    Answer = 1, Email = "Thomas@yahoo.com", Id = 2, UserName = "ThomasTog"
                }
            },
            StartTime = DateTime.Parse("08/06/2500 07:22:16")
        }
    };
    
    //EventSlots
    private List<EventSlot> validEventSlot = new List<EventSlot>()
    {
        new EventSlot()
        {
            Confirmed = false,
            Event = eventIdOne,
            Id = 1,
            EndTime = DateTime.Parse("20/02/2500 07:22:16"),
            SlotAnswers = new List<SlotAnswer>()
            {
                new SlotAnswer()
                {
                    Answer = 1, Email = "Anders@hotmail.com", Id = 1, UserName = "AndersAnd"
                },
                new SlotAnswer()
                {
                    Answer = 2, Email = "Thomas@yahoo.com", Id = 2, UserName = "ThomasTog"
                }
            },
            StartTime = DateTime.Parse("18/02/2500 07:22:16")
        },
        new EventSlot()
        {
            Confirmed = false,
            Event = eventIdOne,
            Id = 1,
            EndTime = DateTime.Parse("08/07/2500 07:22:16"),
            SlotAnswers = new List<SlotAnswer>()
            {
                new SlotAnswer()
                {
                    Answer = 0, Email = "Anders@hotmail.com", Id = 1, UserName = "AndersAnd"
                },
                new SlotAnswer()
                {
                    Answer = 1, Email = "Thomas@yahoo.com", Id = 2, UserName = "ThomasTog"
                }
            },
            StartTime = DateTime.Parse("08/06/2500 07:22:16")
        }
    };
    
    //Events
    public static Event eventIdOne = new Event()
    {
        Description = "BYOB",
        Id = 1,
        Location = "Denmark",
        Title = "Mikkels havefest",
        User = new User()
        {
            Email = "mikkel@gmail.com", Id = 1, Name = "Mikkel", Password = "123abc", Salt = "321cba"
        },
    };
}