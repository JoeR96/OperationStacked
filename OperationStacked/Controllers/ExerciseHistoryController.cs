using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using OperationStacked.Response;
using OperationStacked.Services;

namespace OperationStacked.Controllers;

[ApiController]
[DisplayName("Exercise History")]
[Route("exercise-history/")] 
public class ExerciseHistoryController : ControllerBase
{
    private readonly IExerciseHistoryService _exerciseHistoryService;

    public ExerciseHistoryController(IExerciseHistoryService exerciseHistoryService)
    {
        _exerciseHistoryService = exerciseHistoryService;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(GetExerciseResult))]
    [Route("{exerciseId}")]
    public async Task<IActionResult> GetExerciseHistoryById(
        [FromRoute] Guid exerciseId) =>
        Ok(await _exerciseHistoryService.GetExerciseHistoryById(exerciseId));
}