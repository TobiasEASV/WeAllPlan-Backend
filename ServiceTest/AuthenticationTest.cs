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

    public AuthenticationTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    /// <summary>
    /// Test Case 1.1
    /// </summary>
    [Fact]
    public void ValidAuthenticationServiceTest()
    {
        // Arrange
        Mock<IUserRepository> mockRepo = new Mock<IUserRepository>();
        Mock<IOptions<AppSettings>> mockAppSetting = new Mock<IOptions<AppSettings>>();

        // Act
        IAuthenticationService authService = new AuthenticationService(mockRepo.Object, mockAppSetting.Object);

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

        Mock<IOptions<AppSettings>> mockAppSetting = new Mock<IOptions<AppSettings>>();

        // Act + Assert
        var ex = Assert.Throws<NullReferenceException>(() =>
            authService = new AuthenticationService(null, mockAppSetting.Object));
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

        Mock<IUserRepository> mockRepo = new Mock<IUserRepository>();

        // Act + Assert
        var ex = Assert.Throws<NullReferenceException>(() =>
            authService = new AuthenticationService(mockRepo.Object, null));
        Assert.Equal(expected, ex.Message);
        
    }

    /// <summary>
    /// Test case 2.1
    /// </summary>
    [Fact]
    public void InValidRegisterTest()
    {
    }
}