using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using OperationStacked.Entities;
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
    
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(List<ExerciseHistory>))]
    public async Task<IActionResult> GetExerciseHistoryById(
         List<Guid> exerciseIds) =>
        Ok(await _exerciseHistoryService.GetExerciseHistoryById(exerciseIds));
    
    
}