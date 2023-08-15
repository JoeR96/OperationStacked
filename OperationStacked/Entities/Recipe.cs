using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace OperationStacked.Entities;
public class Recipe
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid  Id{ get; set; }
    [Required]
    public string Name { get; set; }
    
    public string Steps { get; set; }

    public Guid UserId { get; set; }
    
    public List<RecipeIngredient> Ingredients { get; set; }
}