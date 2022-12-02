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
    /// 1.1
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

        string expectedRepo = "Repository is null";
        string expectedMapper = "Mapper is null";
        string expectedValidator = "Validator is null";
        
        //Act + Assert
        NullReferenceException noMock = Assert.Throws<NullReferenceException>(() => new SlotAnswerService(null, _mapper, _validator));
        NullReferenceException noMapper = Assert.Throws<NullReferenceException>(() => new SlotAnswerService(mock.Object, null, _validator));
        NullReferenceException noValidator = Assert.Throws<NullReferenceException>(() => new SlotAnswerService(mock.Object, _mapper, null));
        Assert.Equal(expectedRepo, noMock.Message);
        Assert.Equal(expectedMapper, noMapper.Message);
        Assert.Equal(expectedValidator, noValidator.Message);
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
            Answer = 1, Email = "MinEmail@ok.dk", EventSlotId = 1, UserName = "Carol"
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

        SlotAnswerDTO slotAnswerDto = fakeRepoDtos[0];
        slotAnswerDto.Answer = 1;

        //Act

        service.UpdateSlotAnswer(slotAnswerDto, slotAnswerDto.Id);

        //Assert
        mockRepo.Verify(repo => repo.UpdateSlotAnswer(It.IsAny<SlotAnswer>()), Times.Once);
    }
    
    ///
    /// 4.2 - 4.3 is not created.
    /// 


    
    
    
    
    
    
    
    
    
    
    //_____________________________________________________________________________________________
    
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
    
    
    //The expected list for GetSlotAnswers.
    List<SlotAnswerDTO> expectedGetSlotAnswersDtos = new List<SlotAnswerDTO>()
    {
        new SlotAnswerDTO()
        {
            Answer = 0, Email = "mingus@yo.com", Id = 1, EventSlotId = 1, UserName = "mingyo"
        },
        new SlotAnswerDTO()
        {
            Answer = 1, Email = "mingyo@hotmail.com", Id = 2, EventSlotId = 1, UserName = "youstolemyname"
        }
    };
    
    //The expected list for GetSlotAnswers.
    List<SlotAnswer> expectedGetSlotAnswers = new List<SlotAnswer>()
    {
        new SlotAnswer()
        {
            Answer = 0, Email = "mingus@yo.com", Id = 1, EventSlot = eventslot1, UserName = "mingyo"
        },
        new SlotAnswer()
        {
            Answer = 1, Email = "mingyo@hotmail.com", Id = 2, EventSlot = eventslot1, UserName = "youstolemyname"
        }
    };
    
    //Base List
    List<SlotAnswerDTO> fakeRepoDtos = new List<SlotAnswerDTO>()
    {
        new SlotAnswerDTO()
        {
            Answer = 0, Email = "mingus@yo.com", Id = 1, EventSlotId = 1, UserName = "mingyo"
        },
        new SlotAnswerDTO()
        {
            Answer = 1, Email = "mingyo@hotmail.com", Id = 2, EventSlotId = 1, UserName = "youstolemyname"
        },
        new SlotAnswerDTO()
        {
            Answer = 1, Email = "fakeMing@yo.com", Id = 3, EventSlotId = 2, UserName = "mingfaker"
        }
    };
    
    //Base List
    List<SlotAnswer> fakeRepo = new List<SlotAnswer>()
    {
        new SlotAnswer()
        {
            Answer = 0, Email = "mingus@yo.com", Id = 1, EventSlot = eventslot1, UserName = "mingyo"
        },
        new SlotAnswer()
        {
            Answer = 1, Email = "mingyo@hotmail.com", Id = 2, EventSlot = eventslot1, UserName = "youstolemyname"
        },
        new SlotAnswer()
        {
            Answer = 1, Email = "fakeMing@yo.com", Id = 3, EventSlot = eventslot2, UserName = "mingfaker"
        }
    };

}