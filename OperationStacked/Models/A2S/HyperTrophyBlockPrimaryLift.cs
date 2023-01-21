using OperationStacked.Models.A2S;

public class HyperTrophyBlockPrimaryLift : A2SBlockTemplateValue
{
    public HyperTrophyBlockPrimaryLift()
    {
        amrapRepTarget = new int[] { 10, 8, 6, 9, 7, 5 };
        repsPerSet = new int[] { 5, 4, 3, 5, 4, 3 };
        intensity = new decimal[] { 0.70m, 0.75m, 0.80m, 0.725m, 0.775m, 0.825m };
        sets = 5;
        aux = false;
    }
}