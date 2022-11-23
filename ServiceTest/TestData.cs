using Application;
using Core;

namespace ServiceTest;

public class TestData
{
    public static IEnumerable<Object> CreateValidEventTestData()
    {
        yield return new EventDTO("Kæmpe fedt event", new User(), "kom glad", "saudi-arabien", new List<EventSlot>());
    }
}
