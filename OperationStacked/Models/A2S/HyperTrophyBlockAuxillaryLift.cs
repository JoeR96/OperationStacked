using OperationStacked.Models.A2S;

public class HyperTrophyBlockAuxillaryLift : A2SBlockTemplateValue
{
    public HyperTrophyBlockAuxillaryLift()
    {
        amrapRepTarget = new int[] { 14, 12, 10, 13, 11, 9 };
        repsPerSet = new int[] { 7, 6, 5, 7, 6, 5 };
        intensity = new decimal[] { 0.60m, 0.65m, 0.70m, 0.625m, 0.675m, 0.725m };
        sets = 3;
        aux = true;
    }
}
