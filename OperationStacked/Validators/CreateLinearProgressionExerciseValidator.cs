using FluentValidation;
using OperationStacked.Entities;
using OperationStacked.Requests;

namespace OperationStacked.Validators
{
    public class CreateLinearProgressionExerciseValidator : AbstractValidator<LinearProgressionExercise>
    {
        public CreateLinearProgressionExerciseValidator()
        {
            RuleFor(x => x.AttemptsBeforeDeload).NotEmpty();
            RuleFor(x => x.MinimumReps).NotEmpty();
            RuleFor(x => x.TargetSets).NotEmpty();
            RuleFor(x => x.LiftDay).NotEmpty();
            RuleFor(x => x.Template).NotEmpty();
            RuleFor(x => x.LiftOrder).NotEmpty();
            RuleFor(x => x.WeightProgression).NotEmpty();
            RuleFor(x => x.MinimumReps).NotEmpty();
            RuleFor(x => x.MaximumReps).NotEmpty();
            RuleFor(x => x.ExerciseName).NotEmpty();
            RuleFor(x => x.WeightProgression).NotEmpty();
            RuleFor(x => x.WeightIndex).NotEmpty();
            RuleFor(x => x.StartingWeight).NotEmpty();
            RuleFor(x => x.PrimaryExercise).NotEmpty();
            RuleFor(x => x.TargetSets).NotEmpty();
        }
    }
}
