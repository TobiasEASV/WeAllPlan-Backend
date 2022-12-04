using System.Globalization;
using Application;
using Application.DTO;
using Application.Interfaces;
using AutoMapper;
using Core;
using Moq;
using ServiceTest.Helpers;

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
                Title = "Kæmpe fedt event",
                UserId =1,
                EventSlots = new List<EventSlot>()
            }
        };

        //2.2 Return with all fields

        yield return new Object[]
        {
            new EventDTO()
            {
                Title = "SIMONS GODE FEST",
                UserId = 1,
                EventSlots = new List<EventSlot>(),
                Description = "Mega nice fest. Kom glad.",
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
                Title = "",
                UserId = 1,
                EventSlots = new List<EventSlot>()
            },
            new[] { "The event needs a title" }
        };

        //2.3 Invalid Title, doesnt exist.
        yield return new Object[]
        {
            new EventDTO()
            {
                Title = null,
                UserId = 1,
                EventSlots = new List<EventSlot>(),
                Description = "Mega nice fest. Kom glad.",
                Location = "PÅ SKOLEEEEN"
            },
            new[] { "The event needs a title" }
        };

        //2.3 Invalid User, doesnt exist.
        yield return new Object[]
        {
            new EventDTO()
            {
                Title = "ffs",
                UserId = -5,
                EventSlots = new List<EventSlot>(),
                Description = "Mega nice fest. Kom glad.",
                Location = "PÅ SKOLEEEEN"
            },
            new[] { "Event must have an Event Owner" }
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
                Answer = 1, Email = "MyEmail", EventSlotId = 1, Id = 1, UserName = null
            },
            new[]{"Username cannot be empty"}
        };
        yield return new object[]
        {
            new SlotAnswerDTO()
            {
                Answer = 6, Email = "MyEmail", EventSlotId = 1, Id = 1, UserName = "Mingus"
            },
            new[]{"Answer has to be no, maybe or yes"}
           
        };
        yield return new object[]
        {
            new SlotAnswerDTO()
            {
                Answer = 1, Email = null, EventSlotId = 1, Id = 1, UserName = "Mikkel"
            },
            new[]{"E-mail has to be of a correct format"}
        };
    }

    public static IEnumerable<Object[]> InvalidUpdateOnSlotAnswer()
    {
        yield return new object[]
        {
            new SlotAnswerDTO()
            {
                Answer = 5, Email = "truemingo@shababab.com", Id = 1, EventSlotId = 1, UserName = "mingo"
            },
            new[]{"Answer has to be no, maybe or yes"}
        };
        yield return new object[]
        {
            new SlotAnswerDTO()
            {
                Answer = 1, Email = null, Id = 1, EventSlotId = 1, UserName = "mingo"
            },
            new[]{"E-mail has to be of a correct format"}
        };
        yield return new object[]
        {
            new SlotAnswerDTO()
            {
                Answer = 1, Email = "truemingo@shababab.com", Id = 1, EventSlotId =1, UserName = null
            },
            new[]{"Username cannot be empty"}
        };
        yield return new object[]
        {
            new SlotAnswerDTO()
            {
                Answer = 1, Email = "truemingo@shababab.com", Id = 2, EventSlotId = 1, UserName = "mingo"
            },
            new[]{"You can only change your own answers"}
        };
    }


    //
    //
    //EVENT SLOT
    //
    //

    public static IEnumerable<Object[]> InvalidEventSlots()
    {
        yield return new object[]
        {
            new List<EventSlotDTO>()
            {
                new EventSlotDTO()
                {
                    Confirmed = false,
                    EventId = 1,
                    Id = 4,
                    EndTime = NewDate.Today(),
                    SlotAnswers = new List<SlotAnswer>()
                    {
                        new SlotAnswer()
                        {
                            Answer = 1, Email = "Anders@hotmail.com", Id = 1, UserName = "AndersAnd"
                        },
                        new SlotAnswer()
                        {
                            Answer = 2, Email = "Thomas@yahoo.com", Id = 2, UserName = "ThomasTog"
                        }
                    },
                    StartTime = NewDate.Today().AddMinutes(16)
                }
            }
        };
        yield return new object[]
        {
            new List<EventSlotDTO>()
            {
                new EventSlotDTO()
                {
                    Confirmed = false,
                    EventId = 1,
                    Id = 1,
                    EndTime = NewDate.Today(),
                    SlotAnswers = new List<SlotAnswer>()
                    {
                        new SlotAnswer()
                        {
                            Answer = 0, Email = "Anders@hotmail.com", Id = 1, UserName = "AndersAnd",
                        },
                        new SlotAnswer()
                        {
                            Answer = 1, Email = "Thomas@yahoo.com", Id = 2, UserName = "ThomasTog"
                        }
                    },
                    StartTime = NewDate.Today().AddMinutes(16)
                }
            }
        };
    }
}