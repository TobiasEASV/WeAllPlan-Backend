using Application;
using Application.Helpers;
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
    private EncryptionService _encryptionService;

    public EventController(IEventService eventService, EncryptionService encryptionService)
    {
        _eventService = eventService;
        _encryptionService = encryptionService;
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
    [Route("GetEventFromInviteLink")]
    public async Task<ActionResult<EventDTO>> GetEventFromInviteLink(string EnctyptedEventId)
    {
        try
        {
            string DecryptedEventId = _encryptionService.DecryptMessage(EnctyptedEventId);

            Console.WriteLine(DecryptedEventId);
            EventDTO eventDto = await _eventService.GetEvent(Int32.Parse(DecryptedEventId));
            return Ok(eventDto);
        }
        catch (Exception e)
        {
            return StatusCode(400, "Invalid invite link.");
        }
    }

    [HttpGet]
    [Route("GenerateInviteLink")]
    public ActionResult<string> GenerateInviteLink(string EventId)
    {
        return Ok(_encryptionService.EncryptMessage(EventId));
    }
}