using Application;
using Application.Helpers;
using Application.Interfaces;
using Core.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using Xunit.Abstractions;

namespace ServiceTest;

public class AuthenticationTest
{
    private readonly ITestOutputHelper _testOutputHelper;
    private Mock<IUserRepository> _mockRepo;
    private Mock<IOptions<AppSettings>> _mockAppSetting;

    public AuthenticationTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _mockRepo = new Mock<IUserRepository>();
        _mockAppSetting = new Mock<IOptions<AppSettings>>();
        
    }

    /// <summary>
    /// Test Case 1.1
    /// </summary>
    [Fact]
    public void ValidAuthenticationServiceTest()
    {
        // Arrange
        Mock<IUserRepository> mockRepo = new Mock<IUserRepository>();
        
        // Act
        IAuthenticationService authService = new AuthenticationService(_mockRepo.Object, _mockAppSetting.Object);

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
            authService = new AuthenticationService(null, _mockAppSetting.Object));
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
            authService = new AuthenticationService(_mockRepo.Object, null));
        Assert.Equal(expected, ex.Message);
        
    }

    /// <summary>
    /// Test case 2.1 - 2.2
    /// </summary>
    [InlineData( "jan@easv.dk", "jan12345", "John Doe")]
    [InlineData( null, null, null)]
    public void InValidRegisterTest(string email, string password, string name)
    {
        // Arrange
        

        // Act
        
        
        // Assert
      
    }
    
    /// <summary>
    /// Test case 2.3 - 2.6
    /// </summary>
    [InlineData( "jan@easvdk", "jan12345", "John Doe")]
    [InlineData( "janeasv.dk", "jan12345", "John Doe")]
    [InlineData( "jan@.dk", "jan12345", "John Doe")]
    [InlineData( "jan#@easv.dk", "jan12345", "John Doe")]
    public void InValidEmailTest(string email, string password, string name)
    {
        
    }
    
    /// <summary>
    /// Test case 2.7
    /// </summary>
    [InlineData( "jan@easvdk", "jan1234", "John Doe")]
    public void InValidPasswordTest(string email, string password, string name)
    {
        
    }
    
    /// <summary>
    /// Test case 2.8
    /// </summary>
    [InlineData( "jan@easvdk", "jan12345", "")]
    public void InValidNameTest(string email, string password, string name)
    {
        
    }
}