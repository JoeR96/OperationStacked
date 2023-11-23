using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using OperationStacked.Entities;
using OperationStacked.Enums;
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
    [Route("sorted")]
    [ProducesResponseType(200, Type = typeof(GetSortedHistoricalExerciseResults))]
    public async Task<IActionResult> GetSortedExerciseHistoryById(
        [FromBody] List<Guid> exerciseIds) =>
        Ok(await _exerciseHistoryService.GetExerciseHistorySortedByCategoryByIds(exerciseIds));

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(GetHistoricalExerciseResults))]
    public async Task<IActionResult> GetExerciseHistoryById(
        [FromBody] List<Guid> exerciseIds) =>
        Ok(await _exerciseHistoryService.GetExerciseHistoryByIds(exerciseIds));
}
public class GetSortedHistoricalExerciseResults
{    public sealed record SortedHistoricalExerciseResults(Dictionary<Category,ExerciseHistory> SortedExercisesByCategory);
}

public class GetHistoricalExerciseResults
{    public sealed record HistoricalExerciseResults(List<ExerciseHistory> SortedExercisesByCategory);
}
