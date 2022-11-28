using Application;
using Application.Interfaces;
using AutoMapper;
using Core;
using Moq;

namespace ServiceTest;

public class TestData
{
    private IConfigurationProvider _configuration;
    //
    //
    //EVENT 
    //
    //
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

    
    //
    //
    //SLOT ANSWER
    //
    //

    public static IEnumerable<Object[]> InvalidCreateSlotAnswer()
    {
        yield return new object[]
        {
            new SlotAnswerDTO()
            {
                Answer = 1,Email = "MyEmail",EventSlot = new EventSlot(),Id=1,UserName = null
            },
            new string("Username cannot be empty")
        };
        yield return new object[]
        {
            new SlotAnswerDTO()
            {
                Answer = 6,Email = "MyEmail",EventSlot = new EventSlot(),Id=1,UserName = "Mingus"
            },
            new string("Answer has to be no, maybe or yes")
        };
        yield return new object[]
        {
            new SlotAnswerDTO()
            {
                Answer = 1,Email = null,EventSlot = new EventSlot(),Id=1,UserName = "Mikkel"
            },
            new string("E-mail has to be of a correct format")
        };
    }

    public static IEnumerable<Object[]> InvalidUpdateOnSlotAnswer()
    {
        yield return new object[]
        {
            new SlotAnswerDTO()
            {
                Answer = 5, Email = "truemingo@shababab.com", Id = 1, EventSlot = new EventSlot(), UserName = "mingo"
            },
            new string("Answer has to be no, maybe or yes")
        };
        yield return new object[]
        {
            new SlotAnswerDTO()
            {
                Answer = 1, Email = null, Id = 1, EventSlot = new EventSlot(), UserName = "mingo"
            },
            new string("E-mail has to be of a correct format")
        };
        yield return new object[] {
            
        new SlotAnswerDTO()
            {
                Answer = 1, Email = "truemingo@shababab.com", Id = 1, EventSlot = new EventSlot(), UserName = null
            },
        new string("Username cannot be empty")
        };
        yield return new object[]
        {
            new SlotAnswerDTO()
            {
                Answer = 1, Email = "truemingo@shababab.com", Id = 2, EventSlot = new EventSlot(), UserName = "mingo"
            },
            new string("You can only change your own answers")
        };
    }
    
    
    //
    //
    //EVENT SLOT
    //
    //
}
