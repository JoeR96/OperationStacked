using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace OperationStacked.Entities;

public class Recipe
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public string Steps { get; set; }
    
    public List<string> GetSteps()
    {
        return JsonSerializer.Deserialize<List<string>>(Steps);
    }
    
    public void SetSteps(List<string> steps)
    {
        Steps = JsonSerializer.Serialize(steps);
    }
    
    public Guid UserId { get; set; }
    
    public List<Ingredient> Ingredients { get; set; }
}