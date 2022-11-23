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
            new EventDTO("Kæmpe fedt event", new User(){Email = "Email",Id = 2,Name = "Mikkel",Password = "Hest",Salt = "Ko"})
        }; 

        //2.2 Return with all fields
        
        yield return new Object[]
        {
            new EventDTO("SIMONS GODE FEST",new User()){Description ="Mega nice fest. Kom glad.", Location = "PÅ SKOLEEEEN"}
        };
    }
}
