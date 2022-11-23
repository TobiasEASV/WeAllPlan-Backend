using Application;
using Application.Interfaces;
using Moq;

namespace ServiceTest;

public class EventTest
{
    /// <summary>
    /// 1.1
    /// </summary>
    [Fact]
    public void ValidEventServiceTest()
    {
        // Arrange
        Mock<IRepository> mockRepo = new Mock<IRepository>();

        // Act
        IEventService service = new EventService(mockRepo.Object);
        

        // Assert
        Assert.NotNull(service);
        Assert.True(service is EventService);
    }
    
    /// <summary>
    /// 1.2
    /// </summary>
    [Fact]
    public void InvalidEventServiceTest()
    {
        // Arrange
        IEventService service;
        string expected = "Repository is null";
        
        // Act + Assert
        var ex = Assert.Throws<NullReferenceException>(() => service = new EventService(null));
        Assert.Equal(expected, ex.Message);
    }
    
    [Theory]
    [MemberData(nameof(TestData.CreateValidEventTestData), MemberType = typeof(TestData))]
    
    
}