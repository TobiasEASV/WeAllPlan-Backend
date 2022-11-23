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
}
