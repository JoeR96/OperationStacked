using OperationStacked.Entities;
using OperationStacked.Enums;
using OperationStacked.Models;
using OperationStacked.Models.A2S;

namespace OperationStacked.Services.A2S
{
    public class A2SHypertrophyService : IA2SHypertrophyService
    {
        private Dictionary<A2SBlocks, A2SBlockTemplateValue> a2SPrimaryLifts { get; set; }
        private Dictionary<A2SBlocks, A2SBlockTemplateValue> a2SAuxLifts { get; set; }
        private HyperTrophyBlockPrimaryLift HTPL { get; set; } = new HyperTrophyBlockPrimaryLift();
        private HyperTrophyBlockAuxillaryLift HTAL { get; set; } = new HyperTrophyBlockAuxillaryLift();
        private StrengthBlockPrimaryLift SBPL { get; set; } = new StrengthBlockPrimaryLift();
        private StrengthBlockAuxillaryLift SBAL { get; set; } = new StrengthBlockAuxillaryLift();
        private PeakingBlockPrimaryLift PBPL { get; set; } = new PeakingBlockPrimaryLift();
        private PeakingBlockAuxillaryLift PBAL { get; set; } = new PeakingBlockAuxillaryLift();
        public A2SHypertrophyService()
        {
            a2SPrimaryLifts = new Dictionary<A2SBlocks, A2SBlockTemplateValue>
            {
                {A2SBlocks.Hypertrophy,HTPL},
                {A2SBlocks.Strength,SBPL},
                {A2SBlocks.Peaking,PBPL}
            };

            a2SAuxLifts = new Dictionary<A2SBlocks, A2SBlockTemplateValue>
            {
                {A2SBlocks.Hypertrophy,HTAL},
                {A2SBlocks.Strength,SBAL},
                {A2SBlocks.Peaking,PBAL}
            };
        }
        public A2SBlockTemplateValue GetTemplateValue(A2SBlocks block, bool primary) => primary ? a2SPrimaryLifts[block] : a2SAuxLifts[block];

        public int GetAmprapRepTarget(A2SBlocks block, int week, bool primary)
        => primary ? a2SPrimaryLifts[block].amrapRepTarget[week] : a2SAuxLifts[block].amrapRepTarget[week];

        public decimal GetIntensity(A2SBlocks block, int week, bool primary) => primary ? a2SPrimaryLifts[block].intensity[week] : a2SAuxLifts[block].intensity[week];

        public int GetRepsPerSet(A2SBlocks block, int week, bool primary) => primary ? a2SPrimaryLifts[block].repsPerSet[week] : a2SAuxLifts[block].repsPerSet[week];

        public int GetSets(A2SBlocks block, int week, bool primary) => primary ? a2SPrimaryLifts[block].sets : a2SAuxLifts[block].sets;

        public decimal GetWorkingWeight(A2SBlocks block, int week, bool primary, decimal trainingMax, decimal roundingValue)
        {
            var workingWeight = GetIntensity(block, week, primary) * trainingMax;
            var newWeight = Math.Round(workingWeight / roundingValue);
            return newWeight * roundingValue;

        }
    }
}

