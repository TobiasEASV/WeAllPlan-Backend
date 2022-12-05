using System.Security.Authentication;
using Application;
using Application.Helpers;

using Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[Controller]")]
public class EventController : ControllerBase
{
    private IEventService _eventService;
    private EncryptionService _encryptionService;

    public EventController(IEventService eventService, EncryptionService encryptionService)
    {
        _eventService = eventService;
        _encryptionService = encryptionService;
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
    [Route("GetEventsFromUser")]
    public async Task<IActionResult> GetEventsFromUser(string userId)
    {
        try
        {
            
                var x = await _eventService.GetEventsFromUser(int.Parse(userId));
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
    public IActionResult DeleteEvent(string eventId, string userId)
    {
        try
        {
            _eventService.DeleteEvent(int.Parse(eventId), int.Parse(eventId));
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

    [HttpGet]
    [Route("GetEventFromInviteLink")]
    public async Task<ActionResult<EventDTO>> GetEventFromInviteLink(string EncryptedEventId)
    {
        try
        {
            string DecryptedEventId = _encryptionService.DecryptMessage(EncryptedEventId);
            EventDTO eventDto = await _eventService.GetEvent(Int32.Parse(DecryptedEventId));
            return Ok(eventDto);
        }
        catch (Exception e)
        {
            return StatusCode(400, "Invalid invite link.");
        }
    } 
    
    [HttpGet]
    [Route("GetEventToAnswer")]
    public async Task<ActionResult<EventDTO>> GetEventToAnswer(string EventId)
    {
        EventDTO eventDto = await _eventService.GetEvent(Int32.Parse(EventId));
        return Ok(eventDto);
    }

    [HttpGet]
    [Route("GenerateInviteLink")]
    public ActionResult<string> GenerateInviteLink(string EventId)
    {
        var test = _encryptionService.EncryptMessage(EventId);
        Console.WriteLine(test);
        return Ok(test);

    }
    
}
