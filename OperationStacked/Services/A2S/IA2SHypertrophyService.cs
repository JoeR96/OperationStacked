using OperationStacked.Enums;

namespace OperationStacked.Services.A2S
{
    public interface IA2SHypertrophyService
    {
        int GetAmprapRepTarget(A2SBlocks block, int week, bool auxillaryLift);
        decimal GetIntensity(A2SBlocks block, int week, bool auxillaryLift);
        int GetRepsPerSet(A2SBlocks block, int week, bool auxillaryLift);
        int GetSets(A2SBlocks block, int week, bool auxilllaryLift);
    }
}
