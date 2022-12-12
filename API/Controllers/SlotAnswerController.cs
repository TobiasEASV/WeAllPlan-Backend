using System.ComponentModel.DataAnnotations;
using Application;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[Controller]")]

public class SlotAnswerController : Controller
{
    private ISlotAnswerService _answerService;

    public SlotAnswerController(ISlotAnswerService answerService)
    {
        _answerService = answerService;
    }

    [HttpPost]
    [Route("CreateSlotAnswer")]
    public async Task<IActionResult> CreateSlotAnswer(List<SlotAnswerDTO> answerDto)
    {
        try
        {
            await _answerService.CreateSlotAnswer(answerDto);
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
    [Route("GetSlotAnswer")]
    public async Task<ActionResult<List<SlotAnswerDTO>>> GetSlotAnswer(int eventSlotId)
    {
        try
        {
            List<SlotAnswerDTO> SlotAnswer = await _answerService.GetSlotAnswer(eventSlotId);
            return Ok(SlotAnswer);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPut]
    [Route("UpdateSlotAnswer")]
    public async Task<IActionResult> UpdateSlotAnswer(SlotAnswerDTO slotAnswerDto, int slotAnswerId)
    {
        try
        {
            await _answerService.UpdateSlotAnswer(slotAnswerDto, slotAnswerId);
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

    [HttpDelete]
    [Route("DeleteSlotAnswer")]
    public IActionResult DeleteSlotAnswer(int EventId, string Email, List<SlotAnswerDTO> slotAnswerDtos)
    {
        try
        {
            _answerService.DeleteSlotAnswers(EventId, Email, slotAnswerDtos);
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