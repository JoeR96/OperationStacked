using OperationStacked.Entities;

namespace OperationStacked.Repositories;

public interface IRecipeRepository
{
    Task<Recipe> AddRecipeAsync(Recipe recipe);
    Task<Recipe> GetRecipeAsync(Guid userId, Guid recipeId);
}