using System.ComponentModel.DataAnnotations;
using Application.DTO;
using Application.Interfaces;
using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("[Controller]")]


public class EventSlotController : Controller
{
    private IEventSlotService _slotService;

    public EventSlotController(IEventSlotService slotService)
    {
        _slotService = slotService;
    }

    [HttpPost]
    [Route("CreateEventSlot")]
    public async Task<IActionResult> CreateEventSlot(List<EventSlotDTO> eventSlotDtos, int eventId)
    {
        try
        {
            await _slotService.CreateEventSlot(eventSlotDtos, eventId);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    [Route("GetEventSlots")]
    public async Task<ActionResult<EventSlotDTO>> GetEventSlots(int eventId)
    {
        try
        {
            List<EventSlotDTO> EventSlots = await _slotService.GetEventSlots(eventId);
            return Ok(EventSlots);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut]
    [Route("UpdateEventSlot")]
    public async Task<IActionResult> UpdateEventSlot (List<EventSlotDTO> eventSlotDto, int userId)
    {
        try
        {
            _slotService.UpdateEventSlot(eventSlotDto, userId);
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete]
    [Route("DeleteEventSlots")]
    public async Task<IActionResult> DeleteEventSlots(List<EventSlotDTO> listToDelete, int userId)
    {
        try
        {
            _slotService.DeleteEventSlots(listToDelete, userId);
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
}