using OperationStacked.Entities;
using OperationStacked.Repositories;
using OperationStacked.Requests;
using System.Threading.Tasks;

namespace OperationStacked.Services.RecipeService
{
    public class RecipeService : IRecipeService
    {
        private IRecipeRepository _recipeRepository;

        public RecipeService(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<Recipe> CreateRecipeAsync(CreateRecipeRequest request)
        {
            var stepsString = string.Join(",", request.Steps);

            var recipe = new Recipe()
            {
                Name = request.Name,
                Steps = stepsString,
                UserId = request.UserId,
                Ingredients = request.Ingredients
            };
            
            recipe.Ingredients.ForEach(x => x.RecipeId = recipe.Id);
            await _recipeRepository.AddRecipeAsync(recipe);

            return recipe;
        }
        public Task<Recipe> GetRecipeAsync(Guid userId, Guid recipeId)
        => _recipeRepository.GetRecipeAsync(userId, recipeId);
        
    }
}