using System.ComponentModel.DataAnnotations;
using OperationStacked.Enums;

namespace OperationStacked.Entities;

public class EquipmentStack
{
    [Key]
    public Guid Id { get; set; }
    public decimal StartWeight { get; set; }
    public ICollection<Decimal> InitialIncrements { get; set; }
    public Decimal IncrementValue { get; set; }
    public decimal IncrementCount { get; set; }
    public string EquipmentStackKey { get; set; }
    public Guid UserID { get; set; }
}
