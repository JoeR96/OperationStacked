using OperationStacked.Models.A2S;
#region scaffoldData

public class PeakingBlockPrimaryLift : A2SBlockTemplateValue
{
    public PeakingBlockPrimaryLift()
    {
        amrapRepTarget = new int[] { 6, 4, 2, 4, 2, 2 };
        repsPerSet = new int[] { 3, 2, 1, 2, 1, 1 };
        intensity = new decimal[] { 0.80m, 0.85m, 0.90m, 0.85m, 0.90m, 0.95m };
        sets = 5;
        aux = false;
    }
}
#endregion