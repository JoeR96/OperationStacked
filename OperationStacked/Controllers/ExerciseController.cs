using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OperationStacked.DTOs;
using OperationStacked.Entities;
using OperationStacked.Repositories;
using OperationStacked.Repositories.WorkoutRepository;
using OperationStacked.Requests;
using OperationStacked.Response;
using OperationStacked.Services.ExerciseCreationService;

namespace OperationStacked.Controllers;

[ApiController]
[DisplayName("Exercise")]
[Route("exercise/")]
public class ExerciseController : ControllerBase
{
    private readonly IExerciseCreationService _exerciseCreationService;
    private readonly IExerciseRepository _exerciseRepository;

    public ExerciseController(IExerciseCreationService exerciseCreationService,
        IExerciseRepository exerciseRepository)
    {
        _exerciseCreationService = exerciseCreationService;
        _exerciseRepository = exerciseRepository;
    }

    [HttpPost("CreateExercises")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Exercise>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateExercisesAsync(
        [FromBody] List<CreateExerciseRequest> requests)
        => Ok(await _exerciseCreationService.CreateExercises(requests));

    [HttpDelete]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Route("{exerciseId:guid}/delete")]
    public async Task<IActionResult> DeleteExercise(
        [FromRoute] Guid exerciseId) =>
        Ok(await _exerciseRepository.DeleteExercise(exerciseId));

    [HttpDelete]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Route("{userId:guid}/delete-all")]
    public async Task<IActionResult> DeleteAllExercisesForUser(
        [FromRoute] Guid userId) =>
        Ok(await _exerciseRepository.DeleteAllExercisesForUser(userId));

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(GetExerciseResult))]
    [Route("{exerciseId:guid}")]
    public async Task<IActionResult> GetExerciseById(
        [FromRoute] Guid exerciseId) =>
        Ok(await _exerciseRepository.GetExerciseById(exerciseId));

    [HttpGet]
    [Route("{userId:guid}/all")]
    [ProducesResponseType(200,Type = typeof(List<ExerciseDto>))]
    public async Task<IActionResult> GetAllExercisesByUserId(
        [FromRoute] Guid userId) => Ok(await _exerciseRepository.GetAllExercisesByUserId(userId));

    [HttpGet("with-histories/{exerciseId:guid}")]
    [ProducesResponseType(200, Type = typeof(Exercise))] // Adjust the return type if you have a specific DTO you'd rather use
    public async Task<IActionResult> GetExerciseWithHistoriesById(
        [FromRoute] Guid exerciseId)
    {
        var exerciseWithHistories = await _exerciseRepository.GetExerciseWithHistoriesById(exerciseId);
        if (exerciseWithHistories == null)
        {
            return NotFound();
        }

        return Ok(exerciseWithHistories);
    }

    [HttpGet]
    [Route("{userId:guid}/generate-basic-exercises")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GenerateBasicExercises(Guid userId)
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "BasicExercises.json");
        string json = await System.IO.File.ReadAllTextAsync(filePath);

        List<Exercise> exercises = JsonConvert.DeserializeObject<List<Exercise>>(json);

        foreach (var exercise in exercises)
        {
            exercise.UserId = userId;
        }


        var exercisesToGenerate = new List<CreateExerciseRequest>();

        foreach (var exercise in exercises)
        {
            var createExerciseRequest = new CreateExerciseRequest
            {
                ExerciseName = exercise.ExerciseName,
                Category = exercise.Category,
                EquipmentType = exercise.EquipmentType,
                UserId = exercise.UserId
            };

            exercisesToGenerate.Add(createExerciseRequest);
        }

        var _ = await _exerciseCreationService.CreateExercises(exercisesToGenerate);

        return Ok();
    }

}
