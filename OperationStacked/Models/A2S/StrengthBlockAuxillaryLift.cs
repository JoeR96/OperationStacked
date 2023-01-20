using OperationStacked.Models.A2S;
#region scaffoldData

public class StrengthBlockAuxillaryLift : A2SBlockTemplateValue
{
    public StrengthBlockAuxillaryLift()
    {
        amrapRepTarget = new int[] { 12, 10, 8, 11, 9, 7 };
        repsPerSet = new int[] { 6, 5, 4, 6, 5, 4 };
        intensity = new decimal[] { 0.65m, 0.70m, 0.75m, 0.675m, 0.725m, 0.775m };
        sets = 3;
        aux = true;
    }
}
#endregion