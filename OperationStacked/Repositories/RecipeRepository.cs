using OperationStacked.Data;
using OperationStacked.Entities;

namespace OperationStacked.Repositories;

public class RecipeRepository : IRecipeRepository
{
    private OperationStackedContext _operationStackedContext;
    public RecipeRepository(OperationStackedContext operationStackedContext)
    {
        _operationStackedContext = operationStackedContext;
    }
    public async Task<Recipe> AddRecipeAsync(Recipe recipe)
    {
        await _operationStackedContext.Recipes.AddAsync(recipe);

        return recipe;
    }
}