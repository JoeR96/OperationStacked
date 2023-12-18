using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using OperationStacked.Repositories;
using OperationStacked.Requests;
using OperationStacked.Response;

namespace OperationStacked.Controllers;

[ApiController]
[DisplayName("Equipment Stack")]
[Route("equipment-stack/")]
public class EquipmentStackController : ControllerBase
{
    private readonly IEquipmentStackRepository _equipmentStackRepository;

    public EquipmentStackController(IEquipmentStackRepository equipmentStackRepository)
    {
        _equipmentStackRepository = equipmentStackRepository;
    }

    [HttpPost]
    [ProducesResponseType(200, Type = typeof(EquipmentStackResponse))]
    [Route("create")]
    public async Task<IActionResult> CreateEquipmentStack(
        [FromBody] CreateEquipmentStackRequest equipmentStack) =>
        Ok(await _equipmentStackRepository.InsertEquipmentStack(equipmentStack));

    [HttpGet]
    [ProducesResponseType(200,Type = typeof(List<EquipmentStackResponse>))]
    [Route("{userId}/all")]
    public async Task<IActionResult> GetAllEquipmentStacks([FromRoute] Guid userId) =>
        Ok(await _equipmentStackRepository.GetAllEquipmentStacks(userId));

    [HttpGet]
    [ProducesResponseType(200,Type = typeof(EquipmentStackResponse))]
    [Route("{equipmentStackId}")]
    public async Task<IActionResult> EquipmentStack(
        [FromRoute] Guid equipmentStackId) => Ok(await _equipmentStackRepository.GetEquipmentStack(equipmentStackId));

    [HttpDelete]
    [ProducesResponseType(200, Type = typeof(bool))]
    [Route("{equipmentStackId}")]
    public async Task<IActionResult> DeleteEquipmentStack(
        [FromRoute] Guid equipmentStackId) =>
        Ok(await _equipmentStackRepository.DeleteEquipmentStack(equipmentStackId));
}
