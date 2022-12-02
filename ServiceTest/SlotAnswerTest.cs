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
}