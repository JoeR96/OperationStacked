using System.ComponentModel.DataAnnotations;
using OperationStacked.Enums;

namespace OperationStacked.Entities;

public class EquipmentStack
{
    [Key]
    public Guid Id { get; set; }
    public Decimal StartWeight { get; set; }
    public Decimal?[] InitialIncrements { get; set; }
    public Decimal IncrementValue { get; set; }
    public Decimal IncrementCount { get; set; }
    public string EquipmentStackKey { get; set; }
    public Guid UserID { get; set; }
}
