using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using OperationStacked.Requests;
using OperationStacked.Services.ExerciseProgressionService;

namespace OperationStacked.Controllers;

[ApiController]
[DisplayName("Workout Exercise")]
[Route("workout-exercise/")]
public class WorkoutExerciseController : ControllerBase
{
    private IWorkoutExerciseProgressionService _workoutExerciseProgressionService;

    public WorkoutExerciseController(IWorkoutExerciseProgressionService workoutExerciseProgressionService)
    {
        _workoutExerciseProgressionService = workoutExerciseProgressionService;
    }

    [HttpPut]
    [ProducesResponseType(200, Type = typeof(decimal))]
    [Route("update")]
    public async Task<IActionResult> UpdateWorkoutExerciseById(
        UpdateExerciseRequest request) => Ok(await _workoutExerciseProgressionService.UpdateWorkingWeight(request));
}
