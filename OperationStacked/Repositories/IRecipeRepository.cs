using OperationStacked.Entities;

namespace OperationStacked.Repositories;

public interface IRecipeRepository
{
    Task<Recipe> AddRecipeAsync(Recipe recipe);
}