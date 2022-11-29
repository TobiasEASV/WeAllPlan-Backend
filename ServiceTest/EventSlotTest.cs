using Application;
using Application.DTO;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Core;
using FluentValidation;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
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
        var config = new MapperConfiguration(conf => {
            conf.CreateMap<EventSlotDTO, EventSlot>();
            conf.CreateMap<EventSlot,EventSlotDTO>();
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

    /// <summary>
    /// 2.1
    /// </summary>
    [Fact]
    public void ValidCreateEventSlot()
    {
        // Arrange
        Mock<IEventSlotRepository> mockRepo = new Mock<IEventSlotRepository>();

        mockRepo.Setup(mockRepo => mockRepo.GetAll()).ReturnsAsync(_mapper.Map<List<EventSlot>>(new List<EventSlotDTO>()));
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
        mockRepo.Verify(repo => repo.CreateEventSlot(It.IsAny<List<EventSlot>>()),Times.Never);
        
    }

    [Fact]
    public void ValidUpdateEventSlotTest()
    {
        //Arrange
        Mock<IEventSlotRepository> mockRepo = new Mock<IEventSlotRepository>();
        mockRepo.Setup(mockRepo => mockRepo.GetAll()).ReturnsAsync(_mapper.Map<List<EventSlot>>(validEventSlotDTOs));
        
        IEventSlotService service = new EventSlotService(mockRepo.Object, _mapper, _validator);
        
        EventSlotDTO eventSlotDto = validEventSlotDTOs[0];
        eventSlotDto.StartTime = DateTime.Now;
        eventSlotDto.EndTime = DateTime.Now.AddDays(20);

        int UserId = 1;
        List<EventSlotDTO> eventSlotDtos = new List<EventSlotDTO>()
        {
            eventSlotDto
        };
        //Act
        service.UpdateEventSlot(eventSlotDtos, UserId);
        
        //Arrange
        mockRepo.Verify( repo => repo.UpdateEvent(It.IsAny<List<EventSlot>>()),Times.Never);

    }
    
    
    
    // DATA
    
    //Valid List<EventSlot>
    private List<EventSlotDTO> validEventSlotDTOs = new List<EventSlotDTO>()
    {
        new EventSlotDTO()
        {
            Confirmed = true,
            Event = new Event()
            {
                Description = "BYOB",
                Id = 1,
                Location = "Denmark",
                Title = "Mikkels havefest",
                User = new User()
                {
                    Email = "mikkel@gmail.com", Id = 1, Name = "Mikkel", Password = "123abc", Salt = "321cba"
                },
            },
            Id = 1,
            EndTime =DateTime.Parse("20/02/2500 07:22:16"),
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
        Event = new Event()
        {
            Description = "BYOB",
            Id = 1,
            Location = "Denmark",
            Title = "Mikkels havefest",
            User = new User()
            {
                Email = "mikkel@gmail.com", Id = 1, Name = "Mikkel", Password = "123abc", Salt = "321cba"
            },
        },
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


}