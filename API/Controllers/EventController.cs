using System.Security.Authentication;
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
    public async Task<IActionResult> CreateEvent(EventDTO eventDto)
    {
        try
        {
            await _eventService.CreateEvent(eventDto);
            return Ok();
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
    [Route("GetEvent")]
    public async Task<IActionResult> GetEvent(int eventId)
    {
        try
        {
            var x = await _eventService.GetEvent(eventId);
            return Ok(x);
        }
        catch (NullReferenceException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpGet]
    [Route("GetEventsFromUser/{id}")]
    public async Task<IActionResult> GetEventsFromUser([FromRoute] int id, int userId)
    {
        try
        {
            if (id == userId)
            {
                var x = await _eventService.GetEventsFromUser(userId);
                return Ok(x);
            }
        }
        catch (NullReferenceException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }

        return BadRequest("You dont have access to this Event");
    }

    [HttpPut]
    [Route("UpdateEvent")]
    public async Task<IActionResult> UpdateEvent(EventDTO eventDto, int userId)
    {
        try
        {
            await _eventService.UpdateEvent(eventDto, userId);
            return Ok();
        }
        catch (ValidationException e)
        {
            return BadRequest(e.Message);
        }
        catch (AuthenticationException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete]
    [Route("DeleteEvent")]
    public IActionResult DeleteEvent(int eventId, int userId)
    {
        try
        {
            _eventService.DeleteEvent(eventId, eventId);
            return Ok();
        }
        catch (NullReferenceException e)
        {
            return BadRequest(e.Message);
        }
        catch (AuthenticationException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}