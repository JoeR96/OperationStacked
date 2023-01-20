using OperationStacked.Models.A2S;
#region scaffoldData

public class PeakingBlockAuxillaryLift : A2SBlockTemplateValue
{
    public PeakingBlockAuxillaryLift()
    {
        amrapRepTarget = new int[] { 10, 8, 6, 8, 3, 2 };
        repsPerSet = new int[] { 5, 4, 3, 4, 6, 4 };
        intensity = new decimal[] { 0.70m, 0.75m, 0.80m, 0.75m, 0.80m, 0.85m };
        sets = 3;
        aux = true;
    }
}
#endregion