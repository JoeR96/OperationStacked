using Microsoft.EntityFrameworkCore;
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
        await _operationStackedContext.Ingredients.AddRangeAsync(recipe.Ingredients);
        await _operationStackedContext.Recipes.AddAsync(recipe);
        await _operationStackedContext.SaveChangesAsync();
        return recipe;
    }

    public async Task<Recipe> GetRecipeAsync(Guid userId, Guid recipeId) => 
        await _operationStackedContext.Recipes.Where(x => x.UserId == userId && x.Id == recipeId)
            .FirstOrDefaultAsync();
    
}