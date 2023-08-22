using System.ComponentModel.DataAnnotations;
using OperationStacked.Enums;

namespace OperationStacked.Entities;

public class EquipmentStack
{
    [Key]
    public Guid Id { get; set; }
    public decimal StartWeight { get; set; }
    public decimal?[] InitialIncrements { get; set; }
    public Decimal IncrementValue { get; set; }
    public double IncrementCount { get; set; }
    public string EquipmentStackKey { get; set; }
    public Guid UserID { get; set; }
}
