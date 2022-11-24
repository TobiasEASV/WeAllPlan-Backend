using Application;
using Core;
using Moq;

namespace ServiceTest;

public class TestData
{
    public static IEnumerable<Object[]> CreateValidEventTestData()
    {
        //2.1 Some fields filled out
        yield return new Object[]
        {
            new EventDTO()
            {
                Title="Kæmpe fedt event", 
                User =new User(), 
                EventSlots = new List<EventSlot>()
                
            }
        }; 

        //2.2 Return with all fields
        
        yield return new Object[]
        {
            new EventDTO()
            {
                Title= "SIMONS GODE FEST", 
                User =new User(), 
                EventSlots = new List<EventSlot>(),
                Description ="Mega nice fest. Kom glad.", 
                Location = "PÅ SKOLEEEEN"
            }
        };
    }
    
    public static IEnumerable<Object[]> CreateInvalidEventTestData()
    {
        //2.3 Invalid Title, exists, but empty.
        yield return new Object[]
        {
            new EventDTO()
            {
                Title="", 
                User = new User(), 
                EventSlots = new List<EventSlot>()
            },
            new[]{"The event needs a title"}
        }; 

        //2.3 Invalid Title, doesnt exist.
        yield return new Object[]
        {
            new EventDTO()
            {
                Title = null,
                User = new User(), 
                EventSlots = new List<EventSlot>(),
                Description ="Mega nice fest. Kom glad.", 
                Location = "PÅ SKOLEEEEN"
            },
            new[]{"The event needs a title"}
        };
        
        //2.3 Invalid User, doesnt exist.
        yield return new Object[]
        {
            new EventDTO()
            {
                Title = "ffs",
                User = null,
                EventSlots = new List<EventSlot>(),
                Description ="Mega nice fest. Kom glad.", 
                Location = "PÅ SKOLEEEEN"
            },
            new[]{"Event must have an Event Owner"}
        };
    }
    
}
