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

    /// <summary>
    /// 2.1
    /// </summary>
    [Fact]
    public void ValidCreateEventSlot()
    {
        // Arrange
        Mock<IEventSlotRepository> mockRepo = new Mock<IEventSlotRepository>();

        mockRepo.Setup(mockRepo => mockRepo.GetAll()).ReturnsAsync(_mapper.Map<List<EventSlot>>(validEventSlotDTOs));
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
    public void InvalidCreateEventSlot(List<EventSlotDTO> dtos, string expected)
    {
        // Arrange
        Mock<IEventSlotRepository> mockRepo = new Mock<IEventSlotRepository>();

        List<EventSlotDTO> alreadyExists = new List<EventSlotDTO>()
        {
            validEventSlotDTOs[1]
        };

        mockRepo.Setup(mockRepo => mockRepo.GetAll()).ReturnsAsync(_mapper.Map<List<EventSlot>>(alreadyExists));
        IEventSlotService service = new EventSlotService(mockRepo.Object, _mapper, _validator);

        // Act + Assert
        ValidationException actual =
            Assert.ThrowsAsync<ValidationException>(() => service.CreateEventSlot(dtos, dtos[0].Event.Id)).Result;
        Assert.Equal(expected, actual.Message);
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
            EndTime = DateTime.Now.AddDays(2),
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
            StartTime = DateTime.Now.AddDays(1)
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
        EndTime = DateTime.Now.AddDays(3),
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
            StartTime = DateTime.Now.AddDays(2)
        }
    };


}