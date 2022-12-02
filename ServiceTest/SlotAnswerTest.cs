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
}