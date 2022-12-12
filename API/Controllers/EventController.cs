using System.Security.Authentication;
using Application;
using Application.DTO;
using Application.Helpers;
using Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize]
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
    public async Task<IActionResult> CreateEvent(PostEventDTO eventDto)
    {
        try
        {
            await _eventService.CreateEvent(eventDto);
            return Ok();
        }
        catch (ValidationException e)
        {
            return StatusCode(401, e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    [Route("GetEvent")]
    public async Task<ActionResult<EventDTO>> GetEvent(string eventId)
    {
        try
        {
            EventDTO Event = await _eventService.GetEvent(int.Parse(eventId));
            return Ok(Event);
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
    public async Task<ActionResult<List<EventDTO>>> GetEventsFromUser(string userId)
    {
        try
        {
            List<EventDTO> events = await _eventService.GetEventsFromUser(int.Parse(userId));
            return Ok(events);
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
            _eventService.DeleteEvent(int.Parse(eventId), int.Parse(userId));
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
    [AllowAnonymous]
    [Route("GetEventFromInviteLink")]
    public async Task<ActionResult<EventDTO>> GetEventFromInviteLink(string EncryptedEventId)
    {
        try
        {   string DecryptedEventId = _encryptionService.DecryptMessage(EncryptedEventId + "==");
            EventDTO EventDto = await _eventService.GetEvent(Int32.Parse(DecryptedEventId));
            return Ok(EventDto);
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
        EventDTO EventDto = await _eventService.GetEvent(Int32.Parse(EventId));
        return Ok(EventDto);
    }

    [HttpGet]
    [Route("GenerateInviteLink")]
    public ActionResult<string> GenerateInviteLink(string EventId)
    {
        string GenerateInviteLink = _encryptionService.EncryptMessage(EventId);

        string InviteLink = GenerateInviteLink.Remove(GenerateInviteLink.Length - 2);

        return Ok(InviteLink);
    }
}