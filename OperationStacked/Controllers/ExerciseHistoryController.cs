using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using OperationStacked.DTOs;
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

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(PaginatedResult<ExerciseHistoryDTO>))]
    public async Task<IActionResult> GetExerciseHistoryById(
        [FromBody] List<Guid> exerciseIds,
        [FromQuery] int pageIndex = 0,
        [FromQuery] int pageSize = 10)
    {
        var results = await _exerciseHistoryService.GetExerciseHistoryByIds(exerciseIds, pageIndex, pageSize);

        var totalCount = results.Count();
        var paginatedResults = new PaginatedResult<ExerciseHistoryDTO>(results, totalCount);

        return Ok(paginatedResults);
    }
     [HttpPost]
     [Route("/all")]
      [ProducesResponseType(200, Type = typeof(List<ExerciseHistoryDTO>))]
      public async Task<IActionResult> GetExerciseHistoryById(
          [FromBody] List<Guid> exerciseIds) =>
          Ok(await _exerciseHistoryService.GetExerciseHistoryByIds(exerciseIds));


      [HttpPost]
      [Route("/delete/{exerciseId}")]
      public async Task<IActionResult> DeleteExerciseHistoryById([FromRoute] Guid exerciseId)
      {
          await _exerciseHistoryService.DeleteExerciseHistoryById(exerciseId);

          return Ok();
      }

}
