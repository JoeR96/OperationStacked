using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using OperationStacked.Entities;
using OperationStacked.Repositories;
using OperationStacked.Requests;
using OperationStacked.Services.RecipeService;

namespace OperationStacked.Controllers;

[ApiController]
[DisplayName("Recipe")]
[Route("recipe/")]
public class RecipeController : ControllerBase
{
    private readonly IRecipeService _recipeService;
    private readonly IRecipeRepository _recipeRepository;
    public RecipeController(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(Recipe))]
    public async Task<IActionResult> CreateRecipeAsync([FromBody] CreateRecipeRequest request)
    {
        return Ok(await _recipeService.CreateRecipeAsync(request));
    }
    
    //[Route("/{userId}/{recipeId}")]
    //[HttpGet]
    //[ProducesResponseType(200, Type = typeof(Recipe))]
    //public async Task<IActionResult> GetRecipeAsync([FromRoute] Guid userId,Guid recipeId)
    //{
    //    return Ok(await _recipeService.GetRecipeAsync(userId,recipeId));
    //}
}