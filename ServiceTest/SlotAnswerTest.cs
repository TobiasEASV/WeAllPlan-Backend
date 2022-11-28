using Application;
using Application.Interfaces;
using AutoMapper;
using Core;
using FluentValidation;
using Infrastructure;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Moq;

namespace ServiceTest;

public class SlotAnswerTest
{
    private IMapper _mapper;
    private IValidator<SlotAnswerDTO> _validator;

    public SlotAnswerTest()
    {
        var config = new MapperConfiguration(conf => {
            conf.CreateMap<SlotAnswerDTO, SlotAnswer>(); //RegisterUserDto ==> User - create new user
            conf.CreateMap<SlotAnswer,SlotAnswerDTO>();
        });
        _mapper = config.CreateMapper();
        _validator = new SlotAnswerValidator();
    }

    /// <summary>
    /// 1.2
    /// </summary>
    [Fact]
    public void ValidSlotAnswerServiceCreationTest()
    {
        //Arrange
        Mock<ISlotAnswerRepository> mock = new Mock<ISlotAnswerRepository>();
        //Act
        ISlotAnswerService slotAnswerService = new SlotAnswerService(mock.Object, _mapper, _validator);

        //Assert
        Assert.NotNull(slotAnswerService);
        Assert.True(slotAnswerService is SlotAnswerService);
        
    }

    /// <summary>
    /// 1.2
    /// </summary>
    [Fact]
    public void InvalidSlotAnswerServiceCreationTest()
    {
        //Arrange 
        Mock<ISlotAnswerRepository> mock = new Mock<ISlotAnswerRepository>();

        string expected = "Service can not be created without all parameters.";
        
        //Act + Assert
        NullReferenceException noMock = Assert.Throws<NullReferenceException>(() => new SlotAnswerService(null, _mapper, _validator));
        NullReferenceException noMapper = Assert.Throws<NullReferenceException>(() => new SlotAnswerService(mock.Object, null, _validator));
        NullReferenceException noValidator = Assert.Throws<NullReferenceException>(() => new SlotAnswerService(mock.Object, _mapper, null));
        Assert.Equal(expected, noMock.Message);
        Assert.Equal(expected, noMapper.Message);
        Assert.Equal(expected, noValidator.Message);
    }

    
    /// <summary>
    /// 2.1
    /// </summary>
    [Fact]
    public void ValidCreateSlotAnswerTest()
    {
        //Assert
        Mock<ISlotAnswerRepository> mock = new Mock<ISlotAnswerRepository>();

        SlotAnswerDTO answerDTO = new SlotAnswerDTO()
        {
            Answer = 1, Email = "MinEmail@ok.dk", EventSlot = new EventSlot(), UserName = "Carol"
        };

        ISlotAnswerService slotAnswerService = new SlotAnswerService(mock.Object, _mapper, _validator);
        
        //Act
        slotAnswerService.CreateSlotAnswer(answerDTO);

        //Assert
        mock.Verify(repo => repo.CreateSlotAnswer(It.IsAny<SlotAnswer>()),Times.Once());
    }

    /// <summary>
    /// 2.2-2.3
    /// </summary>
    [Theory]
    [MemberData(nameof(TestData.InvalidCreateSlotAnswer), MemberType = typeof(TestData))]
    public void InvalidCreateSlotAnswerTest(SlotAnswerDTO slotAnswerDto, string expected)

    {

        //Arrange
        Mock<ISlotAnswerRepository> mock = new Mock<ISlotAnswerRepository>();

        ISlotAnswerService service = new SlotAnswerService(mock.Object, _mapper, _validator);

        //Act + Assert
        ValidationException actual =
            Assert.ThrowsAsync<ValidationException>(() => service.CreateSlotAnswer(slotAnswerDto)).Result;
        Assert.Equal(expected, actual.Message);
    }

    
    /// <summary>
    /// 3.1
    /// </summary>
    [Fact]
    public void GetValidSlotAnswersTest()
    {
        //Arrange
        int eventSlotId = 1;

        Mock<ISlotAnswerRepository> mock = new Mock<ISlotAnswerRepository>();
        mock.Setup(repo => repo.GetAll()).ReturnsAsync(_mapper.Map<List<SlotAnswer>>(fakeRepo));
        ISlotAnswerService service = new SlotAnswerService(mock.Object, _mapper, _validator);
        
        // Act

        List<SlotAnswerDTO> actual = service.GetSlotAnswer(eventSlotId).Result;

        // Assert
        Assert.True(expectedGetSlotAnswers[0].Id == actual[0].Id && expectedGetSlotAnswers[1].Id == actual[1].Id);
        Assert.Equal(expectedGetSlotAnswers.Count, actual.Count);
    }
    
    /// <summary>
    /// 3.2
    /// </summary>
    [Fact]
    public void GetEmptySlotAnswersTest()
    {
        //Arrange
        int eventSlotId = 3;

        Mock<ISlotAnswerRepository> mock = new Mock<ISlotAnswerRepository>();
        mock.Setup(repo => repo.GetAll()).ReturnsAsync(_mapper.Map<List<SlotAnswer>>(fakeRepo));
        ISlotAnswerService service = new SlotAnswerService(mock.Object, _mapper, _validator);
        
        // Act

        List<SlotAnswerDTO> actual = service.GetSlotAnswer(eventSlotId).Result;

        // Assert
        Assert.Empty(actual);
    }

    /// <summary>
    /// 4.1
    /// </summary>
    [Fact]
    public void ValidUpdateOnSlotAnswerTest()
    {
        //Arrange
        Mock<ISlotAnswerRepository> mockRepo = new Mock<ISlotAnswerRepository>();

        mockRepo.Setup(repository => repository.GetAll()).ReturnsAsync(_mapper.Map<List<SlotAnswer>>(fakeRepo));
        mockRepo.Setup(repository => repository.UpdateSlotAnswer(It.IsAny<SlotAnswer>())).Callback<SlotAnswer>(
            (SlotAnswer) =>
            {
                
            });
        
        ISlotAnswerService service = new SlotAnswerService(mockRepo.Object, _mapper, _validator);

        SlotAnswerDTO slotAnswerDto = expectedGetSlotAnswers[0];
        slotAnswerDto.Answer = 1;

        //Act

        service.UpdateSlotAnswer(slotAnswerDto, slotAnswerDto.Id);

        //Assert
        mockRepo.Verify(repo => repo.UpdateSlotAnswer(It.IsAny<SlotAnswer>()), Times.Once);
    }
    
    ///
    /// 4.2 - 4.3 is not created.
    /// 


    /// <summary>
    /// 4.4 - 4.7
    /// </summary>
    [Theory]
    [MemberData(nameof(TestData.InvalidUpdateOnSlotAnswer), MemberType = typeof(TestData))]
    public void InvalidUpdateOnSlotAnswerTest(SlotAnswerDTO slotAnswerDto, string expected)
    
    {
        //Arrange
        Mock<ISlotAnswerRepository> mockRepo = new Mock<ISlotAnswerRepository>();

        int slotAnswerId = 1;

        ISlotAnswerService service = new SlotAnswerService(mockRepo.Object, _mapper, _validator);
        
        //Act + Assert
        ValidationException actual = Assert.ThrowsAsync<ValidationException>(() => service.UpdateSlotAnswer(slotAnswerDto, slotAnswerId)).Result;
        Assert.Equal(expected, actual.Message);
    }


    /// <summary>
    /// 5.1
    /// </summary>
    [Fact]
    public void ValidDeletionOfSlotAnswerTest()
    {
        // Arrange
        Mock<ISlotAnswerRepository> mockRepo = new Mock<ISlotAnswerRepository>();
        int eventId = 1;
        string mail = "mingus@yo.com";
        
        List<SlotAnswerDTO> listToDelete = new List<SlotAnswerDTO>()
        {
            new SlotAnswerDTO()
            {
                Answer = 0, Email = "mingus@yo.com", Id = 1, EventSlot = eventslot1, UserName = "mingyo"
            }
        };
        
        mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(_mapper.Map<List<SlotAnswer>>(fakeRepo));

        ISlotAnswerService service = new SlotAnswerService(mockRepo.Object, _mapper, _validator);

        // Act

        service.DeleteSlotAnswers(eventId, mail, listToDelete);
        
        // Assert
        mockRepo.Verify(repo => repo.DeleteSlotAnswers(It.IsAny<List<SlotAnswer>>()), Times.Once);
    }
    
    /// <summary>
    /// 5.2
    /// </summary>
    [Fact]
    public void InvalidDeletionOfSlotAnswerTest()
    {
        // Arrange
        Mock<ISlotAnswerRepository> mockRepo = new Mock<ISlotAnswerRepository>();
        int eventId = 2;
        string mail = "mi@yo.com";
        List<SlotAnswerDTO> listToDelete = fakeRepo;
        
        mockRepo.Setup(repo => repo.GetAll()).ReturnsAsync(_mapper.Map<List<SlotAnswer>>(fakeRepo));

        ISlotAnswerService service = new SlotAnswerService(mockRepo.Object, _mapper, _validator);

        string expected = "You do not have permission to delete these answers";

        // Act + Assert
        ValidationException actual =
            Assert.Throws<ValidationException>(() => service.DeleteSlotAnswers(eventId, mail, listToDelete));
        Assert.Equal(expected, actual.Message);
    }
    
    
    
    
    // Data
    //Event Slots
    static EventSlot eventslot1 = new EventSlot()
        { Id = 1, StartTime = DateTime.Now.AddDays(1), EndTime = DateTime.Now.AddDays(2), Event = new Event()
        {
            Id = 1, 
            User = new User()
            {
                Email = "mingus@yo.com", Id = 1, Name = "mikkel", Password = "123sværtatgætte", Salt = "yolo"
            }, 
            Title = "kurts Havefest"
        }};
   
    
    static EventSlot eventslot2 = new EventSlot()
    { Id = 2, StartTime = DateTime.Now.AddDays(2), EndTime = DateTime.Now.AddDays(2), Event = new Event()
    {   
        Id = 2, 
        User = new User()
        {
            Email = "Myemail@yo.com", Id = 2, Name = "mikkel", Password = "123sværtatgætte", Salt = "yolo"
        }, 
        Title = "kurts Fest"
    }};
    
    
    
    List<SlotAnswerDTO> expectedGetSlotAnswers = new List<SlotAnswerDTO>()
    {
        new SlotAnswerDTO()
        {
            Answer = 0, Email = "mingus@yo.com", Id = 1, EventSlot = eventslot1, UserName = "mingyo"
        },
        new SlotAnswerDTO()
        {
            Answer = 1, Email = "mingyo@hotmail.com", Id = 2, EventSlot = eventslot1, UserName = "youstolemyname"
        }
    };
    
    List<SlotAnswerDTO> fakeRepo = new List<SlotAnswerDTO>()
    {
        new SlotAnswerDTO()
        {
            Answer = 0, Email = "mingus@yo.com", Id = 1, EventSlot = eventslot1, UserName = "mingyo"
        },
        new SlotAnswerDTO()
        {
            Answer = 1, Email = "mingyo@hotmail.com", Id = 2, EventSlot = eventslot1, UserName = "youstolemyname"
        },
        new SlotAnswerDTO()
        {
            Answer = 1, Email = "fakeMing@yo.com", Id = 3, EventSlot = eventslot2, UserName = "mingfaker"
        }
    };


}