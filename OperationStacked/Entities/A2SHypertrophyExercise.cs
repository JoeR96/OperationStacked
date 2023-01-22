using OperationStacked.Abstractions;
using OperationStacked.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace OperationStacked.Entities
{
    [Table("A2SHypertrophyExercise")]
    public class A2SHypertrophyExercise : Exercise
    {
        public decimal TrainingMax { get; set; }
        public bool PrimaryLift { get; set; }
        public A2SBlocks Block { get; set; }
        public int AmrapRepTarget { get; set; }
        public int AmrapRepResult { get; set; }
        public int Week { get; set; }
        public decimal Intensity { get; set; }
        public int Sets { get; set; }
        public int RepsPerSet { get; set; }
        public decimal RoundingValue { get; set; }
    }
    
}
