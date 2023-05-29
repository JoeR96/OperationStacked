using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using OperationStacked.Entities;
using OperationStacked.Repositories;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Controllers;

[ApiController]
[DisplayName("Equipment Stack")]
[Route("equipment-stack/")]
public class EquipmentStackController : ControllerBase
{
    private readonly IExerciseRepository _exerciseRepository;

    public EquipmentStackController(IExerciseRepository exerciseRepository)
    {
        _exerciseRepository = exerciseRepository;
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(EquipmentStackResponse))]
    [Route("create")]
    public async Task<IActionResult> CreateEquipmentStack(
        [FromBody] CreateEquipmentStackRequest equipmentStack) =>
        Ok(await _exerciseRepository.InsertEquipmentStack(equipmentStack));

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(EquipmentStackResponse))]
    [Route("{equipmentStackId}")]
    public async Task<IActionResult> EquipmentStack(
        [FromRoute] Guid equipmentStackId) => Ok(await _exerciseRepository.GetEquipmentStack(equipmentStackId));

    [HttpDelete]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Route("{equipmentStackId}")]
    public async Task<IActionResult> DeleteEquipmentStack(
        [FromRoute] Guid equipmentStackId) =>
        Ok(await _exerciseRepository.DeleteEquipmentStack(equipmentStackId));
}