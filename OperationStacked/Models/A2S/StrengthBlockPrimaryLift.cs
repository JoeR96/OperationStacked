using OperationStacked.Models.A2S;
#region scaffoldData
public class StrengthBlockPrimaryLift : A2SBlockTemplateValue
{
    public StrengthBlockPrimaryLift()
    {
        amrapRepTarget = new int[] { 8, 6, 4, 7, 5, 3 };
        repsPerSet = new int[] { 4, 3, 2, 4, 3, 2 };
        intensity = new decimal[] { 0.75m, 0.80m, 0.85m, 0.775m, 0.825m, 0.875m };
        sets = 5;
        aux = false;
    }
}
#endregion