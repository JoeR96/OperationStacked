using System.Text.Json;
using OperationStacked.Entities;

namespace OperationStacked.Requests;

public class CreateRecipeRequest
{
    public Guid UserId { get; set; }
    public string Name { get; set; }

    public List<string> Steps { get; set; }
    
    public List<RecipeIngredient> Ingredients { get; set; }
}