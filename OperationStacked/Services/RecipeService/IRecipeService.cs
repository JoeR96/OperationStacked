using OperationStacked.Entities;
using OperationStacked.Requests;

namespace OperationStacked.Services.RecipeService;

public interface IRecipeService
{
    Task<Recipe> CreateRecipeAsync(CreateRecipeRequest request0);
}