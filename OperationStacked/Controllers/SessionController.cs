using Microsoft.AspNetCore.Mvc;
using OperationStacked.Entities;
using System.ComponentModel;
using OperationStacked.Requests;
using OperationStacked.Response;
using OperationStacked.Services;

namespace OperationStacked.Controllers;

[ApiController]
[DisplayName("Session")]
[Route("api/session")]
public class SessionController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(SessionCreatedResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateSession([FromBody] Session session)
    {
        try
        {
            var createdSession = await _sessionService.CreateSessionAsync(session);
            return Ok(new SessionCreatedResponse(createdSession.SessionName, createdSession.SessionStartDateTime));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{sessionId}/exercise")]
    public async Task<IActionResult> AddExerciseToSession([FromBody] AddExerciseToSessionRequest sessionRequest)
    {
        try
        {
            await _sessionService.AddExerciseToSessionAsync(sessionRequest);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("add-set")]
    public async Task<IActionResult> AddSetToExercise([FromBody] AddSetRequest addSetRequest)
    {
        try
        {
            await _sessionService.AddSetToExerciseAsync(addSetRequest);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpDelete("delete-set")]
    public async Task<IActionResult> DeleteSet([FromBody] DeleteSetRequest deleteSetRequest)
    {
        try
        {
             await _sessionService.RemoveSetFromExerciseAsync(deleteSetRequest);

             return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{sessionId}")]
    [ProducesResponseType(typeof(Session), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSessionById(Guid sessionId)
    {
        try
        {
            var session = await _sessionService.GetSessionByIdAsync(sessionId);
            if (session == null)
            {
                return NotFound($"Session with ID {sessionId} was not found.");
            }

            return Ok(session);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
}
