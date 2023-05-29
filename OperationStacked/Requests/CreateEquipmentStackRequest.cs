﻿namespace OperationStacked.Requests;

public class CreateEquipmentStackRequest
{
    public Decimal StartWeight { get; set; }
    public Decimal?[] InitialIncrements { get; set; }
    public Decimal IncrementValue { get; set; }
    public Decimal IncrementCount { get; set; }
    public string? EquipmentStackKey { get; set; }
    public Guid UserID { get; set; }
   
}