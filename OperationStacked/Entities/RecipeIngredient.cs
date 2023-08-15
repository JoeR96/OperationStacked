using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OperationStacked.Entities;

public class RecipeIngredient
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public string Measurement { get; set; }
    public Guid RecipeId { get; set; }
}