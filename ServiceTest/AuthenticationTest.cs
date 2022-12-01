using System.Security.Cryptography;
using Application;
using Application.DTO;
using Application.Helpers;
using Application.Interfaces;
using Application.Validators;
using AutoMapper;
using Core;
using Core.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Options;
using Moq;
using Xunit.Abstractions;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace ServiceTest;

public class AuthenticationTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private Mock<IUserRepository> _mockRepo;
    private Mock<IOptions<AppSettings>> _mockAppSetting;
    private IAuthenticationService _service;
    private IMapper _mapper;
    private IValidator<RegisterUserDto> _validator;

    public AuthenticationTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _mockRepo = new Mock<IUserRepository>();
        _mockAppSetting = new Mock<IOptions<AppSettings>>();
        _validator = new UserValidator();
        
        
        var config = new MapperConfiguration(conf => {
            conf.CreateMap<RegisterUserDto, User>(); //RegisterUserDto ==> User - create new user
        });
        _mapper = config.CreateMapper();

        _service = new AuthenticationService(_mockRepo.Object, _mockAppSetting.Object, _validator);
    }

    /// <summary>
    /// Test Case 1.1
    /// </summary>
    [Fact]
    public void ValidAuthenticationServiceTest()
    {
        // Arrange
        // Act
        IAuthenticationService authService = new AuthenticationService(_mockRepo.Object, _mockAppSetting.Object, _validator);
        DateTime ex = DateTime.Now.AddDays(200);
        // Assert
        Assert.NotNull(authService);
        Assert.True(authService is IAuthenticationService);
    }

    /// <summary>
    /// Test case 1.2
    /// </summary>
    [Fact]
    public void InvalidRepositoryAuthenticationServiceTest()
    {
        // Arrange
        IAuthenticationService authService;
        string expected = "Can't create service object with null repository.";

        // Act + Assert
        var ex = Assert.Throws<NullReferenceException>(() =>
            authService = new AuthenticationService(null, _mockAppSetting.Object, _validator));
        Assert.Equal(expected, ex.Message);
    }

    /// <summary>
    /// Test case 1.3
    /// </summary>
    [Fact]
    public void InvalidSecretAuthenticationServiceTest()
    {
        // Arrange
        IAuthenticationService authService;
        string expected = "Can't create service object without secret file (appsetting).";

        // Act + Assert
        var ex = Assert.Throws<NullReferenceException>(() =>
            authService = new AuthenticationService(_mockRepo.Object, null, _validator));
        Assert.Equal(expected, ex.Message);
        
    }

    /// <summary>
    /// Test case 2.1
    /// </summary>
    [Fact]
    public void InvalidRegisterUserExcistTest()
    {
        // Arrange
        RegisterUserDto fakeUser = new RegisterUserDto() { Email = "jan@easv.dk", Password = "jan12345", Name = "John Doe" };
        string expected = fakeUser.Email + " is already in use.";

        // Act + assert
        Exception ex = Assert.ThrowsAsync<Exception>(() =>
            _service.Register(fakeUser)).Result;
        
        Assert.Equal(expected, ex.Message);
        _mockRepo.Verify(repository => repository.CreateNewUser(_mapper.Map<User>(fakeUser)), Times.Never);
    }
    
    /// <summary>
    /// Test case 2.2
    /// </summary>
    [Fact]
    public void InvalidRegisterNullValuesTest()
    {
        // Arrange
        RegisterUserDto fakeUser = new RegisterUserDto() { Email = null, Password = null, Name = null };
        string expected = "null is an invalid input.";

        // Act + assert
        Exception ex = Assert.ThrowsAsync<NullReferenceException>(() =>
            _service.Register(fakeUser)).Result;
        
        Assert.Equal(expected, ex.Message);
        _mockRepo.Verify(repository => repository.CreateNewUser(_mapper.Map<User>(fakeUser)), Times.Never);
    }
    
    /// <summary>
    /// Test case 2.3 - 2.5
    /// </summary>
    [Theory]
    [InlineData( "jan@easvdk", "jan12345", "John Doe")]
    [InlineData( "janeasv.dk", "jan12345", "John Doe")]
    [InlineData( "jan@.dk", "jan12345", "John Doe")]
    public void InvalidRegisterEmailTest(string email, string password, string name)
    {
        // Arrange
        string expected = "invalid email, email must follow pattern John.doe@example.com";
        var fakeUser = new RegisterUserDto() { Email = email, Password = password, Name = name };
        
        // Act
        ValidationException ex = Assert.ThrowsAsync<ValidationException>(() =>
            _service.Register(fakeUser)).Result;
        
        // Assert
        Assert.Equal(expected, ex.Message);
        _mockRepo.Verify(repository => repository.CreateNewUser(_mapper.Map<User>(fakeUser)), Times.Never);
    }
    
    /// <summary>
    /// Test case 2.6
    /// </summary>
    [Fact]
    public void InvalidRegisterPasswordTest()
    {
        // Arrange
        string email = "jan@easvdk";
        string password = "jan1234";
        string name = "John Doe";
        string expected = "invalid password, password must be greater than eight characters.";
        var fakeUser = new RegisterUserDto() { Email = email, Password = password, Name = name };
        
        // Act
        ValidationException ex = Assert.ThrowsAsync<ValidationException>(() =>
            _service.Register(fakeUser)).Result;
        
        // Assert
        Assert.Equal(expected, ex.Message);
        _mockRepo.Verify(repository => repository.CreateNewUser(_mapper.Map<User>(fakeUser)), Times.Never);
    }
    
    /// <summary>
    /// Test case 2.7
    /// </summary>
    [Fact]
    public void InvalidRegisterNameTest()
    {
        // Arrange
        string email = "jan@easvdk";
        string password = "jan12345";
        string name = "";
        string expected =  "invalid name, name cannot be empty";
        var fakeUser = new RegisterUserDto() { Email = email, Password = password, Name = name };
        
        // Act
        ValidationException ex = Assert.ThrowsAsync<ValidationException>(() =>
            _service.Register(fakeUser)).Result;
        
        // Assert
        Assert.Equal(expected, ex.Message);
        _mockRepo.Verify(repository => repository.CreateNewUser(_mapper.Map<User>(fakeUser)), Times.Never);
    }
    
    /// <summary>
    /// Test case 3.1
    /// </summary>
    [Fact]
    public void ValidRegisterTest()
    {
        // Arrange
        string email = "jan@easv.dk";
        string password = "jan12345";
        string name = "John Doe";
        string expected = "User successfully registered";
        var fakeUser = new RegisterUserDto() { Email = email, Password = password, Name = name };
        _mockRepo.Setup(UserRepository => UserRepository.GetUserByEmail(fakeUser.Email)).Throws(new KeyNotFoundException());

        // Act
        string actual = _service.Register(fakeUser).Result;

        // Assert
         Assert.Equal(expected, actual);
         _mockRepo.Verify(repository => repository.CreateNewUser(It.IsAny<User>()), Times.Once);
    }
}