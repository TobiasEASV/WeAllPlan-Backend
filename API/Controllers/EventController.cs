using Application;
using Application.Interfaces;
using Core;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[Controller]")]
public class EventController : ControllerBase
{
    private IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpPost]
    [Route("CreateEvent")]
    public IActionResult CreateEvent(EventDTO eventDto)
    {
        try
        {
            var x = _eventService.CreateEvent(eventDto); //todo redundant?
            return Ok(x);
        }
        catch (ValidationException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    [Route("TestGetEvent")]
    public async Task<ActionResult<EventDTO>> TestGetEvent()
    {
        return await Task.Run(() =>
            Ok(new EventDTO()
            {
                Title = "testEvent",
                Description = "Dette er noget test data",
                Location = "Esbjerg ESAV Skole",
                User = new User()
                {
                    Id = 110000000,
                    Email = "Tobias@Rasmussen.gmail.dk",
                    Name = "Tobias Rasmussen",
                    Password = "HashPassword",
                    Salt = "SaltPassword"
                },
                EventSlots = new List<EventSlot>()
                {
                    new EventSlot()
                    {
                        Id = 110000000,
                        Confirmed = false,
                        Event = new Event()
                        {
                            Title = "testEvent",
                            Description = "Dette er noget test data",
                            Location = "Esbjerg ESAV Skole",
                            User = new User()
                            {
                                Id = 110000000,
                                Email = "Tobias@Rasmussen.gmail.dk",
                                Name = "Tobias Rasmussen",
                                Password = "HashPassword",
                                Salt = "SaltPassword"
                            },
                            EventSlots = new List<EventSlot>()
                        },
                        StartTime = new DateTime().AddDays(2),
                        SlotAnswers = new List<SlotAnswer>(),
                        EndTime = new DateTime().AddHours(5)
                    },
                    new EventSlot()
                    {
                        Id = 120000000,
                        Confirmed = false,
                        Event = new Event()
                        {
                            Title = "testEvent",
                            Description = "Dette er noget test data",
                            Location = "Esbjerg ESAV Skole",
                            User = new User()
                            {
                                Id = 110000000,
                                Email = "Tobias@Rasmussen.gmail.dk",
                                Name = "Tobias Rasmussen",
                                Password = "HashPassword",
                                Salt = "SaltPassword"
                            },
                            EventSlots = new List<EventSlot>()
                        },
                        StartTime = new DateTime().AddDays(10),
                        SlotAnswers = new List<SlotAnswer>(),
                        EndTime = new DateTime().AddHours(5)
                    },
                    new EventSlot()
                    {
                        Id = 130000000,
                        Confirmed = false,
                        Event = new Event()
                        {
                            Title = "testEvent",
                            Description = "Dette er noget test data",
                            Location = "Esbjerg ESAV Skole",
                            User = new User()
                            {
                                Id = 110000000,
                                Email = "Tobias@Rasmussen.gmail.dk",
                                Name = "Tobias Rasmussen",
                                Password = "HashPassword",
                                Salt = "SaltPassword"
                            },
                            EventSlots = new List<EventSlot>()
                        },
                        StartTime = new DateTime().AddDays(7),
                        SlotAnswers = new List<SlotAnswer>(),
                        EndTime = new DateTime().AddHours(5)
                    },
                    new EventSlot()
                    {
                        Id = 140000000,
                        Confirmed = false,
                        Event = new Event()
                        {
                            Title = "testEvent",
                            Description = "Dette er noget test data",
                            Location = "Esbjerg ESAV Skole",
                            User = new User()
                            {
                                Id = 110000000,
                                Email = "Tobias@Rasmussen.gmail.dk",
                                Name = "Tobias Rasmussen",
                                Password = "HashPassword",
                                Salt = "SaltPassword"
                            },
                            EventSlots = new List<EventSlot>()
                        },
                        StartTime = new DateTime().AddDays(3),
                        SlotAnswers = new List<SlotAnswer>(),
                        EndTime = new DateTime().AddHours(5)
                    }
                },
            }));
    }
}