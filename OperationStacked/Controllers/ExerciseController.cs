﻿using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
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
    [ProducesResponseType(200,Type = typeof(List<Exercise>))]
    public async Task<IActionResult> GetAllExercisesByUserId(
        [FromRoute] Guid userId) => Ok(await _exerciseRepository.GetAllExercisesByUserId(userId));
    
}
