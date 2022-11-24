using Application;
using Application.Interfaces;
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
    
}